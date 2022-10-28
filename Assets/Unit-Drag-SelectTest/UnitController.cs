using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : Player1
{
    [SerializeField]
    private GameObject unitMarker;
   // private NavMeshAgent agent;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        //  enemyShooter = new Enemy();
        animator = GetComponent<Animator>();
      //  agent = GetComponent<NavMeshAgent>();
        dead = false;
        hasTarget = false;
        // agent.updateRotation = false;
        isFire = false;

        _HP = 100;
    }

    public void SelectUnit()
    {
        unitMarker.SetActive(true);
    }

    public void DeselectUnit()
    {
        unitMarker.SetActive(false);
    }

    public void MoveTo(Vector3 end)
    {
       // navMeshAgent.SetDestination(end);
        StartCoroutine(SearchEnemy());
    }

    
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

    NavMeshPath path;



    IEnumerator SearchEnemy()
    {
        while (!dead)
        {
            if (hasTarget)
            {

                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(target.transform.position);
                //  agent.SetDestination(path.corners[path.corners.Length -1]);
                //Vector3 dir = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z) - transform.position;
                //transform.forward += dir;

            }

            else
            {

                   navMeshAgent.enabled = true;
                   navMeshAgent.isStopped = true;

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
                    navMeshAgent.enabled = false;

                }

            }
            // Debug.Log(target.name);
            yield return new WaitForSeconds(0.25f);

        }


    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            _HP -= 50;

            if (_HP == 0)
                Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= 10f)
            {
                navMeshAgent.isStopped = true;

                if (Time.time >= shotTime)
                {
                    GameObject bullet = Instantiate(bulletShot, bulletSpawn.position, Quaternion.identity);
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

