using System;
using UnityEngine;
using R3;
using System.Threading.Tasks;
using ServiceButcator;
using ColorSorting3D.Config;

namespace ColorSorting3D.Gameplay
{
    public class ConfirmPurchaseUndoPresenter : ContentPresenter<ConfirmPurchaseUndoViewModel>
    {
        private UserResourcesManager userResourcesManager = default;

        protected override void LoadDepedencies()
        {
            base.LoadDepedencies();
            userResourcesManager = ServiceLocators.ThisScene().Get<UserResourcesManager>();
        }

        protected override Task InitializeInternal()
        {
            viewModel.ItemCount
                     .Subscribe(itemCount => viewModel.CanBuy.Value = itemCount * ITEM_PRICE.UNDO <= userResourcesManager.CurrentCoins.Value)
                     .AddTo(presenterDisposables);

            viewModel.ResetValueCommand
                     .Subscribe(_ => ResetItemsCount())
                     .AddTo(presenterDisposables);

            viewModel.OnClickIncreaseCountButtonCommand
                     .Where(_ => viewModel.ItemCount.Value < 10)
                     .Subscribe(_ => viewModel.ItemCount.Value++)
                     .AddTo(presenterDisposables);

            viewModel.OnClickDecreaseCountButtonCommand
                     .Where(_ => viewModel.ItemCount.Value > 1)
                     .Subscribe(_ => viewModel.ItemCount.Value--)
                     .AddTo(presenterDisposables);

            viewModel.OnClickConfirmButtonCommand
                     .Subscribe(_ =>
                     {
                         userResourcesManager.BuyUndoItem(viewModel.ItemCount.Value);
                         transitionService.MakeTransition(PresentationTransitionID.PopOutModal);
                     })
                     .AddTo(presenterDisposables);

            return Task.CompletedTask;
        }        
        
        private void ResetItemsCount()
            => viewModel.ItemCount.OnNext(1);
    }
}