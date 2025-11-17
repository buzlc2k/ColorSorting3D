using System.Threading.Tasks;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public class ScreenManager : HighLevelEntity
    {
        [Header("Configs")]   
        [SerializeField] private float defaultScreenWidth;
        [SerializeField] private float defaultScreenHeight;
        [SerializeField] private float defaultHorizontalFOV;
        [SerializeField] private float defaultVerticalFOV;
        [SerializeField] private int targetFrameRate;

        [Header("Internals")]
        [field: SerializeField] public Camera MainCamera { get ; private set; }

        private void Awake()
        {
            CalculateCameraFOV();
            SetScreenFrameRate();

            void SetScreenFrameRate()
                => Application.targetFrameRate = targetFrameRate;

            void CalculateCameraFOV()
            {
                float normalizeWidth = (float)Screen.width / defaultScreenWidth;
                float normalizeHeight = (float)Screen.height / defaultScreenHeight;

                if (normalizeWidth >= normalizeHeight)
                    MainCamera.fieldOfView = defaultVerticalFOV;
                else
                {
                    var horizontalRadAngle = defaultHorizontalFOV * Mathf.Deg2Rad;
                    float aspectRatio = (float)Screen.width / Screen.height;
                    var verticalRadAngle = 2 * Mathf.Atan(Mathf.Tan(horizontalRadAngle / 2) / aspectRatio);
                    MainCamera.fieldOfView = verticalRadAngle * Mathf.Rad2Deg; 
                }
            }
        }

        public override Task Init()
            => Task.CompletedTask;

        public void SetCameraPos(Vector3 position)
            =>  MainCamera.transform.position = position;
    }
}