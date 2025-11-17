using System.Threading.Tasks;
using UnityEngine;
using R3;
using System.Collections;
using R3.Triggers;

namespace ColorSorting3D.Gameplay
{
    public abstract class BaseView<TViewModel> : MonoBehaviour where TViewModel : BaseViewModel
    {
        private bool isInitialized = default;
        [SerializeField] private Animator animator;

        protected readonly CompositeDisposable viewDisposables = new();

        public async void Initialize(TViewModel viewModel)
        {
            if (isInitialized)
                return;

            isInitialized = true;
            await InitializeInternal(viewModel);
        }

        protected virtual Task InitializeInternal(TViewModel viewModel)
        {
            viewModel.OnEnterCommand
                         .Subscribe(transitionData => _ = OnDefautEnter(transitionData))
                         .AddTo(viewDisposables);

            viewModel.OnExitCommand
                         .Subscribe(transitionData => _ = OnDefautExit(transitionData))
                         .AddTo(viewDisposables);

            return Task.CompletedTask;
        }

        protected virtual async Task OnDefautEnter(TransitionData transitionData)
        {
            await Task.Delay(transitionData.Duration);

            gameObject.SetActive(true);
            PlayAnim(transitionData.Animation);
        }

        protected virtual async Task OnDefautExit(TransitionData transitionData)
        {
            PlayAnim(transitionData.Animation);
            await Task.Delay(transitionData.Duration);

            gameObject.SetActive(false);
        }
        
        protected virtual void PlayAnim(string animation)
        {
            if (animator == null || string.IsNullOrEmpty(animation))
                return;
                
            animator.Play(animation);
        }
    }
}