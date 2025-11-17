using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using R3;
using TMPro;
using System;
using R3.Triggers;

namespace ColorSorting3D.Gameplay
{
    [Serializable]
    public class ConfirmButton
    {
        public TextMeshProUGUI Text;
        public Button Button;
    }
    
    public class ConfirmPurchaseUndoView : BaseView<ConfirmPurchaseUndoViewModel>
    {
        [SerializeField] private Button increseButton;
        [SerializeField] private Button decreaseButton;
        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private ConfirmButton confirmButton;

        protected override Task InitializeInternal(ConfirmPurchaseUndoViewModel viewModel)
        {
            increseButton.OnClickAsObservable()
                         .Subscribe(_ => viewModel.OnClickIncreaseCountButtonCommand.Execute(Unit.Default))
                         .AddTo(viewDisposables);

            decreaseButton.OnClickAsObservable()
                         .Subscribe(_ => viewModel.OnClickDecreaseCountButtonCommand.Execute(Unit.Default))
                         .AddTo(viewDisposables);

            confirmButton.Button.OnClickAsObservable()
                                .Subscribe(_ => viewModel.OnClickConfirmButtonCommand.Execute(Unit.Default))
                                .AddTo(viewDisposables);

            gameObject.OnEnableAsObservable()
                      .Subscribe(_ => viewModel.ResetValueCommand.Execute(Unit.Default))
                      .AddTo(viewDisposables);

            viewModel.ItemCount.Subscribe(itemCount =>
                                        {
                                            countText.text = $"+ {itemCount}";
                                            confirmButton.Text.text = $"{itemCount * ITEM_PRICE.UNDO}";
                                        })
                                        .AddTo(viewDisposables);

            viewModel.CanBuy
                     .Subscribe(canBuy => confirmButton.Button.interactable = canBuy)
                     .AddTo(viewDisposables);

            return Task.CompletedTask;
        }
    }
}