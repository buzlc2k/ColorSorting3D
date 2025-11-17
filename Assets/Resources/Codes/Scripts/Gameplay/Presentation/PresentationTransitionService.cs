using System;
using UnityEngine;
using R3;
using ColorSorting3D.Config;
using ServiceButcator;
using System.Threading.Tasks;

namespace ColorSorting3D.Gameplay
{
    public struct TransitionData
    {
        public int Duration;
        public string Animation;
        public object[] OtherData;
    }

    public class PresentationTransitionService
    {
        public ReactiveCommand<(PresentationID id, TransitionData data)> TurnOnCommand { get; } = new();
        public ReactiveCommand<(PresentationID id, TransitionData data)> TurnOffCommand { get; } = new();

        // External Depedencies 
        private PresentationTransitionConfig transitionConfig;

        public PresentationTransitionService()
            => transitionConfig = ServiceLocators.ThisScene().Get<PresentationTransitionConfig>();

        public async void MakeTransition(PresentationTransitionID id, params object[] datas)
        {
            if (!TryGetTransition(out var transitionRecord))
                return;
            
            foreach (var step in transitionRecord.TransitionSequence)
            {
                var transitionData = new TransitionData()
                {
                    Duration = step.Duration,
                    Animation = step.Animation,
                    OtherData = datas,
                };

                if (step.Enable)
                    TurnOnCommand.Execute((step.ID, transitionData));
                else
                    TurnOffCommand.Execute((step.ID, transitionData));

                await Task.Delay(step.Duration);               
            }

            bool TryGetTransition(out PresentationTransitionRecord transitionRecord)
            {
                transitionRecord = transitionConfig.Get(id);
                return transitionRecord != null;
            }
        }

        private void Dispose()
        {
            TurnOnCommand?.Dispose();
            TurnOffCommand?.Dispose();
        }

        ~PresentationTransitionService()
            => Dispose();
    }
}