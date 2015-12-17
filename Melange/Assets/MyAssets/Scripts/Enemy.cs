using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public bool _isDead;
    public ParticleSystem particle;
    public ParagonAI.HealthScript myScript;
    public VoiceModule _voice;
	// Use this for initialization

    string[] _attackPhrases = new string[] { "I...Am..Sorry!",
                                              "Kill..Me!",
                                              "Destroy...desssstrrroyyyyy...",
                                               "Arrgghhhh!",
                                                "Get this thing off me!",
                                                "Run..run..",
                                                "Get out of my head!"};

    string[] _deathPhrases = new string[] { "Thank...you",
                                              "finally...",
                                              "peace...",
                                               "no...more",
                                                "good...luck.."};
	void Start () {
        _isDead = false;
        StartCoroutine(Loop_Speak());
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
        int rand = Random.Range(0, _deathPhrases.Length);
        _voice.Speak(_deathPhrases[rand]);
        yield return new WaitForSeconds(0.25f);
        myScript.Damage(100);
        GameController.Instance.OnEnemyKilled();
        yield return null;
    }

    IEnumerator Loop_Speak()
    {
        while(!_isDead)
        {
            yield return new WaitForSeconds(Random.Range(9, 12));
            int rand = Random.Range(0,_attackPhrases.Length);
            _voice.Speak(_attackPhrases[rand]);
        }
    }

}
