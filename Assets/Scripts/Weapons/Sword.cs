using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour
{
    public GameObject particleHit;

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == (int)LayersEnum.ENEMY)
        {
            Debug.Log("ejecuto el script SWORD");
            c.gameObject.GetComponent<EntityFSM>().health -= 10;
            var part = Instantiate(particleHit);
            part.transform.position = c.transform.position;
        }
    }
}
