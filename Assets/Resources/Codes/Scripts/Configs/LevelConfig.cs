using System;
using System.Collections.Generic;
using ButbleConfig;
using UnityEngine;

namespace ColorSorting3D.Config
{
    [Serializable]
    public class SavedRingsContainer
    {
        public Vector3 Position;
        public int Size;
        public bool Available;
        public List<RingID> Rings;

        public SavedRingsContainer(Vector3 Position, int Size, bool Available, List<RingID> Rings)
        {
            this.Position = Position;
            this.Size = Size;
            this.Available = Available;
            this.Rings = Rings;
        } 
    }

    [Serializable]
    public class LevelConfigRecord
    {
        [RecordKeyField(0)]
        public int Index;
        public List<SavedRingsContainer> Containers;
        public int TargetContainerFinished;
        public Vector3 CameraPos;

        public LevelConfigRecord(int Index, List<SavedRingsContainer> Containers, int TargetContainerFinished, Vector3 CameraPos)
        {
            this.Index = Index;
            this.Containers = Containers;
            this.TargetContainerFinished = TargetContainerFinished;
            this.CameraPos = CameraPos;
        }
    }

    [CreateAssetMenu(fileName = "LevelConfig", menuName = "LevelConfig")]
    public class LevelConfig : ConfigTable<LevelConfigRecord>
    {
        protected override LevelConfigRecord CreateDefaultInstance()
        {
            throw new NotImplementedException();
        }

        
    }
}
