using System;
using System.Threading.Tasks;
using R3;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public abstract class ContentPresenter<TViewModel> : BasePresenter<TViewModel> where TViewModel : BaseViewModel {}
}