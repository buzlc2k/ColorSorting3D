using System;
using UnityEngine;
using R3;
using System.Threading.Tasks;
using ServiceButcator;
using System.Collections.Generic;
using ObservableCollections;
using ColorSorting3D.Config;

namespace ColorSorting3D.Gameplay
{
    public class HUDPresenter : ModelPresenter<HUDViewModel>
    {
        // Internals
        private Dictionary<Vector2, RingsContainer> containersMap = new();
        private Dictionary<RingsContainer, Vector2> positionsMap = new();

        // Externals Depedencies
        private LevelManager levelManager = default;
        private UserResourcesManager userResourcesManager = default;
        private ScreenManager screenManager = default;

        protected override void SetID()
            => id = PresentationID.HUD;

        protected override void LoadDepedencies()
        {
            base.LoadDepedencies();
            levelManager = ServiceLocators.ThisScene().Get<LevelManager>();
            userResourcesManager = ServiceLocators.ThisScene().Get<UserResourcesManager>();
            screenManager = ServiceLocators.ThisScene().Get<ScreenManager>();
        }
        
        protected override Task InitializeInternal()
        {
            base.InitializeInternal();

            levelManager.CurrentLevel
                        .Subscribe(x => viewModel.CurrentLevel.Value = x)
                        .AddTo(presenterDisposables);

            levelManager.RingsDispatcher.CanUndo
                                        .Subscribe(canUndo => viewModel.CanUndo.Value = canUndo)
                                        .AddTo(presenterDisposables);
            
            levelManager.UnAvailableContainers
                        .ObserveAdd()
                        .Subscribe(container => {
                            var sceenPos = AddNewElementInMap(container.Value);
                            viewModel.OnAddUnAvailableContainerCommand.Execute(sceenPos);
                        })
                        .AddTo(presenterDisposables);
            
            levelManager.UnAvailableContainers
                        .ObserveRemove()
                        .Subscribe(containerArgs => 
                        {
                            viewModel.OnRemoveUnAvailableContainerCommand.Execute(positionsMap[containerArgs.Value]);
                            RemoveElementInMap(containerArgs.Value);
                        })
                        .AddTo(presenterDisposables);

            levelManager.UnAvailableContainers
                        .ObserveClear()
                        .Subscribe(_ => { containersMap.Clear() ; positionsMap.Clear() ; })
                        .AddTo(presenterDisposables);

            userResourcesManager.CurrentCoins
                                .Subscribe(currentCoins => viewModel.CurrentCoins.Value = currentCoins)
                                .AddTo(presenterDisposables);

            userResourcesManager.CurrentUndoItems
                                .Subscribe(currentUndoItems => viewModel.CurrentUndoItems.Value = currentUndoItems == 0 ? "+" : $"{currentUndoItems}")
                                .AddTo(presenterDisposables);
            
            viewModel.OnClickReloadButtonCommand
                     .Subscribe(_ => {
                         transitionService.MakeTransition(PresentationTransitionID.ReloadHUD);
                         levelManager.LoadLevel();
                        })
                     .AddTo(presenterDisposables);

            viewModel.OnClickSettingButtonCommand
                    .Subscribe(_ => transitionService.MakeTransition(PresentationTransitionID.PopUpModal, ModalContentID.Setting))
                    .AddTo(presenterDisposables);

            viewModel.OnClickHomeButtonCommand
                    .Subscribe(_ => transitionService.MakeTransition(PresentationTransitionID.HUDToHome))
                    .AddTo(presenterDisposables);
            
            viewModel.OnClickUndoButtonCommand
                     .Subscribe(async _ =>
                     {
                        if (userResourcesManager.CurrentUndoItems.Value == 0)
                            transitionService.MakeTransition(PresentationTransitionID.PopUpModal, ModalContentID.ConfirmPurchaseUndo);
                        else
                        {
                            userResourcesManager.RemoveUndoItem(1);
                            await levelManager.RingsDispatcher.UndoTranfer();
                        }
                     })
                     .AddTo(presenterDisposables);
            
            viewModel.OnClickAddContainerButtonCommand
                     .Subscribe(position => transitionService.MakeTransition(
                                                            PresentationTransitionID.PopUpModal, 
                                                            ModalContentID.ConfirmPurchaseContainer, 
                                                            containersMap[position]))
                     .AddTo(presenterDisposables);

            return Task.CompletedTask;
        }

        private Vector2 AddNewElementInMap(RingsContainer container)
        {            
            var screenPos = GetScreenPosition();

            containersMap[screenPos] = container;
            positionsMap[container] = screenPos;

            return screenPos;

            Vector2 GetScreenPosition()
            {
                var worldPos = container.GetEdgeRingPos() + Vector3.right * 0.1f;
                var screenPos = RectTransformUtility.WorldToScreenPoint(screenManager.MainCamera, worldPos);
                return screenPos;
            }
        }

        private void RemoveElementInMap(RingsContainer container)
        {
            var position = positionsMap[container];

            containersMap.Remove(position);
            positionsMap.Remove(container);
        }
    }
}