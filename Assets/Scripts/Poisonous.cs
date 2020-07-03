using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poisonous : MonoBehaviour
{
    private GameObject splatterPrefab;
    private Vector2 previousLocation;
    private Vector2 currentLocation;
    private SpriteRenderer sr;
    private float distance;


    // Start is called before the first frame update
    void Start()
    {
        var prefabs = GameObject.FindGameObjectWithTag("Load").GetComponent<LoadedPrefabs>().prefabs;
        foreach (var prefab1 in prefabs)
        {
            if(prefab1.name == "PoisonSplatter")
            {
                splatterPrefab = prefab1;
                break;
            }
        }
        foreach (var prefab2 in prefabs)
        {
            if (prefab2.name == "PoisonDrip")
            {
                Instantiate(prefab2, gameObject.transform);
                break;
            }
        }
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(0.6f, 1f, 0.7f);
        previousLocation = gameObject.transform.position;
        //instantiate poison drip particles object as child of this gameobject

    }

    // Update is called once per frame
    void Update()
    {
        currentLocation = gameObject.transform.position;
        distance = Vector2.Distance(previousLocation, currentLocation);
        if(distance >= 0.25)
        {
            OozePoison();
            previousLocation = gameObject.transform.position;
        }
    }

    private void OozePoison()
    {
        Instantiate(splatterPrefab, gameObject.transform.position, Quaternion.identity);
    }
}
