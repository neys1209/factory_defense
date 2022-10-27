using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterProperty : MonoBehaviour
{

    public class Enemy
    {       
        private int Hp;

        public int HP
        {
            get => Hp;

            set
            {
                if (value <= -1)
                {
                    value = 0;
                    Hp = value;
                }

                Hp = value;
            }

        }
        public int Damage { get; set; }


    }
    public NavMeshAgent agent1 { get; }

    Rigidbody _rigid = null;
    protected Rigidbody myRigid
    {
        get
        {
            if (_rigid == null)
            {
                _rigid = GetComponent<Rigidbody>();
                if (_rigid == null) _rigid = GetComponentInChildren<Rigidbody>();
            }
            return _rigid;
        }
    }

    Animator _amim = null;
    protected Animator myAnim
    {
        get
        {
            if (_amim == null)
            {
                _amim = GetComponent<Animator>();
                if (_amim == null) _amim = GetComponentInChildren<Animator>();
            }
            return _amim;
        }
    }

    Renderer _render = null;
    protected Renderer myRenderer
    {
        get
        {
            if (_render == null)
            {
                _render = GetComponent<Renderer>();
                if (_render == null) _render = GetComponentInChildren<Renderer>();
            }
            return _render;
        }
    }

    Collider _collider = null;
    protected Collider myCollider
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider>();
                if (_collider == null) _collider = GetComponentInChildren<Collider>();
            }
            return _collider;
        }
    }
}
//myAnim.SetFloat("x", curDir.x);
//myAnim.SetFloat("y", curDir.y);