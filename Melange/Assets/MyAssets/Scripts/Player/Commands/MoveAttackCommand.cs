using UnityEngine;
using System.Collections;
using Shared;

public class MoveAttackCommand : BaseCommand
{
    private const PlayerState _state = PlayerState.MOVE_ATTACK;
    private bool isAttacking;
    public bool _isAttacking
    {
        get
        {
            return isAttacking;
        }

        set
        {
            isAttacking = value;
            _animator.SetBool("isAttacking", value);
            _animator.SetInteger("AttackAnimType", Random.Range(0, 2));
        }

    }

    bool isRunning = false;
    public bool _isRunning
    {
        get
        {
            return isRunning;
        }

        set
        {
            isRunning = value;
            _animator.SetBool("isRunning", value);
        }

    }

    private GameObject _enemy;

    private Animator _animator;
    private NavMeshPath path;
    private int currentWaypoint = 0;
    private Transform t;
    private bool needPathfinding = false; // if there is no need for pathfinding, just dash to position to save calculation time
    private Callback _callback;
    public float _attackLength = 0.75f;
	// Use this for initialization
    public override void Init(Animator animator, GameObject objectParam,Callback callback )
    {
        _isInteruptable = false;
        _animator = animator;
        _enemy = objectParam;
        _callback = callback;
        t = transform;

        needPathfinding = GetPath(_enemy.transform.position);

        _isAttacking = true;
        _isRunning = true;
    }
    public PlayerState GetState()
    {
        return _state;
    }
    public override void Execute()
    {
        if (!_isRunning) return;

        if (!needPathfinding)
        {
            t.position = Vector3.Lerp(t.position, _enemy.transform.position, 20 * Time.deltaTime);
            if (Vector3.Distance(t.position, _enemy.transform.position) < 1.5f)
            {
                Attack();
                return;
            }
        }
        else
        {
            if (path == null)
            {
                //We have no path to move after yet
                return;
            }

            if (currentWaypoint >= path.corners.Length)
            {
                Attack();
                return;
            }

            t.position = Vector3.Lerp(t.position, path.corners[currentWaypoint], 20 * Time.deltaTime);

            //Check if we are close enough to the next waypoint
            //If we are, proceed to follow the next waypoint
            if (Vector3.Distance(t.position, path.corners[currentWaypoint]) < 1.5f)
            {
                currentWaypoint++;
                return;
            }
        }
    }

    private void Attack()
    {
        _isRunning = false;
        StartCoroutine(WaitForEndOfAttackAnimation(_enemy));
    }

    IEnumerator WaitForEndOfAttackAnimation(GameObject go)
    {
          Enemy enemy = go.GetComponent<Enemy>();
          Vector3 enemyPosition = enemy.transform.position;

          if (enemy != null)
          {
              enemy.MarkedForDeath();
              enemy.Stop();
          }
           yield return new WaitForSeconds(_attackLength / 2);

           if (enemy != null)
           {
               enemy.Hit();
           }

           yield return new WaitForSeconds(_attackLength / 2);

           _isAttacking = false;
       
           if (_callback != null)
           _callback();

    }

    private bool GetPath(Vector3 destination)
    {
        path = new NavMeshPath();

        if (Physics.Linecast(t.position, destination))
        {
            currentWaypoint = 0;
            NavMesh.CalculatePath(transform.position, destination, -1, path);
            return true;
        }
        else
        {
            return false;
        }
    }
    }

