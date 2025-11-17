using System;
using System.Collections.Generic;
using ColorSorting3D.Config;
using ObjectPuuler;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.LevelEditor
{
    internal class PreviewRing : MonoBehaviour
    {
        [SerializeField] private MeshRenderer ringRenderer;

        // ExternalDepedencies
        private RingConfig ringConfig = default;

        private void Start()
        {
            LoadDepedencies();
            gameObject.SetActive(false);
        }

        private void LoadDepedencies()
        {
            ringConfig = ServiceLocators.SceneOf(this).Get<RingConfig>();
        }

        public void InitPreviewRing(RingID id) {
            ringRenderer.material = ringConfig.Get(id).Material;
        }
    }
}