using System;
using UnityEngine;
using R3;
using System.Threading.Tasks;
using ServiceButcator;
using ColorSorting3D.Config;

namespace ColorSorting3D.Gameplay
{
    public class CompleteLevelPresenter : ModelPresenter<CompleteLevelViewModel>
    {
        private LevelManager levelManager = default;
        private UserResourcesManager userResourcesManager = default;

        protected override void SetID()
            => id = PresentationID.CompleteLevel;

        protected override void LoadDepedencies()
        {
            base.LoadDepedencies();
            levelManager = ServiceLocators.ThisScene().Get<LevelManager>();
            userResourcesManager = ServiceLocators.ThisScene().Get<UserResourcesManager>();
        }

        protected override Task InitializeInternal()
        {
            base.InitializeInternal();

            viewModel.OnClickNextLevelButtonCommand.Subscribe(_ =>
                                                    {
                                                        levelManager.LoadNextLevel();
                                                        userResourcesManager.AddCoins(NUM_COINS_ADDED.PASSED_LEVEL);
                                                        transitionService.MakeTransition(PresentationTransitionID.CompleteLevelToHUD);
                                                    })
                                                   .AddTo(presenterDisposables);
                                                   
            viewModel.OnClickHomeButtonCommand.Subscribe(_ => transitionService.MakeTransition(PresentationTransitionID.CompleteLevelToHome))
                                                   .AddTo(presenterDisposables);

            return Task.CompletedTask;
        }
    }
}