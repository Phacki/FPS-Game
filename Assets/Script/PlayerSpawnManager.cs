using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance;
    public List<GameObject> SpawnPoints;

    void Awake()
    {
        Instance = this;
    }


    public Transform GetSpawnPoint()
    {
        return SpawnPoints[Random.Range(0, SpawnPoints.Count - 1)].transform;
    }
}
