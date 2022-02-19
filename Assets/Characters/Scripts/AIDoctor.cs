using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIDoctor : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeInterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int m_CurrentWayPointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;

    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWayPointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[m_CurrentWayPointIndex].position);
    }


    void Update()
    {
        EnviromentView();

        if(!m_IsPatrol)
        {
            Chasing();

        }
        else
        {
            Patroling();
        }
    }
    private void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if(!m_CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if(m_WaitTime <=0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 6f)
            {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWayPointIndex].position);
            }
            else
            {
                if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position)>= 2.5f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }
    private void Patroling()
    {
        if(m_PlayerNear)
        {
            if(m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWayPointIndex].position);
            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if(m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }
    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {
        m_CurrentWayPointIndex = (m_CurrentWayPointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWayPointIndex].position);
    }




    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if(Vector3.Distance(transform.position, player)<= 0.3)
        {
            if(m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWayPointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position , viewRadius , playerMask);

        for(int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward , dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position , player.position);
                if(!Physics.Raycast(transform.position , dirToPlayer , dstToPlayer , obstacleMask))
                {
                    m_PlayerInRange = true;
                    m_IsPatrol = false;

                }
                else
                {
                    m_PlayerInRange = false;
                }
            }
            if(Vector3.Distance(transform.position , player.position) > viewRadius)
            {
                m_PlayerInRange = false;
            }

            if(m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }

        }

    }

    /*    public GameObject Doctor;

        [Header("Layer Mask")]
        [Tooltip("Which Layers can be walked on?")]
        public LayerMask walkableMask;

        Transform groundCheck;
        float gravity;
        bool isGrounded;
        Vector3 velocity;


        public Transform target;
        public float speed = 1f;

        Rigidbody rigD, rigA;

        public float MoveSpeed = 1;
        int MaxDist = 6;
        int MinDist = 2;

        // Start is called before the first frame update
        void Start()
        {
            rigD = Doctor.GetComponent<Rigidbody>();
            *//*        rigA = attackRange.GetComponent<Rigidbody>();*/

    /*        Physics.gravity = Vector3.down * 20;
            groundCheck = transform.Find("GroundCheckD");*/
    /*        gravity = Physics.gravity.y;*/

    /*        source = Doctor.GetComponent<AudioSource>();*//*
}*/

    // Update is called once per frame
    /*    void Update()
        {
    *//*        velocity.y += gravity * Time.deltaTime;
            isGrounded = Physics.CheckSphere(groundCheck.position , 0.2f , walkableMask);*//*

            if(Vector3.Distance(transform.position , target.position) *//*>= MinDist*//* <= MaxDist)
            {

                if(Vector3.Distance(transform.position , target.position) *//*<= MaxDist*//* <= MinDist)
                {
                    Doctor.GetComponent<Animator>().Play("PunchingL");
                    Doctor.GetComponent<AudioSource>().Play();
                    // Put what do you want to happen here
                    rigD.velocity = Vector3.zero;
                    rigD.angularVelocity = Vector3.zero;
                    Doctor.GetComponent<Animator>().Play("ZombieKick_L");
                    Doctor.GetComponent<Animator>().Play("NeutralIdle");



                }
                else
                {
                    transform.LookAt(target);

                    Doctor.GetComponent<Animator>().Play("Walking");

                    transform.position += transform.forward * MoveSpeed * Time.deltaTime;

                    Vector3 pos = Vector3.MoveTowards(transform.position , target.position , speed * Time.fixedDeltaTime);
                    rigD.MovePosition(pos);
                }
            }
            else
            {
                Doctor.GetComponent<Animator>().Play("Neutral Idle");

            }
        }*/
    /*    void FixedUpdate()
        {
            Follow();
        }

        public void Follow()
        {
            Vector3 lookVector = target.transform.position - transform.position;
            lookVector.y = transform.position.y;
            Quaternion rot = Quaternion.LookRotation(lookVector);
            transform.rotation = Quaternion.Slerp(transform.rotation , rot , 1);

            Vector3 pos = Vector3.MoveTowards(transform.position , target.position , speed * Time.fixedDeltaTime);
            rigD.MovePosition(pos);

            Doctor.GetComponent<Animator>().Play("Walking");
        }*/
}
