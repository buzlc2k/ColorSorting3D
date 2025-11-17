using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ObservableCollections;
using R3;
using UnityEngine;

namespace ColorSorting3D.Gameplay.DispatcherStates
{
    public interface IDispatcherState
    {
        public Task<DispatcherState> HandleSelection(RingsContainer container);
        public Task<DispatcherState> HandleUndoTransfer(); 
    }
    
    public abstract class BaseState : IDispatcherState
    {
        protected readonly ContainerBuffer containersBuffer;
        protected readonly ObservableStack<RingsTransferCommand> ringsTransferHistory;

        public BaseState(ContainerBuffer containersBuffer, ObservableStack<RingsTransferCommand> ringsTransferHistory)
        {
            this.containersBuffer = containersBuffer;
            this.ringsTransferHistory = ringsTransferHistory;
        }

        public abstract Task<DispatcherState> HandleSelection(RingsContainer container);

        public virtual async Task<DispatcherState> HandleUndoTransfer()
        {
            var ringsTransferCommand = ringsTransferHistory.Pop();
            await ringsTransferCommand.Undo();

            return DispatcherState.None;
        }
    }
}