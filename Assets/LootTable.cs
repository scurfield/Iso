using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    public List<GameObject> orbiterBuffs;
    public GameObject health;
    public GameObject stamina;
    public GameObject mark;

    private GameObject chosenBuffType;
    public List<GameObject> currentLoot;


    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public void DropLoot(LootRank rank, DamageType dt, Vector3 position) //what loot table to pick from, and where to instatiate the loot.
    {
        

        AddOrbBuff(dt);
        var roll1 = Random.Range(0f, 1f);
        var roll2 = Random.Range(0f, 1f);
        switch (rank)
        {
            case LootRank.rank1:
                if (roll1 > 0.5f)
                {
                    currentLoot.Add(health);
                    if (roll1 > 0.75f)
                    {
                        currentLoot.Add(mark);
                    }

                }else if (roll1 < 0.25f)
                {
                    currentLoot.Add(stamina);
                }
                break;
            case LootRank.rank2:
                //marks
                if (roll1 > 0.75f)
                {
                    currentLoot.Add(mark);
                    currentLoot.Add(mark);
                }
                else
                {
                    currentLoot.Add(mark);
                }
                //health-stam
                if(roll2 <= 0.33f)
                {
                    currentLoot.Add(stamina);
                }else if (roll2 > 0.33)
                {
                    currentLoot.Add(health);
                    if(roll2 >= 0.75)
                    {
                        currentLoot.Add(stamina);
                    }
                }


                break;
            case LootRank.rank3:
                currentLoot.Add(mark);
                currentLoot.Add(mark);
                currentLoot.Add(mark);
                currentLoot.Add(mark);
                currentLoot.Add(health);
                currentLoot.Add(stamina);
                break;
        }

        foreach(GameObject loot in currentLoot)
        {
            //instatiate that item of loot at target position
            var randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            var ejectForce = Random.Range(100f, 200f);
            var direction = (position - player.transform.position).normalized * 2;
            GameObject lootItem = Instantiate(loot, position, Quaternion.identity);
            lootItem.GetComponentInChildren<Rigidbody2D>().AddForce((direction + randomDir) * ejectForce);
            Debug.Log("Add force -  Direction: " + direction + ", RandomDir: " + randomDir + ", Eject Force: " + ejectForce);
            
        }
        currentLoot.Clear();//remove previous list of loot from this enemy, prepared for ther next;

    }

    private void AddOrbBuff(DamageType dt)
    {
        switch (dt)
        {
            case DamageType.fire:
                chosenBuffType = orbiterBuffs[0];
                break;
            case DamageType.ice:
                chosenBuffType = orbiterBuffs[1];
                break;
            case DamageType.lightning:
                chosenBuffType = orbiterBuffs[2];
                break;
            case DamageType.strength:
                chosenBuffType = orbiterBuffs[3];
                break;
        }
        currentLoot.Add(chosenBuffType);
    }
}
