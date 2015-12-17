using UnityEngine;
using System.Collections;
using Shared;

public class MoveCommand : BaseCommand
{
    private const PlayerState _state = PlayerState.DASH;
    private int currentWaypoint = 0;
    private bool needPathfinding; // if there is no need for pathfinding, just dash to position to save calculation time
    private Transform t;
    private NavMeshPath path;
    private Animator _animator;
    private Vector3 _destination;

    private Callback _callback;
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

	// Use this for initialization
	void Start () {

	}
    public override void Init(Animator animator, Vector3 destination,Callback callback = null)
    {
        _destination = destination;
        _animator = animator;
        _isInteruptable = true;
        t = transform;

        if(callback != null)
        _callback = callback;

        _isRunning = true;
        needPathfinding = GetPath(_destination);
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
            t.position = Vector3.Lerp(t.position, _destination, 20 * Time.deltaTime);
            if (Vector3.Distance(t.position, _destination) < 1f)
            {
                 _isRunning = false;
                 if (_callback != null)
                 _callback();
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
               _isRunning = false;
               if (_callback != null)
               _callback();
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



     private bool GetPath(Vector3 destination)
     {
         path = new NavMeshPath();

         if (Physics.Linecast(t.position, destination))
         {
             currentWaypoint = 0;
             NavMesh.CalculatePath(t.position, destination, -1, path);
             print("On Path");
             return true;
         }
         else
         {
             print("No Path");
             return false;
         }
     }

}
