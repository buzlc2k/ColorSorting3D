using System.Collections.Generic;
using System.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ColorSorting3D.Gameplay
{
    public class InputManager : HighLevelEntity
    {
        private InputAction touchPosition = default;
        private InputAction touchAction = default;
        private InputSystem_Actions inputAction = default;
        
        public Observable<Vector2> OnTouchStream { get; private set; }

        private void Awake()
        {
            LoadInputActions();
            DefineObservable();

            void LoadInputActions()
            {
                inputAction = new();
                inputAction.Enable();

                touchPosition = inputAction.Player.TouchPosition;
                touchAction = inputAction.Player.TouchAction;
            }

            void DefineObservable()
            {
                OnTouchStream = Observable.FromEvent<InputAction.CallbackContext>(
                    h => touchAction.performed += h,
                    h => touchAction.performed -= h
                )
                .ObserveOn(UnityFrameProvider.PostLateUpdate)
                .Where(_ => !EventSystem.current.IsPointerOverGameObject())
                .Select(_ => touchPosition.ReadValue<Vector2>());
            }
        }

        public override Task Init() // Nothing
            => Task.CompletedTask;
    }
}