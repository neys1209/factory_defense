using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    protected UNITTYPE type = UNITTYPE.Air;
    public UnitStat stat;

    UNITSTATE state = UNITSTATE.Create;

    Coroutine coMove = null;

    protected void ChangeState(UNITSTATE s)
    {
        if (state == s) return;
        state = s;
        switch (state)
        {
            case UNITSTATE.Create:
                break;
            case UNITSTATE.Attack:
                break;
            case UNITSTATE.Build:
                break;
        }
    }

    protected void StateProcess()
    {
        switch (state)
        {
            case UNITSTATE.Create:
                break;
            case UNITSTATE.Attack:
                break;
            case UNITSTATE.Build:
                break;
        }
    }


    protected IEnumerator MoveToTarget(Vector3 target)
    {
        yield return null;
    }

    protected IEnumerator MoveToMovingTarget()
    {
        yield return null;
    }

    protected bool isMoveing()
    {
        return (coMove != null);
    }

    protected void SetUnitType(UNITTYPE t)
    {
        type = t;
    }
    protected UNITTYPE GetUnitType()
    {
        return type;
    }
    
}
