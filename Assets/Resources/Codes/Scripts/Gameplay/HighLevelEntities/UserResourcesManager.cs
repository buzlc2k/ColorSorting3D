using System;
using System.Threading.Tasks;
using R3;
using ServiceButcator;
using Unity.Mathematics;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public class UserResourcesManager : HighLevelEntity
    {
        public ReactiveProperty<int> CurrentCoins { get; } = new();
        public ReactiveProperty<int> CurrentUndoItems { get; } = new();

        // External Depedencies
        private DataManager dataManager;

        private void Awake()
        {
            LoadDepedencies();

            void LoadDepedencies()
            {
                dataManager = ServiceLocators.SceneOf(this).Get<DataManager>();
            }
        }

        public override Task Init()
        {
            InitReactiveProperties();

            return Task.CompletedTask;

            void InitReactiveProperties()
            {
                CurrentCoins.Value = dataManager.GetCurrentCoins();
                CurrentCoins.Skip(1)
                            .Subscribe(currentCoins => dataManager.SetCurrentCoins(currentCoins));

                CurrentUndoItems.Value = dataManager.GetCurrentUndoItems();
                CurrentUndoItems.Skip(1)
                                .Subscribe(currentUndoItems => dataManager.SetCurrentUndoItems(currentUndoItems));

                CurrentCoins.AddTo(this);
                CurrentUndoItems.AddTo(this);
            }
        }

        public void AddCoins(int numsAdded)
            => CurrentCoins.Value += numsAdded;

        public void RemoveCoins(int numsRemoved)
            => CurrentCoins.Value -= numsRemoved;

        public void AddUndoItem(int numsAdded)
            => CurrentUndoItems.Value += numsAdded;

        public void RemoveUndoItem(int numsRemoved)
            => CurrentUndoItems.Value -= numsRemoved;

        public void BuyUndoItem(int count)
        {
            RemoveCoins(count * ITEM_PRICE.UNDO);
            AddUndoItem(count);
        }
    }
}