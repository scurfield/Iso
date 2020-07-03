using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntBoxController : MonoBehaviour
{
    //Player int box is just for interactions in front of the player (read sign, talk to npc, open door)
    //Attacking will be handled in a more Untiy-Traditional way in the PlayerAttack script


    public Vector3 offset;
    private Vector3 boxOffset;
    public PlayerMovement pm;
    public CircleCollider2D cc;
    public GameObject player;

    public List<GameObject> withinReach;
    
    




    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponentInParent<PlayerMovement>();
        cc = GetComponent<CircleCollider2D>();


    }

    // Update is called once per frame
    void Update()
    {
        //if (pm.lastHoriz != 0 && pm.lastVert != 0)
        //{

        //    offset.x = (pm.lastHoriz) / 2.825f;
        //    offset.y = (pm.lastVert) / 2.825f;
        //    cc.transform.position = player.transform.position + offset;
        //}

        //else
        //{
        //    offset.x = (pm.lastHoriz) / 2;
        //    offset.y = (pm.lastVert) / 2;
        //    cc.transform.position = player.transform.position + offset;
        //}
        offset = new Vector2(pm.lastHoriz, pm.lastVert).normalized;
        cc.transform.position = player.transform.position + offset /2;



    }

    //When an interactable is within reach of your IntBox
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "NPC" || other.gameObject.tag == "Openable") //for now there only NPC's (aka doors, signs, triggers...) but in theory you could add Items or other things here.
        {
            //show speech prompt animation, somehow
            GameObject interactable = other.gameObject;
            withinReach.Add(interactable);
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "NPC" || other.gameObject.tag == "Openable") //for now there only NPC's (aka doors, signs, triggers...) but in theory you could add Items or other things here.
        {
            //show speech prompt animation, somehow
            GameObject interactable = other.gameObject;
            withinReach.Remove(interactable);
        }
    }


}
