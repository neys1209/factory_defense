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

    private bool isFire;

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
        isFire = false;
    }

    private void Start()
    {
        path = new NavMeshPath();
        _HP = 100;
        Damage = 10;
        StartCoroutine(SearchPlayer());
    }


    IEnumerator SearchPlayer()
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

                        //    agent.CalculatePath(enemyColliders[i].transform.position, path);
                        //if (path.status == NavMeshPathStatus.PathInvalid)
                        //{
                        //    agent.ResetPath();
                        //}

                        //if (path.status != NavMeshPathStatus.PathInvalid)
                        //{
                        //    if (path.status != NavMeshPathStatus.PathPartial)
                        //    {
                        //        if (Distance < shortDis)
                        //        {
                        //           // agent.CalculatePath(enemyColliders[i].transform.position, path);
                        //            //if (path.status != NavMeshPathStatus.PathInvalid)
                        //            //{
                        //            //    if (path.status != NavMeshPathStatus.PathPartial)
                        //            //    {   
                                   
                        //            target = enemyColliders[i].gameObject;
                        //            shortDis = Distance;
                        //            //    }
                        //            //}
                        //        }

                        //    }
                        //}
                      //  NavMeshPath path = new NavMeshPath();
                      //  bool  p = agent.CalculatePath(enemyColliders[i].transform.position, path);
                       
                        //if(agent.CalculatePath(enemyColliders[i].transform.position, path))
                        // {
                        //    continue;
                        ////     Debug.Log(i);
                        ////     Debug.Log(p);

                        // }
                        //if (path.status == NavMeshPathStatus.PathComplete)
                        //{
                        //   // agent.CalculatePath(enemyColliders[i].transform.position, path);
                        //    // 목적지 도달 가능
                        //    Debug.Log(target.name);
                        //    print("목적지 도달 가능");
                        //    target = enemyColliders[i].gameObject;

                        //    // 경로 설정                            
                        //   // agent.SetPath(path);

                           
                        //}

                        //else if (path.status == NavMeshPathStatus.PathPartial)
                        //{
                        //    agent.enabled = false;
                        //    // 목적지 인근 도달 가능
                        //    Debug.Log(target.name);
                        //    print("목적지 인근 도달 가능");
                        //    break;
                        //    // agent.ResetPath();
                        //}

                        //else if (path.status == NavMeshPathStatus.PathInvalid)
                        //{
                        //    agent.enabled = false;
                        //    // 목적지 도달 불가 : hasFoundPath 가 false 
                        //    Debug.Log(target.name);
                        //    print("목적지 도달 불가");

                        //    // 경로 재설정
                        //    break;
                        //   // agent.ResetPath();
                            
                        //}

                        //if (Distance < shortDis )
                        //{
                        //    //agent.CalculatePath(enemyColliders[i].transform.position, path);
                        //    //if (path.status != NavMeshPathStatus.PathInvalid)
                        //    //{
                        //    //    if (path.status != NavMeshPathStatus.PathPartial)
                        //    //    {
                        //            target = enemyColliders[i].gameObject;
                        //            shortDis = Distance;
                        //    //    }
                        //    //}
                        //}
                    }
                    //if (path.status == NavMeshPathStatus.PathComplete)
                    //{
                    //    hasTarget = true;
                    //    // 목적지 도달 가능
                    //    Debug.Log(target.name);
                    //    print("목적지 도달 가능");

                    //    // 경로 설정

                    //    agent.SetPath(path);
                    //}

                    //else if (path.status == NavMeshPathStatus.PathPartial)
                    //{
                    //    agent.enabled = false;
                    //    // 목적지 인근 도달 가능
                    //    Debug.Log(target.name);
                    //    print("목적지 인근 도달 가능");
                    //}

                    //else if (path.status == NavMeshPathStatus.PathInvalid)
                    //{
                    //    agent.enabled = false;
                    //    // 목적지 도달 불가 : hasFoundPath 가 false 
                    //    Debug.Log(target.name);
                    //    print("목적지 도달 불가");                       

                    //    // 경로 재설정

                    //    agent.ResetPath();
                    //}
                 //  bool p = agent.CalculatePath(target.transform.position, path);

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

    /*
    IEnumerator EnemyMove()
    {
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);
        Vector3 dir = new Vector3(agent.steeringTarget.x, animator.transform.position.y, agent.steeringTarget.z) - transform.position;
       transform.forward = dir;

        Debug.Log("cho");
        yield return new WaitForSeconds(1.25f);
    }
    

    void SearchEnemy()
    {
        agent.isStopped = true;

        enemyColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, Mathf.Infinity, enemyColliders, layerMask);

        shortDis = Vector3.SqrMagnitude(transform.position - enemyColliders[0].transform.position); // 첫번째를 기준으로 잡아주기 

        target = enemyColliders[0].gameObject;
        if (numColliders != 0)
        {
            for (int i = 0; i < numColliders; i++)
            {
                float Distance = Vector3.SqrMagnitude(transform.position - enemyColliders[i].transform.position);

                if (Distance < shortDis)
                {
                    target = enemyColliders[i].gameObject;
                    shortDis = Distance;
                }
            }

            enemyState = STATE.Move;
        }
    }
    */

    // private Vector3 dir;

    //void EnemyState(STATE state)
    //{
    //    // enemyState = state;

    //    switch (state)
    //    {
    //        case STATE.Search:
    //            SearchPlayer();
    //            break;

    //        case STATE.Move:

    //            //  StartCoroutine(EnemyMove());

    //            if (Vector3.SqrMagnitude(transform.position - target.transform.position) <= 30f)
    //            {
    //                agent.isStopped = true;
    //                enemyState = STATE.Attack;
    //            }
    //            if (target == null)
    //            {
    //                agent.isStopped = false;
    //                enemyState = STATE.Search;
    //            }

    //            break;


    //        case STATE.Attack:

    //            //   StopCoroutine(EnemyMove());
    //            transform.localRotation =
    //           Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(target.transform.position - transform.position), 5 * Time.deltaTime);


    //            if (target != null)
    //            {

    //                if (Time.time >= shotTime)
    //                {
    //                    GameObject bullet = Instantiate(bulletShot, bulletSpawn.position, Quaternion.identity);
    //                    //clone.GetComponentInChildren<Tower>().Setup(tiles, gridPositionList);
    //                    bullet.GetComponent<Beam>().Target(target);
    //                    shotTime = Time.time + timeBetweenShots;

    //                }
    //                if (Vector3.Distance(transform.position , target.transform.position) > 30f)
    //                {
    //                    agent.isStopped = false;
    //                    enemyState = STATE.Search;
    //                }

    //            }
    //            else if (target == null)
    //            {
    //                agent.isStopped = false;
    //                enemyState = STATE.Search;
    //            }

    //            break;

    //        case STATE.Die:
    //            agent.isStopped = false;
    //            agent.enabled = false;
    //            dead = true;
    //            Destroy(gameObject);
    //            break;
    //    }
    //}


    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            _HP -= 50;

            if(_HP == 0)
            Destroy(gameObject);
        }
    }

    void Update()
    {
       

        /*
        float fb = Vector3.Dot(transform.forward, _enemy.transform.position - transform.position);

        if (fb > 0)
        {
            Debug.Log("전방에 적 발견");
        }
        else if (fb < 0)
        {
            Debug.Log("후방에 적 발견");
        }
        */

        //  EnemyState(enemyState);                 
        //Vector3 dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
        //transform.forward = dir;

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), transform.forward, out hit, 5f))
        //{
        //    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        //    {
        //        Debug.DrawLine(transform.position + new Vector3(0, 1f, 0), hit.point);
        //        isFire = false;
        //    }
        //}
        //else
        //{
        //    isFire = true;
        //}
        //if (target != null && isFire)
        //{
        //    if (Vector3.Distance(transform.position , target.transform.position) <= 10f)
        //    {
        //        agent.isStopped = true;

        //        if (Time.time >= shotTime)
        //        {
        //            GameObject bullet = Instantiate(bulletShot, bulletSpawn.position, Quaternion.identity);
        //            //clone.GetComponentInChildren<Tower>().Setup(tiles, gridPositionList);
        //            bullet.GetComponent<Beam>().Target(target);
        //            shotTime = Time.time + timeBetweenShots;
        //            enemyShooter.HP -= 10;
        //            if (enemyShooter.HP == 0)
        //            {
        //                Destroy(gameObject);
        //            }
        //        }
        //    }
        //}
        //else
        //{            
        //    hasTarget = false;
        //}



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
                    //_HP -= 50;
                    //if (_HP == 0)
                    //{
                    //    Destroy(gameObject);
                    //}
                }
            }
        }
        else
        {
            hasTarget = false;
        }


    }

}
