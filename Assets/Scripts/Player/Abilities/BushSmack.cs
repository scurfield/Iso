using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushSmack : MonoBehaviour
{
    private PlayerCombat pc;
    private PlayerMovement pm;
    private Rigidbody2D rb;
    private Animator anim;



    public int staminaCost;
    public int damage;
    public Sprite icon;
    public float cooldown;


    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponentInParent<PlayerCombat>();
        pm = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();

    }

    public void UseAbility()
    {
        StartCoroutine(AbilitySmack());
        pc.UpdateCooldowns(cooldown);
    }


    public IEnumerator AbilitySmack()
    {
        if (staminaCost < pc.currentStamina)
        {
            pc.usingAbility = true;
            pc.currentStamina -= staminaCost;
            pm.currentState = PlayerState.attack;
            rb.velocity = Vector2.zero;
            //when the player smacks, they move forward a little to make it feel more powerful.
            Vector2 direction = new Vector2(pm.lastHoriz, pm.lastVert);
            rb.AddForce(rb.position + direction * pm.moveSpeed * 100); //100 here snaps forward, linear drag of 20 slows it down nicely, but these numbers should be played with
            //play attack animation
            pc.currentDamage = damage;
            anim.SetTrigger("SmackAttack");
            while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) //wait for first frame of anim before starting to check if the anim is over.
            {
                yield return null;
            }
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // check  if anim is still playing
            {

                yield return null;
            }
            pm.currentState = PlayerState.active;
            pc.usingAbility = false;

        }
        yield return null;


    }
}
