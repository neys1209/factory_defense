using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    [SerializeField] public GameObject GridCellPrefep = null;
    [SerializeField] int width = 10;
    [SerializeField] int height = 10;

    static GameObject[,] gameGrid;
    float gridSpaceSize = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateGrid()
    {
        StartCoroutine(CreateGridCell());
    }

    private IEnumerator CreateGridCell()
    {
        if (GridCellPrefep == null)
        {
            Debug.Log("Error : 그리드 셀이 널입니다.");
            yield break;
        }
        
        gameGrid = new GameObject[width,height];

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                gameGrid[x, y] = Instantiate(GridCellPrefep, new Vector3(x * gridSpaceSize, 0, y * gridSpaceSize), Quaternion.identity);
                gameGrid[x, y].GetComponent<GridCell>().SetPosition(x, y);
                gameGrid[x, y].transform.parent = transform;
                gameGrid[x, y].gameObject.name = $"GridSpace : ({x},{y})";
            }
        }
    }

    public Vector2Int GetGridPosFromWorld(Vector3 WorldPosition)
    {
        int x = Mathf.FloorToInt(WorldPosition.x /gridSpaceSize);
        int y = Mathf.FloorToInt(WorldPosition.z / gridSpaceSize);

        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(y, 0, height);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosFromGrid(Vector2Int GridPosition)
    {
        float x = GridPosition.x * gridSpaceSize;
        float y = GridPosition.y * gridSpaceSize;

        return new Vector3(x, 0, y);
    }

    public GameObject GetCellWithGridPos(int x,int y)
    {
        return gameGrid[x, y];
    }
}

