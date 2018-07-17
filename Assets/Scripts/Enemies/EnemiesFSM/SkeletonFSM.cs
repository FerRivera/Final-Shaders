﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine.AI;

public class SkeletonFSM : EntityFSM
{
    public float timeToActivateWeapon;
    public float decidingTime;
    public float timeToEndSpawn;

    public float beingHitTimeTotal;
    float _beingHitActual;
    bool _beingHit;

    protected override void Start()
    {
        base.Start();

        var idle = new State<EnemiesAction>("idle");
        var beforeSpawn = new State<EnemiesAction>("beforeSpawn");
        var spawn = new State<EnemiesAction>("spawn");
        var chase = new State<EnemiesAction>("chasing");
        var attack = new State<EnemiesAction>("attack");
        var death = new State<EnemiesAction>("death");
        var decide = new State<EnemiesAction>("decide");
        var getHit = new State<EnemiesAction>("getHit");

        StateConfigurer.New(beforeSpawn)
        .SetTransition(EnemiesAction.Spawn, spawn)
        .Done();

        StateConfigurer.New(spawn)
        .SetTransition(EnemiesAction.Idle, idle)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(idle)
        .SetTransition(EnemiesAction.Chase, chase)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.GetHit, getHit)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(chase)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.Decide, decide)
        .SetTransition(EnemiesAction.Idle, idle)
        .SetTransition(EnemiesAction.GetHit, getHit)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(attack)
        .SetTransition(EnemiesAction.Chase, chase)
        .SetTransition(EnemiesAction.Decide, decide)
        .SetTransition(EnemiesAction.Idle, idle)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(decide)
        .SetTransition(EnemiesAction.Idle, idle)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(getHit)
        .SetTransition(EnemiesAction.Idle, idle)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.Chase, chase)
        .SetTransition(EnemiesAction.Decide, decide)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(death)
        .SetTransition(EnemiesAction.Death, death)
        .SetTransition(EnemiesAction.Null, null)
        .Done();

        spawn.OnEnter += action => SpawnEnter();
        chase.OnEnter += action => ChaseEnter();
        beforeSpawn.OnEnter += action => beforeSpawnEnter();
        attack.OnEnter += action => AttackEnter();
        decide.OnEnter += action => DecideEnter();
        //getHit.OnEnter += action => GetHitEnter();
        death.OnEnter += action => DeathEnter();

        chase.OnUpdate += ChaseUpdate;
        idle.OnUpdate += IdleUpdate;
        //getHit.OnUpdate += GetHitUpdate;

        _fsm = new EventFSM<EnemiesAction>(beforeSpawn);

        _fsm.Feed(EnemiesAction.BeforeSpawn); 
    }

    #region Enter
    public void SpawnEnter()
    {
        _spawned = false;
        Spawn();
        //_animator.Play("SSpawn");
        StartCoroutine(Spawned());
    }

    public void ChaseEnter()
    {
        //DeactivateWeaponBoxCollider();
        //decidingTime = 0.7f;
        speed = maxSpeed;
        if (_animator != null)
        {
            _animator.Play("Run");
        }
    }

    public void beforeSpawnEnter()
    {
        DeactivateWeaponBoxCollider();
        BeforeSpawned();

        if(trigger != null)
        {
            trigger.chaseCharacter += () =>
            {
                if (this != null && _fsm != null)
                {
                    _fsm.Feed(EnemiesAction.Spawn);
                }
            };
        }        

        if(triggerComplex != null)
        {
            triggerComplex.chaseCharacter += () =>
            {
                if (this != null && _fsm != null)
                {
                    _fsm.Feed(EnemiesAction.Spawn);
                }
            };
        }
        
    }

    public void AttackEnter()
    {        
        decidingTime = 0.7f;
        _animator.Play("SAttackWithoutSword");        
        speed = 0;
        _rb.Sleep();
        weapon.SetActive(false);
        _fsm.Feed(EnemiesAction.Decide);
        StartCoroutine(DeactivateWeapon(timeToActivateWeapon));
        var enemyDir = new Vector3(_target.transform.position.x, 0, _target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
        transform.forward = enemyDir;
    }

    public void DecideEnter()
    {
        StartCoroutine(Decide());
    }

    public void DeathEnter()
    {
        dead = true;
        speed = 0;        
        _animator.SetTrigger("dead");
        RemoveBothColliders();
        if (weapon != null)
        {
            if (weapon.GetComponent<Rigidbody>() == null)
            {
                weapon.AddComponent<Rigidbody>();
            }
            weapon.transform.parent = null;
        }
        //_fsm.Feed(EnemiesAction.Null);
        //_fsm = null;
        Destroy(parent.gameObject, timeToDisappear);
    }
    #endregion

    #region Update

    public void ChaseUpdate()
    {
        Vector3 _moveDir = _target.transform.position - transform.position;
        _moveDir = _moveDir.normalized;
        _moveDir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, _moveDir, rotationSpeed * Time.deltaTime);
        _rb.velocity = _moveDir * speed;
    }

    public void IdleUpdate()
    {
        _beingHitActual += Time.deltaTime;

        if (_beingHitActual >= beingHitTimeTotal && _beingHit)
        {
            _beingHit = false;
            _beingHitActual = 0;

        }

    }
    #endregion

    IEnumerator DeactivateWeapon(float timeToActivateWeapon)
    {
        yield return new WaitForSeconds(timeToActivateWeapon);
        weapon.SetActive(true);
        yield return new WaitForSeconds(0.2f); //para volver a desactivar el collider del arma       
        weapon.SetActive(false);
    }

    IEnumerator Spawned()
    {
        yield return new WaitForSeconds(timeToEndSpawn);
        if (!dead)
        {
            _fsm.Feed(EnemiesAction.Idle);
            _spawned = true;
        }
            
    }

    IEnumerator Decide()
    {
        yield return new WaitForSeconds(decidingTime);        
        if (!dead)
            _fsm.Feed(EnemiesAction.Idle);
    }

    protected override void Update()
    {        
        base.Update();

        if (_fsm != null || _target.health > 0)
        {
            if (health <= 0)
            {
                _fsm.Feed(EnemiesAction.Death);
                return;
            }                

            if (_target.health <= 0)
            {
                _animator.Play("SIdle");
                speed = 0;
                return;
            }

            if (_beingHit)
                return;

            if ((int)Vector3.Distance(transform.position, _target.transform.position) <= distanceToAttackTarget)
                _fsm.Feed(EnemiesAction.Attack);
            else
                _fsm.Feed(EnemiesAction.Chase);
        }

    }

    protected override void OnTriggerEnter(Collider c)
    {
        base.OnTriggerEnter(c);

        if (c.gameObject.layer == (int)LayersEnum.SWORD || c.gameObject.layer == (int)LayersEnum.SKILL)
        {
            _rb.Sleep();    
            _beingHitActual = 0;
            if(_spawned)
                StopAllCoroutines();       
            _animator.Play("SGetHit");
            _animator.Play("SGetHit", -1, 0);
            _beingHit = true;
            _fsm.Feed(EnemiesAction.Idle);
        }
    }
}
