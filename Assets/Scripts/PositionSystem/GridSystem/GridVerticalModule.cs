using UnityEngine;

namespace PositionSystem.GridSystem
{
    public class GridVerticalModule : IGridModule
    {
        private readonly Vector3 center;
        private readonly Vector3 offset;
        private readonly float cellSize;
        private readonly float width;
        private readonly float height;

        public GridVerticalModule(Vector3 center, Vector3 offset, float cellSize, float width, float height)
        {
            this.center = center;
            this.offset = offset;
            this.cellSize = cellSize;
            this.width = width;
            this.height = height;
        }

        public Vector3 GridPositionToWorld(int x, int y)
        {
            Vector3 worldPosition = new Vector3(x, y, center.z + offset.z);
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
                Mathf.Clamp(roundedPosition.y, center.y + offset.y, height * cellSize - 1)
            );

            int x = (int)((roundedPosition.x - (center.x + offset.x)) / cellSize);
            int y = (int)((roundedPosition.y - (center.y + offset.y)) / cellSize);
            
            return (x, y);
        }

        private Vector3 RoundToNearestCell(Vector3 position)
        {
            float x = Mathf.Round(position.x / cellSize) * cellSize;
            float y = Mathf.Round(position.y / cellSize) * cellSize;

            return new Vector3(x, y, center.z + offset.z);
        }
    }
}