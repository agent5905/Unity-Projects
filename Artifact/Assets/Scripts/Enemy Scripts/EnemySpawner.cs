using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject wolfPrefab, wolfEaterPrefab;
    [SerializeField]
    private Transform[] spawnPoints;
    [SerializeField]
    private int eatChance = 3; // Chance out of 10 to spawn bush eater wolf
    [SerializeField]
    private float spawnTime = 12f; // Initial spawn time delay per wolf
    [SerializeField]
    private float spawnReductionPerWolf = 1f; // Reduction in spawn delay per wach wolf
    [SerializeField]
    private float minSpawnDelay = 3.5f; // Min spawn delay per wolf

    private float currentSpawnTime;
    private float timer;

    private void Start()
    {
        currentSpawnTime = spawnTime;
        timer = Time.time;
    }

    void Update()
    {
        if(Time.time > timer)
        {
            Spawn();
            
            currentSpawnTime -= spawnReductionPerWolf;
            
            if(currentSpawnTime <= minSpawnDelay)
                currentSpawnTime = minSpawnDelay;

            timer = Time.time + currentSpawnTime;
        }
    }

    void Spawn()
    {
        if(Random.Range(0, 11) > eatChance)
        {
            Instantiate(wolfPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        }
        else
        {
            Instantiate(wolfEaterPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        }
    }
}
