using System;
using UnityEngine;
using R3;
using UnityEngine.UI;
using TMPro;

namespace ColorSorting3D.Gameplay
{
    public class ConfirmPurchaseContainerViewModel : BaseViewModel
    {
        public ReactiveProperty<bool> CanBuy { get; } = new(true);
        public ReactiveCommand OnClickConfirmButtonCommand { get; } = new();

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
            CanBuy?.Dispose();
            OnClickConfirmButtonCommand?.Dispose();
        }
    }
}