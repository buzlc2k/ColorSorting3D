using System;
using UnityEngine;
using R3;

namespace ColorSorting3D.Gameplay
{
    public class CompleteLevelViewModel : BaseViewModel
    {
        public ReactiveCommand OnClickNextLevelButtonCommand { get; } = new();
        public ReactiveCommand OnClickHomeButtonCommand { get; } = new();

        protected override void DisposeInternal()
        {
            base.DisposeInternal();

            OnClickNextLevelButtonCommand?.Dispose();
            OnClickHomeButtonCommand?.Dispose();
        }
    }
}