using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using ColorSorting3D.Gameplay.DispatcherStates;
using ObservableCollections;
using R3;
using ServiceButcator;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public class RingsDispatcher : MonoBehaviour
    {
        // Runtime fields
        private bool isProcessing = false;
        private IDispatcherState currentState = default;

        // Internals
        private ContainerBuffer containersBuffer = default;
        private ObservableStack<RingsTransferCommand> ringsTransferHistory = new();
        private Dictionary<DispatcherState, IDispatcherState> dispathcherStates = default;

        // ExternalDepedencies
        private ScreenManager screenManager = default;
        private InputManager inputManager = default;

        // Properties
        public ReactiveProperty<bool> CanUndo { get; } = new(false);
        
        private void Awake()
        {
            InitCollections();
            LoadDepedencies();
            InitStateContext();

            void InitCollections()
            {
                containersBuffer = new();
                ringsTransferHistory = new();
            }

            void LoadDepedencies()
            {
                screenManager = ServiceLocators.SceneOf(this).Get<ScreenManager>();
                inputManager = ServiceLocators.SceneOf(this).Get<InputManager>();
            }

            void InitStateContext()
            {
                dispathcherStates = new()
                {
                    { DispatcherState.Normal, new NormalState(containersBuffer, ringsTransferHistory) },
                    { DispatcherState.Selecting, new SelectingState(containersBuffer, ringsTransferHistory) },
                };

                SetState(DispatcherState.Normal);
            }
        }

        public void InitReactiveProperties()
        {
            inputManager.OnTouchStream
                        .Subscribe(touchPos => ProcessContainerSelection(touchPos))
                        .AddTo(this);

            ringsTransferHistory.ObserveAdd()
                                .Where(_ => ringsTransferHistory.Count == 1)
                                .Subscribe(_ => CanUndo.Value = true)
                                .AddTo(this);

            ringsTransferHistory.ObserveRemove()
                                .Where(_ => ringsTransferHistory.Count == 0)
                                .Subscribe(_ => CanUndo.Value = false)
                                .AddTo(this);

            CanUndo.AddTo(this);
        }

        public void Clear()
        {
            CanUndo.Value = false;
            containersBuffer?.Clear();
            ringsTransferHistory?.Clear();
        }

       private void SetState(DispatcherState nextState)
        {
            if (nextState == DispatcherState.None)
                return;

            currentState = dispathcherStates[nextState];
        }

        private async void ProcessContainerSelection(Vector2 touchPosition)
        {
            if (isProcessing || !TryGetContainer(touchPosition, out var selectedContainer))
                return;

            isProcessing = true;

            var nextState = await currentState.HandleSelection(selectedContainer);
            SetState(nextState);

            isProcessing = false;

            bool TryGetContainer(Vector2 touchPosition, out RingsContainer selectedContainer)
            {
                selectedContainer = null;

                Ray directionOfTouch = screenManager.MainCamera.ScreenPointToRay(touchPosition);
                if (!Physics.Raycast(directionOfTouch.origin, directionOfTouch.direction, out RaycastHit hit, Mathf.Infinity, 1 << 8))
                    return false;

                selectedContainer = hit.transform.GetComponentInParent<RingsContainer>();
                return true;
            }
        }
    
        public async Task UndoTranfer()
        {
            while (isProcessing)
                await Task.Delay(200);

            isProcessing = true;

            var nextState = await currentState.HandleUndoTransfer();
            SetState(nextState);

            isProcessing = false;
            return;
        }
    }
}