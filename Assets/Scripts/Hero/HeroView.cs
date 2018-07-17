using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroView : MonoBehaviour
{
    Animator _animator;
    //bool _idle;
    bool _moving;
    [HideInInspector]public bool attacking;
    bool _dashing;
    bool _blocking;
    bool _walkAndBlock;
    //bool _isDeath;
    //bool _isCasting;
    HeroModel _hero;

    void Start ()
    {
        _animator = GetComponent<Animator>();
        _hero = GetComponent<HeroModel>();
    }
	
	void Update ()
    {
        //if(_hero.velX > 0)
        //WalkAndBlock();

        if (!_hero.shieldActive)
        {
            _blocking = false;
            _walkAndBlock = false;
            _animator.SetBool("Block", _blocking);
            _animator.SetBool("WalkAndBlock", _walkAndBlock);

            if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Block"))
                _animator.SetTrigger("Idle");
        }

        //if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && _walkAndBlock)
        //{
        //    _animator.SetBool("WalkAndBlock", _walkAndBlock);
        //}

        //if (_hero.attacking)
        //{
        //    _blocking = false;
        //    _walkAndBlock = false;
        //}           

        if (!_hero.dashing)
        {
            _dashing = false;
        }
        
        Death();
    }

    public void Idle()
    {
        if (_blocking || _walkAndBlock || attacking || _dashing /*|| _moving*/)
            return;

        if(_moving)
        {
           
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                _animator.SetTrigger("RunToIdle");
        }
        else
        {
       
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                _animator.SetTrigger("Idle");
            
        }
        _dashing = false;
        _moving = false;
        _blocking = false;
        _walkAndBlock = false;
        //_idle = true;
    }

    public void Attack()
    {
        if (_blocking || /*_dashing ||*/ attacking)
            return;

        //_idle = false;
        _dashing = false;
        _moving = false;
        _blocking = false;
        attacking = true;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            _animator.SetTrigger("Attack1");

    }

    public void Movement()
    {
        if (_walkAndBlock || attacking || _blocking)
            return;

        //_idle = false;
        _moving = true;
        _dashing = false;
        //_blocking = false;
        attacking = false;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            _animator.SetTrigger("Run");
    }

    public void Dashing()
    {
        if (_blocking || _walkAndBlock)
            return;

        //_idle = false;
        _moving = false;
        _dashing = true;

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
            _animator.SetTrigger("Dash");
    }

    public void Block()
    {
        if (attacking)
            return;
        //attacking = false;
        _dashing = false;
        if(_hero.velX <= 0)
        {
            _blocking = true;
            _walkAndBlock = false;
        }
        else
        {
            _blocking = false;
            _walkAndBlock = true;
        }

        _animator.SetBool("Block", _blocking);
        _animator.SetBool("WalkAndBlock", _walkAndBlock);
    }

    public void Unblock()
    {
        _blocking = false;
        _walkAndBlock = false;
        _animator.SetBool("Block", _blocking);
        _animator.SetBool("WalkAndBlock", _walkAndBlock);
    }

    public void WalkAndBlock()
    {
        if (attacking)
            return;
        //attacking = false;
        //_animator.ResetTrigger("Run");
        if (_blocking && _moving)
        {
            _walkAndBlock = true;
            _blocking = false;
            //_moving = false;
        }
        //else
        //    walkAndBlock = false;

        _animator.SetBool("WalkAndBlock", _walkAndBlock);
        _animator.SetBool("Block", _blocking);
    }

    public void SkillCast()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Casting"))
            _animator.SetTrigger("Cast");

        //_isCasting = true;
    }


    public void Death()
    {
        if(_hero.deadHero && !_animator.GetCurrentAnimatorStateInfo(0).IsName("PJDeath"))
        {
            //_idle = false;
            _moving = false;
            attacking = false;
            _dashing = false;
            _blocking = false;
            _walkAndBlock = false;
            _animator.SetBool("Block", _blocking);
            _animator.SetBool("WalkAndBlock", _walkAndBlock);
            _animator.SetTrigger("Death");
        }            
    }
}
