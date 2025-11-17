using System.Collections.Generic;
using System.Linq;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.LevelEditor
{
    [RequireComponent(typeof(Camera))]
    internal class CameraController : MonoBehaviour
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }

        // ExternalDepedencies
        private UIServices uiServices = default;

        private void Start()
        {
            LoadDepedencies();

            void LoadDepedencies()
            {
                uiServices = ServiceLocators.SceneOf(this).Get<UIServices>();
            }
        }

        private void Update()
        {
            MoveCamera();

            void MoveCamera()
            {
                var posVal = uiServices.GetPosSliderValue();
                transform.position = new(0, posVal, -posVal);
            }
        }
    }
}