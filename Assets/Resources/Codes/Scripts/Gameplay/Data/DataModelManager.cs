using System.Collections.Generic;
using ColorSorting3D.Gameplay;
using UnityEngine;

namespace ColorSorting3D.Data
{
    public class DataModelManager
    {
        private Dictionary<DataModelID, IDataModel> dataModels;

        public DataModelManager()
        {
            InitDataModels();

            void InitDataModels()
            {
                dataModels = new()
                {
                    { DataModelID.Progress, new ProgressModel() },
                    {DataModelID.Recources, new ResourcesModel() }
                };

                foreach (var model in dataModels.Values)
                    model.LoadData();
            }
        }

        public Model GetDataModel<Model>(DataModelID dataID) where Model : class, IDataModel
        {
            if (dataModels.TryGetValue(dataID, out var dataModel))
                if (dataModel is Model)
                    return dataModel as Model;

            Debug.Log("Not Found Data Model");
            return null;
        }
    }
}