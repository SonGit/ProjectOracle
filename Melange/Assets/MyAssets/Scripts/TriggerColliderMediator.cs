using UnityEngine;
using System.Collections;

public class TriggerColliderMediator : MonoBehaviour {

    public GameObject _receiver;
    public string _methodToCall;
    public string _validTag;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other) 
    {
        if (other.tag == _validTag)
        {
            //can send message to whomever or run our code here
            if (_receiver != null && _methodToCall != null)
                _receiver.SendMessage(_methodToCall, other.gameObject);
        }

    }
}
