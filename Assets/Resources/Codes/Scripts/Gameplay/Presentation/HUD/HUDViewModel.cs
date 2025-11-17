using System;
using UnityEngine;
using R3;
using System.Collections.Generic;

namespace ColorSorting3D.Gameplay
{
    public class HUDViewModel : BaseViewModel
    {
        public ReactiveProperty<int> CurrentLevel { get; } = new();
        public ReactiveProperty<int> CurrentCoins { get; } = new();
        public ReactiveProperty<string> CurrentUndoItems { get; } = new();
        public ReactiveProperty<bool> CanUndo { get; } = new();
        public ReactiveCommand<Vector2> OnAddUnAvailableContainerCommand { get; } = new();
        public ReactiveCommand<Vector2> OnRemoveUnAvailableContainerCommand { get; } = new();
        public ReactiveCommand OnClickReloadButtonCommand { get; } = new();
        public ReactiveCommand OnClickSettingButtonCommand { get; } = new();
        public ReactiveCommand OnClickHomeButtonCommand { get; } = new();
        public ReactiveCommand OnClickUndoButtonCommand { get; } = new();
        public ReactiveCommand<Vector2> OnClickAddContainerButtonCommand { get; } = new();

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
            CurrentLevel?.Dispose();
            CurrentCoins?.Dispose();
            CurrentUndoItems?.Dispose();
            CanUndo?.Dispose();
            OnClickReloadButtonCommand?.Dispose();
            OnClickSettingButtonCommand?.Dispose();
            OnClickHomeButtonCommand?.Dispose();
            OnClickUndoButtonCommand?.Dispose();
            OnAddUnAvailableContainerCommand?.Dispose();
            OnRemoveUnAvailableContainerCommand?.Dispose();
            OnClickAddContainerButtonCommand?.Dispose();
        }
    }
}