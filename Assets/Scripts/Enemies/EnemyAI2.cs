using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyState
{
    aggro,
    retreating,
    idle,
    attack,
    dead,
    flee,
    special
}


public class EnemyAI2 : MonoBehaviour
{
    

    public EnemyState currentState;
    [HideInInspector] public Transform playerLocation;

    //Pathfinding
    Seeker seeker;
    private float nextWaypointDistance = 0.5f;                  //Lower number for smoother, more often updated pathfinding
    Path path;                                                  //current path we are following
    private int currentWaypoint = 0;                            //which point on the path we are targeting, starting with 0
    private bool reachedEndOfPath = false;                      //currently not changing anything, but useful in future maybe.

    //Movement
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public float lastHoriz;
    [HideInInspector] public float lastVert;
    [HideInInspector] public Vector2 facing;                    //used to translate movement to last horiz/vert
    public float inputspeed;
    private float speed;
    private Vector2 force;
    private float movementDeadZone = 0.05f;
    private bool updatingFacing;
    [HideInInspector] public float startAnimSpeed;
    public float aggroSpeedMultiplier;
    private Vector2 oldFacing = new Vector2(1,-1);
    public bool stunned = false;

    //Idling
    private bool idlePathChosen = true;
    [HideInInspector] public Vector2 spawnLocation;
    [HideInInspector] public Vector2 idleTarget;
    public float wanderRange;
    private float wanderTime;
    private float fleeTime;
    [HideInInspector] public bool idleWait = false;

    //Attacking and Aggro
    [HideInInspector] public bool following = false;
    private bool waitingToUpdateAggro = false;
    public bool attacking;
    [HideInInspector] public EnemyIntBox enemyIntBox;
    [HideInInspector] public EnemyRange range;
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public GameObject player;
    public bool fleeWhenTooClose;
    public float fleeRange;
    private Vector3 fleePoint;
    [HideInInspector] public bool tooClose;
    public bool freezeFacingWhileAttacking;
    public bool specialMove = false;

    //Swarm
    private Transform swarmCenter;
    public float swarmRange;
    public LayerMask enemyLayers;
    public List<Collider2D> inSwarm;
    private Vector2 swarmDirection;
    [HideInInspector] public CircleCollider2D cc;

    //This Enemy
    private Component enemyController;
    private string enemyName;
    public DamageType dt;

    //Knockback
    public bool knockback = false;
    private Vector2 knockbackDirection;

    void Start()
    {
        //This Enemy
        enemyName = gameObject.name;
        enemyController = gameObject.GetComponent(enemyName);



        //Initialized Components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        range = GetComponentInChildren<EnemyRange>();
        anim = GetComponent<Animator>();
        enemyIntBox = GetComponentInChildren<EnemyIntBox>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        cc = GetComponent<CircleCollider2D>();

        //Initialized Variables
        currentState = EnemyState.idle;
        spawnLocation = (Vector2)GetComponent<Transform>().position;
        idleTarget = spawnLocation + new Vector2(Random.Range(-1,1), Random.Range(-1,1));
        speed = inputspeed * 1000;     // times 1000 to simplify the menu selected speed
        startAnimSpeed = anim.speed;
        //Debug.Log("start anim Speed " + startAnimSpeed);

        playerLocation = player.transform;

        InvokeRepeating("ConstantFacingUpdate", 0.1f, 0.1f);
    }



    void FixedUpdate()
    {
        if(gameObject.transform.position.z != 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
        }
        switch (currentState)
        {
            case EnemyState.dead:
                break;
            case EnemyState.special:
                break;

            case EnemyState.attack:
                CheckFlee();
                AttackState();
                break;

            case EnemyState.aggro:
                if (fleeWhenTooClose && range.playerInRange == true && tooClose == false)
                {
                    currentState = EnemyState.attack;
                }
                if (!waitingToUpdateAggro)
                {
                    StartCoroutine(UpdatePathOccasionally());
                }
                CheckFlee();
                MovementState();
                break;

            case EnemyState.retreating:
                MovementState();
                break;

            case EnemyState.idle:
                CheckFlee();
                MovementState();
                break;

            case EnemyState.flee:
                MovementState();
                break;
        }
    }

