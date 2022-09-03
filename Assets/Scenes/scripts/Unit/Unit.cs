using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public enum UNITTYPE { Air, Ground }
    public UNITTYPE type = UNITTYPE.Air;

    enum STATE { Create, Attack, Build, Move }
    
    STATE state = STATE.Create;

    void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;
        switch (state)
        { 
            case STATE.Create:
                break;
            case STATE.Attack:
                break;
            case STATE.Build:
                break;
            case STATE.Move:
                break;
        }
    }

    void StateProcess()
    {
        switch (state)
        {
            case STATE.Create:
                break;
            case STATE.Attack:
                break;
            case STATE.Build:
                break;
            case STATE.Move:

                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    
}
