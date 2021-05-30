using UnityEngine;

namespace PositionSystem.GridSystem
{
    public class Grid<T>
    {
        private readonly int width;
        private readonly int height;
        private readonly IGridModule gridModule;

        private readonly T[,] grid;
        
        public Grid(int width, int height, IGridModule gridModule)
        {
            this.width = width;
            this.height = height;
            this.gridModule = gridModule;
            
            grid = new T[width, height];
        }
        
        public T GetElement(int x, int y) => grid[x, y];
        
        public void SetElement(T element, int x, int y)
        {
            if (x >= width || y >= height)
                return;
            
            grid[x, y] = element;
        }
        
        public Vector3 GridPositionToWorld(int x, int y) => gridModule.GridPositionToWorld(x, y);
        public (int x, int y) WorldPositionToGrid(Vector3 position) => gridModule.WorldPositionToGrid(position);
    }
}