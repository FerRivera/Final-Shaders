using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallowedGround : SkillSelfCast
{
    Item _item;
    public float damage;
    //bool touching;
    public List<GameObject> enemies;
    //public MinotaurBoss boss;
    public float currentTime;
    public float totalTime;
    public float posY;

    protected override void Start ()
    {
        base.Start();
        _item = _itemsDatabase.FetchItemByID(29);
        transform.position = new Vector3(caster.transform.position.x, posY, caster.transform.position.z);
        StartCoroutine(Destroy());
    }

    protected override void Update ()
    {
        base.Update();

        currentTime += Time.deltaTime;

        if (currentTime >= totalTime)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                EventsManager.TriggerEvent(EventsType.spawnText, new object[] { enemies[i].GetComponent<EnemyCharacter>().canvasHP.GetComponentInParent<RectTransform>(), damage, _textColor });
                enemies[i].GetComponent<EnemyCharacter>().health -= damage;

                //if (enemies[i].gameObject.layer == (int)LayersEnum.ENEMY)
                //{
                //    EventsManager.TriggerEvent(EventsType.spawnText, new object[] { enemies[i].GetComponent<EntityFSM>().canvasHP.GetComponentInParent<RectTransform>(), damage, _textColor });
                //    enemies[i].GetComponent<EntityFSM>().health -= damage;                    
                //}
                //else if (enemies[i].gameObject.layer == (int)LayersEnum.BOSS)
                //{
                //    EventsManager.TriggerEvent(EventsType.spawnText, new object[] { enemies[i].GetComponent<EntityFSM>().canvasHP.GetComponentInParent<RectTransform>(), damage, _textColor });
                //    enemies[i].GetComponent<MinotaurBoss>().health -= damage;                    
                //}
            }

            currentTime = 0;
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.layer == (int)LayersEnum.ENEMY || c.gameObject.layer == (int)LayersEnum.BOSS)
            enemies.Add(c.gameObject);
    }

    private void OnTriggerExit(Collider c)
    {
        enemies.Remove(c.gameObject);
    }

    protected IEnumerator Destroy()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(this.gameObject);
    }
}
