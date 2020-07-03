using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBallFinder : MonoBehaviour
{
    public List<GameObject> enemies;
    private LightningballController controller;
    private CircleCollider2D cc;

    


    void Start()
    {
        controller = GetComponentInParent<LightningballController>();
        cc = GetComponent<CircleCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(controller.currentState == LightningballallState.attacking)
        {
            if (other.tag == "Enemy")
            {
                var ai = other.GetComponent<EnemyAI2>();
                if(ai.currentState == EnemyState.dead)
                {
                    return;
                }
                if (enemies.Count == 0)
                {
                    Debug.Log("enemies = 0, next step");
                    controller.currentTarget = other.gameObject;
                    cc.radius *= 2f;
                }
                enemies.Add(other.gameObject);
                    if (!controller.attacking)
                    {
                    Debug.Log("calling zipzoop from trigger enter");

                        controller.StartCoroutine("ZipZoop");
                    }

            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Remove(other.gameObject);
        }
    }

    public void FinderOnOff()
    {
        switch (controller.currentState)
        {
            case LightningballallState.following:
                cc.enabled = false;
                enemies.Clear();
                break;

            case LightningballallState.attacking:
                cc.enabled = true;
                break;
            case LightningballallState.returning:
                cc.enabled = true;
                break;
        }
    }
}
