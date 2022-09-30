using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public enum UNITTYPE { AIR, GROUND }
    public UNITTYPE type = UNITTYPE.AIR;

    enum STATE { CREATE, ATTACK, BUILD }
}
