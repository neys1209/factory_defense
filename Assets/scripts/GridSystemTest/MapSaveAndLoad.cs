using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class datafile
{
    public Cell[] mapData;
    public Vector2 mapSize;
    public Vector2 cellSize;
    public List<GameObject> blockList;
}

public class MapSaveAndLoad : MonoBehaviour
{

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }
    }

    public void Save()
    {
        datafile data = new datafile();
        data.mapData = twodim2onedim(GridSystem.MapData);
        data.mapSize = new Vector2(GridSystem.MapWidth, GridSystem.MapHeight);
        data.cellSize = GridSystem.CellSize;
        data.blockList = Blocks.Inst.BlockList;



        string fileName = $"{Random.value}";
        string path = Path.Combine(Application.dataPath + "/Data/Maps/" + fileName + "save.Json");
        string file = JsonUtility.ToJson(data);
        Debug.Log(file);
        File.WriteAllText(path, file);
    }

    public void Load()
    {
        new GameObject().GetInstanceID();
    }

    T[] twodim2onedim<T>(T[,] array)
    {
        int length = array.GetLength(0) * array.GetLength(1);
        T[] retArray = new T[length];
        int index = 0;
        for (int x=0; x < array.GetLength(0); x++)
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                retArray[index] = array[x,y];
                index++;
            }
        }
        return retArray;
    }
    
}
