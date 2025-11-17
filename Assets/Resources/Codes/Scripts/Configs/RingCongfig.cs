using System;
using ButbleConfig;
using UnityEngine;

namespace ColorSorting3D.Config
{
    [Serializable]
    public class RingConfigRecord
    {
        [RecordKeyField(0)]
        public RingID ID;
        public Material Material;

        public RingConfigRecord(RingID ID, Material Material)
        {
            this.ID = ID;
            this.Material = Material;
        }
    }

    [CreateAssetMenu(fileName = "RingConfig", menuName = "RingConfig")]
    public class RingConfig : ConfigTable<RingConfigRecord>
    {
        protected override RingConfigRecord CreateDefaultInstance()
            => new(RingID.None, null);
    }
}
