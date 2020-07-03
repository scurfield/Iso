using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBarrier : MonoBehaviour
{
    private PlayerCombat pc;
    private PlayerStats stats;
    private PlayerMovement pm;
    private Rigidbody2D rb;
    private Animator anim;
    public GameObject IceCrystalPrefab;
    public Sprite icon;

    public int staminaCost;
    public int baseDamage;
    public float cooldown;


    public bool rotateProjectile = false;


    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponentInParent<PlayerCombat>();
        pm = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();
        stats = GetComponentInParent<PlayerStats>();
        anim = GetComponentInParent<Animator>();
    }

    public void UseAbility()
    {
        StartCoroutine(AbilityIceBarrier());
        pc.UpdateCooldowns(cooldown);
    }
    public void EndAbility()
    {

    }

    public IEnumerator AbilityIceBarrier()
    {
        if (staminaCost < pc.currentStamina)
        {
            pc.usingAbility = true;
            pc.UseStamina(staminaCost);
            pm.currentState = PlayerState.attack;
            rb.velocity = Vector2.zero;
            pc.projectile = IceCrystalPrefab;
            pc.projectileSpeed = 0;
            pc.rotateProjectile = rotateProjectile;
            pc.castFromChest = false;

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

        }
        yield return null;


    }
}
