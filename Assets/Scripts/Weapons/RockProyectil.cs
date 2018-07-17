﻿using UnityEngine;
using System.Collections;

public class RockProyectil : Proyectiles
{
    void Awake ()
    {
        timeToRefreshAttackTotal = 0.06f;
    }

    private void Start()
    {
        
    }   
    
    public void CheckBlock()
    {
        if (blockedAttack)
        {
            if (timeToRefreshAttackActual >= timeToRefreshAttackTotal)
            {
                blockedAttack = false;
                timeToRefreshAttackActual = 0;
            }
            else
            {
                timeToRefreshAttackActual += Time.deltaTime;
            }
        }
    }

    void Update ()
    {
        //transform.position += transform.forward * speed * Time.deltaTime;
        CheckBlock();
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.layer == (int)LayersEnum.WALL || c.gameObject.layer == (int)LayersEnum.MAP || c.gameObject.layer == (int)LayersEnum.HERO)
        {         
            var p = Instantiate(hitParticle);
            p.transform.position = this.transform.position;
            Destroy(gameObject);
            
        }
        if(c.gameObject.layer == (int)LayersEnum.SHIELD)
        {
            timeToRefreshAttackActual = 0;
            blockedAttack = true;
            
        }
    }
    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.layer == (int)LayersEnum.FLOOR)
        {
            var p = Instantiate(hitParticle);
            p.transform.position = this.transform.position;
            Destroy(gameObject);
        }
    }
    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.layer == (int)LayersEnum.SHIELD)
        {
            timeToRefreshAttackActual = 0;
            blockedAttack = true;
        }
    }


    
}
