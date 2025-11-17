using System;
using UnityEngine;
using R3;

namespace ColorSorting3D.Gameplay
{
    public class ModalViewModel : BaseViewModel
    {
        public SettingViewModel SettingViewModel { get; } = new();
        public RemoveAdsViewModel RemoveAdsViewModel { get; } = new();
        public AddCoinsViewModel AddCoinsViewModel { get; } = new();
        public ConfirmPurchaseUndoViewModel ConfirmPurchaseUndoViewModel { get; } = new();
        public ConfirmPurchaseContainerViewModel ConfirmPurchaseContainerViewModel { get; } = new();
        
        public ReactiveCommand OnClickCloseButtonCommand { get; } = new();

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
            OnClickCloseButtonCommand?.Dispose();
        }
    }
}