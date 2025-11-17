using ColorSorting3D.Config;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.LevelEditor
{
    internal class LevelEditorInstaller : SceneInstaller
    {
        [SerializeField] private CameraController cameraController;
        [SerializeField] private RingConfig ringConfig;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private LevelEditorManager editorManager;
        [SerializeField] private EventManager eventManager;
        [SerializeField] private LevelConfig levelConfig;
        [SerializeField] private ContainerConfig containerConfig;
        [SerializeField] private RingsContainerManager containerManager;
        [SerializeField] private RingManager ringManager;
        [SerializeField] private UIServices uiServices;

        public override void Binds()
        {
            serviceLocator.RegisterService<InputManager>(inputManager);
            serviceLocator.RegisterService<RingConfig>(ringConfig);
            serviceLocator.RegisterService<Camera>(cameraController.MainCamera);
            serviceLocator.RegisterService<GridManager>(gridManager);
            serviceLocator.RegisterService<LevelEditorManager>(editorManager);
            serviceLocator.RegisterService<EventManager>(eventManager);
            serviceLocator.RegisterService<LevelConfig>(levelConfig);
            serviceLocator.RegisterService<ContainerConfig>(containerConfig);
            serviceLocator.RegisterService<RingsContainerManager>(containerManager);
            serviceLocator.RegisterService<RingManager>(ringManager);
            serviceLocator.RegisterService<UIServices>(uiServices);
        }
    }
}