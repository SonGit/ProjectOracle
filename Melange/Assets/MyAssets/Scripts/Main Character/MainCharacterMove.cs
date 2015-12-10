using UnityEngine;
using System.Collections;

public partial class MainCharacter
{
    //The point to move to
    public NavMeshAgent _agent;
    private int currentWaypoint = 0;
    private bool needPathfinding; // if there is no need for pathfinding, just dash to position to save calculation time

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

    private bool Dash(Vector3 To)
    {
        float distance = Vector3.Distance(t.position, To);

        Vector3 direction = (To - t.position).normalized;

        _dustParticle.transform.rotation = Quaternion.LookRotation(direction);
        _dustParticle.transform.position = To;

        _dustParticle.Play();

        if (distance <= 1)
        {
            return false;
        }

        needPathfinding = GetPath(To);
        _isRunning = true;

        return true;
    }

    private void RunUpdateComponent(Vector3 destination)
    {
        if (!needPathfinding)
        {
            t.position = Vector3.Lerp(t.position, destination, 20 * Time.deltaTime);
            if (Vector3.Distance(t.position, destination) < 1f)
            {
                if(_pendingDash &  _cacheDestination!= Vector3.zero)
                {
                    destination = _cacheDestination;
                    Dash(destination);
                }
                else
                {
                    _isRunning = false;
                }
               
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
                if (_pendingDash && _cacheDestination != Vector3.zero)
                {
                    destination = _cacheDestination;
                    Dash(destination);
                }
                else
                {
                    _isRunning = false;
                }
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

        if (Physics.Linecast(t.position, destination))
        {
            currentWaypoint = 0;
            NavMesh.CalculatePath(transform.position, destination, -1, path);
          //  print("true");
            return true;
        }
        else
        {
         //   print("false");
            return false;
        }
    }



}
