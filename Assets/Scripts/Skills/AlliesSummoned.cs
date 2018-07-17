using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlliesSummoned : MonoBehaviour
{
    public int timeToDestroy;

	void Start ()
    {
        StartCoroutine(timeToKillSummons(timeToDestroy));
        transform.parent = null;
	}
	
	void Update ()
    {
	
	}

    IEnumerator timeToKillSummons(int timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
