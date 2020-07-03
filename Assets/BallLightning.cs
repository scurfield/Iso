using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLightning : MonoBehaviour
{
    private PlayerCombat pc;
    private PlayerMovement pm;
    private Rigidbody2D rb;
    private Animator anim;
    public GameObject ballPrefab;
    public Sprite icon;

    public int staminaCost;
    public int baseDamage;
    public float speed;
    public float cooldown = 1f;

    public GameObject currentBall;



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
        if(currentBall == null)
        {
            StartCoroutine(AbilityBallLightning());
            pc.UpdateCooldowns(cooldown);
            
        }
        else
        {
            
            currentBall.SendMessage("CommandBall");
        }

    }
    public void EndAbility()
    {

    }

    public IEnumerator AbilityBallLightning()
    {
        if (staminaCost < pc.currentStamina)
        {
            pc.usingAbility = true;
            pc.UseStamina(staminaCost);
            pm.currentState = PlayerState.attack;
            rb.velocity = Vector2.zero;

            //play attack animation
            anim.SetTrigger("Evoke");
            while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Evoke")) //wait for first frame of anim before starting to check if the anim is over.
            {
                yield return null;
            }

            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length /2); // check  if anim is still playing
            currentBall = Instantiate(ballPrefab, gameObject.transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.identity, gameObject.transform);
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length / 2);
            pm.currentState = PlayerState.active;
            pc.usingAbility = false;

        }
        yield return null;


    }

    
}
