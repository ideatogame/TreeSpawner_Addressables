using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PositionSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TreeSpawners
{
    public class TreeSpawnerAddressables : MonoBehaviour
    {
        [SerializeField] private AssetReference[] treeAssets = null;
        [SerializeField] private int quantity = 5;
        [SerializeField] private TreePositioner treePositioner = null;

        private readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> loadOperations =
            new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();

        private readonly Dictionary<AssetReference, List<GameObject>> spawnedAssets =
            new Dictionary<AssetReference, List<GameObject>>();
        
        public void SpawnTrees(int index)
        {
            if (index < 0 || index >= treeAssets.Length || !treeAssets[index].RuntimeKeyIsValid())
                return;
            
            AssetReference assetReference = treeAssets[index];

            if (loadOperations.ContainsKey(assetReference))
            {
                if (loadOperations[assetReference].IsDone)
                    SpawnTrees(assetReference);
                return;
            }
            
            LoadAndSpawn(assetReference);
        }

        public void DestroyAll()
        {
            treePositioner.RemoveAllTrees();

            foreach (AssetReference assetReference in spawnedAssets.Keys.ToList())
            {
                foreach (GameObject spawnedAsset in spawnedAssets[assetReference].ToList())
                    RemoveTree(assetReference, spawnedAsset);
            }
        }      
        private void LoadAndSpawn(AssetReference assetReference)
        {
            AsyncOperationHandle<GameObject> loadOperation = Addressables.LoadAssetAsync<GameObject>(assetReference);
            loadOperations[assetReference] = loadOperation;
            loadOperation.Completed += operation => SpawnTrees(assetReference);
        }

        private void SpawnTrees(AssetReference assetReference)
        {
            for (int i = 0; i < quantity; i++)
                SpawnTreeFromLoadedReference(assetReference);
        }
        
        private void SpawnTreeFromLoadedReference(AssetReference assetReference)
        {
            AsyncOperationHandle<GameObject> instantiateOperation = assetReference.InstantiateAsync();
            instantiateOperation.Completed += operation =>
            {
                if (!spawnedAssets.ContainsKey(assetReference))
                    spawnedAssets[assetReference] = new List<GameObject>();
                
                spawnedAssets[assetReference].Add(operation.Result);
                operation.Result.SetActive(true);
                bool result = treePositioner.RandomizeAndPlace(operation.Result);
                if (!result) RemoveTree(assetReference, operation.Result);
            };
        }

        private void RemoveTree(AssetReference assetReference, GameObject tree)
        {
            if (spawnedAssets[assetReference].Contains(tree))
                spawnedAssets[assetReference].Remove(tree);
            
            Addressables.ReleaseInstance(tree);

            if (spawnedAssets[assetReference].Count == 0)
                StartCoroutine(UnloadAfterTime(assetReference, 6f));
        }
        
        private IEnumerator UnloadAfterTime(AssetReference assetReference, float time)
        {
            yield return new WaitForSeconds(time);
            
            if (spawnedAssets[assetReference].Count == 0)
            {
                if (loadOperations[assetReference].IsValid())
                {
                    Addressables.Release(loadOperations[assetReference]);
                    print("Released");
                }

                loadOperations.Remove(assetReference);
            }
        }
    }
}