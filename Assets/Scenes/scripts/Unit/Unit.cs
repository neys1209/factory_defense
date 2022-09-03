using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public enum UNITTYPE { AIR, GROUND }
    public UNITTYPE type = UNITTYPE.AIR;

    enum STATE { CREATE, ATTACK, BUILD }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    
}
