using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Script : MonoBehaviour
{

    public NavMeshAgent agent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 10f;
    public float viewAngle = 60f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] wayPoints;
    private Transform player;
    int m_currentWayPointIndex = 0;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_playerPosition;

    float m_WaitTime;
    float m_TimeToRotate;

    bool m_playerInRange;
    bool m_playerNear;
    bool m_isPatrol;
    bool m_caughtPlayer;


    // Start is called before the first frame update
    void Start()
    {
        m_playerPosition = Vector3.zero;
        //m_playerNear = true;
        m_isPatrol = true;
        //m_playerNear = true;
        m_caughtPlayer = false;
        m_playerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;
        player = GameObject.FindWithTag("Player").transform;
        m_currentWayPointIndex = 0;
        agent = GetComponent<NavMeshAgent>();

        agent.isStopped = false;
        agent.speed = speedWalk;

        if (m_currentWayPointIndex >= 0 && m_currentWayPointIndex < wayPoints.Length)
        {
            agent.SetDestination(wayPoints[m_currentWayPointIndex].position);
        }
        else
        {
            Debug.LogError("Invalid waypoint index: " + m_currentWayPointIndex);
        }

    }

    // Update is called once per frame
    void Update()
    {
        environemntView();
        if (!m_isPatrol)
        {
            Chasing();
        }
        else
        {
            Patrolling();
        }

    }

    void CaughtPlayer()
    {
        m_caughtPlayer |= true;
    }

    void lookingPlayer(Vector3 player)
    {
        agent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_playerNear = false;
                Move(speedWalk);
                agent.SetDestination(wayPoints[m_currentWayPointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;


            }
            else
            {
                Stop();
            }
        }
    }

    void Move(float speed)
    {
        agent.isStopped = false;
        agent.speed = speed;
    }

    void Stop()
    {
        agent.isStopped = true;
        agent.speed = 0;
    }

    public void nextPoint()
    {
        m_currentWayPointIndex = (m_currentWayPointIndex + 1) % wayPoints.Length;
        agent.SetDestination(wayPoints[m_currentWayPointIndex].position);
    }

    void environemntView()
    {
        print("Checking player visibility...");

        Collider[] playerRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        bool playerDetected = false;  // Flag to track if player is detected

        for (int i = 0; i < playerRange.Length; i++)
        {
            print("Checking...");
            Transform player = playerRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                // checks if the player is within the enemy's detection radius
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;
                    playerDetected = true;
                    m_isPatrol = false;
                    print("player is within the enemy's detection radius");
                    break; // Player detected, exit the loop

                }
                else
                {
                    m_playerInRange = false;
                    print("not within the radius");
                }

            }
            if (Vector3.Distance(transform.forward, player.position) > viewRadius)
            {
                m_playerInRange = false;
            }
            if (m_playerInRange)
            {
                m_playerPosition = player.transform.position;
            }
        }
        // Set player detection status based on the result of the loop
        m_playerInRange = playerDetected;

        if (!m_playerInRange)
        {
            print("Player not detected.");
        }

    }

    private void Patrolling()
    {
        if (m_playerNear)
        {
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                lookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_playerNear = false;
            playerLastPosition = Vector3.zero;
            agent.SetDestination(wayPoints[m_currentWayPointIndex].position);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (m_WaitTime <= 0f)
                {
                    nextPoint();
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

    private void Chasing()
    {
        m_playerNear = true;
        playerLastPosition = Vector3.zero;
        if (!m_caughtPlayer)
        {
            Move(speedRun);
            agent.SetDestination(m_playerPosition);
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (m_WaitTime <= 0 && !m_caughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                m_isPatrol = false;
                m_playerNear = false;
                Move(speedWalk);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
                agent.SetDestination(wayPoints[m_currentWayPointIndex].position);

            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;


                }
            }
        }
    }
}
