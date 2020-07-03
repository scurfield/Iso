using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostballController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    private PlayerCombat pc;
    public CircleCollider2D cc;
    private Animator anim;
    private float speed;
    public bool exploding = false;
    public GameObject target;
    public bool targetFound;
    private bool startExploding = false;
    public int damage;

    private Transform swarmCenter;
    public float swarmRange;
    public LayerMask FrostOrbLayer;
    public List<Collider2D> inSwarm;
    private Vector2 swarmDirection;
     //public CircleCollider2D ptherCC;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        anim = GetComponentInChildren<Animator>();
        direction = pc.projectileDirection;
        speed = pc.projectileSpeed;
        rb.velocity = (direction * 1);

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!exploding)
        {
            if (!targetFound)
            {
                //swarm maths

                swarmDirection = Vector2.zero;  //reset swarm direction info so as to not grow to infinite amounts
                swarmCenter = gameObject.transform;
                inSwarm = new List<Collider2D>(Physics2D.OverlapCircleAll(swarmCenter.position, swarmRange, FrostOrbLayer)); //Check which enemies are within his swarm range
                foreach (Collider2D forstBall in inSwarm)
                {
                    swarmDirection += ((Vector2)forstBall.transform.position - rb.position);
                }
                swarmDirection = new Vector2(swarmDirection.x * 1f, swarmDirection.y * 1f);

                //orbit maths

                Vector3 offset = pc.transform.position - gameObject.transform.position;
                float distance = offset.magnitude;
                float sqrLen = 1.01f * offset.sqrMagnitude;
                Vector3 vectorToTarget = gameObject.transform.position - pc.transform.position;
                Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 130) * vectorToTarget;
                gameObject.transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
                //apply force in that direciton
                direction = rotatedVectorToTarget;
                direction = (direction - swarmDirection/ 2).normalized;
                if(distance > 1)
                {
                rb.AddForce(direction * speed * (sqrLen/2));

                }
                else
                {
                    rb.AddForce(direction * speed * (1/sqrLen));
                }

            }
            else if (targetFound)
            {
                //rotate the "front" of the fireball to point at the target.
                Vector3 vectorToTarget = gameObject.transform.position - target.transform.position;
                Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 180) * vectorToTarget;
                gameObject.transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
                //apply force in that direciton
                direction = (Vector2)(target.transform.position - gameObject.transform.position).normalized;
                rb.AddForce(direction * speed * 20);
            }
        }

    }



    IEnumerator Explode()
    {
        startExploding = true;
        rb.velocity = Vector2.zero;

        //expode and damage
        anim.SetTrigger("Contact");
        while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Explode")) //wait for first frame of anim before starting to check if the anim is over.
        {
            yield return null;
        }

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // check  if anim is still playing
        {

            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()             //Draw the anit-swarm range for calculations
    {
        if (swarmCenter == null)
            return;
        Gizmos.DrawWireSphere(swarmCenter.position, swarmRange);
    }


}
