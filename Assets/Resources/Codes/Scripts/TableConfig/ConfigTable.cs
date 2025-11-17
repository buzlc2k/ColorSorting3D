using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ButbleConfig
{
    public abstract class ConfigTable<T> : ScriptableObject, ISerializationCallbackReceiver where T : class
    {
        private RecordComparer<T> recordComparer;
        [SerializeField, SerializeReference] protected List<T> records = new();

        protected virtual void OnValidate() {
            OnNewRecordAddedDefault();
        }

        protected virtual void OnEnable()
            => InitConfigtable();

        protected virtual void InitConfigtable()
            => recordComparer ??= new RecordComparer<T>();

        protected abstract T CreateDefaultInstance();

        private void OnNewRecordAddedDefault()
        {
            if (records.Count == 0 || records[^1] != null)
                return;

            records[^1] = CreateDefaultInstance();
        }

        public virtual void Add(object record)
            => records.Add(record as T);

        public List<T> GetAllRecord()
            => records;

        public virtual T Get(params object[] keyValues)
        {
            int resultIndex = records.R_BinarySearch(recordComparer, keyValues);

            if (resultIndex >= 0)
                return records[resultIndex];
            else
            {
                Debug.Log("Can't find element by key");
                return null;
            }
        }

        public virtual void Remove(object record)
            => records.Remove(record as T);
            
        public void OnBeforeSerialize()
            => records.Sort(recordComparer);

        public void OnAfterDeserialize(){}
    }
}