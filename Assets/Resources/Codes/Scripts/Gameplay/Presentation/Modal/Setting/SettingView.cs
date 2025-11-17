using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using R3;
using System.Collections;
using System;

namespace ColorSorting3D.Gameplay
{
    [Serializable]
    public class ToggleSprites
    {
        public Sprite On;
        public Sprite Off;
    }

    [Serializable]
    public class Toggle
    {
        [SerializeField] private ToggleSprites sprites;

        public bool Enable;
        public Button Button;
        public Image Image;

        public void SetEnable(bool enable)
        {
            Enable = enable;
            Image.sprite = Enable ? sprites.On : sprites.Off;
        }

        public void Click()
        {
            Enable = !Enable;
            Image.sprite = Enable ? sprites.On : sprites.Off;
        }
    }

    public class SettingView : BaseView<SettingViewModel>
    {
        [SerializeField] private Toggle sfxToggle;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle vibrateToggle;

        protected override Task InitializeInternal(SettingViewModel viewModel)
        {
            base.InitializeInternal(viewModel);

            sfxToggle.Button.OnClickAsObservable()
                            .Subscribe(_ => sfxToggle.Click())
                            .AddTo(viewDisposables);
            
            musicToggle.Button.OnClickAsObservable()
                            .Subscribe(_ => musicToggle.Click())
                            .AddTo(viewDisposables);
            
            vibrateToggle.Button.OnClickAsObservable()
                            .Subscribe(_ => vibrateToggle.Click())
                            .AddTo(viewDisposables);

            return Task.CompletedTask;
        }
    }
}