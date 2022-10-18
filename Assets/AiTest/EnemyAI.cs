using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Enemy
{
    public enum STATE
    {
        Search, Move, Attack, Die
    }

    public STATE enemyState = STATE.Search;

   // private Enemy enemyShooter;

    private NavMeshAgent agent;
    public GameObject bulletShot;
    public LayerMask layerMask;
    public LayerMask layerMask2;
    private Collider[] enemyColliders;
    public int maxColliders = 10;
    public GameObject target;   
    public float shortDis;
    public Transform bulletSpawn;

    private Animator animator;

    public float timeBetweenShots;

    private float shotTime;

    private bool dead;

    //private bool isFire;

    private bool hasTarget;

    NavMeshPath path ;

    // Start is called before the first frame update
    void Awake()
    {
      //  enemyShooter = new Enemy();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        dead = false;
        hasTarget = false;
       // agent.updateRotation = false;
        //isFire = false;
    }

    private void Start()
    {
        path = new NavMeshPath();
        HP = 100;
        Damage = 10;
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
              //  agent.SetDestination(path.corners[path.corners.Length -1]);
                //Vector3 dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
                //transform.forward += dir;

            }

            else
            {
                
                agent.enabled = true;
                agent.isStopped = true;

                enemyColliders = new Collider[maxColliders];
                int numColliders = Physics.OverlapSphereNonAlloc(transform.position, Mathf.Infinity, enemyColliders, layerMask);

                
                if (numColliders != 0)
                {
                    shortDis = Vector3.Distance(transform.position, enemyColliders[0].transform.position); // 첫번째를 기준으로 잡아주기 
                  //  NavMeshPath path = new NavMeshPath();
                      target = enemyColliders[0].gameObject;
                   //   bool p = agent.CalculatePath(enemyColliders[0].transform.position, path);
                    for (int i = 0; i < numColliders; i++)
                    {
                        float Distance = Vector3.Distance(transform.position , enemyColliders[i].transform.position);


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

   

    void EnemyState(STATE state)
    {
        // enemyState = state;

        switch (state)
        {
            case STATE.Search:
                SearchEnemy();
                break;

            case STATE.Move:

                //  StartCoroutine(EnemyMove());

                if (Vector3.SqrMagnitude(transform.position - target.transform.position) <= 30f)
                {
                    agent.isStopped = true;
                    enemyState = STATE.Attack;
                }
                if (target == null)
                {
                    agent.isStopped = false;
                    enemyState = STATE.Search;
                }

                break;


            case STATE.Attack:

                //   StopCoroutine(EnemyMove());
                transform.localRotation =
               Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(target.transform.position - transform.position), 5 * Time.deltaTime);


                if (target != null)
                {

                    if (Time.time >= shotTime)
                    {
                        GameObject bullet = Instantiate(bulletShot, bulletSpawn.position, Quaternion.identity);
                        //clone.GetComponentInChildren<Tower>().Setup(tiles, gridPositionList);
                        bullet.GetComponent<Beam>().Target(target);
                        shotTime = Time.time + timeBetweenShots;

                    }
                    if (Vector3.Distance(transform.position , target.transform.position) > 30f)
                    {
                        agent.isStopped = false;
                        enemyState = STATE.Search;
                    }

                }
                else if (target == null)
                {
                    agent.isStopped = false;
                    enemyState = STATE.Search;
                }

                break;

            case STATE.Die:
                agent.isStopped = false;
                agent.enabled = false;
                dead = true;
                Destroy(gameObject);
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
       

      


        if (target != null )
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= 10f)
            {
                agent.isStopped = true;

                if (Time.time >= shotTime)
                {
                    GameObject bullet = Instantiate(bulletShot, bulletSpawn.position, Quaternion.identity);
                    //clone.GetComponentInChildren<Tower>().Setup(tiles, gridPositionList);
                    bullet.GetComponent<Beam>().Target(target);
                    shotTime = Time.time + timeBetweenShots;
                    HP -= 50;
                    if (HP == 0)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
        else
        {
            hasTarget = false;
        }


    }

}
