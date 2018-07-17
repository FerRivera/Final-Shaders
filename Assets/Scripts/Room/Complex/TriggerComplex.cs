using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TriggerComplex : MonoBehaviour
{
    public List<Door> closeDoor;
    public List<Door> openDoor;
    public event Action chaseCharacter = delegate { };
    public GameObject[] fogToDissapear;
    public MinotaurBoss boss;

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == (int)LayersEnum.HERO)
        {
            chaseCharacter();

            if(closeDoor != null)
            {
                for (int i = 0; i < closeDoor.Count; i++)
                {
                    closeDoor[i].CloseDoor();
                }                
            }

            if(openDoor != null)
            {
                for (int i = 0; i < openDoor.Count; i++)
                {
                    openDoor[i].OpenDoor();
                }
            }            

            if (boss != null && !boss.gameObject.activeSelf)
            {
                boss.gameObject.SetActive(true);
            }

            foreach (var item in fogToDissapear)
            {
                item.SetActive(false);
            }

            EventsManager.TriggerEvent(EventsType.resetHeroAura);
        }
    }
}
