using System;
using ColorSorting3D.Config;
using ObjectPuuler;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.LevelEditor
{
    internal class RingsContainer : MonoBehaviour, IPooled
    {
        [SerializeField] private MeshRenderer containerRenderer;
        private bool available;

        public Action<GameObject> ReleaseCallback { get; set; }

        // ExternalDepedencies
        private ContainerConfig containerConfig = default;

        private void Awake()
            => ReleaseCallback += (gameObject) => gameObject.SetActive(false);
        
        private void OnEnable()
        {
            LoadDepedencies();

            void LoadDepedencies()
            {
                containerConfig = containerConfig != null ? containerConfig : ServiceLocators.SceneOf(this).Get<ContainerConfig>();
            }
        }

        public void InitContainer(int size, bool available)
        {
            transform.localScale = new(1, size + 0.25f, 1);
            SetAvailable(available);
        }

        public void IncreaseSize(float addedSize = 1)
        {
            var scale = transform.localScale + new Vector3(0, addedSize, 0);
            transform.localScale = scale;
        }

        public void DecreaseSize(float removedSize = 1)
        {
            var scale = transform.localScale - new Vector3(0, removedSize, 0);
            transform.localScale = scale;   
        }

        public int GetSize()
            => (int)transform.localScale.y;

        public bool GetAvailable()
            => available;

        public void SetPos(Vector3 pos)
            => transform.position = pos;

        public void SetAvailable(bool available)
        {
            this.available = available;
            SetColor();

            void SetColor()
                => containerRenderer.material = containerConfig.Get(available).Material;
        }
    }
}