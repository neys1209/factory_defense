using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    
    [System.Serializable]
   public class Wave
    {
        public EnemyAI[] enemies;
        public int count;
        public float timeBetweenSpawns;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves;

    private Wave currentWave;
    private int currentWaveIndex;
    public Transform player;  // 이것을 건물 및 유닛 등 개수로 교체하기

    private bool finishiedSpawning;

    private void Start()
    {
        
        StartCoroutine(StartNextWave(currentWaveIndex));
    }

    IEnumerator StartNextWave(int index)
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        StartCoroutine(SpawnWave(index));
    }

    IEnumerator SpawnWave(int index)
    {
        currentWave = waves[index];

        for(int i=0; i<currentWave.count; i++)
        {
            //if(player == null)
            //{
            //    yield break;
            //}

            EnemyAI randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpot = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomSpot.position, randomSpot.rotation);

            if(i == currentWave.count -1)
            {
                finishiedSpawning = true;
            }
            else
            {
                finishiedSpawning = false;
            }
        }

        yield return new WaitForSeconds(currentWave.timeBetweenSpawns);
    }

    private void Update()
    {
        if(finishiedSpawning == true && GameObject.FindGameObjectsWithTag("Enemy").Length == 0 )
        {
            finishiedSpawning = false;
            if(currentWaveIndex + 1 < waves.Length)
            {
                currentWaveIndex++;
                StartCoroutine(StartNextWave(currentWaveIndex));
            }
            else
            {
                Debug.Log("GAME FINISHED !!");
            }
        }
    }

}