    private void AttackState()
    {
        if (playerHealth.isDead)
        {
            currentState = EnemyState.idle;
            return;
        }
        if (!attacking)
        {
            if (playerHealth.currentHealth > 0)
            {
                enemyController.SendMessage("Attack");
            }
        }
    }
    private void CheckFlee()
    {
        if (fleeWhenTooClose && tooClose && range.playerInRange == true && !attacking && !playerHealth.isDead)
        {
            currentState = EnemyState.flee;
        }

    }
    private void MovementState()
    {
        if (path == null)
        {
            UpdatePath();
            return;
        }

        CheckPlayerHP();

        //Check if we have reached the end of the Path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            //stop moving
            reachedEndOfPath = true;
            rb.velocity = Vector2.zero;
            //lastHoriz = force.x;
            //lastVert = force.y;

            //Cases
                //Done leaving aggro back to spawn
                if (currentState == EnemyState.retreating) 
                {
                    idlePathChosen = true;
                    currentState = EnemyState.idle;
                    //Then we want it to segue into the idle loop =>
                }
                //Done Ilde Walking or coming from retreating
                if (currentState == EnemyState.idle && idlePathChosen == true)
                {

                    idlePathChosen = false;
                    StartCoroutine(ChooseNewIdleTarget());
                    return;
                }
                //Done runing away
                if (currentState == EnemyState.flee)
                {
                    //reach end point, turn arround and cast a spell at the player
                    currentState = EnemyState.attack;
                    enemyController.SendMessage("Attack");
                    return;
                }
                return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        //Movement

            //Swarm
                swarmDirection = Vector2.zero;  //reset swarm direction info so as to not grow to infinite amounts
                swarmCenter = gameObject.transform;
                inSwarm = new List<Collider2D>(Physics2D.OverlapCircleAll(swarmCenter.position, swarmRange, enemyLayers)); //Check which enemies are within his swarm range
                foreach (Collider2D enemy in inSwarm)
                {
                    swarmDirection += ((Vector2)enemy.transform.position - rb.position);
                }
                swarmDirection = new Vector2(swarmDirection.x * 1f, swarmDirection.y * 1f);

        //Direction
        if (stunned)
        {
            speed = inputspeed *500;
        }
        else
        {
            speed = inputspeed * 1000;

        }
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                if (currentState == EnemyState.aggro || currentState == EnemyState.flee)
                {
                    //Aggro speed multiplier
                    force = (direction - swarmDirection).normalized * speed * aggroSpeedMultiplier * Time.deltaTime; 
                    if (anim.speed == startAnimSpeed)
                    {
                        anim.speed *= aggroSpeedMultiplier / 2;
                    }

                }
                else
                {
                    //normal speed
                    force = (direction - swarmDirection).normalized * speed * Time.deltaTime; // slower wandering speed
                    anim.speed = startAnimSpeed;
                }
                rb.AddForce(force);
                CalledFacingUpdate();

        //finally, we want to figure out our next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            //then we have reached the current waypoint
            currentWaypoint++;  //we are ready to move onto the next waypoint!
        }

    }
       
    private void CheckPlayerHP()
    {
        if (playerHealth.currentHealth <= 0)
        {
            currentState = EnemyState.idle;
            return;
        }
    }


    public IEnumerator ChooseNewIdleTarget()
    {
        idleWait = true;
        wanderTime = Random.Range(4f, 6f);
        //anim.SetBool("Moving", false);

        yield return new WaitForSeconds(wanderTime);
        idleWait = false;
        idleTarget = spawnLocation + new Vector2(Random.Range(-wanderRange, wanderRange), Random.Range(-wanderRange, wanderRange));
        //anim.SetBool("Moving", true);
        UpdatePath();
    }

    public void ConstantFacingUpdate() //invokerepeating from start ever 0.1 seconds
    {
        //also check if enemy is on a zero z height
        if(gameObject.transform.position.z != 0f)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0f);
        }
        //now update facing
        if (!attacking)
        {
            if (Mathf.Abs(rb.velocity.x) < movementDeadZone && Mathf.Abs(rb.velocity.y) < movementDeadZone)
            {
                anim.SetBool("Moving", false);
                //Debug.Log("Not moving : " + rb.velocity);
            }
            else if (Mathf.Abs(force.x) > movementDeadZone || Mathf.Abs(force.y) > movementDeadZone) //this line is to stop the enemy from changing direction if it is getting just a little moved by the player pushing into them. The deadzone can be adjusted in the variables.
            {
                anim.SetBool("Moving", true);
                //Debug.Log("Moving : " + force);
            }


        }
        CalledFacingUpdate();
    }

    public void CalledFacingUpdate()
    {
        if (!updatingFacing)
        {
            if (!attacking)
            {
                updatingFacing = true;
                //yield return new WaitForSeconds(0.1f);
                facing = force.normalized;
                lastHoriz = Mathf.Round(facing.x);
                lastVert = Mathf.Round(facing.y);

                if(lastHoriz != 0 || lastVert != 0)
                {
                    oldFacing = new Vector2(lastHoriz, lastVert);
                }
                if(lastHoriz == 0 && lastVert == 0) //catch errors that make box vars 0
                {
                    lastHoriz = oldFacing.x;
                    lastVert = oldFacing.y;
                }


                anim.SetFloat("LastVertical", lastVert);
                anim.SetFloat("LastHorizontal", lastHoriz);
                updatingFacing = false;
                
            }
            if (attacking && !freezeFacingWhileAttacking)
            {

                updatingFacing = true;
                //yield return new WaitForSeconds(0.1f);
                facing = (player.transform.position - gameObject.transform.position).normalized; //updates facing based on player location, generally while not moving
                lastHoriz = Mathf.Round(facing.x);
                lastVert = Mathf.Round(facing.y);
                if (lastHoriz != 0 || lastVert != 0)
                {
                    oldFacing = new Vector2(lastHoriz, lastVert);
                }
                if (lastHoriz == 0 && lastVert == 0) //catch errors that make box vars 0
                {
                    lastHoriz = oldFacing.x;
                    lastVert = oldFacing.y;
                }
                anim.SetFloat("LastVertical", lastVert);
                anim.SetFloat("LastHorizontal", lastHoriz);
                updatingFacing = false;
            }
            if (enemyIntBox != null)
            {
                enemyIntBox.UpdateOffset();

            }
        }
    }


    public void TooClose()
    {
        if (fleeWhenTooClose)
        {

            float distanceToPlayer = (gameObject.transform.position - player.transform.position).magnitude;
            Vector3 directionToPlayer = (player.transform.position - gameObject.transform.position).normalized;
            if (distanceToPlayer <= fleeRange)
            {
                tooClose = true;
                fleePoint = directionToPlayer * (-1) * 2;
                fleeTime = 3f;
                StartCoroutine(FleeTime());
                return;
            }
            else
            {
                tooClose = false;
            }
        }

    }

    IEnumerator FleeTime()
    {
        yield return new WaitForSeconds(fleeTime);
        currentState = EnemyState.attack;
        enemyController.SendMessage("Attack");
        yield return null;
    }

    public void UpdatePath()                        //Depending on EnemyState, pick a different target to Path to.
    {
        if(currentState == EnemyState.special || currentState == EnemyState.attack)
        {
            return;
        }

        CalledFacingUpdate();

        if (seeker.IsDone())
        {
            var destination = new Vector2(0, 0);
            if (currentState == EnemyState.flee)
            {
                destination = fleePoint;
            }
            else if (currentState == EnemyState.aggro)
            {
                destination = playerLocation.position;
            }
            else if (currentState == EnemyState.retreating)
            {
                destination = spawnLocation;
            }
            else if (currentState == EnemyState.idle)
            {
                destination = idleTarget;
                idlePathChosen = true;
            }
            else
            {
                Debug.LogError("Seeker.isDone() checked for EnemyState " + currentState + ", but no EnemyState found.");
            }
            seeker.StartPath(rb.position, destination, OnPathComplete);
        }
    }
    public IEnumerator UpdatePathOccasionally()     //This function is to tonly update the Pathfinding AI at intervals to stop it from trying to update more than once per frame
    {

        waitingToUpdateAggro = true;
        yield return new WaitForSeconds(0.5f);
        UpdatePath();
        waitingToUpdateAggro = false;
        yield return null;
    }
    void OnPathComplete(Path p)                     //This function exists to keep the path from failing, resets the path when finished and if it stops working
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public void CheckEnemyState()
    {
        //Debug.Log("CheckEnemyState");
        if (currentState != EnemyState.dead)
        {
            if (enemyIntBox.playerInRange == true)
            {
                currentState = EnemyState.attack;
            }
            else
            if (following)
            {
                currentState = EnemyState.aggro;
            }
            else
            {
                currentState = EnemyState.idle;
            }
        }
    }

    private void OnDrawGizmosSelected()             //Draw the anit-swarm range for calculations
    {
        if (swarmCenter == null)
            return;
        Gizmos.DrawWireSphere(swarmCenter.position, swarmRange);
    }

    public void Knockback(Vector2 source)
    {
        if (currentState != EnemyState.dead)
        {
            knockbackDirection = (source - (Vector2)gameObject.transform.position).normalized;
            //lastHoriz = knockbackDirection.x;
            //lastVert = knockbackDirection.y;
            StartCoroutine(KnockbackCoroutine());

        }
    }
    IEnumerator KnockbackCoroutine()
    {
        knockback = true;
        anim.SetBool("Knockback", true);
        float duration = 1f;
        float normalizedTime = 0;
        rb.velocity = (-knockbackDirection * 10);
        while (normalizedTime <= 0.3f)
        {
            normalizedTime += Time.deltaTime / duration;
            rb.AddForce(-knockbackDirection * 1);
            yield return null;
        }
        knockback = false;
        anim.SetBool("Knockback", false);
        yield return null;
    }
}
