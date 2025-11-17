using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ColorSorting3D.LevelEditor
{
    internal class GridCell
    {        
        public bool Available { get; private set; }
        public bool CanNotSpawnRing { get => Available || !Container.GetAvailable(); }
        public Vector3 WorldPos { get; private set; }
        public RingsContainer Container { get; private set; }
        public List<Ring> Rings { get; private set; }

        public GridCell(Vector3 WorldPos)
        {
            this.WorldPos = WorldPos;
            SetAvailable(true);
        }

        private Vector3 GetRingPosByIndex(int index)
            => WorldPos + new Vector3(0, index * 0.0875f, 0);

        public Vector3 GetNextRingPos()
        {
            int index = Rings == null ? 0 : Rings.Count;
            return GetRingPosByIndex(index);
        }

        public void AddContainer(RingsContainer Container)
        {
            this.Container = Container;
            this.Container.SetPos(WorldPos);

            Rings ??= new();

            SetAvailable(false);
        }

        public void IncreseSizeOfContainer()
            => Container.IncreaseSize();

        public void DecreseSizeOfContainer()
        {
            Container.DecreaseSize();
            RemoveRing(Rings[^1]);
        } 
        
        public void SetContainerAvailble(bool available)
        {
            if (!available)
                RemoveAllRings();

            Container.SetAvailable(available);
        }

        public void RemoveContainer()
        {
            RemoveAllRings();

            Container.ReleaseCallback.Invoke(Container.gameObject);
            Container = null;

            SetAvailable(true);
        }

        public void AddRing(Ring ring)
        {
            ring.SetPos(GetNextRingPos());
            Rings.Add(ring);

            if (Rings.Count > Container.transform.localScale.y)
                Container.IncreaseSize();
        }

        public void RemoveAllRings()
        {
            foreach (var Ring in Rings)
                Ring.ReleaseCallback.Invoke(Ring.gameObject);
            Rings.Clear();
        }

        public void RemoveRing(Ring ring)
        {
            Rings.Remove(ring);
            ring.ReleaseCallback.Invoke(ring.gameObject);

            for (int i = 0; i < Rings.Count; i++)
                Rings[i].SetPos(GetRingPosByIndex(i));
            
            if (Rings.Count == (int)Container.transform.localScale.y - 1)
                Container.DecreaseSize();
        }

        public void SetAvailable(bool available)
            => Available = available;
    }

    [RequireComponent(typeof(Grid))]
    internal class GridManager : MonoBehaviour
    {
        private Dictionary<Vector3Int, GridCell> cells;
        private Grid grid;

        // Grid Size (Half)
        [SerializeField] private int cellCountX;
        [SerializeField] private int cellCountZ;

        private void Awake()
            => InitGrid();

        private void InitGrid()
        {
            cells = new();
            grid = GetComponent<Grid>();
            FillCells();
        }

        private void FillCells()
        {
            foreach (int j in Enumerable.Range(-cellCountZ, cellCountZ * 2 + 1))
                foreach (int i in Enumerable.Range(-cellCountX, cellCountX * 2 + 1))
                {
                    var index = new Vector3Int(i, 0, j);
                    cells[index] = new(grid.GetCellCenterWorld(index));
                }
        }

        public GridCell GetCell(Vector3 pos)
        {
            var index = grid.WorldToCell(pos);
            index.x = Mathf.Clamp(index.x, -cellCountX, cellCountX);
            index.y = 0;
            index.z = Mathf.Clamp(index.z, -cellCountZ, cellCountZ);

            return cells[index];
        }

        public IEnumerable<GridCell> GetAllCells()
        {
            foreach (var cell in cells)
                yield return cell.Value;
        }
    }
}