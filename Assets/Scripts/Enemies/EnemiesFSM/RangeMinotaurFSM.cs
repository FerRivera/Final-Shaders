using UnityEngine;
using System.Collections;
using FSM;

public class RangeMinotaurFSM : EntityFSM
{
    public GameObject proyectil;
    public GameObject proyectilProjector;
    bool _rotating;
    bool _canAttackAgain;
    float _timeToAttackRockMinotaurActual;
    float _timeToAttackRockMinotaurTotal;
    public float firingAngle;
    public float gravity;
    public float timeToThrowRock;

    protected override void Start()
    {

        base.Start();
        firingAngle = 45f;
        gravity = 9.8f;
        var idle = new State<EnemiesAction>("idle");
        var beforeSpawn = new State<EnemiesAction>("beforeSpawn");
        var spawn = new State<EnemiesAction>("spawn");
        var chase = new State<EnemiesAction>("chasing");
        var attack = new State<EnemiesAction>("attack");
        var death = new State<EnemiesAction>("death");

        StateConfigurer.New(beforeSpawn)
        .SetTransition(EnemiesAction.Spawn, spawn)
        .Done();

        StateConfigurer.New(spawn)
        .SetTransition(EnemiesAction.Chase, chase)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(idle)
        .SetTransition(EnemiesAction.Chase, chase)
        .SetTransition(EnemiesAction.Spawn, spawn)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(chase)
        .SetTransition(EnemiesAction.Attack, attack)
        .SetTransition(EnemiesAction.Death, death)
        .Done();

        StateConfigurer.New(attack)
        .SetTransition(EnemiesAction.Chase, chase)
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
        death.OnEnter += action => DeathEnter();

        spawn.OnUpdate += SpawnUpdate;
        chase.OnUpdate += ChaseUpdate;
        beforeSpawn.OnUpdate += beforeSpawnUpdate;
        attack.OnUpdate += AttackUpdate;

        attack.OnExit += action => AttackExit();
        chase.OnExit += action => ChaseExit();

        _fsm = new EventFSM<EnemiesAction>(beforeSpawn);

        _fsm.Feed(EnemiesAction.BeforeSpawn);

        if (proyectil != null)
        {
            proyectil.GetComponent<RockProyectil>().damage = damage;
            proyectil.GetComponent<RockProyectil>().speed = data.proyectilSpeed;
        }
    }    
    #region Enter
    public void SpawnEnter()
    {
        Spawn();
        //_animator.Play("Spawn");
    }

    public void ChaseEnter()
    {
        //DeactivateWeaponBoxCollider();
        speed = maxSpeed;
        if (_animator != null)
        {
            _animator.SetTrigger("walk");
        }

        _timeToAttackRockMinotaurTotal = Random.Range(1, 5);
    }

    public void beforeSpawnEnter()
    {
        DeactivateWeaponBoxCollider();
        BeforeSpawned();

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

    public void AttackEnter()
    {
        _rotating = true;
        speed = 0;
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
    public void SpawnUpdate()
    {
        Spawn();

        if (_spawned)
            _fsm.Feed(EnemiesAction.Chase);
    }

    public void ChaseUpdate()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("MBasicAttack"))
        {
            DeactivateWeaponBoxCollider();
            Vector3 _moveDir = _target.transform.position - transform.position;
            _moveDir.y = 0;

            Vector3 _scapeDir = transform.position - _target.transform.position;
            _scapeDir.y = 0;

            var dist = Vector3.Distance(transform.position, _target.transform.position);

            if (dist > distanceToAttackTarget)
            {
                transform.forward = Vector3.Lerp(transform.forward, _moveDir, 3 * Time.deltaTime);
            }
            else if (dist < distanceToAttackTarget && dist < 7) 
            {
                transform.forward = Vector3.Lerp(transform.forward, _scapeDir, 3 * Time.deltaTime);
            }
            else
            {
                _fsm.Feed(EnemiesAction.Attack);
               
            }
            if (_canAttackAgain)
            {
                _fsm.Feed(EnemiesAction.Attack);
            }
            if (_timeToAttackRockMinotaurActual >= _timeToAttackRockMinotaurTotal)
            {
                _timeToAttackRockMinotaurActual = 0;
                _canAttackAgain = true;
            }
            else
            {
                _timeToAttackRockMinotaurActual += Time.deltaTime;
            }
            if (_coll != null)
            {
                _rb.velocity = transform.forward * speed;
                //_coll.Move(transform.forward * speed * Time.deltaTime);
            }

        }

    }

    public void beforeSpawnUpdate()
    {
        trigger.chaseCharacter += () =>
        {
            if (this != null)
            {
                _fsm.Feed(EnemiesAction.Spawn);
            }
            //_activated = true;
        };
    }

    public void AttackUpdate()
    {
        Vector3 targetDir = _target.transform.position - transform.position;
        targetDir.y = transform.forward.y;

        if (_rotating)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, rotationSpeed * Time.deltaTime, 0.0F);

            if (transform.forward == newDir)
            {
                _rotating = false;
               _animator.SetTrigger("attack");
                StartCoroutine(SimulateProjectile(_target.transform));
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("MBasicAttack"))
        {                      
            _fsm.Feed(EnemiesAction.Chase);
        }

    }
    #endregion

    #region Exit
    public void AttackExit()
    {
        speed = maxSpeed;
    }

    public void ChaseExit()
    {
        _canAttackAgain = false;
    }
    #endregion

    protected override void Update()
    {
        base.Update();

        if (health <= 0 && _fsm != null)
        {
            _fsm.Feed(EnemiesAction.Death);
            return;
        }
            

        if (_target.health <= 0)
        {
            _animator.Play("Idle");
            speed = 0;
            return;
        }

    }

    IEnumerator SimulateProjectile(Transform target)
    {
        if (dead)
            yield break;

        yield return new WaitForSeconds(timeToThrowRock);

        if (dead)
            yield break;

        var p = Instantiate(proyectil);
        p.GetComponent<Proyectiles>().owner = this;
        var proyector = Instantiate(proyectilProjector, new Vector3(target.position.x,target.position.y+10,target.position.z),Quaternion.Euler(90,0,0));

        p.transform.position = transform.position;

        float target_Distance = Vector3.Distance(p.transform.position, target.position - new Vector3(0,-transform.position.y,0));

        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);


        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        float flightDuration = target_Distance / Vx;
        proyector.GetComponent<Destroy>().time = (int)flightDuration;
        p.transform.rotation = Quaternion.LookRotation(target.position - p.transform.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration && p != null)
        {
            p.transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
           
            yield return null;
        }
        if(elapse_time > flightDuration && p != null)
        {
            p.GetComponent<RockProyectil>().DestroyProyectil();
        }
       
    }
    public void Fire(Vector3 dir, Vector3 pos)
    {
        var p = Instantiate(proyectil);
        p.transform.position = pos + transform.forward * 2;
        p.transform.forward = dir;
    }
}
