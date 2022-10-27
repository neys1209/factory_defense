using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class datafile
{
    public Cell[] mapData;
    

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

        datafile SaveData = new datafile();
        SaveData.mapData = ChoiceSaveCell();
        //SaveData.mapSize = new Vector2(GridSystem.MapWidth, GridSystem.MapHeight);
        //SaveData.cellSize = new Vector2(GridSystem.CellSize.x, GridSystem.CellSize.y);
        string fliePath = Application.dataPath + "/Data/Maps";

        FileStream fs = new  FileStream(fliePath+"/Savefile.Save",FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, SaveData);
        fs.Close();
    }

    public void Load()
    {
        
    }

    Cell[] ChoiceSaveCell()
    {
        Cell[] retArray = new Cell[GridSystem.ActivatedCell.Count];
        for (int i = 0; i < retArray.Length; i++)
        {
            (int x, int y) Index = GridSystem.ActivatedCell[i];
            retArray[i] = GridSystem.MapData[Index.x, Index.y];
        }
        return retArray;
        
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
