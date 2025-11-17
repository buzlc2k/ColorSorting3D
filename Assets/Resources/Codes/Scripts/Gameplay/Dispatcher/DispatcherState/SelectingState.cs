using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ObservableCollections;
using R3;
using UnityEngine;

namespace ColorSorting3D.Gameplay.DispatcherStates
{
    public class SelectingState : BaseState
    {
        public SelectingState(ContainerBuffer containersBuffer, ObservableStack<RingsTransferCommand> ringsTransferHistory) 
                : base(containersBuffer, ringsTransferHistory) {}

        public override async Task<DispatcherState> HandleSelection(RingsContainer container)
        {
            if (!container.GetAvailable())
                return DispatcherState.None;

            containersBuffer.Dest = container;
            var transferResult = await TryTransferRings();

            return transferResult ? HandleTransferSuccess() : await HandleTransferFailed();

            async Task<bool> TryTransferRings()
            {
                var tempTransfer = new RingsTransferCommand(containersBuffer.Source, containersBuffer.Dest);
                ringsTransferHistory.Push(tempTransfer);

                return await tempTransfer.TryExcute();
            }

            DispatcherState HandleTransferSuccess()
            {
                containersBuffer.Clear();
                return DispatcherState.Normal;
            }

            async Task<DispatcherState> HandleTransferFailed()
            {
                await containersBuffer.Source.MarkUnSelected();
                ringsTransferHistory.Pop();
                containersBuffer.Clear();
                return DispatcherState.Normal;
            }
        }

        public override async Task<DispatcherState> HandleUndoTransfer()
        {
            await containersBuffer.Source.MarkUnSelected(); 
            await base.HandleUndoTransfer();

            return DispatcherState.Normal;

        }
    }
}