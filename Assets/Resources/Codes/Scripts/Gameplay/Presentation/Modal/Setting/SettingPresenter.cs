using System;
using UnityEngine;
using R3;
using System.Threading.Tasks;

namespace ColorSorting3D.Gameplay
{
    public class SettingPresenter : ContentPresenter<SettingViewModel>
    {
        protected override Task InitializeInternal()
        {
            return Task.CompletedTask;
        }
    }
}