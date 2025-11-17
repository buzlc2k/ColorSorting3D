using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace ColorSorting3D.Data
{
    public abstract class DataModel<DTO> : IDataModel where DTO : class
    {
        protected string dataKey;
        protected DTO dto;

        protected abstract void SetKey();
        
        protected abstract DTO CreateDefaultDTO();

        public void LoadData()
        {
            SetKey();

            string jsonString = SaveSystem.HasKey(dataKey)
                ? SaveSystem.GetString(dataKey)
                : null;

            dto = !string.IsNullOrEmpty(jsonString) 
                    ? JsonUtility.FromJson<DTO>(jsonString) 
                    : CreateDefaultDTO();
        }

        public virtual T Read<T>(Expression<Func<DTO, T>> fieldSelector)
        {
            var func = fieldSelector.Compile();
            return func(dto);
        }

        public virtual void Write<T>(Expression<Func<DTO, T>> fieldSelector, T overridedValue)
        {
            if (fieldSelector.Body is MemberExpression memberExpression
               && memberExpression.Member is FieldInfo fieldInfo)
            {
                fieldInfo.SetValue(dto, overridedValue);
                SaveData();
            }
        }

        public void SaveData()
        {
            string jsonString = JsonUtility.ToJson(dto);
            
            SaveSystem.SetString(dataKey, jsonString);
            SaveSystem.SaveToDisk();
        }
    }
}