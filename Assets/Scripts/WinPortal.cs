using UnityEngine;
using System.Collections;

public class WinPortal : MonoBehaviour
{
    public GameObject portalToAppear;

	void Start ()
    {
        EventsManager.SubscribeToEvent(EventsType.firstBossAfter, SpawnAfterBoss);
        EventsManager.SubscribeToEvent(EventsType.changeScene, sceneChanged);
    }


    public void sceneChanged(params object[] p)
    {
        EventsManager.UnsubscribeToEvent(EventsType.firstBossAfter, SpawnAfterBoss);
        EventsManager.UnsubscribeToEvent(EventsType.changeScene, sceneChanged);
    }


    void SpawnAfterBoss(params object[] p)
    {
        portalToAppear.SetActive(true);
        EventsManager.UnsubscribeToEvent(EventsType.firstBossAfter, SpawnAfterBoss);
    }


    void Update () {
	
	}
}
