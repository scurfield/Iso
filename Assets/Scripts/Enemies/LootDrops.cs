

using System.Collections.Generic;
using UnityEngine;

public enum LootRank 
{
    rank1,
    rank2,
    rank3
}
public class LootDrops : MonoBehaviour
{
    public List<GameObject> guaranteedLoot;

    //private Vector3 playerPos;
    //private Quaternion rotation;
    //private GameObject player;
    private float ejectForce = 0.2f;
    private Vector3 direction;
    //public float lootListProbability;
    private Vector3 randomDir;
    private bool fromEnemy; //Each individual object that can spawn loot needs this bool picked in the inspector
    public LootRank rank;
    private LootTable lootTable;
    private EnemyAI2 ai;


    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        lootTable = GameObject.FindGameObjectWithTag("GameController").GetComponent<LootTable>();
        if (gameObject.GetComponent<EnemyAI2>() != null)
        {
            fromEnemy = true;
            ai = gameObject.GetComponent<EnemyAI2>();
        }

    }
    public void DropLoot()
    {
        if (fromEnemy)
        {
            ////Debug.Log("From Enemy Loot");
            //randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            //ejectForce = Random.Range(100f, 200f);
            //direction = (gameObject.transform.position - player.transform.position).normalized * 2;
            foreach (GameObject loot in guaranteedLoot)
            {
                lootTable.currentLoot.Add(loot);
                Debug.Log("Adding guaranteed loot: " + loot);
            }
            lootTable.DropLoot(rank, ai.dt, gameObject.transform.position);
            return;
        }

        else //if not from an enemy, ie, a crate, spawn all items on list NOT away from the player
        {
            foreach (GameObject loot in guaranteedLoot)
            {
                //float rng = Random.Range(0f, 1f);
                //if (rng <= lootListProbability)
                //{
                //    playerPos = player.transform.position;

                //}
                //else
                //{
                    //Debug.Log("From Crate Loot");
                    randomDir = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
                    ejectForce = Random.Range(50f, 100f);
                    direction = new Vector3(0f, 0f, 0f);
                
                GameObject lootItem = Instantiate(loot, gameObject.transform.position, gameObject.transform.rotation);
                lootItem.GetComponentInChildren<Rigidbody2D>().AddForce((direction + randomDir) * ejectForce);
                Debug.Log("Add force -  Direction: " + direction + ", RandomDir: " + randomDir + ", Eject Force: " + ejectForce);

            }
        }
    }
}





