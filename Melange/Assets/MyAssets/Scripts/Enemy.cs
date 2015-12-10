using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public bool _isDead;
    public ParticleSystem particle;
    public ParagonAI.HealthScript myScript;

	// Use this for initialization
	void Start () {
        _isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MarkedForDeath()
    {
        transform.tag = "DeadEnemy";
    }

    public void Hit()
    {
        _isDead = true;
        particle.Play();
        StartCoroutine(Damage());
       // Damage();
    }

    public void Stop()
    {
        NavMeshAgent agent = this.GetComponent<NavMeshAgent>();
        agent.Stop();
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(0.25f);
        myScript.Damage(100);
        yield return null;
    }

}
