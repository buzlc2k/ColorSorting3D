using System;
using UnityEngine;
using R3;
using UnityEngine.UI;
using TMPro;

namespace ColorSorting3D.Gameplay
{
    public class ConfirmPurchaseUndoViewModel : BaseViewModel
    {
        public ReactiveProperty<int> ItemCount { get; } = new(1);
        public ReactiveProperty<bool> CanBuy { get; } = new(true);
        public ReactiveCommand ResetValueCommand { get; } = new();
        public ReactiveCommand OnClickIncreaseCountButtonCommand { get; } = new();
        public ReactiveCommand OnClickDecreaseCountButtonCommand { get; } = new();
        public ReactiveCommand OnClickConfirmButtonCommand { get; } = new();

        protected override void DisposeInternal()
        {
            base.DisposeInternal();

            ItemCount?.Dispose();
            CanBuy?.Dispose();

            ResetValueCommand?.Dispose();
            OnClickIncreaseCountButtonCommand?.Dispose();
            OnClickDecreaseCountButtonCommand?.Dispose();
            OnClickConfirmButtonCommand?.Dispose();
        }
    }
}