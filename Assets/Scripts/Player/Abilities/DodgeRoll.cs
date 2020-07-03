using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRoll : MonoBehaviour
{
    private PlayerCombat pc;
    private PlayerMovement pm;
    private Rigidbody2D rb;
    private Animator anim;
    private CircleCollider2D hb; //to shrint the player's hitbox on roll.

    public int staminaCost;
    private float hbTempRadius = 0.1f;
    private float hbRadius;
    public Sprite icon;

    public float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponentInParent<PlayerCombat>();
        pm = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();
        hb = GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>();
        hbRadius = hb.radius;
    }

    public void UseAbility()
    {
        StartCoroutine(AbilityDodgeRoll());
        pc.UpdateCooldowns(cooldown);
    }
    public void EndAbility()
    {

    }

    IEnumerator AbilityDodgeRoll()
    {
        if (staminaCost < pc.currentStamina)
        {
            pc.usingAbility = true;
            pc.UseStamina(staminaCost);
            pc.rolling = true; //referenced in the PlayerHealth script to not take damage while rolling
            pm.currentState = PlayerState.active;
            rb.velocity = Vector2.zero;
            hb.radius = hbTempRadius;
            //rolling causes the player to lose current velocity, then move the right amount
            Vector2 direction = new Vector2(pm.lastHoriz, pm.lastVert).normalized;
            rb.drag = 3;
            rb.AddForce(rb.position + direction * pm.moveSpeed * 1000); //300 here snaps forward, linear drag of 20 slows it down nicely, but these numbers should be played with
            //play animation
            anim.SetTrigger("DodgeRoll");
            while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("DodgeRoll")) //wait for first frame of anim before starting to check if the anim is over.
            {

                yield return null;
            }
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // check  if anim is still playing
            {

                yield return null;
            }

            hb.radius = hbRadius;
            pm.currentState = PlayerState.active;
            pc.rolling = false;
            pc.usingAbility = false;

        }
        yield return null;


    }
}
