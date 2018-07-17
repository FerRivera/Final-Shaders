using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectiles : MonoBehaviour
{

    [HideInInspector]
    public float speed;
    //[HideInInspector]
    public float damage;
    public bool blockedAttack;
    public float timeToRefreshAttackActual;
    public float timeToRefreshAttackTotal;
    public EntityFSM owner;
    public GameObject hitParticle;

    public void DestroyProyectil()
    {
        if(hitParticle != null)
        {
            var p = Instantiate(hitParticle);
            p.transform.position = this.transform.position;
            Destroy(gameObject);
        }        

    }
}
