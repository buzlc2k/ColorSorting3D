using System;
using System.Collections.Generic;
using ColorSorting3D.Config;
using ObjectPuuler;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.LevelEditor
{
    internal class Ring : MonoBehaviour, IPooled
    {
        private RingID id = default;
        [SerializeField] private MeshRenderer ringRenderer;

        // ExternalDepedencies
        private RingConfig ringConfig = default;

        public Action<GameObject> ReleaseCallback { get; set; }

        private void Awake()
            => ReleaseCallback += (gameObject) => gameObject.SetActive(false);

        private void OnEnable()
           => LoadDepedencies();

        private void LoadDepedencies()
        {
            ringConfig = ringConfig != null ? ringConfig : ServiceLocators.SceneOf(this).Get<RingConfig>();
        }

        public RingID GetID()
            => id;

        public void InitRing(RingID id)
        {
            this.id = id;
            SetColor();

            void SetColor()
                => ringRenderer.material = ringConfig.Get(id).Material;
        }

        public void SetPos(Vector3 pos)
            => transform.position = pos;        
    }
}