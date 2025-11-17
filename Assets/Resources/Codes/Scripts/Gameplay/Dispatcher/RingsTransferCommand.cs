using System.Threading.Tasks;
using ColorSorting3D.Config;
using UnityEngine;

namespace ColorSorting3D.Gameplay
{
    public class RingsTransferCommand
    {
        private RingsContainer from = default;
        private RingsContainer to = default;
        private int transferNumber = default;

        public RingsTransferCommand(RingsContainer from, RingsContainer to)
        {
            this.from = from;
            this.to = to;
        }
        
        public async Task<bool> TryExcute()
        {
            if (!TryInitTransfer())
                return false;

            await TransferAllPossibleRings();
            await PostTransfer();

            return true;

            bool TryInitTransfer()
                => IsNotDuplicate() 
                   && CanTransfer() 
                   && (TryInitNewColorStack() || IsColorsMatch());

            bool IsNotDuplicate()
                => from != to;

            bool CanTransfer()
                => from.HasRings && to.CanAcceptRings;
            
            bool TryInitNewColorStack()
            {
                if (to.GetLastColorStack() != RingID.None)
                    return false;

                to.InitNewStack(from.GetLastColorStack());
                return true;
            }

            bool IsColorsMatch()
            {
                var fromColor = from.GetLastColorStack();
                var toColor = to.GetLastColorStack();

                return toColor == fromColor;
            }

            async Task TransferAllPossibleRings()
            {
                Task lastSequence;
                do
                {
                    lastSequence = TransferSingleRing(from, to);
                    transferNumber++;
                    await Task.Delay(135);
                }
                while (CanTransfer() && IsColorsMatch());

                await lastSequence;
            }

            async Task PostTransfer()
            {
                if (to.GetStackCount() != 1 || to.CanAcceptRings)
                    return;

                await to.SetReadOnly(true);
            }
        }

        public async Task Undo()
        {
            PreUndo();
            await UndoAllRings();

            void PreUndo()
            {
                if (to.IsReadOnly.Value)
                    _ = to.SetReadOnly(false);

                var fromColor = from.GetLastColorStack();
                var toColor = to.GetLastColorStack();
                if (fromColor != toColor)
                    from.InitNewStack(toColor);
            }

            async Task UndoAllRings()
            {
                Task lastSequence = null;
                for (int i = 0; i < transferNumber; i++)
                {
                    lastSequence = TransferSingleRing(to, from);
                    await Task.Delay(135);
                }
                await lastSequence;
            }
        }
        
        private async Task TransferSingleRing(RingsContainer from, RingsContainer to)
        {
            var ring = from.RemoveRing();
            (Vector3 upPos, Vector3 toPos, Vector3 downPos) = (from.GetEdgeRingPos(), to.GetEdgeRingPos(), to.GetNextRingPos());
            to.AddRing(ring);

            await ring.MoveUp(upPos);
            await ring.MoveTo(toPos);
            await ring.MoveDown(downPos);
        }
    }
}