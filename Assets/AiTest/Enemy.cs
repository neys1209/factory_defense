using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    private int Hp;

    public int _HP
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
