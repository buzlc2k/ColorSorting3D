using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using R3;
using TMPro;
using ColorSorting3D.Util;
using R3.Triggers;
using System;

namespace ColorSorting3D.Gameplay
{
    [Serializable]
    public class CoinsCollectedLabel
    {
        public RectTransform Wrapper;
        public Animator Animator;
        public TextMeshProUGUI Text;
    }

    public class CompleteLevelView : BaseView<CompleteLevelViewModel>
    {
        [SerializeField] private Button watchAdsButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private CoinsCollectedLabel coinsCollectedLabel;
        protected override Task InitializeInternal(CompleteLevelViewModel viewModel)
        {
            base.InitializeInternal(viewModel);

            nextLevelButton.OnClickAsObservable()
                        .Subscribe(async x =>
                        {
                            await PopUpCoinsCollectedLabel(nextLevelButton.transform as RectTransform, NUM_COINS_ADDED.PASSED_LEVEL);
                            viewModel.OnClickNextLevelButtonCommand.Execute(Unit.Default);
                        })
                        .AddTo(viewDisposables);

            homeButton.OnClickAsObservable()
                        .Subscribe(x => viewModel.OnClickHomeButtonCommand.Execute(Unit.Default))
                        .AddTo(viewDisposables);

            return Task.CompletedTask;
        }
        
        public async Task PopUpCoinsCollectedLabel(RectTransform rect, int coinsNums)
        {
            while (coinsCollectedLabel.Animator.GetCurrentAnimatorStateInfo(0).IsName("PopUp"))
                await Task.Delay(200);

            coinsCollectedLabel.Text.text = coinsNums.ToString();
            
            SetPosition();

            await PopUp();

            void SetPosition()
            {
                var pos = rect.GetTopRightPos();
                pos.x += coinsCollectedLabel.Wrapper.rect.width / 2;
                coinsCollectedLabel.Wrapper.position = pos;
            }

            async Task PopUp()
            {
                coinsCollectedLabel.Animator.Play("PopUp");
                await Task.Delay(380);
            }
        }
    }
}