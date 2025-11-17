using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using R3;
using System.Collections;
using TMPro;
using System;
using System.Collections.Generic;

namespace ColorSorting3D.Gameplay
{
    [Serializable]
    public class UndoButton
    {
        public Button Button;
        public TextMeshProUGUI RemainCountText;
    }

    [Serializable]
    public class AddContainerButton
    {
        public RectTransform Wrapper;
        public Button Button;
    }

    public class HUDView : BaseView<HUDViewModel>
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private Button reloadButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private UndoButton undoButton;
        [SerializeField] private List<AddContainerButton> addContainerButtons;

        protected override Task InitializeInternal(HUDViewModel viewModel)
        {
            base.InitializeInternal(viewModel);

            viewModel.CurrentLevel
                     .Subscribe(currentLevel => levelText.text = $"Level: {currentLevel}")
                     .AddTo(viewDisposables);

            viewModel.CurrentCoins
                     .Subscribe(currentCoins => coinText.text = $"{currentCoins}")
                     .AddTo(viewDisposables);

            viewModel.CurrentUndoItems
                     .Subscribe(currentUndoItems => undoButton.RemainCountText.text = currentUndoItems)
                     .AddTo(viewDisposables);
                     
            viewModel.CanUndo
                     .Subscribe(canUndo => undoButton.Button.interactable = canUndo)
                     .AddTo(viewDisposables);
            
            viewModel.OnAddUnAvailableContainerCommand
                     .Subscribe(position => TurnOnAddContainerButton(position))
                     .AddTo(this);
            
            viewModel.OnRemoveUnAvailableContainerCommand
                     .Subscribe(position => TurnOffAddContainerButton(position))
                     .AddTo(this);

            reloadButton.OnClickAsObservable()
                        .Subscribe(_ => viewModel.OnClickReloadButtonCommand.Execute(Unit.Default))
                        .AddTo(viewDisposables);

            settingButton.OnClickAsObservable()
                        .Subscribe(_ => viewModel.OnClickSettingButtonCommand.Execute(Unit.Default))
                        .AddTo(viewDisposables);

            homeButton.OnClickAsObservable()
                     .Subscribe(_ => viewModel.OnClickHomeButtonCommand.Execute(Unit.Default))
                     .AddTo(viewDisposables);    
            
            undoButton.Button.OnClickAsObservable()
                             .Subscribe(_ => viewModel.OnClickUndoButtonCommand.Execute(Unit.Default))
                             .AddTo(viewDisposables);

            foreach(var addContainerButton in addContainerButtons)
            addContainerButton.Button.OnClickAsObservable()
                                     .Subscribe(_ => viewModel.OnClickAddContainerButtonCommand.Execute(addContainerButton.Wrapper.position))
                                     .AddTo(viewDisposables);
            
            return Task.CompletedTask;
        }

        protected override async Task OnDefautExit(TransitionData transitionData)
        {
            await base.OnDefautExit(transitionData);
            TurnOffAllAddContainerButtons();
        }

        private void TurnOnAddContainerButton(Vector2 position)
        {
            for (int i = 0; i < addContainerButtons.Count; i++)
            {
                if (addContainerButtons[i].Wrapper.gameObject.activeSelf)
                    continue;
                    
                addContainerButtons[i].Wrapper.position = position;
                addContainerButtons[i].Wrapper.gameObject.SetActive(true);
                return;
            }
        }

        private void TurnOffAddContainerButton(Vector2 position)
        {
            for (int i = 0; i < addContainerButtons.Count; i++)
            {
                if ((Vector2)addContainerButtons[i].Wrapper.position != position)
                    continue;

                addContainerButtons[i].Wrapper.gameObject.SetActive(false);
                return;
            }
        }

        private void TurnOffAllAddContainerButtons()
        {
            foreach(var button in addContainerButtons)
            button.Wrapper.gameObject.SetActive(false);
        }
    }
}