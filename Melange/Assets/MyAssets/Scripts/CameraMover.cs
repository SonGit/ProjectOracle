using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {
    public float smoothTime = 0.3F;
    public Vector3 velocity;

    private GameObject _objectToFollow;
    private Vector3 _destination;
    private bool _startRolling;
    private bool _startFollowing;
	// Use this for initialization
    void Start()
    {

    }
	public void TransitioningLevel(GameObject objectToFollow,Vector3 destination)
    {
        Vector3 newPos = new Vector3(0, 0, destination.z);
        _destination = newPos;
        _objectToFollow = objectToFollow;
        _startFollowing = true;
    }
	
	// Update is called once per frame
	void Update () {
	    if(_startRolling)
        {
            if(Vector3.Distance(transform.position,_destination) < 1)
            {
                _startRolling = false;
                GameController.Instance.StartLevel();
            }

            transform.position = Vector3.SmoothDamp(transform.position, _destination, ref velocity, smoothTime);
        }

        if(_startFollowing)
        {
            //If near a camera point, stop following player and roll to that point
            if (Vector3.Distance(transform.position, _destination) < 12)
            {
                _startFollowing = false;
                _startRolling = true;
            }
            //print(Vector3.Distance(transform.position, _destination));
            Vector3 newValue = new Vector3(0, 0, _objectToFollow.transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, newValue, ref velocity, smoothTime);
        }
	}
}
