using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : UnitIdle
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
