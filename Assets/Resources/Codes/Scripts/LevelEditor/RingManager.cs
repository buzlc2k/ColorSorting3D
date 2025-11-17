using System;
using System.Collections;
using System.Collections.Generic;
using ColorSorting3D.Config;
using ObjectPuuler;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.LevelEditor
{
    internal class RingManager : MonoBehaviour
    {
        private Ring selectedRing;
        private GridCell selectedCell;
        private RingID selectedRingID;
        [SerializeField] private Ring prefab;
        private ObjectPooler<Ring> pooler;

        // InternalComponents
        [SerializeField] private PreviewRing previewRing;

        // ExternalDepedencies
        private InputManager inputManager = default;
        private EventManager eventManager = default;

        private void Awake()
            => InitSpawner();

        private void Start()
        {
            LoadDepedencies();
            LaunchSelecting();
        }

        private void InitSpawner()
            => pooler = new(prefab, transform, 5);

        private void LoadDepedencies()
        {
            inputManager = ServiceLocators.SceneOf(this).Get<InputManager>();
            eventManager = ServiceLocators.SceneOf(this).Get<EventManager>();
        }

        public void LaunchAdding(string idStr)
        {
            selectedRingID = (RingID)Enum.Parse(typeof(RingID), idStr);
            TogglePreview(true);
            Invoke(nameof(LazyStartProcessAdding), 0.02f);
        }

        private void LazyStartProcessAdding()
                => processAdding = StartCoroutine(ProcessAdding());

        private Coroutine processAdding = default;
        private IEnumerator ProcessAdding()
        {
            while (true)
            {
                if (!inputManager.IsMouseDownOnGrid())
                {
                    selectedCell = inputManager.GetMouseCell();
                    previewRing.transform.position = selectedCell.GetNextRingPos();
                }
                else
                    FinishAdding();

                yield return null;
            }
        }

        private void FinishAdding()
        {
            if (selectedCell.CanNotSpawnRing)
                return;

            Spawn();
            KillAdding();
        }

        public void KillAdding()
        {
            if (processAdding == null)
                return;

            StopCoroutine(processAdding);
            processAdding = null;

            TogglePreview(false);

            eventManager.PostEvent(EventID.KillRingProcess);
        }

        public void LaunchSelecting()
            => Invoke(nameof(LazyStartProcessSelecting), 0.02f);

        private void LazyStartProcessSelecting()
            => processSelecting = StartCoroutine(ProcessSelecting());

        private Coroutine processSelecting = default;
        private IEnumerator ProcessSelecting()
        {
            while (true)
            {
                if (inputManager.IsMouseDownOnRing(out var ring))
                    SelectRing(ring);
                    
                yield return null;
            }
        }

        public void KillSelecting()
        {
            if (processSelecting == null)
                return;

            StopCoroutine(processSelecting);
            processSelecting = null;
        }

        public void SelectRing(Ring ring)
        {
            if (selectedRing != null)
                UnZoomSelectedRing();

            selectedRing = ring;
            ZoomSelectedRing();
            selectedCell = inputManager.GetMouseCell();
            eventManager.PostEvent(EventID.RingSelected);
        }

        public void UnselectedRing()
        {
            if (processSelecting == null)
                return;

            UnZoomSelectedRing();
            eventManager.PostEvent(EventID.RingUnselected);
            selectedRing = null;
        }

        public void Remove()
        {
            selectedCell.RemoveRing(selectedRing);
            UnselectedRing();
        }

        private void Spawn()
        {
            var ringSpawned = pooler.Get();
            ringSpawned.InitRing(selectedRingID);
            selectedCell.AddRing(ringSpawned);
        }

        public void Spawn(GridCell cell, List<RingID> ringIDs)
        {
            foreach (var ringID in ringIDs)
            {
                var ringSpawned = pooler.Get();
                ringSpawned.InitRing(ringID);
                cell.AddRing(ringSpawned);
            }
        }

        private void TogglePreview(bool active)
        {
            if (active)
                previewRing.InitPreviewRing(selectedRingID);

            previewRing.gameObject.SetActive(active);
        }

        private void ZoomSelectedRing()
            => selectedRing.transform.localScale *= 1.1f;

        private void UnZoomSelectedRing()
            => selectedRing.transform.localScale /= 1.1f;
    }
}