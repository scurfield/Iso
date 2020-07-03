using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneCircle : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
       
    }

    // Update is called once per frame
public void PowerOrbAbsorb()
    {
        Debug.Log("Check 1");
        StartCoroutine(Absorb());
    }

    public IEnumerator Absorb()
    {
        //anim.SetBool("PowerOrb", true);
        Debug.Log("Check 2");
        while (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Absorb")) //wait for first frame of anim before starting to check if the anim is over.
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
