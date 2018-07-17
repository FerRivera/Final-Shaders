using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : SkillProyectil
{
    bool _start;
    public float delay;

    protected override void Start ()
    {
        base.Start();
        StartCoroutine(Init(delay));
        Item item = _itemsDatabase.FetchItemByID(5);
        _damage = item.HitDamage;
        _textColor = Color.white;
        _skillInitialized = true;
    }

    protected override void Update ()
    {
        base.Update();

        if (_start)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (_start)
        {
            if (c.gameObject.layer == (int)LayersEnum.ENEMY)
            {
                EventsManager.TriggerEvent(EventsType.spawnText, new object[] { c.GetComponent<EntityFSM>().canvasHP.GetComponentInParent<RectTransform>(), _damage, _textColor });
                c.GetComponent<EntityFSM>().health -= _damage;
                var particle = Instantiate(explosion);
                particle.transform.position = this.transform.position;
                Destroy(this.gameObject);
            }
            if (c.gameObject.layer == (int)LayersEnum.BOSS)
            {
                EventsManager.TriggerEvent(EventsType.spawnText, new object[] { c.GetComponent<MinotaurBoss>().canvasHP.GetComponentInParent<RectTransform>(), _damage, _textColor });
                c.GetComponent<MinotaurBoss>().health -= _damage;
                var particle = Instantiate(explosion);
                particle.transform.position = this.transform.position;
                Destroy(this.gameObject);
            }
            if (c.gameObject.layer == (int)LayersEnum.FLOOR || c.gameObject.layer == (int)LayersEnum.WALL)
            {
                var particle = Instantiate(explosion);
                particle.transform.position = this.transform.position;
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator Init(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<BoxCollider>().enabled = true;
        _start = true;
    }
}
