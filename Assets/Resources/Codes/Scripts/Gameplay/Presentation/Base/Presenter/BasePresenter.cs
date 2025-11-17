using System;
using System.Threading.Tasks;
using R3;
using ServiceButcator;

namespace ColorSorting3D.Gameplay
{
    public abstract class BasePresenter<TViewModel> : IDisposable where TViewModel : BaseViewModel
    {        
        private bool isInitialized = false;
        private bool isDisposed = false;

        protected TViewModel viewModel = default;
        protected readonly CompositeDisposable presenterDisposables = new();

        // External Depedencies
        protected PresentationTransitionService transitionService;

        public async void Initialize(TViewModel viewModel)
        {
            if (isInitialized)
                return;

            isInitialized = true;
            this.viewModel = viewModel;

            LoadInternals();
            LoadDepedencies();

            await InitializeInternal();
        }

        protected abstract Task InitializeInternal();

        protected virtual void LoadDepedencies()
            => transitionService = ServiceLocators.ThisScene().Get<PresentationTransitionService>();
        
        protected virtual void LoadInternals(){}

        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;
            viewModel?.Dispose();
            presenterDisposables?.Dispose();
        }

        ~BasePresenter()
            => Dispose();
    }
}