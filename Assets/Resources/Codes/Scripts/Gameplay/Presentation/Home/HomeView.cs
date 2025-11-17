using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using R3;
using TMPro;

namespace ColorSorting3D.Gameplay
{
    public class HomeView : BaseView<HomeViewModel>
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button removeAdsButton;
        [SerializeField] private Button addCoinsButton;

        protected override Task InitializeInternal(HomeViewModel viewModel)
        {
            base.InitializeInternal(viewModel);

            viewModel.CurrentLevel
                     .Subscribe(currentLevel => levelText.text = $"Level: {currentLevel}")
                     .AddTo(viewDisposables);
            
            viewModel.CurrentCoins
                     .Subscribe(currentCoins => coinText.text = $"{currentCoins}")
                     .AddTo(viewDisposables);

            playButton.OnClickAsObservable()
                        .Subscribe(_ => viewModel.OnClickPlayButtonCommand.Execute(Unit.Default))
                        .AddTo(viewDisposables);

            settingButton.OnClickAsObservable()
                        .Subscribe(_ => viewModel.OnClickSettingButtonCommand.Execute(Unit.Default))
                        .AddTo(viewDisposables);

            removeAdsButton.OnClickAsObservable()
                        .Subscribe(_ => viewModel.OnClickRemoveAdsButtonCommand.Execute(Unit.Default))
                        .AddTo(viewDisposables);
                        
            addCoinsButton.OnClickAsObservable()
                        .Subscribe(_ => viewModel.OnClickAddCoinsCommand.Execute(Unit.Default))
                        .AddTo(viewDisposables);

            return Task.CompletedTask;
        }
    }
}