using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;

public enum LightningballallState
{
    following,
    returning,
    attacking
}
public class LightningballController : MonoBehaviour
{
    private GameObject player;
    private float playerDistance; //how far away from the player we are, used for follow distance and callback possibly
    private float followDistance = 0.5f;
    private float followBuffer = 0.1f;
    public LightningballallState currentState;
    public Rigidbody2D rb;
    private BallLightning bl;

    //movement
    private Vector3 direction;
    public float baseSpeed;
    private float speed;
    private float maxVelocity = 1.5f;

    public LightningBallFinder finder;
    private GameObject recentTarget;

    //attacking
    public GameObject currentTarget; //who to attack. If null, return to player.
    private float targetDistance; //Zip towards target based on distance
    private PlayerMovement pm;
    public PlayerStats stats;
    public bool criticalHit = false;
    public int baseDamage;
    public DamageType damageType = DamageType.lightning;
    public bool attacking = false;
    private bool firstTarget = false;


    //damage
    private CircleCollider2D cc;
    public int maxHits = 10;
    public int hitCount = 0;
    



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bl = player.GetComponentInChildren<BallLightning>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        speed = baseSpeed * 1000;
        cc = GetComponent<CircleCollider2D>();
        pm = player.GetComponent<PlayerMovement>();
        stats = player.GetComponent<PlayerStats>();
        finder = GetComponentInChildren<LightningBallFinder>();
        baseDamage = bl.baseDamage;


    }

    void Update()
    {
        switch (currentState)
        {
            
            case LightningballallState.following:
                //max velocity
                if(rb.velocity.x > maxVelocity)
                {
                    rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
                }
                if (rb.velocity.y > maxVelocity)
                {
                    rb.velocity = new Vector2(rb.velocity.x, maxVelocity);
                }
                //find distance to player and move until you are within a good range of it.
                playerDistance = (gameObject.transform.position - player.transform.position).magnitude;
                if(playerDistance >= (followDistance + followBuffer))
                {
                    FollowPlayerMove();
                    break;
                }
                if(playerDistance <= (followDistance - followBuffer))
                {
                    BackAwayFromPlayer();
                    break;
                }
                break;
            case LightningballallState.attacking:

                break;

            case LightningballallState.returning:
                playerDistance = (gameObject.transform.position - player.transform.position).magnitude;
                if (playerDistance >= (followDistance + followBuffer))
                {
                    FollowPlayerMove();
                    break;
                }
                if (playerDistance <= (followDistance - followBuffer))
                {
                    rb.velocity = Vector3.zero;
                    currentState = LightningballallState.following;
                    finder.FinderOnOff();
                    break;
                }
                break;

        }
    }

    private void FollowPlayerMove()
    {
        direction = (player.transform.position - gameObject.transform.position).normalized;
        rb.AddForce(direction * speed * playerDistance * Time.deltaTime);
    }

    private void BackAwayFromPlayer()
    {
        direction = -(player.transform.position - gameObject.transform.position).normalized;
        rb.AddForce(direction * speed / 4 * Time.deltaTime);
    }

    public void CommandBall()//called by the Ability istelf when button is pressed.
    {
        switch (currentState)
        {
            case LightningballallState.following:
                Debug.Log("Command given: Attack!");
                currentState = LightningballallState.attacking;
                // change state to attacking, then shoot out in direction of player facing
                var shootDir = new Vector2(pm.lastHoriz, pm.lastVert);
                rb.velocity = shootDir * 40;
                StartCoroutine(LookForEnemies());
                break;

            case LightningballallState.attacking:
                Debug.Log("Command given: Return");
                StopAllCoroutines();
                attacking = false;
                firstTarget = false;
                finder.enemies.Clear();
                currentState = LightningballallState.returning;
                //change target to the player so that the ball zips back, then once within a certain distance set state to following
                break;

            case LightningballallState.returning:
                break;
        }
        finder.FinderOnOff();
    }

    private IEnumerator LookForEnemies()
    {
        var timer = 0.2f;
        var elapsedTime = 0f;
        while(elapsedTime <= timer)
        {
            if (finder.enemies.Count == 0)//no enemies yet, keep looking
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                StopCoroutine(LookForEnemies());//enemy found, zipzoop will be called
                yield return null;
            }
        }
        rb.velocity /= new Vector3(-5f, -5f, 1f);
        Debug.Log(rb.velocity);
        CommandBall();//no enemy was ever found, so return ball
    }



    public IEnumerator ZipZoop()
    {
        attacking = true;
        //first, find whicever enemy is closest
        Debug.Log("starting foreachloop");
        while(finder.enemies.Count == 0)
        {

            yield return null;
        }
        rb.velocity = Vector3.zero;
        foreach (GameObject enemy in finder.enemies)
        {

            if (firstTarget == false)//first enemy is automatically made the target
            {
                targetDistance = (gameObject.transform.position - enemy.transform.position).magnitude;
                Debug.Log(targetDistance);
                currentTarget = enemy;
                firstTarget = true;
            }
            var tempDistance = (gameObject.transform.position - enemy.transform.position).magnitude;
            Debug.Log("tempDistance = " + tempDistance);
            if (tempDistance < targetDistance && enemy != recentTarget)//compare subsequent enemies in list, find closest.
            {

                targetDistance = tempDistance;
                currentTarget = enemy;
                Debug.Log("New shortest distance found, updating enemy target");
            }
        }
        //if(currentTarget = null)
        //{
        //    CommandBall();//No valid enemy found, stop attack.
        //}
        Debug.Log("Enemy target decided. Start zipp.");
        //now that an enemy has been decided, we zip towards that enemy
        var randomizedLoc = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0f);
        direction = (currentTarget.transform.position - gameObject.transform.position) + randomizedLoc;//find full vector towards enemy
        var targetLocation = (currentTarget.transform.position + direction.normalized) + randomizedLoc; //new taget location is on the exact opposite side of the enemy from current position
        targetDistance = (gameObject.transform.position - targetLocation).magnitude;
        Debug.Log("starting while loop");
        var startingLocation = gameObject.transform.position;
        var distanceMoved = 0f;
        while (distanceMoved <= targetDistance)
        {
            rb.velocity = direction.normalized * speed * 8 *  Time.deltaTime;
            distanceMoved = (gameObject.transform.position - startingLocation).magnitude;
            Debug.Log("distance left: " + (distanceMoved - targetDistance));
            yield return null;
        }
        recentTarget = currentTarget;
        //currentTarget = null;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.15f);

        attacking = false;
        hitCount += 1;

        if (finder.enemies.Count > 0)
        {
            StartCoroutine(ZipZoop());
        }
        else
        {
            CommandBall();//return ball to player
        }
        yield return null;
    }
}

    

