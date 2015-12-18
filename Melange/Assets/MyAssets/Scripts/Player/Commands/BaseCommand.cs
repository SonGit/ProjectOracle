using UnityEngine;
using System.Collections;
using Shared;

public abstract class BaseCommand : MonoBehaviour {
    public bool _isInteruptable = true;
	// Use this for initialization
    public virtual void Init(Animator animator, GameObject objectParam,Callback callback = null) {}
    public virtual void Init(Animator animator, Vector3 vectorParam, Callback callback = null) { }
    public virtual void Init(){}
    public abstract void Execute();
    public abstract void Reset();
}
