using System;
using System.Collections.Generic;
using ButbleConfig;
using UnityEngine;

namespace ColorSorting3D.Config
{
    [Serializable]
    public struct PresentationTransitionStep
    {
        public PresentationID ID;
        public bool Enable;
        public int Duration;
        public string Animation;
    }

    [Serializable]
    public class PresentationTransitionRecord
    {
        [RecordKeyField(0)]
        public PresentationTransitionID ID;
        public List<PresentationTransitionStep> TransitionSequence;

        public PresentationTransitionRecord(PresentationTransitionID ID, List<PresentationTransitionStep> TransitionSequence)
        {
            this.ID = ID;
            this.TransitionSequence = TransitionSequence;
        }
    }

    [CreateAssetMenu(fileName = "PresentationTransitionConfig", menuName = "PresentationTransitionConfig")]
    public class PresentationTransitionConfig : ConfigTable<PresentationTransitionRecord>
    {
        protected override PresentationTransitionRecord CreateDefaultInstance()
            => new(PresentationTransitionID.None, null);
    }
}
