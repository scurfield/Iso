using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int powerOrbs;
    public int marks;
    public float criticalChance;

    // damage raken is multiplied by this value, and divided by 100. above 100 is good, below 100 will hinder you.
    public float armour; //affects all damage souces
    public float fireResist;
    public float iceResist;
    public float lightningResist;

    public float strength;
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    public float stunChance;

}
