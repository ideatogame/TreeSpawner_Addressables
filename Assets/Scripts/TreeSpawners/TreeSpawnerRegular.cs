using System.Collections.Generic;
using PositionSystem;
using UnityEngine;

namespace TreeSpawners
{
    public class TreeSpawnerRegular : MonoBehaviour
    {
        [SerializeField] private GameObject[] treePrefabs = null;
        [SerializeField] private int quantity = 5;
        [SerializeField] private TreePositioner treePositioner = null;
        
        public void SpawnTrees(int index)
        {
            if (index < 0 || index >= treePrefabs.Length)
                return;
            
            GameObject treePrefab = treePrefabs[index];

            for (int i = 0; i < quantity; i++)
            {
                GameObject instance = Instantiate(treePrefab);
                instance.SetActive(true);
                bool result = treePositioner.RandomizeAndPlace(instance);
                if (!result)
                    Destroy(instance);
            }
        }

        public void DestroyAll()
        {
            List<GameObject> removedTrees = treePositioner.RemoveAllTrees();
            for (int i = 0; i < removedTrees.Count; i++)
                Destroy(removedTrees[i]);
        }
    }
}