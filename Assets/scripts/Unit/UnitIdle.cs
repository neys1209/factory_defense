using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdle : MonoBehaviour
{
    private Animator _myAnim = null;
    public Animator myAnim
    {
        get
        {
            if (_myAnim == null)
            {
                _myAnim = GetComponent<Animator>();
                if (_myAnim == null)
                {
                    _myAnim = GetComponentInChildren<Animator>();
                }
            }
            return _myAnim;
        }

    }
}
