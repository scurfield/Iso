using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum DamageType
    {
        fire,
        ice,
        strength,
        lightning,
        none //for when there is no special resistances
    }


public class DamageSource : MonoBehaviour
{
        public DamageType damageTypeDealt;


    // Start is called before the first frame update
    void Start()
    {
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
