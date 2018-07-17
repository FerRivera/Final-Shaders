using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    HeroModel _model;
    HeroView _view;

    void Start ()
    {
        _model = GetComponent<HeroModel>();
        _view = GetComponent<HeroView>();

        _model.OnMovement += _view.Movement;
        _model.OnIdle += _view.Idle;
        _model.OnDash += _view.Dashing;
        _model.OnBlocking += _view.Block;
        _model.OnUnblocking += _view.Unblock;
        _model.OnWalkingAndBlock += _view.WalkAndBlock;
        _model.OnAttack += _view.Attack;
        _model.OnCasting += _view.SkillCast;
        _model.OnDeath += _view.Death;

    }
	
	void Update ()
    {
        if (pauseGame.pauseGameInstance.deny || _model.stun)
            return;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            _model.Movement();
        else
            _model.Idle();

        if (Input.GetKeyDown(KeyCode.Space))
            _model.Dash();

        if (Input.GetKey(KeyCode.LeftShift))
            _model.Block();
        //else
        //    _model.Unblock();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            _model.Attack();

        if (Input.GetKeyDown(KeyCode.Q))
            _model.CastSkill(KeyCode.Q);
        else if (Input.GetKeyDown(KeyCode.E))
            _model.CastSkill(KeyCode.E);
        else if (Input.GetKeyDown(KeyCode.Mouse1))
            _model.CastSkill(KeyCode.Mouse1);
        else if (Input.GetKeyDown(KeyCode.F))
            _model.CastSkill(KeyCode.F);
    }
}
