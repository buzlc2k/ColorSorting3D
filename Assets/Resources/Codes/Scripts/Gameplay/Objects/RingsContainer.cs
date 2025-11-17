using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ColorSorting3D.Config;
using DG.Tweening;
using ObjectPuuler;
using R3;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public class RingsContainer : MonoBehaviour, IPooled
    {
        // Runtime fields
        private int currentSize = default;
        private int size = default;
        private Vector3 rootPos = default;
        private bool available = default;
        private Stack<(RingID id, Stack<Ring> rings)> ringStacks = default;
        
        // Properties
        public bool HasRings { get => currentSize != 0; }
        public bool CanAcceptRings { get => currentSize != size; }
        public ReactiveProperty<bool> IsReadOnly { get; } = new(false);
        public Action<GameObject> ReleaseCallback { get; set; }

        // Internals
        [SerializeField] private MeshRenderer containerRenderer = default;

        // External Depedencies
        private ContainerConfig containerConfig = default;

        private void Awake()
        {
            LoadDepedencies();

            void LoadDepedencies()
            {
                containerConfig = ServiceLocators.SceneOf(this).Get<ContainerConfig>();
            }
        }

        public void Create(Vector3 pos, int size, bool available, Stack<(RingID id, Stack<Ring> rings)> ringStacks)
        {
            rootPos = pos;
            transform.position = pos;
            transform.localScale = new(1, size + 0.25f, 1);
            this.size = size;
            _ = SetReadOnly(false);
            SetAvailable(available);

            this.ringStacks = ringStacks;
            foreach (var (id, rings) in ringStacks)
                currentSize += rings.Count;
            MarkLoaded();
        }

        #region GetData

        public bool GetAvailable()
            => available;

        public int GetStackCount()
            => ringStacks.Count;

        public RingID GetLastColorStack()
        {
            if (!HasRings)
                return RingID.None;

            return ringStacks.Peek().id;
        }

        public Ring GetLastRing()
        {
            if (!HasRings)
                return null;

            return ringStacks.Peek().rings.Peek();
        }
        
        private Vector3 GetPosByIndex(int index)
            => rootPos + new Vector3(0, index * 0.0875f, 0);

        public Vector3 GetNextRingPos()
            => GetPosByIndex(currentSize);

        public Vector3 GetLastRingPos()
            => GetPosByIndex(currentSize - 1);

        public Vector3 GetEdgeRingPos()
            => GetPosByIndex(size + 1);

        #endregion

        #region Transfer Behavior

        public void InitNewStack(RingID id)
            => ringStacks.Push((id, new()));

        public void SetAvailable(bool available)
        {
            this.available = available;
            SetColor();

            void SetColor()
                => containerRenderer.material = containerConfig.Get(available).Material;
        }

        public async Task SetReadOnly(bool IsReadOnly)
        {
            this.IsReadOnly.Value = IsReadOnly;

            if (IsReadOnly)
                await MarkReadOnly();
        }

        public void AddRing(Ring addedRing)
        {
            currentSize++;
            ringStacks.Peek().rings.Push(addedRing);
        }

        public Ring RemoveRing()
        {
            currentSize--;
            var ringsStack = ringStacks.Peek().rings;
            var removedRing = ringsStack.Pop();

            if (ringsStack.Count == 0)
                ringStacks.Pop();

            return removedRing;
        }

        #endregion

        #region Animation

        public void MarkLoaded()
        {
            Sequence seq = DOTween.Sequence();
            
            seq.Append(transform
                .DOMoveY(transform.position.y, 0.45f)
                .From(transform.position.y - 0.25f)
                .SetEase(Ease.OutQuad));
            seq.Join(transform
                .DOScale(new Vector3(1, size + 0.25f, 1), 0.45f)
                .From(Vector3.zero)
                .SetEase(Ease.OutBack));

            if (!HasRings)
                return;
                
            float delayPerRing = 0.05f;
            int i = 0;
            foreach (var (id, rings) in ringStacks)
            foreach (var ring in rings)
            {
                ring.transform.localScale = Vector3.zero;
                seq.Insert(0.3f + i * delayPerRing, ring.transform
                    .DOScale(1.1f, 0.25f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => ring.transform.DOScale(1f, 0.15f)));
                i++;
            }
        }

        public async Task MarkReadOnly()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(transform.localScale * 1.1f, 0.15f).SetEase(Ease.OutBack));
            seq.Append(transform.DOScale(transform.localScale * 1f, 0.2f).SetEase(Ease.InOutSine));

            var rings = ringStacks.Peek().rings;
            int i = 0;

            foreach(var ring in rings)
            {
                seq.Insert(i * 0.02f, ring.transform
                    .DOScale(1.1f, 0.15f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => ring.transform.DOScale(1f, 0.15f)));
                i++;
            }

            await seq.AsyncWaitForCompletion();
        }

        public async Task MarkSelected()
        {
            var lastRing = GetLastRing();

            if (lastRing == null)
                await transform.DOScale(transform.localScale * 1.1f, 0.16f)
                                .SetEase(Ease.InOutBack)
                                .AsyncWaitForCompletion();
            else
                await lastRing.MoveUp(GetEdgeRingPos());
        }
        
        public async Task MarkSelectedReadOnly()
        {
            var rings = ringStacks.Peek().rings; // ReadOnly when ringStacks.Count = 1
            var targetEuelerAngle = new Vector3(0, 20, 0);
            
            Task lastSequence = null;
            foreach (var ring in rings)
            {
                lastSequence = ring.Rotate(targetEuelerAngle, 0.1f, 2);
                await Task.Delay(55);
            }
            
            await lastSequence;
        }

        public async Task MarkUnSelected()
        {
            var lastRing = GetLastRing();

            if (lastRing == null)
                await transform.DOScale(transform.localScale / 1.1f, 0.16f)
                                .SetEase(Ease.InOutBack)
                                .AsyncWaitForCompletion();
            else
                await lastRing.MoveDown(GetLastRingPos());
        }

        #endregion
    }
}