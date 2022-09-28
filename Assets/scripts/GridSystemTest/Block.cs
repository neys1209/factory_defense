using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum Type {None, Tower, Factory, Wall}
    public Vector2 CellSize = new Vector2(1, 1);
    public Type blockType = Type.None;
}
