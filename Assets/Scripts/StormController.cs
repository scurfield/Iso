using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormController : MonoBehaviour
{
    public float radiusX;
    public float radiusY;
    public GameObject stormBoltPrefab;
    public float stormTime;
    public float currentTime;
    public float timeTweenBolts;
    private bool stormActive = false;
    private ParticleSystem ps;
    private bool stormcooldown = false;
    

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(StormDuration());
        StartCoroutine(StormAttack());
    }
    public IEnumerator StormDuration()
    {
        stormActive = true;
        var emission = ps.emission; //creates a dusk cloud while charging
        emission.enabled = true;
        currentTime = 0f;
        while (currentTime < stormTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        stormActive = false;
        var em = ps.emission;
        em.enabled = false;
        stormcooldown = true;
        yield return new WaitForSeconds(2f);
        stormcooldown = false;
        yield return new WaitForSeconds(3.5f);//time to finish all animations
        Destroy(gameObject);
        yield return null;
    }

    public IEnumerator StormAttack()
    {
        while (stormActive)
        {
            var boltLocation = new Vector3(Random.Range(-radiusX, radiusX), Random.Range(-radiusY, radiusY), 0);
            Instantiate(stormBoltPrefab, gameObject.transform.position + boltLocation, Quaternion.identity, gameObject.transform);
            yield return new WaitForSeconds(Random.Range(0.01f, 0.3f));

        }
        while (stormcooldown)
        {
            var boltLocation = new Vector3(Random.Range(-radiusX, radiusX), Random.Range(-radiusY, radiusY), 0);
            Instantiate(stormBoltPrefab, gameObject.transform.position + boltLocation, Quaternion.identity, gameObject.transform);
            yield return new WaitForSeconds(Random.Range(0.3f, 1f));
        }


    }


}
