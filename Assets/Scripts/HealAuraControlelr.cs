using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAuraControlelr : MonoBehaviour
{
    private Animator anim;
    
    //This class also works for all other auras based on the original Heal Aura gameobject. Visuals such as colours and particle emitters can be altered without needing to adjust this script.
    void Start()
    {
        anim = GetComponent<Animator>();
        
        StartCoroutine(HealAnim());
        
    }

    IEnumerator HealAnim()
    {
        anim.SetTrigger("HealAura");
        while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("HealAura")) //wait for first frame of anim before starting to check if the anim is over.
        {
            yield return null;
        }

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // check  if anim is still playing
        {

            yield return null;
        }
        Destroy(gameObject);
    }
}
