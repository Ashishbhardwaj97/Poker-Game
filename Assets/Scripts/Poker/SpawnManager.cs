using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }


    public GameObject Spawn(string poolName, string objectName, Vector3 spawnPosition, Vector3 spawnAngle)
    {
        Transform objectPrefab = PoolManager.Pools[poolName].prefabs[objectName];
        Transform objectInstance = PoolManager.Pools[poolName].Spawn(objectPrefab);
        objectInstance.position = spawnPosition;
        objectInstance.eulerAngles = spawnAngle;
        return objectInstance.gameObject;
    }

    public void Despawn(string poolName, Transform obj)
    {

        PoolManager.Pools[poolName].Despawn(obj);

    }
}
