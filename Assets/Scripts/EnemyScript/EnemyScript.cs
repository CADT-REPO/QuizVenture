using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{

    public GameTimeManager gameTimeManager;
    public NavMeshAgent agent;
    public Animator animator;
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

    public int Health = 10;
    private int hitCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Enemy spawned: " + gameObject.name);  // This should print for each instance
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
        animator = GetComponent<Animator>();


        agent.isStopped = false;
        agent.speed = speedWalk;

        if (m_currentWayPointIndex >= 0 && m_currentWayPointIndex < wayPoints.Length)
        {
            agent.SetDestination(wayPoints[m_currentWayPointIndex].position);
            print(wayPoints[m_currentWayPointIndex].position);
        }
        else
        {
            // Debug.LogError("Invalid waypoint index: " + m_currentWayPointIndex);
        }

    }


    public void PlayHitTransition()
    {
        if (animator != null)
        {

            animator.SetTrigger("Hit");
        }
    }

    public void OnBulletHit()
    {
        hitCount++;
        gameTimeManager.DeductTime(10f);
        print("Player caught, deducting time...");
        if (hitCount == 1)
        {
            // idle attack mode
            Debug.Log("Playing Hit Transition Animation");
            animator.SetTrigger("Hit");

        }
        else
        {
            // attack mode 
            Debug.Log("Switching to Attack Mode Animation");
            animator.SetTrigger("Attack");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Enemy active: " + gameObject.activeSelf);  // Log if the object is still active
        if (gameTimeManager == null)
        {
            Debug.LogError("gameTimeManager is null!");
            return;
        }
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
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Debug.Log("Enemy has been slained");
            //animator.SetTrigger("Die");
            Die();
            //Destroy(this.gameObject);
        }
        else if (Health <= 2)
        {
            Debug.Log("Enemy stunned");
            animator.SetTrigger("Dizzy");
        }
    }

    public void Die()
    {
        Debug.Log("Enemy has been slain");
        animator.SetTrigger("Die");

        // Start a coroutine to wait for the animation to finish before destroying the object
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        // Get the length of the "Die" animation (optional, see note below)
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        // Wait for the animation to complete
        yield return new WaitForSeconds(animationDuration);

        // Destroy the enemy object
        Destroy(this.gameObject);
    }


    void CaughtPlayer()
    {
        m_caughtPlayer |= true;

        // 
    }

    void lookingPlayer(Vector3 player)
    {
        agent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 1.0f)
        {
            if (m_WaitTime <= 0)
            {
                m_playerNear = false;
                Move(speedWalk);
                agent.SetDestination(wayPoints[m_currentWayPointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
                gameTimeManager.DeductTime(10f);
                print("Player caught, deducting time...");

            }
            else
            {
                gameTimeManager.DeductTime(10f);
                print("Player caught, deducting time...");

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
        // print("Checking player visibility...");

        Collider[] playerRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        bool playerDetected = false;  // Flag to track if player is detected

        for (int i = 0; i < playerRange.Length; i++)
        {
            // print("Checking...");
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
                    // print("player is within the enemy's detection radius");
                    break; // Player detected, exit the loop

                }
                else
                {
                    m_playerInRange = false;
                    // print("not within the radius");
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
            // print("Player not detected.");
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
                print("Player Nearby, Chase!");
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
