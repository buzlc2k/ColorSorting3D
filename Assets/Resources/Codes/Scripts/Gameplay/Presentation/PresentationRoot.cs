using System.Threading.Tasks;
using ColorSorting3D.Config;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public class PresentationRoot : HighLevelEntity
    {
        // Presenters
        private LoadingPresenter loadingPresenter = new();
        private HomePresenter homePresenter = new();
        private HUDPresenter hudPresenter = new();
        private CompleteLevelPresenter completeLevelPresenter = new();
        private ModalPresenter modalPresenter = new();

        // ViewModels
        private LoadingViewModel loadingViewModel = new();
        private HomeViewModel homeViewModel = new();
        private HUDViewModel hudViewModel = new();
        private CompleteLevelViewModel completeLevelViewModel = new();
        private ModalViewModel modalViewModel = new();

        // Views
        [SerializeField] private LoadingView loadingView;
        [SerializeField] private HomeView homeView;
        [SerializeField] private HUDView hudView;
        [SerializeField] private CompleteLevelView completeLevelView;
        [SerializeField] private ModalView modalView;

        public override Task Init()
        {
            // Presenters
            loadingPresenter.Initialize(loadingViewModel);
            homePresenter.Initialize(homeViewModel);
            hudPresenter.Initialize(hudViewModel);
            completeLevelPresenter.Initialize(completeLevelViewModel);
            modalPresenter.Initialize(modalViewModel);

            // Views 
            loadingView.Initialize(loadingViewModel);
            homeView.Initialize(homeViewModel);
            hudView.Initialize(hudViewModel);
            completeLevelView.Initialize(completeLevelViewModel);
            modalView.Initialize(modalViewModel);

            StartLoading();

            return Task.CompletedTask;

            void StartLoading()
                => ServiceLocators.ThisScene().Get<PresentationTransitionService>().MakeTransition(PresentationTransitionID.SetLoading);
        }
    }
}