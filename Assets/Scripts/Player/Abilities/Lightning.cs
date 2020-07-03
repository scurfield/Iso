using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    private PlayerCombat pc;
    private PlayerMovement pm;
    private Animator anim;
    public GameObject lightningPrefab;
    public Sprite icon;
    private LightningController lc;

    public int staminaCost;
    public int damage;

    public float cooldown;

    public bool castingLightning = false;

    private int ca;
    
    void Start()
    {
        pc = GetComponentInParent<PlayerCombat>();
        pm = GetComponentInParent<PlayerMovement>();
        anim = GetComponentInParent<Animator>();
        lc = lightningPrefab.GetComponent<LightningController>();
    }
    
    public void UseAbility(int currentAbility)
    {
        
        ca = currentAbility;
        StartCoroutine(AbilityLightning());
        pc.UpdateCooldowns(cooldown);
    }

    public void EndAbility()
    {
        
        EndAttack();
    }

    public IEnumerator AbilityLightning()
    {
        if(pc.currentStamina >= staminaCost)
        {
            pc.UseStamina(staminaCost);
            pc.usingAbility = true;
            pc.canRotateWhileAttacking = true;
            pm.currentState = PlayerState.attack;

            anim.SetBool("HoldCast", true);
            castingLightning = true;
            Vector3 vectorToTarget = gameObject.transform.position - pc.attackPoint.position;
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 180) * vectorToTarget;
            Instantiate(lightningPrefab, new Vector3(pc.attackPoint.position.x, pc.attackPoint.position.y + 0.4f, pc.attackPoint.position.z), Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget));
            yield return null;

        }
        else
        {
            StopAllCoroutines();
        }
    }

    private void FixedUpdate()
    {
        if (pc.currentStamina <= 0)
        {
            EndAttack();
            return;
        }
        else
        {
            if (castingLightning)
            {
                pc.UseStamina(1);
            }

            if (ca == 1)
            {
                if (!pc.abOneHold && pc.usingAbility)
                {
                    EndAttack();
                    return;
                }
            }
            if (ca == 2)
            {
                if (!pc.abTwoHold && pc.usingAbility)
                {
                    EndAttack();
                    return;
                }
            }
            if (ca == 3)
            {
                if (!pc.abThreeHold && pc.usingAbility)
                {
                    EndAttack();
                    return;
                }

            }
        }
    }


    private void EndAttack()
    {
        pc.canRotateWhileAttacking = false;
        castingLightning = false;
        anim.SetBool("HoldCast", false) ;
        pc.holdCast = false;
        //Debug.Log("EndAttack");
        StopAllCoroutines();
        pm.currentState = PlayerState.active;
        pc.usingAbility = false;
    }
}
