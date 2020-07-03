using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAutoTarget : MonoBehaviour
{
    public CircleCollider2D cc;
    private bool targetFound = false;
    private FireballController fbc;

    void Start()
    {
        fbc = GetComponentInParent<FireballController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            if (!targetFound)
            {
                targetFound = true;
                fbc.targetFound = true;
                fbc.target = other.gameObject;

            }
        }
    }
}
