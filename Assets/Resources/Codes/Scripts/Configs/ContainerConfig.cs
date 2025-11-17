using System;
using ButbleConfig;
using UnityEngine;

namespace ColorSorting3D.Config
{
    [Serializable]
    public class ContainerConfigRecord
    {
        [RecordKeyField(0)]
        public bool Available;
        public Material Material;

        public ContainerConfigRecord(bool Available, Material Material)
        {
            this.Available = Available;
            this.Material = Material;
        }
    }

    [CreateAssetMenu(fileName = "ContainerConfig", menuName = "ContainerConfig")]
    public class ContainerConfig : ConfigTable<ContainerConfigRecord>
    {
        protected override ContainerConfigRecord CreateDefaultInstance()
            => new(true, null);
    }
}
