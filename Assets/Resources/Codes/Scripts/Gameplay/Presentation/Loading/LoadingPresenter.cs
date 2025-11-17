using System;
using UnityEngine;
using ColorSorting3D.Config;

namespace ColorSorting3D.Gameplay
{
    public class LoadingPresenter : ModelPresenter<LoadingViewModel>
    {
        protected override void SetID()
            => id = PresentationID.Loading;
    }
}