using PositionSystem.GridSystem;
using UnityEngine;

namespace PositionSystem
{
    public class TreeGrids
    {
        public GridHorizontalModule HorizontalModule { get; }
        public int ValidPlacesCount { get; set; }
        
        private readonly Grid<GameObject> treeGrid;
        private readonly bool[,] availablePlaces;
        
        public TreeGrids(int width, int height, float cellSize, Vector3 center, Vector3 offset)
        {
            HorizontalModule = new GridHorizontalModule(center, offset, cellSize, width, height);
            treeGrid = new Grid<GameObject>(width, height, HorizontalModule);
            availablePlaces = new bool[width, height];

            ValidPlacesCount = width * height;
            
            ResetAvailablePlaces();
        }

        public bool SetNewTree(GameObject tree, int x, int y)
        {
            bool indexesNotValid = CheckIndexes(x, y);
            if (indexesNotValid || !availablePlaces[x, y])
                return false;
            
            treeGrid.SetElement(tree, x, y);
            
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    int xIndex = Mathf.Clamp(i, 0, availablePlaces.GetLength(0) - 1);
                    int yIndex = Mathf.Clamp(j, 0, availablePlaces.GetLength(1) - 1);
                    availablePlaces[xIndex, yIndex] = false;
                    ValidPlacesCount--;
                }
            }

            return true;
        }

        public bool RemoveTree(int x, int y)
        {
            bool indexesNotValid = CheckIndexes(x, y);
            if (indexesNotValid || availablePlaces[x, y])
                return false;

            treeGrid.SetElement(null, x, y);
            availablePlaces[x, y] = true;
            return true;
        }
        
        private bool CheckIndexes(int x, int y)
        {
            bool indexesNotValid = x < 0 || x >= availablePlaces.GetLength(0) ||
                                   y < 0 || y >= availablePlaces.GetLength(1);
            return indexesNotValid;
        }

        private void ResetAvailablePlaces()
        {
            for (int xIndex = 0; xIndex < availablePlaces.GetLength(0); xIndex++)
            {
                for (int yIndex = 0; yIndex < availablePlaces.GetLength(1); yIndex++)
                    availablePlaces[xIndex, yIndex] = true;
            }
        }

        public GameObject GetElement(int x, int y) => treeGrid.GetElement(x, y);
    }
}