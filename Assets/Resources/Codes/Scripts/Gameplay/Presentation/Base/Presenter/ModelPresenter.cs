using System;
using System.Threading.Tasks;
using ColorSorting3D.Config;
using R3;

namespace ColorSorting3D.Gameplay
{
    public abstract class ModelPresenter<TViewModel> : BasePresenter<TViewModel> where TViewModel : BaseViewModel
    {
        protected PresentationID id = default;

        protected override Task InitializeInternal()
        {
            SetID();

            transitionService.TurnOnCommand
                             .Subscribe(param =>
                             {
                                if (param.id == id)
                                OnEnterState(param.data);
                             })
                             .AddTo(presenterDisposables);
                             
            transitionService.TurnOffCommand
                             .Subscribe(param =>
                             {
                                if (param.id == id) 
                                OnExitState(param.data);  
                             })
                             .AddTo(presenterDisposables);
            
            return Task.CompletedTask;
        }

        protected abstract void SetID();

        protected virtual void OnEnterState(TransitionData transitionData)
            => viewModel.OnEnterCommand.Execute(transitionData);
        
        protected virtual void OnExitState(TransitionData transitionData)
            => viewModel.OnExitCommand.Execute(transitionData);
    }
}