﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerState
{
    IDLE,
    DASH,
    ATTACK,
    MOVE_ATTACK
}

namespace Shared
{
    public delegate void Callback();
}
public class Player : MonoBehaviour {

    public Animator _animator;
    public BaseCommand _currentCommand;
    public PlayerState _currentState;
    public GameObject _enemyFound;
    //Caches
    private Transform t;
    private string[] _walkableTerrain;
    private RaycastHit rayHit;
    private GameObject[] _enemies;
    private bool Lock;
    private BaseCommand _cacheCommand;
    //Available Commands
    IdleCommand _idleCommand;
    MoveCommand _moveCommand;
    MoveAttackCommand _moveAttackCommand;
	// Use this for initialization
	void Start () {
        t = transform;
        _walkableTerrain = new string[] { "Terrain", "ActiveEnemy","HitBox" };
        _currentState = PlayerState.IDLE;

        _idleCommand = gameObject.AddComponent<IdleCommand>();
        _moveCommand = gameObject.AddComponent<MoveCommand>();
        _moveAttackCommand = gameObject.AddComponent<MoveAttackCommand>();
        _currentCommand = _idleCommand;

        Lock = false;
        FlushPreviousCommand();
	}
	
	// Update is called once per frame
	void Update () {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out rayHit))
            {
                GameObject hit = rayHit.transform.gameObject;

                foreach (string terrain in _walkableTerrain)
                {
                    if (hit.tag == terrain)
                    {
                        _enemyFound = GetEnemyNearVicinity(rayHit.point, 2f);

                        if (_enemyFound != null)
                        {
                            ChangeState(PlayerState.MOVE_ATTACK);
                        }
                        else
                        {
                            ChangeState(PlayerState.DASH);
                        }
                     
                    }
                }
            }
        }

        _currentCommand.Execute();
	}

    void FixedUpdate()
    {
        GetEnemies();
    }

    GameObject _storedEnemy;
    Vector3 _storedVector;
    void ChangeState(PlayerState state)
    {
        FlushPreviousCommand();

        BaseCommand command = _idleCommand;
       
        switch (state)
        {
            case PlayerState.DASH:
                command = _moveCommand;
                command.Init(_animator, rayHit.point, Unlock);
                LookAt(rayHit.point);
                break;
            case PlayerState.MOVE_ATTACK:
                command = _moveAttackCommand;
                if (!_currentCommand._isInteruptable)
                {
                    _storedEnemy = _enemyFound;
                }
                else
                {
                    command.Init(_animator, _enemyFound, Unlock);
                }
             
                LookAt(_enemyFound.transform.position);
                break;
        }

        if (!_currentCommand._isInteruptable)
        {
            _cacheCommand = command;
        }
        else
        {
            command.enabled = true;
            _currentCommand = command;
        }

      
    }

    private void Unlock()
    {
        if(_cacheCommand != null)
        {
            print("release");
            FlushPreviousCommand();
            _cacheCommand.enabled = true;

            if(_storedEnemy != null) //As a rule, if stored enemy is not null, means the cache command need to be init with a enemy
            {
                _cacheCommand.Init(_animator, _storedEnemy, Unlock);
                _storedEnemy = null;
            }

            _currentCommand = _cacheCommand;
            _cacheCommand = null;
        }
        else
        _currentCommand = _idleCommand;
    }

    void FlushPreviousCommand()
    {
        BaseCommand[] commands = this.GetComponentsInChildren<BaseCommand>();
        foreach(BaseCommand command in commands)
        {
            command.enabled = false;
        }
    }
    private void GetEnemies()
    {
        _enemies = GameObject.FindGameObjectsWithTag("ActiveEnemy");
    }

    private GameObject GetEnemyNearVicinity(Vector3 point,float radius)
    {
        if (_enemies == null || _enemies.Length == 0)
        {
            print("No enemies!");
            return null;
        }

        float smallest = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in _enemies)
        {
            float distance = Vector3.Distance(point, enemy.transform.position);

            if (distance < smallest)
            {
                smallest = distance;
                nearestEnemy = enemy;
            }
        }

        if (Vector3.Distance(nearestEnemy.transform.position, point) <= radius)
            return nearestEnemy;
        else
            return null;
    }

    private void LookAt(Vector3 worldPosition)
    {
        Vector3 targetPostition = new Vector3(worldPosition.x,
                                        t.position.y,
                                       worldPosition.z);
        t.LookAt(targetPostition);
    }
}
