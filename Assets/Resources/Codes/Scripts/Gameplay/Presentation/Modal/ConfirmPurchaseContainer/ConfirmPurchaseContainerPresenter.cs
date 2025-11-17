using System;
using UnityEngine;
using R3;
using System.Threading.Tasks;
using ServiceButcator;
using ColorSorting3D.Config;

namespace ColorSorting3D.Gameplay
{
    public class ConfirmPurchaseContainerPresenter : ContentPresenter<ConfirmPurchaseContainerViewModel>
    {
        // Runtime fields
        private RingsContainer selectedContainer = default;

        // External Depedencies
        private LevelManager levelManager = default;
        private UserResourcesManager userResourcesManager = default;

        protected override void LoadDepedencies()
        {
            base.LoadDepedencies();
            userResourcesManager = ServiceLocators.ThisScene().Get<UserResourcesManager>();
            levelManager = ServiceLocators.ThisScene().Get<LevelManager>();
        }

        public void SetSelectedContainer(RingsContainer selectedContainer)
            => this.selectedContainer = selectedContainer;

        protected override Task InitializeInternal()
        {
            userResourcesManager.CurrentCoins
                                .Subscribe(currentCoin => viewModel.CanBuy.Value = currentCoin >= ITEM_PRICE.ADD_CONTAINER)
                                .AddTo(presenterDisposables);

            viewModel.OnClickConfirmButtonCommand
                     .Subscribe(_ =>
                     {
                        selectedContainer.SetAvailable(true);
                        userResourcesManager.RemoveCoins(ITEM_PRICE.ADD_CONTAINER);
                        levelManager.UnAvailableContainers.Remove(selectedContainer);
                        levelManager.AvailableContainers.Add(selectedContainer);
                        transitionService.MakeTransition(PresentationTransitionID.PopOutModal);
                     })
                     .AddTo(presenterDisposables);

            return Task.CompletedTask;
        }    
        

    }
}