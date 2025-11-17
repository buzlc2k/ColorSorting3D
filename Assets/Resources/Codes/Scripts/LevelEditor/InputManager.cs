using ServiceButcator;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ColorSorting3D.LevelEditor
{
    internal class InputManager : MonoBehaviour
    {
        // ExternalDepedencies
        private Camera mainCamera = default;
        private GridManager gridManager = default;

        private void Start()
        {
            LoadDepedencies();

            void LoadDepedencies()
            {
                mainCamera = ServiceLocators.SceneOf(this).Get<Camera>();
                gridManager = ServiceLocators.SceneOf(this).Get<GridManager>();
            }
        }

        private Vector3 lastPos = default;
        public Vector3 GetMousePos()
        {
            Ray directionOfTouch = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(directionOfTouch.origin, directionOfTouch.direction, out RaycastHit hit, Mathf.Infinity))
                lastPos = hit.point;

            return lastPos;
        }
        
        private GridCell lastCell = default;
        public GridCell GetMouseCell()
        {
            lastCell = gridManager.GetCell(GetMousePos());

            return lastCell;
        }

        public bool IsMouseDownOnGrid()
            => Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject();

        public bool IsMouseDownOnRing(out Ring ring)
        {
            ring = null;
            if (!Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.LeftShift) || EventSystem.current.IsPointerOverGameObject())
                return false;
            
            Ray directionOfTouch = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(directionOfTouch.origin, directionOfTouch.direction, out RaycastHit hit, Mathf.Infinity, 1 << 7))
                return false;
            
            ring = hit.transform.GetComponentInParent<Ring>();
            return true;
        }

        public bool IsMouseDownOnContainer(out RingsContainer container)
        {
            container = null;
            if (!Input.GetMouseButtonDown(0) || !Input.GetKey(KeyCode.LeftShift) || EventSystem.current.IsPointerOverGameObject())
                return false;
            
            Ray directionOfTouch = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(directionOfTouch.origin, directionOfTouch.direction, out RaycastHit hit, Mathf.Infinity, 1 << 8))
                return false;
            
            container = hit.transform.GetComponentInParent<RingsContainer>();
            return true;
        }
    }
}