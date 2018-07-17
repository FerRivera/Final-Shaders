using UnityEngine;
using System.Collections;
using FSM;

public class ArcherSkeletonFSM : EntityFSM
{
    public GameObject proyectil;
    public GameObject weaponFBX;
    public LayerMask walls;

    public float beingHitTimeTotal;
    float _beingHitActual;
    bool _beingHit;
    public float timeToEndSpawn;
    public float decidingTime;

    protected override void Start()
    {
        base.Start();

        var idle = new State<EnemiesAction>("idle");
        var beforeSpawn = new State<EnemiesAction>("beforeSpawn");
        var spawn = new State<EnemiesAction>("spawn");
        var chase = new State<EnemiesAction>("chasing");
        var attack = new State<EnemiesAction>("attack");
        var decide = new State<EnemiesAction>("decide");
        var scape = new State<EnemiesAction>("scape");
        var death = new State<EnemiesAction>("death");

        StateConfigurer.New(beforeSpawn)
        .SetTransition(EnemiesAction.Spawn, spawn)
        .Done();

        StateConfigurer.New(spawn)
        .SetTransition(EnemiesAction.Idle, idle)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(idle)
        .SetTransition(EnemiesAction.Chase, chase)
        .SetTransition(EnemiesAction.Spawn, spawn)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.Scape, scape)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(chase)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.Decide, decide)
        .SetTransition(EnemiesAction.Scape, scape)
        .SetTransition(EnemiesAction.Idle, idle)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(scape)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.Chase, chase)
        .SetTransition(EnemiesAction.Decide, decide)
        .SetTransition(EnemiesAction.Idle, idle)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(attack)
        .SetTransition(EnemiesAction.Idle, idle)
        .SetTransition(EnemiesAction.Decide, decide)
        .SetTransition(EnemiesAction.Scape, scape)
        .SetTransition(EnemiesAction.Chase, chase)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(decide)
        .SetTransition(EnemiesAction.Idle, idle)
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
        scape.OnEnter += action => ScapeEnter();
        death.OnEnter += action => DeathEnter();

        //idle.OnUpdate += IdleUpdate;
        chase.OnUpdate += ChaseUpdate;
        scape.OnUpdate += ScapeUpdate;
        //spawn.OnUpdate += SpawnUpdate;
        //beforeSpawn.OnUpdate += beforeSpawnUpdate;
        //attack.OnUpdate += AttackUpdate;

        //attack.OnExit += action => AttackExit();
        //chase.OnExit += action => ChaseExit();

        _fsm = new EventFSM<EnemiesAction>(beforeSpawn);

        _fsm.Feed(EnemiesAction.BeforeSpawn);

        if (proyectil != null)
        {
            proyectil.GetComponent<Arrow>().damage = damage;
            proyectil.GetComponent<Arrow>().speed = data.proyectilSpeed;
        }
            
    }    
    #region Enter
    public void SpawnEnter()
    {
        Spawn();
        StartCoroutine(Spawned());
        weaponFBX.GetComponent<MeshRenderer>().enabled = true;
    }

    public void ChaseEnter()
    {
        //DeactivateWeaponBoxCollider();
        speed = maxSpeed;
        if (_animator != null)
        {
            //_animator.SetTrigger("walk");
            _animator.Play("Run");
        }

        //_timeToAttackRockMinotaurTotal = Random.Range(1, 5);
    }

    public void ScapeEnter()
    {
        speed = maxSpeed;
        if (_animator != null)
        {
            //_animator.SetTrigger("walk");
            _animator.Play("Run");
        }
    }

    public void beforeSpawnEnter()
    {
        DeactivateWeaponBoxCollider();
        BeforeSpawned();

        if (weaponFBX != null)
            weaponFBX.GetComponent<MeshRenderer>().enabled = false;

        if (trigger != null)
        {
            trigger.chaseCharacter += () =>
            {
                if (this != null && _fsm != null)
                {
                    _fsm.Feed(EnemiesAction.Spawn);
                }
            };
        }

        if (triggerComplex != null)
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

    public void DecideEnter()
    {
        StartCoroutine(Decide());        
    }

    public void AttackEnter()
    {
        //_rotating = true;
        //speed = 0;

        //decidingTime = 0.7f;
        _animator.Play("RangeAttack");
        _animator.Play("RangeAttack", -1, 0);
        speed = 0;
        _rb.Sleep();
        //weapon.SetActive(false);
        _fsm.Feed(EnemiesAction.Decide);
        var enemyDir = new Vector3(_target.transform.position.x, 0, _target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
        transform.forward = enemyDir;
        StartCoroutine(Fire(new Vector3(transform.forward.x, 0, transform.forward.z), transform.position , decidingTime)); // solo se nota en el primer ataque
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
        _fsm = null;
        Destroy(parent.gameObject, timeToDisappear);
    }
    #endregion

    #region Update

    public void IdleUpdate()
    {
        _beingHitActual += Time.deltaTime;

        if (_beingHitActual >= beingHitTimeTotal && _beingHit)
        {
            _beingHit = false;
            _beingHitActual = 0;
            _fsm.Feed(EnemiesAction.Chase);
        }
    }

    public void SpawnUpdate()
    {
        Spawn();

        if (_spawned)
            _fsm.Feed(EnemiesAction.Chase);
    }

    IEnumerator Spawned()
    {
        yield return new WaitForSeconds(timeToEndSpawn);
        if (!dead)
            _fsm.Feed(EnemiesAction.Idle);
    }

    IEnumerator Decide()
    {
        yield return new WaitForSeconds(decidingTime);
        if (!dead)
            _fsm.Feed(EnemiesAction.Idle);
    }

    public void ChaseUpdate()
    {
        Vector3 _moveDir = _target.transform.position - transform.position;
        _moveDir = _moveDir.normalized;
        _moveDir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, _moveDir, rotationSpeed * Time.deltaTime);
        _rb.velocity = _moveDir * speed;
        //    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("RangeAttack"))
        //    {
        //        DeactivateWeaponBoxCollider();
        //        Vector3 _moveDir = _target.transform.position - transform.position;
        //        _moveDir.y = 0;

        //        Vector3 _scapeDir = transform.position - _target.transform.position;
        //        _scapeDir.y = 0;

        //        var dist = Vector3.Distance(transform.position, _target.transform.position);            

        //        if (Physics.Raycast(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up, 4, walls))
        //        {
        //            Debug.Log(Physics.Raycast(transform.position, transform.forward, 4, walls));
        //            _fsm.Feed(EnemiesAction.Attack);
        //            return;
        //        }


        //        if (dist > distanceToAttackTarget)
        //        {
        //            transform.forward = Vector3.Lerp(transform.forward, _moveDir, 3 * Time.deltaTime);
        //        }
        //        else if (dist < distanceToAttackTarget && dist < 7)
        //        {
        //            transform.forward = Vector3.Lerp(transform.forward, _scapeDir, 3 * Time.deltaTime);
        //        }
        //        else
        //        {
        //            _fsm.Feed(EnemiesAction.Attack);
        //        }
        //        if (_canAttackAgain)
        //        {
        //            _fsm.Feed(EnemiesAction.Attack);
        //        }
        //        if (_timeToAttackRockMinotaurActual >= _timeToAttackRockMinotaurTotal)
        //        {
        //            _timeToAttackRockMinotaurActual = 0;
        //            _canAttackAgain = true;
        //        }
        //        else
        //        {
        //            _timeToAttackRockMinotaurActual += Time.deltaTime;
        //        }
        //        if (_coll != null)
        //        {
        //            _rb.velocity = transform.forward * speed;
        //            //_coll.Move(transform.forward * speed * Time.deltaTime);
        //        }

        //    }

    }

    public void ScapeUpdate()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.position + transform.forward + Vector3.up, 2, walls))
        {
            Debug.DrawLine(transform.position + Vector3.up, transform.position + transform.forward * 2 + Vector3.up, Color.green);
            _fsm.Feed(EnemiesAction.Attack);
            return;
        }            

        Vector3 _scapeDir = transform.position - _target.transform.position;
        _scapeDir = _scapeDir.normalized;
        _scapeDir.y = 0;        
        transform.forward = Vector3.Lerp(transform.forward, _scapeDir, rotationSpeed * Time.deltaTime);
        _rb.velocity = _scapeDir * speed;
    }

    public void AttackUpdate()
    {
        //Vector3 targetDir = _target.transform.position - transform.position;
        //targetDir.y = transform.forward.y;

        //if (_rotating)
        //{
        //    Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotationSpeed * Time.deltaTime, 0.0F);

        //    if (transform.forward == newDir)
        //    {
        //        _rotating = false;
        //        _animator.SetTrigger("attack");
        //        StartCoroutine(Fire(new Vector3(newDir.x, 0, newDir.z), transform.position , 1.2f));
        //        //Fire(new Vector3(newDir.x, 0, newDir.z), transform.position);
        //    }
        //    else
        //    {
        //        transform.rotation = Quaternion.LookRotation(newDir);
        //    }
        //}

        //if (_animator.GetCurrentAnimatorStateInfo(0).IsName("RangeAttack"))
        //{
        //    _fsm.Feed(EnemiesAction.Chase);
        //}

        

    }
    #endregion

    #region Exit
    //public void AttackExit()
    //{
    //    speed = maxSpeed;
    //}

    //public void ChaseExit()
    //{
    //    _canAttackAgain = false;
    //}
    #endregion

    protected override void Update()
    {
        base.Update();

        if (_fsm != null)
        {
            if (health <= 0)
            {
                _fsm.Feed(EnemiesAction.Death);
                return;
            }
                

            if (_target.health <= 0)
            {
                StopAllCoroutines();
                _animator.Play("Idle");
                speed = 0;
                return;
            }

            //if (_beingHit)
            //    return;

            int dist = (int)Vector3.Distance(transform.position, _target.transform.position);

            if (_fsm != null && dist < distanceToAttackTarget && dist < 7)
                _fsm.Feed(EnemiesAction.Scape);
            else if (_fsm != null && dist <= distanceToAttackTarget)
                _fsm.Feed(EnemiesAction.Attack);
            else if (_fsm != null)
                _fsm.Feed(EnemiesAction.Chase);
        }
    }

    IEnumerator Fire(Vector3 dir, Vector3 pos, float timer)
    {
        yield return new WaitForSeconds(timer);
        if (dead)
            yield break;
        var p = Instantiate(proyectil,transform.position,Quaternion.identity);
        p.transform.forward = dir;
        p.transform.position = new Vector3(transform.position.x,transform.position.y + 1,transform.position.z);
        p.GetComponent<Proyectiles>().owner = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.up,transform.position + transform.forward * 2 + Vector3.up);        
    }

    //protected override void OnTriggerEnter(Collider c)
    //{
    //    base.OnTriggerEnter(c);

    //    if (c.gameObject.layer == (int)LayersEnum.SWORD || c.gameObject.layer == (int)LayersEnum.SKILL)
    //    {
    //        _rb.Sleep();
    //        _beingHitActual = 0;
    //        StopAllCoroutines();
    //        _animator.Play("SGetHit");
    //        _animator.Play("SGetHit", -1, 0);
    //        _beingHit = true;
    //        _fsm.Feed(EnemiesAction.Idle);
    //    }
    //}
}
