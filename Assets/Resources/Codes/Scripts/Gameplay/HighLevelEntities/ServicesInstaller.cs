using ColorSorting3D.Config;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public class ServicesInstaller : SceneInstaller
    {
        [Header("Entities")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private DataManager dataManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private RingsDispatcher ringsDispatcher;
        [SerializeField] private UserResourcesManager resourcesManager;
        [SerializeField] private ScreenManager screenManager;

        [Header("Configs")]
        [SerializeField] private RingConfig ringConfig;
        [SerializeField] private ContainerConfig containerConfig;
        [SerializeField] private LevelConfig levelConfig;
        [SerializeField] private PresentationTransitionConfig transitionConfig;

        public override void Binds()
        {
            serviceLocator.RegisterService<RingConfig>(ringConfig);
            serviceLocator.RegisterService<LevelConfig>(levelConfig);
            serviceLocator.RegisterService<PresentationTransitionConfig>(transitionConfig);
            serviceLocator.RegisterService<ContainerConfig>(containerConfig);

            serviceLocator.RegisterService<Camera>(mainCamera);
            serviceLocator.RegisterService<InputManager>(inputManager);
            serviceLocator.RegisterService<DataManager>(dataManager);
            serviceLocator.RegisterService<RingsDispatcher>(ringsDispatcher);
            serviceLocator.RegisterService<LevelManager>(levelManager);
            serviceLocator.RegisterService<UserResourcesManager>(resourcesManager);
            serviceLocator.RegisterService<ScreenManager>(screenManager);

            serviceLocator.RegisterService<PresentationTransitionService>(new());
        }
    }
}