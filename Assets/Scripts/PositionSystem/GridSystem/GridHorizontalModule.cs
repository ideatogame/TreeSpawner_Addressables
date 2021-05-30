using UnityEngine;

namespace PositionSystem.GridSystem
{
    public class GridHorizontalModule : IGridModule
    {
        private readonly Vector3 center;
        private readonly Vector3 offset;
        private readonly float cellSize;
        private readonly float width;
        private readonly float height;

        public GridHorizontalModule(Vector3 center, Vector3 offset, float cellSize, float width, float height)
        {
            this.center = center;
            this.offset = offset;
            this.cellSize = cellSize;
            this.width = width;
            this.height = height;
        }

        public Vector3 GridPositionToWorld(int x, int y)
        {
            Vector3 worldPosition = new Vector3(x, center.y + offset.y, y);
            worldPosition *= cellSize;
            worldPosition += center + offset;
            return worldPosition;
        }

        public (int x, int y) WorldPositionToGrid(Vector3 position)
        {
            Vector3 roundedPosition = RoundToNearestCell(position);
            roundedPosition = new Vector3
            (
                Mathf.Clamp(roundedPosition.x, center.x + offset.x, width * cellSize - 1), 0f,
                Mathf.Clamp(roundedPosition.z, center.z + offset.z, height * cellSize - 1)
            );

            int x = (int)((roundedPosition.x - (center.x + offset.x)) / cellSize);
            int y = (int)((roundedPosition.z - (center.z + offset.z)) / cellSize);
            
            return (x, y);
        }

        private Vector3 RoundToNearestCell(Vector3 position)
        {
            float x = Mathf.Round(position.x / cellSize) * cellSize;
            float z = Mathf.Round(position.z / cellSize) * cellSize;

            return new Vector3(x, center.y + offset.y, z);
        }
    }
}