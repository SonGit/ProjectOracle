using UnityEngine;
using System.Collections;
using Shared;

public class MoveToAnotherLevelCommand : BaseCommand
{
    private Animator _animator;
    private Callback _callback;
    private Vector3 _destination;
    private Transform t;
    private NavMeshPath path;
    private int currentWaypoint = 0;
    private NavMeshAgent agent;
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
    public override void Init(Animator animator, Vector3 destination, Callback callback = null)
    {
        _destination = destination;
        _animator = animator;
        t = transform;

        if (callback != null)
            _callback = callback;

        _isRunning = true;
        _isInteruptable = true;

        if (agent == null)
        {
            agent = this.GetComponent<NavMeshAgent>();
        }

        agent.SetDestination(destination);
    }

    public override void Execute()
    {
        if (!_isRunning) return;

        if(Vector3.Distance(t.position,_destination) < 1)
        {
            _isRunning = false;
            agent.Stop();
            _callback();
        }
    }

    public override void Reset()
    {
        _isRunning = false;
    }

}
