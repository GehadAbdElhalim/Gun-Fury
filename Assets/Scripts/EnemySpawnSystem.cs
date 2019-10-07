using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    public GameObject[] Enemies;

    public Transform[] SpawnLocations;

    public int maxNumberOfEnemies;

    public static int currentNumberOfEnemies = 0;

    private void Start()
    {
        currentNumberOfEnemies = 0;
    }

    private void Update()
    {
        if(currentNumberOfEnemies < maxNumberOfEnemies)
        {
            currentNumberOfEnemies++;
            Instantiate(Enemies[Random.Range(0, Enemies.Length)], SpawnLocations[Random.Range(0, SpawnLocations.Length)].position, Quaternion.identity);
        }
    }

}
