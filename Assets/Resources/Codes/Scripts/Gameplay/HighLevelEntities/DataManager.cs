using System.Collections.Generic;
using System.Threading.Tasks;
using ColorSorting3D.Data;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public class DataManager : HighLevelEntity
    {
        // Runtime Internals
        private DataModelManager dataModelManager = default;

        private void Awake()
        {
            LoadInternals();

            void LoadInternals()
            {
                dataModelManager = new();
            }
        }

        public override async Task Init() // Nothing
            => await Task.Delay(2000);

        public int GetCurrentLevel()
            => dataModelManager.GetDataModel<ProgressModel>(DataModelID.Progress)
                               .Read(progessModel => progessModel.CurrentLevel);

        public void SetCurrentLevel(int currentLevel)
            => dataModelManager.GetDataModel<ProgressModel>(DataModelID.Progress)
                                .Write(progessModel => progessModel.CurrentLevel, currentLevel);

        public int GetCurrentCoins()
            => dataModelManager.GetDataModel<ResourcesModel>(DataModelID.Recources)
                               .Read(resourcesModel => resourcesModel.CurrentCoins);

        public void SetCurrentCoins(int currentCoins)
            => dataModelManager.GetDataModel<ResourcesModel>(DataModelID.Recources)
                               .Write(resourcesModel => resourcesModel.CurrentCoins, currentCoins);

        public int GetCurrentUndoItems()
            => dataModelManager.GetDataModel<ResourcesModel>(DataModelID.Recources)
                               .Read(resourcesModel => resourcesModel.CurrentUndoItems);
        
        public void SetCurrentUndoItems(int currentUndoItems)
            => dataModelManager.GetDataModel<ResourcesModel>(DataModelID.Recources)
                               .Write(resourcesModel => resourcesModel.CurrentUndoItems, currentUndoItems);
    }
}