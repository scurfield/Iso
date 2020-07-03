using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunnable : MonoBehaviour
{

    private PlayerStats stats;
    private EnemyHealth eh;
    public GameObject strStun;
    public GameObject fireStun;
    public GameObject iceStun;
    public GameObject lighStun;
    private EnemyAI2 ai;

    public int strCount = 0;
    public int fireCount = 0;
    public int iceCount = 0;
    public int lighCount = 0;

    public bool isStunned;
    public GameObject currentStun;

    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        eh = GetComponent<EnemyHealth>();
        ai = GetComponent<EnemyAI2>();
    }
    public void CallStun(DamageType type) //called by send message in Enemy health after taking any damage
    {
        if (!isStunned)
        {
            switch (type)
            {
                case DamageType.strength:
                    strCount += 1;
                    CheckStun(strCount, type);
                    break;
                case DamageType.fire:
                    fireCount += 1;
                    CheckStun(strCount, type);
                    break;
                case DamageType.ice:
                    iceCount += 1;
                    CheckStun(strCount, type);
                    break;
                case DamageType.lightning:
                    lighCount += 1;
                    CheckStun(strCount, type);
                    break;

            }

        }
    }

    private void CheckStun(int count, DamageType type)
    {
        var stunRoll = Random.Range(0f, 1f);
        Debug.Log("Stun roll: " + stunRoll);
        if (stunRoll <= stats.stunChance + count * 0.05f)
        {
            isStunned = true;
            ai.stunned = true;
            StunEnemy(type);
        }
    }

    private void StunEnemy(DamageType type)
    {
        switch (type)
        {
            case DamageType.strength:
                eh.stunStr = true;
                currentStun = Instantiate(strStun, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                
                break;
            case DamageType.fire:
                eh.stunFire = true;
                currentStun = Instantiate(fireStun, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                
                break;
            case DamageType.ice:
                eh.stunIce = true;
                currentStun = Instantiate(iceStun, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                
                break;
            case DamageType.lightning:
                eh.stunLigh = true;
                currentStun = Instantiate(lighStun, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                
                break;

        }
    }
}
