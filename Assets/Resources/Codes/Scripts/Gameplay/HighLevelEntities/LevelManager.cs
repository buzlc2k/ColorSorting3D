using System.Collections.Generic;
using System.Threading.Tasks;
using ColorSorting3D.Config;
using ObjectPuuler;
using ObservableCollections;
using R3;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public class LevelManager : HighLevelEntity
    {
        // Runtime Fields
        private int currentContainerFinished = 0;
        private int targetContainerFinished = 0;

        // Prefabs
        [SerializeField] private RingsContainer containerPrefab;
        [SerializeField] private Ring ringPrefab;

        // Runtime Internals
        private ObjectPooler<RingsContainer> containerPooler;
        private ObjectPooler<Ring> ringPooler;
        protected CompositeDisposable activeContainerDisposables;

        // External Depedencies
        private LevelConfig levelConfig;
        private DataManager dataManager;
        protected PresentationTransitionService transitionService;
        private ScreenManager screenManager;
        
        // Properties
        public RingsDispatcher RingsDispatcher { get; private set; }
        public ReactiveProperty<int> CurrentLevel { get; } = new(0);
        public ObservableList<RingsContainer> AvailableContainers { get; private set; }
        public ObservableList<RingsContainer> UnAvailableContainers { get; private set; }
        public List<Ring> ActiveRings { get; private set; }

        private void Awake()
        {
            LoadInternals();
            LoadDepedencies();

            void LoadInternals()
            {
                AvailableContainers = new();
                UnAvailableContainers = new();
                ActiveRings = new();

                containerPooler = new(containerPrefab, transform, 8);
                ringPooler = new(ringPrefab, transform, 50);

                activeContainerDisposables = new();
            }

            void LoadDepedencies()
            {
                levelConfig = ServiceLocators.SceneOf(this).Get<LevelConfig>();
                dataManager = ServiceLocators.SceneOf(this).Get<DataManager>();
                RingsDispatcher = ServiceLocators.SceneOf(this).Get<RingsDispatcher>();
                transitionService = ServiceLocators.SceneOf(this).Get<PresentationTransitionService>();
                screenManager = ServiceLocators.SceneOf(this).Get<ScreenManager>();
            }
        }

        public override Task Init()
        {
            InitReactiveProperties();
            RingsDispatcher.InitReactiveProperties();

            return Task.CompletedTask;

            void InitReactiveProperties()
            {
                CurrentLevel.Value = dataManager.GetCurrentLevel();
                CurrentLevel.Skip(1)
                            .Subscribe(currentLevel => dataManager.SetCurrentLevel(currentLevel));
                CurrentLevel.AddTo(this);
            }
        }
        
        public void LoadNextLevel()
        {
            CurrentLevel.Value++;
            LoadLevel();
        }

        public void LoadLevel()
        {
            LevelConfigRecord levelConfigRecord;
            while (!TryGetLevel(out levelConfigRecord))
                CurrentLevel.Value--;
                
            ClearPrevLevel();
            LoadLevel();

            bool TryGetLevel(out LevelConfigRecord levelConfigRecord)
            {
                levelConfigRecord = levelConfig.Get(CurrentLevel.Value);
                if (levelConfigRecord != null)
                    return true;

                Debug.LogWarning($"LevelManager: Level {CurrentLevel.Value} does not exist!");
                return false;
            }

            void ClearPrevLevel()
            {
                foreach (var container in AvailableContainers)
                    container.gameObject.SetActive(false);
                AvailableContainers.Clear();

                foreach (var container in UnAvailableContainers)
                    container.gameObject.SetActive(false);
                UnAvailableContainers.Clear();

                foreach (var ring in ActiveRings)
                    ring.gameObject.SetActive(false);
                ActiveRings.Clear();
                
                RingsDispatcher.Clear();
                activeContainerDisposables.Clear();
            }

            void LoadLevel()
            {
                SetContainerFinished();
                LoadCamera();
                LoadContainers();
            }

            void LoadCamera()
                => screenManager.SetCameraPos(levelConfigRecord.CameraPos);

            void SetContainerFinished()
            {
                currentContainerFinished = 0;
                targetContainerFinished = levelConfigRecord.TargetContainerFinished;
            }
            
            void LoadContainers()
            {
                foreach (var savedContainer in levelConfigRecord.Containers)
                    LoadObjectsInContainer(savedContainer);
            }

            void LoadObjectsInContainer(SavedRingsContainer savedContainer)
            {
                var ringsStacks = CreateRingsStack(savedContainer);
                var container = containerPooler.Get();

                InitContainer();

                void InitContainer()
                {
                    container.Create(savedContainer.Position, savedContainer.Size, savedContainer.Available, ringsStacks);
                    container.IsReadOnly
                             .Skip(1)
                             .Subscribe(isReadOnly => {
                                    currentContainerFinished += isReadOnly ? 1 : -1;
                                    if (currentContainerFinished == targetContainerFinished)
                                        transitionService.MakeTransition(PresentationTransitionID.HUDToCompleteLevel);
                                })
                             .AddTo(activeContainerDisposables);
                             
                    var containersGroup = savedContainer.Available ? AvailableContainers : UnAvailableContainers;
                    containersGroup.Add(container);
                }
            }

            Stack<(RingID Id, Stack<Ring> Rings)> CreateRingsStack(SavedRingsContainer savedContainer)
            {
                var ringsStacks = new Stack<(RingID Id, Stack<Ring> Rings)>();
                var savedRings = savedContainer.Rings;

                for (int i = 0; i < savedRings.Count; i++)
                {
                    var ring = ringPooler.Get();
                    var pos = savedContainer.Position + Vector3.up * (i * 0.0875f);
                    ring.InitRing(savedRings[i], pos);
                    ActiveRings.Add(ring);

                    if (i == 0 || savedRings[i] != savedRings[i - 1])
                        ringsStacks.Push((savedRings[i], new Stack<Ring>(new[] { ring })));
                    else
                        ringsStacks.Peek().Rings.Push(ring);
                }

                return ringsStacks;
            }
        }
    }
}