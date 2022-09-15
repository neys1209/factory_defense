using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UNITTYPE { Air, Ground }
public enum UNITSTATE { Create, Attack, Build }

[System.Serializable]
public struct UnitStat
{
    [SerializeField] float maxHp; //�ִ� hp
    [SerializeField] float hp; //���� hp
    public float MaxHP { get { return maxHp; } set { maxHp = value;  } }
    public float HP { get { return hp; } set { hp = value; } }

    [SerializeField] float atk; //���ݷ�
    [SerializeField] float atkSpeed; //���ݼӵ� (�ʴ� ��� ����?)
    public float Atk { get { return atk; } set { atk = value; } }
    public float AtkSpeed { get { return atkSpeed; } set { atkSpeed = value; } }


    [SerializeField] float moveSpeed; //�ӵ�
    [SerializeField] float rotateSpeed;
    public float RotateSpeed { get { return rotateSpeed; } set { rotateSpeed = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    public UnitStat(float hp, float atk, float atkSpeed, float moveSpeed, float rotateSpeed)
    {
        this.hp = hp;
        this.maxHp = hp;
        this.atk = atk;
        this.atkSpeed = atkSpeed;
        this.moveSpeed = moveSpeed;
        this.rotateSpeed = rotateSpeed;
    }
}