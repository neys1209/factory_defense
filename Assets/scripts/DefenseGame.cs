using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FDBlock;

public class DefenseGame : MonoBehaviour
{

    public GameObject EnemyBase;

    IEnumerator InitGame()
    {
        yield return new WaitUntil(()=>GridSystem.Inst.GameStart);
        int x = UnityEngine.Random.Range(5, GridSystem.MapWidth-5);
        int y = UnityEngine.Random.Range(5, GridSystem.MapHeight-5);
        GridSystem.Inst.PlaceBlock(EnemyBase, GridSystem.Inst.MapPosition2WorldPostion(new Vector2(x, y)), EnemyBase.GetComponent<Block>().CellSize);

    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
