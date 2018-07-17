using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageOverTime : MonoBehaviour
{ 

    public void Destroy(float timeToDestroy)
    {
        Destroy(this, timeToDestroy);
    }

    public void StartdamageOverTime(EnemyCharacter nme, float dmg, Color color)
    {
        StartDot(dmg, 0.5f, 180,nme,color);
    }
    //public void StartdamageOverTimeBoss(MinotaurBoss nme, float dmg)
    //{
    //    // _itemsDatabase = GameObject.Find("Inventory").GetComponent<ItemDatabase>();
    //    //_item = _itemsDatabase.FetchItemByID(0);
    //    StartDotMinotaur(dmg, 0.5f, 180, nme);
    //}

    public void StartDot(float dmg, float timePerHit, float totalDmg, EnemyCharacter nme, Color color)
    {
        StartCoroutine(DOT(dmg, timePerHit, totalDmg, nme,color));
    }
    //public void StartDotMinotaur(float dmg, float timePerHit, float totalDmg, MinotaurBoss nme)
    //{
    //    StartCoroutine(DOTBoss(dmg, timePerHit, totalDmg, nme));
    //}
    public IEnumerator DOT(float dmg, float timePerHit, float totalDmg, EnemyCharacter nme, Color color)
    {
        float currentDmg = 0;

        while (currentDmg < totalDmg)
        {
            if (nme.dead)
            {
                yield break;
            }
            if (nme.canvasHP.gameObject != null)
            {
                EventsManager.TriggerEvent(EventsType.spawnText, new object[] { nme.canvasHP.GetComponentInParent<RectTransform>(), dmg, color});
            }

            nme.health -= dmg;
            currentDmg += dmg;

            if (currentDmg >= totalDmg)
            {
                //enemy.gotHitOverTime = false;
                var temp = nme.transform.FindChild("ParticleOnFire 1(Clone)");
                if (temp != null)
                {
                    Destroy(temp.gameObject);
                }
                Destroy(GetComponent<damageOverTime>());

                yield break;
            }
            yield return new WaitForSeconds(timePerHit);
        }
    }
    //public IEnumerator DOTBoss(float dmg, float timePerHit, float totalDmg, MinotaurBoss nme)
    //{
    //    float currentDmg = 0;

    //    while (currentDmg < totalDmg)
    //    {
           
    //        if (nme.canvasHP.gameObject != null)
    //        {
    //            EventsManager.TriggerEvent(EventsType.spawnText, new object[] { nme.canvasHP.GetComponentInParent<RectTransform>(), dmg, Color.red });
    //        }

    //        nme.health -= dmg;
    //        currentDmg += dmg;

    //        if (currentDmg >= totalDmg)
    //        {
    //            //enemy.gotHitOverTime = false;
    //            var temp = nme.transform.FindChild("ParticleOnFire 1(Clone)");
    //            if (temp != null)
    //            {
    //                Destroy(temp.gameObject);
    //            }
    //            Destroy(GetComponent<damageOverTime>());

    //            yield break;
    //        }
    //        yield return new WaitForSeconds(timePerHit);
    //    }
    //}
}
