using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraVFX : MonoBehaviour
{
    public GameObject healAura;
    public GameObject staminaAura;
    public GameObject runeCircle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RuneCircle()
    {
        var runeCirclePrefab = Instantiate(runeCircle, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), gameObject.transform.rotation);
        runeCirclePrefab.transform.parent = gameObject.transform;
        runeCirclePrefab.SendMessage("PowerOrbAbsorb");
    }

    public void HealAura()
    {
        var healAuraPrefab = Instantiate(healAura, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.35f), gameObject.transform.rotation);
        healAuraPrefab.transform.parent = gameObject.transform;
        
    }

    public void StaminaAura()
    {
        var staminaAuraPrefab = Instantiate(staminaAura, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.35f), gameObject.transform.rotation);
        staminaAuraPrefab.transform.parent = gameObject.transform;

    }

}
