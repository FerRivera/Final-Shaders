using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostNova : SkillSelfCast
{
    public float radius;
    public float speedPercentToReduce;
    Collider[] _colliders;
    List<float> _enemiesCurrentSpeed = new List<float>();
    public LayerMask enemiesToAffect;
    Item _item;
    public ParticleSystem particle;
    public float particlePosY;

	protected override void Start ()
    {
        base.Start();
        _item = _itemsDatabase.FetchItemByID(27);

        _colliders = (Physics.OverlapSphere(caster.transform.position, radius, enemiesToAffect));

        if(_colliders.Length > 0)
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                //if(_colliders[i].GetComponent<EnemyCharacter>())

                _enemiesCurrentSpeed.Add(_colliders[i].GetComponent<EnemyCharacter>().speed);
                var speedToReduce = (_colliders[i].GetComponent<EnemyCharacter>().speed * speedPercentToReduce) / 100;
                if (_colliders[i].GetComponent<EntityFSM>())
                    _colliders[i].GetComponent<EntityFSM>().maxSpeed -= speedToReduce;
                _colliders[i].GetComponent<EnemyCharacter>().speed -= speedToReduce;

                //if (_colliders[i].GetComponent<EntityFSM>())
                //{
                //    _enemiesCurrentSpeed.Add(_colliders[i].GetComponent<EntityFSM>().maxSpeed);
                //    var speedToReduce = (_colliders[i].GetComponent<EntityFSM>().maxSpeed * speedPercentToReduce) / 100;
                //    _colliders[i].GetComponent<EntityFSM>().maxSpeed -= speedToReduce;
                //    _colliders[i].GetComponent<EnemyCharacter>().speed -= speedToReduce;
                //}
                //else if (_colliders[i].GetComponent<MinotaurBoss>())
                //{
                //    _enemiesCurrentSpeed.Add(_colliders[i].GetComponent<EnemyCharacter>().speed);
                //    var speedToReduce = (_colliders[i].GetComponent<MinotaurBoss>().speed * speedPercentToReduce) / 100;
                //    _colliders[i].GetComponent<EnemyCharacter>().speed -= speedToReduce;
                //}
            }
        }

        particle = Instantiate(particle);
        particle.transform.position = new Vector3(caster.transform.position.x, particlePosY, caster.transform.position.z);
        particle.transform.SetParent(transform);

        StartCoroutine(Destroy());
	}
	
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(timeToDestroy);

        for (int i = 0; i < _colliders.Length; i++)
        {
            if (_colliders[i].GetComponent<EntityFSM>())
                _colliders[i].GetComponent<EntityFSM>().maxSpeed = _enemiesCurrentSpeed[i];
            _colliders[i].GetComponent<EnemyCharacter>().speed = _enemiesCurrentSpeed[i];

            //if (_colliders[i].GetComponent<EntityFSM>())
            //{
            //    _colliders[i].GetComponent<EntityFSM>().maxSpeed = _enemiesCurrentSpeed[i];
            //    _colliders[i].GetComponent<EntityFSM>().speed = _enemiesCurrentSpeed[i];
            //}
            //else if (_colliders[i].GetComponent<MinotaurBoss>())
            //    _colliders[i].GetComponent<MinotaurBoss>().speed = _enemiesCurrentSpeed[i];

        }

        Destroy(this.gameObject);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(caster.transform.position,radius);
    }

    protected override void Update ()
    {
        base.Update();
	}
}
