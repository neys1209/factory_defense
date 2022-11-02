using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FDBlock;

public class EnemyBase : Block
{

    public float HP = 100;
    [SerializeField] float timer = 10f;
    [SerializeField] GameObject enemyPrefab = null;
    int stage = 0;

    private void Awake()
    {
        Unbreakable = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StartCoroutine(Spawning());     
            timer = 120.0f;
            stage++;
        }
    }

    IEnumerator Spawning()
    {
        for (int i = 0; i < (stage) * 0.2 + 1; i++)
        {
            Instantiate(enemyPrefab,transform.position + Vector3.forward * 2, Quaternion.identity, transform);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
