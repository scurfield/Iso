using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostballOrbit : MonoBehaviour
{
    private PlayerCombat pc;
    private PlayerMovement pm;
    private Rigidbody2D rb;
    private Animator anim;
    private Animator fbAnim;
    public GameObject frostballPrefab;
    public Sprite icon;

    public int staminaCost;
    public int damage;
    public float speed;
    public float cooldown;


    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponentInParent<PlayerCombat>();
        pm = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();
        fbAnim = frostballPrefab.GetComponent<Animator>();
    }

    public void UseAbility()
    {
        StartCoroutine(AbilityFrostball());
        pc.UpdateCooldowns(cooldown);
    }

    public IEnumerator AbilityFrostball()
    {
        if (staminaCost < pc.currentStamina)
        {
            pc.usingAbility = true;
            pc.currentStamina -= staminaCost;
            pm.currentState = PlayerState.attack;
            rb.velocity = Vector2.zero;
            pc.projectile = frostballPrefab;
            pc.projectileSpeed = speed;


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
