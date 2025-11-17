using System;
using UnityEngine;
using R3;
using System.Threading.Tasks;
using ServiceButcator;
using ColorSorting3D.Config;

namespace ColorSorting3D.Gameplay
{
    public class HomePresenter : ModelPresenter<HomeViewModel>
    {
        private LevelManager levelManager = default;
        private UserResourcesManager userResourcesManager = default;

        protected override void SetID()
            => id = PresentationID.Home;

        protected override void LoadDepedencies()
        {
            base.LoadDepedencies();
            levelManager = ServiceLocators.ThisScene().Get<LevelManager>();
            userResourcesManager = ServiceLocators.ThisScene().Get<UserResourcesManager>();
        }

        protected override Task InitializeInternal()
        {
            base.InitializeInternal();

            viewModel.OnClickPlayButtonCommand
                    .Subscribe(_ => levelManager.LoadLevel())
                    .AddTo(presenterDisposables);

            viewModel.OnClickPlayButtonCommand
                    .Subscribe(_ => transitionService.MakeTransition(PresentationTransitionID.HomeToHUD))
                    .AddTo(presenterDisposables);

            viewModel.OnClickSettingButtonCommand
                    .Subscribe(_ => transitionService.MakeTransition(PresentationTransitionID.PopUpModal, ModalContentID.Setting))
                    .AddTo(presenterDisposables);
            
            viewModel.OnClickRemoveAdsButtonCommand
                    .Subscribe(_ => transitionService.MakeTransition(PresentationTransitionID.PopUpModal, ModalContentID.RemoveAds))
                    .AddTo(presenterDisposables);
            
            viewModel.OnClickAddCoinsCommand
                     .Subscribe(_ => transitionService.MakeTransition(PresentationTransitionID.PopUpModal, ModalContentID.AddCoins))
                     .AddTo(presenterDisposables);

            levelManager.CurrentLevel
                        .Subscribe(currentLevel => viewModel.CurrentLevel.Value = currentLevel)
                        .AddTo(presenterDisposables);
            
            userResourcesManager.CurrentCoins
                                .Subscribe(currentCoins => viewModel.CurrentCoins.Value = currentCoins)
                                .AddTo(presenterDisposables);

            return Task.CompletedTask;
        }
    }
}