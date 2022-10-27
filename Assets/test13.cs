using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test13 : CharacterProperty
{
    public UnityEngine.AI.NavMeshAgent agent;
    public LayerMask layerMask;
    private Collider[] enemyColliders;
    public int maxColliders = 10;
    public GameObject target;
    public float shortDis;
    public Transform bulletSpawn;

    private Animator animator;

    public float timeBetweenShots;

    private float shotTime;

    private bool dead;

    private bool isFire;

    private bool hasTarget;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        dead = false;
        hasTarget = false;
        // agent.updateRotation = false;
        isFire = false;
        StartCoroutine(SearchEnemy());
    }

    IEnumerator SearchEnemy()
    {
        while (!dead)
        {
            if (hasTarget)
            {

                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
                //Vector3 dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
                //transform.forward += dir;

            }

            else
            {
              //  agent.enabled = true;
                agent.isStopped = true;

                enemyColliders = new Collider[maxColliders];
                int numColliders = Physics.OverlapSphereNonAlloc(transform.position, Mathf.Infinity, enemyColliders, layerMask);


                if (numColliders != 0)
                {
                    shortDis = Vector3.Distance(transform.position, enemyColliders[0].transform.position); // ù��°�� �������� ����ֱ� 

                    target = enemyColliders[0].gameObject;
                    for (int i = 0; i < numColliders; i++)
                    {
                        float Distance = Vector3.Distance(transform.position, enemyColliders[i].transform.position);

                        if (Distance < shortDis)
                        {
                            target = enemyColliders[i].gameObject;
                            shortDis = Distance;
                        }
                    }
                    hasTarget = true;
                }
                else
                {
                    agent.enabled = false;

                }


            }
            // Debug.Log(target.name);
            yield return new WaitForSeconds(0.25f);

        }

    }
    void Update()
    {
        
    }
}
