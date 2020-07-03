using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePotion : MonoBehaviour
{
    private PlayerCombat pc;
    public float cooldown;
    public GameObject flamePotionPrefab;
    public int staminaCost;
    private PlayerMovement pm;
    private Rigidbody2D rb;
    private float elapsedTime = 0;
    public float projectileSpeed;
    private Animator anim;
    public int damage;
    private float maxChargeTime = 0.5f;
    private bool chargingThrow = false;
    private bool throwing = false;

    public bool rotateProjectile = false;


    private int ca;

    void Start()
    {
        pc = GetComponentInParent<PlayerCombat>();
        pm = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent <Rigidbody2D>();
        anim = GetComponentInParent<Animator>();
    }

    public void UseAbility(int currentAbility)
    {
        throwing = true;
        ca = currentAbility;
        StartCoroutine(ChargeThrow());
    }

    private IEnumerator ChargeThrow()
    {
        pc.usingAbility = true;
        pc.canRotateWhileAttacking = true;
        pm.currentState = PlayerState.attack;
        anim.SetBool("HoldCast", true);
        chargingThrow = true;
        elapsedTime = 0f;
        while(elapsedTime < maxChargeTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        EndAbility();
        yield return null;
    }
    private void FixedUpdate() 
    {
        if (chargingThrow)//when ability button is let go, throw the potion
        {
            if (ca == 1)
            {
                if (!pc.abOneHold && pc.usingAbility)
                {
                    EndAbility();
                    return;
                }
            }
            if (ca == 2)
            {
                if (!pc.abTwoHold && pc.usingAbility)
                {
                    EndAbility();
                    return;
                }
            }
            if (ca == 3)
            {
                if (!pc.abThreeHold && pc.usingAbility)
                {
                    EndAbility();
                    return;
                }

            }

        }

    }
    public void EndAbility()
    {
        if (throwing)
        {
            anim.SetBool("HoldCast", false);
            throwing = false;
            chargingThrow = false;
            StartCoroutine(AbilityFlamePotion());//Throw the potion on release
            pc.UpdateCooldowns(cooldown);

        }
    }

    private IEnumerator AbilityFlamePotion()
    {
        if (staminaCost < pc.currentStamina)
        {
            
            pc.UseStamina(staminaCost);
            pm.currentState = PlayerState.attack;
            rb.velocity = Vector2.zero;
            pc.projectile = flamePotionPrefab;
            pc.projectileSpeed = projectileSpeed * elapsedTime*2; //since max charge time is half a second, this works as a percentage ratio of full speed.
            pc.rotateProjectile = rotateProjectile;
            pc.castFromChest = true;



            //play attack animation
            anim.SetTrigger("Cast");
            while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Cast")) //wait for first frame of anim before starting to check if the anim is over.
            {
                yield return null;
            }

            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // check  if anim is still playing
            {
                //The animation will call the function below to instantiate the prefab
                yield return null;
            }
            pm.currentState = PlayerState.active;
            pc.usingAbility = false;
            pc.canRotateWhileAttacking = false;

        }
        yield return null;
    }

}
