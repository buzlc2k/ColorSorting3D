using System.Collections;
using ObjectPuuler;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.LevelEditor
{
    internal class RingsContainerManager : MonoBehaviour
    {
        private GridCell selectedCell = default;
        private RingsContainer selectedContainer = default;
        [SerializeField] private RingsContainer prefab;
        private ObjectPooler<RingsContainer> pooler;

        // InternalComponents
        [SerializeField] private GameObject previewContainer;

        // ExternalDepedencies
        private InputManager inputManager = default;
        private EventManager eventManager = default;
        private UIServices uiServices = default;

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
            uiServices = ServiceLocators.SceneOf(this).Get<UIServices>();
        }

        public void LaunchAdding()
        {
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
                    previewContainer.transform.position = selectedCell.WorldPos;
                }
                else
                    FinishAdding();

                yield return null;
            }
        }

        private void FinishAdding()
        {
            if (!selectedCell.Available)
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

            eventManager.PostEvent(EventID.KillAddingContainerProcess);
        }

        public void LaunchSelecting()
        {
            Invoke(nameof(LazyStartProcessSelecting), 0.02f);
        }

        private void LazyStartProcessSelecting()
            => processSelecting = StartCoroutine(ProcessSelecting());

        private Coroutine processSelecting = default;
        private IEnumerator ProcessSelecting()
        {
            while (true)
            {
                if (inputManager.IsMouseDownOnContainer(out var container))
                    SelectContainer(container);

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

        public void SelectContainer(RingsContainer container)
        {
            if (selectedContainer != null)
                UnZoomSelectedContainer();

            selectedContainer = container;
            ZoomSelectedContainer();
            selectedCell = inputManager.GetMouseCell();
            uiServices.SetToggleValue(selectedContainer.GetAvailable());
            eventManager.PostEvent(EventID.ContainerSelected);
        }

        public void UnselectedContainer()
        {
            if (processSelecting == null)
                return;

            UnZoomSelectedContainer();
            eventManager.PostEvent(EventID.ContainerUnselected);
            selectedContainer = null;
        }

        public void AddSize()
            => selectedCell.IncreseSizeOfContainer();
        
        public void DecreseSize()
            => selectedCell.DecreseSizeOfContainer();

        public void Remove()
        {
            selectedCell.RemoveContainer();
            UnselectedContainer();
        }

        public void SetAvailable()
        {
            var available = uiServices.GetToggleValue();
            selectedCell.SetContainerAvailble(available);
        }
        
        private void Spawn()
        {
            var containerSpawned = pooler.Get();
            containerSpawned.InitContainer(1, true);
            selectedCell.AddContainer(containerSpawned);
        }

        public void Spawn(GridCell cell, int size, bool available)
        {
            var containerSpawned = pooler.Get();
            containerSpawned.InitContainer(size, available);
            cell.AddContainer(containerSpawned);
        }

        private void TogglePreview(bool active)
            => previewContainer.SetActive(active);
            
        private void ZoomSelectedContainer()
            => selectedContainer.transform.localScale *= 1.1f;

        private void UnZoomSelectedContainer()
            => selectedContainer.transform.localScale /= 1.1f;
    }
}