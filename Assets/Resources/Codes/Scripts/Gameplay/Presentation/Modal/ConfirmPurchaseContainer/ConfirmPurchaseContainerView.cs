using System.Threading.Tasks;
using UnityEngine;
using R3;
using UnityEngine.UI;

namespace ColorSorting3D.Gameplay
{    
    public class ConfirmPurchaseContainerView : BaseView<ConfirmPurchaseContainerViewModel>
    {
        [SerializeField] private Button confirmButton;

        protected override Task InitializeInternal(ConfirmPurchaseContainerViewModel viewModel)
        {
            confirmButton.OnClickAsObservable()
                                .Subscribe(_ => viewModel.OnClickConfirmButtonCommand.Execute(Unit.Default))
                                .AddTo(viewDisposables);

            viewModel.CanBuy
                     .Subscribe(canBuy => confirmButton.interactable = canBuy)
                     .AddTo(viewDisposables);

            return Task.CompletedTask;
        }
    }
}