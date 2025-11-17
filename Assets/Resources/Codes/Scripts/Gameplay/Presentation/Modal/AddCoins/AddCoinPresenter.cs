using System;
using UnityEngine;
using R3;
using System.Threading.Tasks;

namespace ColorSorting3D.Gameplay
{
    public class AddCoinsPresenter : ContentPresenter<AddCoinsViewModel>
    {
        protected override Task InitializeInternal()
        {
            return Task.CompletedTask;
        }
    }
}