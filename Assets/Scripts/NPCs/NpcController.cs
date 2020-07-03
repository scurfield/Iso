using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum NpcState
{
    pointWalking,
    idle,
    talking
    
}

public class NpcController : MonoBehaviour
{
    public NpcState currentstate;
    private Transform playerLocation;
    GameObject player;

    //Pathfinding
    Seeker seeker;
    private float nextWaypointDistance = 0.5f;                  //Lower number for smoother, more often updated pathfinding
    Path path;                                          //current path we are following
    private int currentWaypoint = 0;                            //which point on the path we are targeting, starting with 0
    private bool reachedEndOfPath = false;

    //Movement
    private Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D cc;
    private Animator anim;
    public float speed;
    private float calculatedSpeed;
    private Vector2 direction;
    public float lastHoriz;
    public float lastVert;
    public Vector2 facing;
    private Vector2 force;
    private bool updatingFacing;

    private bool talking = false;

    public List<GameObject> walkPoints;
    private GameObject target;
    private int nextWalkPoint = 0;
    private int maxWalkPoints;
    private bool pointPathChosen = false;
    private bool pointIdle;
    private float pointIdleTime;
    private bool pointWalking;
    WalkPointData wpd;

    //Talking
    public bool turnToPlayer = false;
    private Interactable interactable;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        interactable = GetComponent<Interactable>();

        calculatedSpeed = speed * 1000;

        currentstate = NpcState.pointWalking;
        maxWalkPoints = walkPoints.Count -1; //minnus one because the List starts at 0 and if  you count to the list.count.end you'll run out of list.
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StartCoroutine("CalledFacingUpdate");

        if (currentstate == NpcState.talking) //if talked to, stop the npc and face the player
        {
            rb.velocity = Vector2.zero;
            //The Talking functions are all organized at the bottom in Talk()
        }
        if(currentstate == NpcState.idle) //this mode is used to stand still. NPCs intended to Idle will not have a rigidbody.
        {
            //npcstate can be moved only to talking from here
            if (pointWalking == true)
            {
                currentstate = NpcState.pointWalking;
            }
        }

        if(currentstate == NpcState.pointWalking)
        {

            if (path == null)
            {
                //Debug.Log("path null");
                UpdatePath(nextWalkPoint);
                return;
            }
            if (currentWaypoint >= path.vectorPath.Count) //If the point we are on is the final point or beyond it, the path is over
            {
                //Debug.Log("reached end of path");
                reachedEndOfPath = true;
                rb.velocity = Vector2.zero;
                force.x = lastHoriz;
                force.y = lastVert;
                //We have reached the walk point. By default, we will make it so the NPC stands here for a few seconds before moving on.
                //now that this path is complete, we will idle for a bit before continuing to the new-chosen path.
                if(pointIdle == true)
                {
                    pointPathChosen = true;
                    StartCoroutine("PointIdle");
                    
                    CheckNextPoint();

                    return;
                }

                CheckNextPoint();

            }
            else
            {

                reachedEndOfPath = false;
            }

            //Movement
            //This part is actually where the movement forces are calculated.

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            force = direction * calculatedSpeed * Time.deltaTime;
            rb.AddForce(force);
            StartCoroutine("CalledFacingUpdate");

            //finally, we want to figure out our next waypoint
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                //then we have reached the current waypoint
                currentWaypoint++;  //we are ready to move onto the next waypoint!
            }
        }


    }

    private void CheckNextPoint()
    {
        if (pointPathChosen == true) //reset the target and pick the next walkpoint
        {
            //Debug.Log("no pointpathchosen. Pick next point");

            pointPathChosen = false;
            if (nextWalkPoint == maxWalkPoints)
            {
                //Debug.Log("reset to 0");
                nextWalkPoint = 0;
                UpdatePath(nextWalkPoint);//if at end of all walkpoint, go to the first one in a loop
            }
            else
            {
                nextWalkPoint++;
                //Debug.Log("Nextwalkpoint plus 1, not at " + nextWalkPoint);
                UpdatePath(nextWalkPoint);//or simply go to next walkpoint
            }
        }
        return;
    }

    public IEnumerator CalledFacingUpdate()
    {
        if (!updatingFacing)
        {
                updatingFacing = true;
                yield return new WaitForSeconds(0.1f);
            if(currentstate != NpcState.talking)
            {
                facing = force.normalized;
                if (Mathf.Abs(force.x) > 100 || Mathf.Abs(force.y) > 100)
                {
                    anim.SetBool("Moving", true);

                }
                else
                {
                    anim.SetBool("Moving", false);

                }
                lastHoriz = Mathf.Round(facing.x);
                lastVert = Mathf.Round(facing.y);
            }

            anim.SetFloat("LastVertical", lastVert);
            anim.SetFloat("LastHorizontal", lastHoriz);
            anim.SetFloat("LastHorizontal", lastHoriz);
            updatingFacing = false;

        }
    }


    public void UpdatePath(int walkPointInt)                        //Depending on EnemyState, pick a different target to Path to.
    {
            //walkpointint is the "nextwaypoint" we are going to
            //if just starting, then walkpoint int = 0;
            //when reachedendofpath = true, walkpoint int will change so we need to update to the next path.

            //code
            //look though the list. Find the GameObject at index walkpointInt and get its data.
            target = walkPoints[walkPointInt];
            wpd = target.GetComponent<WalkPointData>();
            if(wpd.invokeIdle == true)
            {
                pointIdle = true;
                pointIdleTime = wpd.idleTime;
            }
            else
            {
                pointIdle = false;
            }


            //GamgeObject "target" should become the newly found gameobject from the list
            //get the transform.position of the new "target", which gets passed to the IF statement below
            pointPathChosen = true;


            if (seeker.IsDone())
            {
                seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
                pointPathChosen = true;
                

            }

    }

    IEnumerator PointIdle()
    {
        while (turnToPlayer)
        {
            yield return null;
        }
        pointWalking = false;
        currentstate = NpcState.idle;
        yield return new WaitForSeconds(pointIdleTime);
        while (turnToPlayer)
        {
            yield return null;
        }
        pointWalking = true;
        currentstate = NpcState.pointWalking;
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

    //Talking
    //Called by the player using SendMessage("NPCTalk") to reorient the NPC. Dialog handed with SendMessage("TriggerDialog")
    public void NPCTalk()
    {
        if (!turnToPlayer)
        {
            turnToPlayer = true;
            anim.SetBool("Moving", false);
            direction = (player.transform.position - gameObject.transform.position).normalized;
            lastHoriz = direction.x;
            lastVert = direction.y;
            currentstate = NpcState.talking;
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            pm.currentState = PlayerState.interact;
            
        }
    }

    public void EndDialog()
    {
        turnToPlayer = false;
        currentstate = NpcState.pointWalking;
    }


}
