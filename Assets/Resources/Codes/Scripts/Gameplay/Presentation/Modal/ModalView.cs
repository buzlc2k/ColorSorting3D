using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using R3;
using System.Collections;
using R3.Triggers;
using System.Linq;

namespace ColorSorting3D.Gameplay
{
    public class ModalView : BaseView<ModalViewModel>
    {
        [SerializeField] private SettingView settingView;
        [SerializeField] private RemoveAdsView removeAdsView;
        [SerializeField] private AddCoinsView addCoinsView;
        [SerializeField] private ConfirmPurchaseUndoView confirmPurchaseUndoView;
        [SerializeField] private ConfirmPurchaseContainerView confirmPurchaseContainerView;
        [SerializeField] private Button closeButton;

        protected override Task InitializeInternal(ModalViewModel viewModel)
        {
            base.InitializeInternal(viewModel);

            settingView.Initialize(viewModel.SettingViewModel);
            removeAdsView.Initialize(viewModel.RemoveAdsViewModel);
            addCoinsView.Initialize(viewModel.AddCoinsViewModel);
            confirmPurchaseUndoView.Initialize(viewModel.ConfirmPurchaseUndoViewModel);
            confirmPurchaseContainerView.Initialize(viewModel.ConfirmPurchaseContainerViewModel);

            closeButton.OnClickAsObservable()
                        .Subscribe(x => viewModel.OnClickCloseButtonCommand.Execute(Unit.Default))
                        .AddTo(viewDisposables);

            return Task.CompletedTask;
        }

        protected override async Task OnDefautEnter(TransitionData transitionData)
        {
            if (transitionData.OtherData.Count() < 1 
                || transitionData.OtherData[0] is not ModalContentID modalContentID)
                return;     

            await base.OnDefautEnter(transitionData);
            ShowContent(modalContentID);
        }

        protected override async Task OnDefautExit(TransitionData transitionData)
        {
            await base.OnDefautExit(transitionData);
            TurnOffContents();
        }
        
        private void ShowContent(ModalContentID contentID)
        {
            switch (contentID)
            {
                case ModalContentID.Setting:
                    settingView.gameObject.SetActive(true);
                    break;
                case ModalContentID.RemoveAds:
                    removeAdsView.gameObject.SetActive(true);
                    break;
                case ModalContentID.AddCoins:
                    addCoinsView.gameObject.SetActive(true);
                    break;
                case ModalContentID.ConfirmPurchaseUndo:
                    confirmPurchaseUndoView.gameObject.SetActive(true);
                    break;
                case ModalContentID.ConfirmPurchaseContainer:
                    confirmPurchaseContainerView.gameObject.SetActive(true);
                    break;
            }
        }

        private void TurnOffContents()
        {
            settingView.gameObject.SetActive(false);
            removeAdsView.gameObject.SetActive(false);
            addCoinsView.gameObject.SetActive(false);
            confirmPurchaseUndoView.gameObject.SetActive(false);
            confirmPurchaseContainerView.gameObject.SetActive(false);
        }
    }
}