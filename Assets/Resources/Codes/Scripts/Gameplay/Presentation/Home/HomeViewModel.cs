using System;
using UnityEngine;
using R3;

namespace ColorSorting3D.Gameplay
{
    public class HomeViewModel : BaseViewModel
    {
        public ReactiveProperty<int> CurrentLevel { get; } = new();
        public ReactiveProperty<int> CurrentCoins { get; } = new();
        public ReactiveCommand OnClickPlayButtonCommand { get; } = new();
        public ReactiveCommand OnClickSettingButtonCommand { get; } = new();
        public ReactiveCommand OnClickRemoveAdsButtonCommand { get; } = new();
        public ReactiveCommand OnClickAddCoinsCommand { get; } = new();

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
            CurrentLevel?.Dispose();
            CurrentCoins?.Dispose();
            OnClickPlayButtonCommand?.Dispose();
            OnClickSettingButtonCommand?.Dispose();
            OnClickRemoveAdsButtonCommand?.Dispose();
            OnClickAddCoinsCommand?.Dispose();
        }
    }
}