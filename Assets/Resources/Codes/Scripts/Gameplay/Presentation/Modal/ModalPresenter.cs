using System;
using UnityEngine;
using R3;
using System.Threading.Tasks;
using System.Linq;
using ColorSorting3D.Config;

namespace ColorSorting3D.Gameplay
{
    public class ModalPresenter : ModelPresenter<ModalViewModel>
    {
        // Internals
        private SettingPresenter settingPresenter = new();
        private RemoveAdsPresenter removeAdsPresenter = new();
        private AddCoinsPresenter addCoinsPresenter = new();
        private ConfirmPurchaseUndoPresenter confirmPurchaseUndoPresenter = new();
        private ConfirmPurchaseContainerPresenter confirmPurchaseContainerPresenter = new();

        protected override void LoadInternals()
        {
            base.LoadInternals();
            settingPresenter.Initialize(viewModel.SettingViewModel);
            removeAdsPresenter.Initialize(viewModel.RemoveAdsViewModel);
            addCoinsPresenter.Initialize(viewModel.AddCoinsViewModel);
            confirmPurchaseUndoPresenter.Initialize(viewModel.ConfirmPurchaseUndoViewModel);
            confirmPurchaseContainerPresenter.Initialize(viewModel.ConfirmPurchaseContainerViewModel);
        }

        protected override void SetID()
            => id = PresentationID.Modal;

        protected override Task InitializeInternal()
        {
            base.InitializeInternal();

            viewModel.OnClickCloseButtonCommand.Subscribe(_ => transitionService.MakeTransition(PresentationTransitionID.PopOutModal))
                                               .AddTo(presenterDisposables);

            return Task.CompletedTask;
        }

        protected override void OnEnterState(TransitionData transitionData)
        {
            if(transitionData.OtherData.Count() >= 2 
            && transitionData.OtherData[1] is RingsContainer selectedContainer)
                confirmPurchaseContainerPresenter.SetSelectedContainer(selectedContainer);

            base.OnEnterState(transitionData);
        }
    }
}