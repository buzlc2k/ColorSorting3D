using System;
using System.Collections;
using System.Threading.Tasks;
using ColorSorting3D.Config;
using DG.Tweening;
using ObjectPuuler;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public class Ring : MonoBehaviour, IPooled
    {
        // Internals
        [SerializeField] private MeshRenderer ringRenderer;

        // ExternalDepedencies
        private RingConfig ringConfig = default;

        public Action<GameObject> ReleaseCallback { get; set; }

        private void Awake()
        {
            LoadDepedencies();

            void LoadDepedencies()
            {
                ringConfig = ServiceLocators.SceneOf(this).Get<RingConfig>();
            }
        }

        public void InitRing(RingID id, Vector3 pos)
        {
            SetColor();
            transform.position = pos;

            void SetColor()
                => ringRenderer.material = ringConfig.Get(id).Material;
        }

        public async Task Move(Vector3 targetPos, float time)
        {
            var moveTween = transform.DOMove(targetPos, time)
                                     .SetEase(Ease.OutQuad);

            await moveTween.AsyncWaitForCompletion();
        }

        public async Task Rotate(Vector3 targetEulerAngle, float time, int loop = 1)
        {
            var rotateTween = transform.DORotate(targetEulerAngle, time)
                                       .SetLoops(loop, LoopType.Yoyo)
                                       .SetEase(Ease.InOutSine);

            await rotateTween.AsyncWaitForCompletion();
        }

        public async Task MoveUp(Vector3 targetPos)
        {
            var moveTween = Move(targetPos, 0.16f);
            var rotateTween = Rotate(new Vector3(0, 120, 0), 0.16f);

            await Task.WhenAll(moveTween, rotateTween);
        }

        public async Task MoveTo(Vector3 targetPos)
            => await Move(targetPos, 0.16f);

        public async Task MoveDown(Vector3 targetPos)
        {
            var moveTween = Move(targetPos, 0.16f);
            var rotateTween = Rotate(new Vector3(0, 0, 0), 0.16f);

            await Task.WhenAll(moveTween, rotateTween);
        }
    }   
}