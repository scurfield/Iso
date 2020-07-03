using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

public enum PlayerState
{
    active,
    attack,
    interact,
    dead
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    [HideInInspector]public Animator anim;
    public float lastHoriz;
    public float lastVert;
    Vector2 movement;
    PlayerHealth ph;
    PlayerCombat pc;
    private GameHandler gh;

    public bool knockback = false;
    private Vector2 knockbackDirection;


    //newimput system
    private Vector2 inputVector = new Vector2 (0, 0);

    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.active;
        ph = GetComponent<PlayerHealth>();
        pc = GetComponent<PlayerCombat>();
        anim = GetComponent<Animator>();
        gh = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameHandler>();
    }

    public void MovementInput(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.z != 0) //keep this object on z = 0;
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        switch (currentState)
        {
            case PlayerState.dead:
                break;
            case PlayerState.interact:
                anim.Play("Idle");
                CheckDead();
                break;
            case PlayerState.active:
                if (!gh.abilityEditing && !gh.inEscMenu) //player can move
                {
                    if (!ph.isDead)
                    {
                        UpdateFacing();
                    }

                }
                CheckDead();
                break;
            case PlayerState.attack:
                if (pc.canRotateWhileAttacking)
                {
                    if (!gh.abilityEditing && !gh.inEscMenu) //player can move
                    {
                        if (!ph.isDead)
                        {
                            UpdateFacing();
                        }
                    }
                }
                CheckDead();
                break;
        }

    }
    void CheckDead()
        {
            if (ph.isDead)
            {
                rb.velocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                currentState = PlayerState.dead;
            }
        }

    void UpdateFacing()
    {


        if (!knockback)
        {
            //Idle Animation triggers
            if (Mathf.Abs(inputVector.x) >= 0.1 || Mathf.Abs(inputVector.y) >= 0.1)
            {
                if (pc.canRotateWhileAttacking)
                {
                    anim.SetBool("Moving", false);
                }
                else
                {
                    anim.SetBool("Moving", true);
                }
                //anim.speed = 1 * (inputVector.x + inputVector.y) / 2;
                lastHoriz = inputVector.x;
                lastVert = inputVector.y;

            }
            else
            {
                anim.SetBool("Moving", false);
            }

            //Animation Branches
            //anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("LastHorizontalRounded", Mathf.Round(lastHoriz)); //This made the character always face north at 0, 0
            anim.SetFloat("LastHorizontal", lastHoriz);
            //anim.SetFloat("Vertical", movement.y);
            anim.SetFloat("LastVerticalRounded", Mathf.Round(lastVert));
            anim.SetFloat("LastVertical", lastVert);

        }


    }


    private void FixedUpdate()
    {
        //Movement
        if (currentState == PlayerState.active && !gh.abilityEditing &&!gh.inEscMenu)
        {
            if (!pc.rolling && !knockback)
            {
                rb.MovePosition(rb.position + inputVector * moveSpeed * Time.fixedDeltaTime);
            }

        }


    }

    public void Knockback(Vector2 enemyPos)
    {
        if (!ph.isDead && !pc.rolling)
        {
            knockbackDirection = (enemyPos - (Vector2)gameObject.transform.position).normalized;
            lastHoriz = knockbackDirection.x;
            lastVert = knockbackDirection.y;
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
        while (normalizedTime <= 0.2f)
        {
            normalizedTime += Time.deltaTime / duration;
            rb.AddForce(-knockbackDirection * 1);
            yield return null;
        }
        knockback = false;
        anim.SetBool("Knockback", false) ;
        yield return null;
    }
}
