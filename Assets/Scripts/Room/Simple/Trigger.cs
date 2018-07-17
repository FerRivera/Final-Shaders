using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Trigger : MonoBehaviour
{
    public List<Door> door;
    public event Action chaseCharacter = delegate { };
    public GameObject[] fogToDissapear;
    public MinotaurBoss boss;

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == (int)LayersEnum.HERO)
        {
            chaseCharacter();

            if(door != null)
            {
                for (int i = 0; i < door.Count; i++)
                {
                    door[i].CloseDoor();
                }
            }

            if (boss != null && !boss.gameObject.activeSelf)
            {
                boss.gameObject.SetActive(true);

                for (int i = 0; i < door.Count; i++)
                {
                    door[i].CloseDoor();
                    door[i].GetComponent<BoxCollider>().enabled = true;
                }
            }

            foreach (var item in fogToDissapear)
            {
                item.SetActive(false);
            }


        }
    }
}
