using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffsController : MonoBehaviour
{
    public List<GameObject> orbitBuffs;

    private Rigidbody2D rb;
    private float playerVelocity;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, 90 * Time.deltaTime);
    }
}
