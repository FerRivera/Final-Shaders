using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillobtained : MonoBehaviour
{
    public GameObject storm;
    public GameObject raySkinObtained;
    Vector3 rayStartingPosition;

    void Start ()
    {
        EventsManager.SubscribeToEvent(EventsType.resetRayAura, ResetHeroAura);
        rayStartingPosition = raySkinObtained.transform.position;
    }
	
	void Update ()
    {
        storm.transform.position = Finder.Instance.hero.transform.position;
        raySkinObtained.transform.position = new Vector3(Finder.Instance.hero.transform.position.x, raySkinObtained.transform.position.y, Finder.Instance.hero.transform.position.z);
    }

    public void ResetHeroAura(params object[] p)
    {
        raySkinObtained.transform.position = rayStartingPosition;
        raySkinObtained.SetActive(false);
    }
}
