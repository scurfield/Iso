using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCrate : MonoBehaviour
{
    private bool opened = false;
    private LootDrops loot;
    private SpriteRenderer sr;
    public Sprite closedSprite;
    public Sprite openedSprite;
    public PlayerCombat pc;

    // Start is called before the first frame update
    void Start()
    {
        loot = GetComponent<LootDrops>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedSprite;
        
        
    }

    // Update is called once per frame
    public void OpenObject() //not a talking NPC, but can be used as this message is already being sent
    {
        //pc.talking = false;
        if (!opened)
        {
            opened = true;
            sr.sprite = openedSprite;
            loot.DropLoot();// false, not from an enemy

        }
    }

}

