using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostballAutoTarget : MonoBehaviour
{
    public CircleCollider2D cc;
    private bool targetFound = false;
    private FrostballController fbc;

    void Start()
    {
        fbc = GetComponentInParent<FrostballController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
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
