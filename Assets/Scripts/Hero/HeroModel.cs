using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroModel : MonoBehaviour
{
    public event Action OnMovement = delegate { };
    public event Action OnIdle = delegate { };
    public event Action OnDash = delegate { };
    public event Action OnBlocking = delegate { };
    public event Action OnUnblocking = delegate { };
    public event Action OnWalkingAndBlock = delegate { };
    public event Action OnAttack = delegate { };
    public event Action OnDeath = delegate { };
    public event Action OnCasting = delegate { };

    public float velY;
    public float velX;
    public float velXShield;
    public float hitDisplacement;
    public float dashDisplacement;
    //public float dashDuration;
    public float dashStaminaCost;
    //public float shieldRotationSpeed;
    [HideInInspector]public float stamina;
    public float maxStamina;
    public float hitStaminaCost;
    public float blockStaminaCost;
    public float dashCD;    
    public float maxHealth;
    [HideInInspector]
    public float maxhealthInitial;
    [HideInInspector]
    public float health;
    public float defence;
    [HideInInspector]
    public float damage;
    public float damageFirstHit;
    public float damageSecondHit;
    public float damageThirdHit;
    public float damageOffset;
    //public float timeToActiveWeaponCollider;
    [HideInInspector]
    public float actualTimeToTriggerCombo;
    public float timeToTriggerCombo;
    public float shieldAngle; //85
    public float shieldDistance; //3
    [HideInInspector]
    public float damageToReduce;
    [HideInInspector]
    public float saveHp;
    public float blockDefencePercent;
    //public float timeToUnblock;

    float _actualTimeToUnblock;
    float _totalTimeToUnblock;

    public int gold;

    float _maxVelX;
    //float _dashLerp;

    //CharacterController _cc;
    [HideInInspector]
    public Rigidbody rb;

    //bool _moving;
    //bool _idle;
    [HideInInspector]public bool dashing;
    [HideInInspector]public bool shieldActive;
    [HideInInspector]public bool healing;
    [HideInInspector]public bool chargeHit;
    //bool _walkAndBlock;
    bool _blockCanvasDeath;
    [HideInInspector]public bool attacking;
    [HideInInspector]
    public bool casting;
    [HideInInspector]
    public bool attackNotReady;
    [HideInInspector]
    public bool deadHero;
    [HideInInspector]
    public bool recoverStamina;
    //[HideInInspector]
    public bool stun;
    bool _lowSpeed;

    public LayerMask map;
    public LayerMask collisionsForDash;

    public Projector projectorShield;

    [HideInInspector]public BoxCollider swordCollider;

    public GameObject sword;
    public GameObject deathParticle;
    public GameObject canvasDeath;
    public GameObject projectorFillProyectil;
    public GameObject projectorFillProyectilDistance;
    public GameObject projectorSkillAOE;
    public GameObject projectorSkillAOEFill;
    public GameObject healthBar;
    public GameObject swordTrail;
    public GameObject shield;

    public Projector projectorSkill;

    public Dictionary<KeyCode, Slot> allSlots = new Dictionary<KeyCode, Slot>();

    [HideInInspector]
    public Stack<int> hitsAmount = new Stack<int>();

    public Slot _q;
    public Slot _e;
    public Slot _rc;
    public Slot _f;

    [HideInInspector]public Inventory inv;

    public Color textColor;

    [HideInInspector]
    public int hitsCountForSound;

    void Start()
    {
        _totalTimeToUnblock = 0.1f;
        damage = damageFirstHit;
        //allSkills = new List<GameObject>(3);
        swordTrail.SetActive(false);
        textColor = Color.red;
        //_cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        _maxVelX = velX;
        health = maxHealth;
        swordCollider = transform.FindChild("SwordHit").GetComponent<BoxCollider>();
        inv = Finder.Instance.inventory;
        allSlots.Add(KeyCode.Q,_q);
        allSlots.Add(KeyCode.E,_e);
        allSlots.Add(KeyCode.Mouse1,_rc);
        allSlots.Add(KeyCode.F,_f);
    }

    public void Idle()
    {
        //if (_walkAndBlock || _shieldActive || _dashing)
        //    return;

        if(!dashing)
            rb.velocity = Vector3.zero;

        recoverStamina = true;
        //_idle = true;
        //_moving = false;
        //attacking = false; //comentar esto cuando haga el combo
        OnIdle();
        velX = 0;
    }

    public void Movement()
    {
        if (attacking || dashing || casting)
            return;

        //_moving = true;
        //shieldActive = false;
        recoverStamina = true;

        OnMovement();
        velX = _maxVelX;

        if (_lowSpeed)
            velX = velX / 2;            

        float zAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(xAxis, 0.0f, zAxis).normalized;

        //if (movement != Vector3.zero)
        //    _cc.transform.rotation = Quaternion.LookRotation(movement);

        //if (movement != Vector3.zero)
        //    _cc.Move(movement * velX * Time.deltaTime);

        if (movement != Vector3.zero)
            rb.MoveRotation(Quaternion.LookRotation(movement));//rb.transform.rotation = Quaternion.LookRotation(movement);

        rb.velocity = movement * velX;
        // (movement * velX * Time.deltaTime);
    }

    public void Dash()
    {
        if (stamina <= dashStaminaCost || shieldActive || attacking || dashing || casting)
            return;

        OnDash();
        stamina -= dashStaminaCost;
        recoverStamina = false;
        dashing = true;
        rb.velocity = transform.forward * dashDisplacement;
        Invoke("CancelDashMovement", 0.5f);
        //iTween.MoveBy(gameObject, iTween.Hash("z", dashLength, "time", dashDuration, "easetype", iTween.EaseType.linear, "oncomplete", "OnTweenDashComplete"));
    }

    public void DashCheker()
    {
        if (Physics.Raycast(transform.position, transform.forward, 1, collisionsForDash))
            Invoke("CancelDashMovement", 0);
    }

    public void CancelDashMovement()
    {
        rb.velocity = Vector3.zero;
        dashing = false;
    }

    public void Block()
    {
        if (attacking || casting || stamina < blockStaminaCost)
            return;
            
        _lowSpeed = true;
        recoverStamina = false;
        shieldActive = true;
        OnBlocking();
        transform.forward = (GetMousePos() - transform.position).normalized;//Vector3.Lerp(transform.position, GetMousePos(), shieldRotationSpeed);
        projectorShield.enabled = true;
        _actualTimeToUnblock = 0;
        shield.SetActive(true);
    }

    //public void Unblock()
    //{
    //    //_lowSpeed = false;
    //    //recoverStamina = true;
    //}

    public void UnblockTimer()
    {
        if (!attacking && shieldActive && Input.GetKeyUp(KeyCode.LeftShift) )
            return;

        _actualTimeToUnblock += Time.deltaTime;

        if(_actualTimeToUnblock >= _totalTimeToUnblock)
        {
            _lowSpeed = false;
            recoverStamina = true;
            shieldActive = false;
            //attacking = false;
            projectorShield.enabled = false;
            OnUnblocking();
            shield.SetActive(false);
        }        
    }

    public void CastSkill(KeyCode skill)
    {
        if (attacking || dashing || casting)
            return;

        if (allSlots[skill].transform.childCount > 1 && !allSlots[skill].locked)
        {
            casting = true;
            OnCasting();            
            allSlots[skill].Cast();
        }
    }

    public void Attack()
    {
        if (attackNotReady || shieldActive || stamina <= hitStaminaCost /*|| attacking*/ || dashing)
            return;

        Vector3 forward = (GetMousePos() - transform.position).normalized;
        this.transform.forward = forward;
        rb.MoveRotation(Quaternion.LookRotation(forward));// = ;
        //iTween.MoveBy(gameObject, iTween.Hash("z", hitLength, "time", hitDuration, "easetype", iTween.EaseType.linear));
        rb.velocity = transform.forward * hitDisplacement;
        Invoke("CancelAttackMovement", 0.1f);
        swordTrail.SetActive(true);
        recoverStamina = false;
        attacking = true;
        OnAttack();
        swordCollider.enabled = true;
        stamina -= hitStaminaCost;
        attackNotReady = true;        

        //if (hitsCountForSound == 0)
        //    AudioManager.instance.PlaySound(SoundsEnum.HERO_ATTACK_HIT_1);
        //else if (hitsCountForSound == 1)
        //    AudioManager.instance.PlaySound(SoundsEnum.HERO_ATTACK_HIT_2);
        //else
        //{            
        //    AudioManager.instance.PlaySound(SoundsEnum.HERO_ATTACK_HIT_3);
        //}

        //hitsCountForSound++;

    }

    public void CancelAttackMovement()
    {
        rb.velocity = Vector3.zero;
    }

    public Vector3 GetMousePos()
    {
        RaycastHit MousePosHit;
        Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(MouseRay, out MousePosHit, float.MaxValue, map))
        {
            var dir = MousePosHit.point + (Camera.main.transform.position - MousePosHit.point).normalized;
            return new Vector3(dir.x, transform.position.y, dir.z);
        }
        else
        {
            return transform.forward;
        }
    }

    public Vector3 GetMousePosAoE()
    {
        RaycastHit MousePosHit;
        Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(MouseRay, out MousePosHit, float.MaxValue, map))
        {
            return new Vector3(MousePosHit.point.x, 0, MousePosHit.point.z);
        }
        else
        {
            return transform.forward;
        }
    }

    public int GetDamageValue()
    {
        return (int)UnityEngine.Random.Range(damage - damageOffset, damage);
    }

    void CheckHP()
    {
        if (health <= 0)
        {
            health = 0;
            //inputBlock = true;
            Destroy(GetComponent<HeroController>());

            if (!_blockCanvasDeath)
            {
                deadHero = true;
                _blockCanvasDeath = true;
                Instantiate(deathParticle, transform.position, Quaternion.identity);
                Destroy(rb);
                GetComponent<CapsuleCollider>().enabled = false;
                StartCoroutine(ActivateDeathCanvas());
            }
        }
    }

    void Update()
    {
        //Debug.Log(hitsCountForSound + "sound amount");
        if (!pauseGame.paused)
        {
            CheckHP();
        }

        UnblockTimer();

        //if (stamina < blockStaminaCost)
        //    Unblock();        

        if (dashing)
        {
            DashCheker();
        }
        else if (attacking && !shieldActive)
        {
            if (timeToTriggerCombo <= actualTimeToTriggerCombo && hitsAmount.Count < 2 && Input.GetKeyDown(KeyCode.Mouse0))
            {
                hitsAmount.Push(1);
                actualTimeToTriggerCombo = 0;
            }
            else
            {
                actualTimeToTriggerCombo += Time.deltaTime;
            }
        }        
    }

    IEnumerator ActivateDeathCanvas()
    {
        yield return new WaitForSeconds(2);
        canvasDeath.SetActive(true);
    }

    public bool InFrontOfShield(Vector3 enemyPos)
    {
        var dirToTarget = enemyPos - transform.position;

        var angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

        var distanceToTarget = Vector3.Distance(transform.position - (transform.forward * 0.5f), enemyPos);

        if (angleToTarget <= shieldAngle && distanceToTarget <= shieldDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float CalcFinalDefense(float dmg)
    {
        var calc = (defence * dmg) / 100;
        damageToReduce = calc;
        return calc;
    }

    public float CalcFinalShieldDefense(float dmg)
    {
        var calc = (blockDefencePercent * dmg) / 100;
        return calc;
    }

    public IEnumerator ChargeMinotaurDamageAgain()
    {
        yield return new WaitForSeconds(1);
        chargeHit = false;
    }

    public IEnumerator UnStun(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        stun = false;
    }
}
