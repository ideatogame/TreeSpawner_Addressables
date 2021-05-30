using System;
using System.Collections.Generic;
using PositionSystem.GridSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PositionSystem
{
    public class TreePositioner : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = Vector3.zero;
        [SerializeField] private int width = 16;
        [SerializeField] private int height = 16;
        [SerializeField] private float cellSize = 2f;

        private GameObject treeFolder = null;
        private TreeGrids treeGrids = null;

        private void Awake()
        {
            treeFolder = new GameObject("[Tree Folder]");
            treeGrids = new TreeGrids(width, height, cellSize, transform.position, offset);
        }

        public bool RandomizeAndPlace(GameObject tree)
        {
            bool successfullyDone = true;
            
            while (true)
            {
                int xIndex = Random.Range(0, width);
                int yIndex = Random.Range(0, height);

                bool result = treeGrids.SetNewTree(tree, xIndex, yIndex);
                if (!result)
                {
                    if (treeGrids.ValidPlacesCount > 0) 
                        continue;

                    successfullyDone = false;
                    break;
                }
                
                tree.transform.position = treeGrids.HorizontalModule.GridPositionToWorld(xIndex, yIndex);
                tree.transform.parent = treeFolder.transform;
                break;
            }

            return successfullyDone;
        }

        public List<GameObject> RemoveAllTrees()
        {
            List<GameObject> treesRemoved = new List<GameObject>();
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    treesRemoved.Add(treeGrids.GetElement(x, y));
                    treeGrids.RemoveTree(x, y);
                }
            }

            return treesRemoved;
        }

        private void OnDrawGizmosSelected()
        {
            GridHorizontalModule horizontalModule =
                new GridHorizontalModule(transform.position, offset, cellSize, width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.DrawCube(horizontalModule.GridPositionToWorld(x, y), Vector3.one / 3f);
                }
            }
        }
    }
}
