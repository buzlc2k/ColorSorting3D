using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ObservableCollections;
using R3;
using UnityEngine;

namespace ColorSorting3D.Gameplay.DispatcherStates
{
    public class NormalState : BaseState
    {
        public NormalState(ContainerBuffer containersBuffer, ObservableStack<RingsTransferCommand> ringsTransferHistory) 
                : base(containersBuffer, ringsTransferHistory) {}

        public override async Task<DispatcherState> HandleSelection(RingsContainer container)
        {
            if (!container.GetAvailable())
                return DispatcherState.None;

            if (container.IsReadOnly.Value)
                return await HandleReadOnly();

            return await HandleSelectable();

            async Task<DispatcherState> HandleReadOnly()
            {
                await container.MarkSelectedReadOnly();
                return DispatcherState.None;
            }

            async Task<DispatcherState> HandleSelectable()
            {
                containersBuffer.Source = container;
                await container.MarkSelected();
                return DispatcherState.Selecting;
            }
        }
    }
}