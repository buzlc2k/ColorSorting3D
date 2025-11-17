using System;
using UnityEngine;
using R3;

namespace ColorSorting3D.Gameplay
{
    public abstract class BaseViewModel : IDisposable 
    {
        private bool isDisposed = false;
        public ReactiveCommand<TransitionData> OnEnterCommand { get; } = new();
        public ReactiveCommand<TransitionData> OnExitCommand { get; } = new();

        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;
            DisposeInternal();
        }

        protected virtual void DisposeInternal()
        {
            OnEnterCommand?.Dispose();
            OnExitCommand?.Dispose();
        }
    }
}