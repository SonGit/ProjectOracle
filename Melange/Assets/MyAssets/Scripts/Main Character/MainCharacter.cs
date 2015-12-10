using UnityEngine;
using System.Collections;

public partial class MainCharacter : MonoBehaviour
{
    public string[] _walkableTerrain;
    public Animator _animator;
    public float _attackRange;

    public ParticleSystem _dustParticle;

    Vector3 destination;
    GameObject[] _enemies;
    Transform t;

    private NavMeshPath path;
    private float elapsed = 0.0f;
    private bool _pendingDash;
    private Vector3 _cacheDestination;
    // Use this for initialization
    void Start()
    {
        t = transform;
        path = new NavMeshPath();
        elapsed = 0.0f;
    }
    float distanceFromPlayerToNearestEnemy = Mathf.Infinity;
    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       // Debug.DrawRay(ray.origin, ray.direction, Color.green);
        RaycastHit rayHit;

        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(ray, out rayHit))
            {
//                Debug.DrawRay(ray.origin, ray.direction, Color.red);
//                Debug.Log("Hook was hit  " + rayHit.transform.gameObject.name);

                GameObject hit = rayHit.transform.gameObject;

                foreach (string terrain in _walkableTerrain)
                {
                    if (hit.tag == terrain)
                    {
                        if (_pendingDash)
                        {
                            _cacheDestination = rayHit.point;
                            print(_cacheDestination);
                        }
                        else
                        {
                            destination = rayHit.point;
                        }
                      
                    }
                }

            }

          
            if (destination != null)
            {

                _enemyFound = GetEnemyNearVicinity(destination);
    

                if(_enemyFound != null)
                {
                    LookAt(_enemyFound.transform.position);

                    Dash(destination);

                    float distanceFromNearestEnemy = Vector3.Distance(destination, _enemyFound.transform.position);

                    if (distanceFromNearestEnemy < _attackRange)
                    {
                        OnAttack(_enemyFound);
                        _pendingDash = true;
                    }

                    LookAt(_enemyFound.transform.position);
                }
               
            }
        }

        if(_isRunning)
        {
            RunUpdateComponent(destination);
        }

        if (_enemyFound != null && !_isRunning)
        {
            distanceFromPlayerToNearestEnemy = Vector3.Distance(t.position, _enemyFound.transform.position);
            
          //   LookAt(_enemyFound.transform.position);

            if (distanceFromPlayerToNearestEnemy < _attackRange)
            {
                OnAttack(_enemyFound);
            }
        }

    }

    public GameObject _enemyFound;
    void FixedUpdate()
    {
        GetEnemies();
        _enemyFound = GetNearestEnemy();

    }

    private void LookAt(Vector3 worldPosition)
    {
        Vector3 targetPostition = new Vector3(worldPosition.x,
                                        t.position.y,
                                       worldPosition.z);
        t.LookAt(targetPostition);
    }

    private void GetEnemies()
    {
        _enemies = GameObject.FindGameObjectsWithTag("ActiveEnemy");
    }

    private GameObject GetNearestEnemy()
    {
        if (_enemies == null || _enemies.Length == 0)
        {
          //  print("No enemies!");
            return null;
        }

        float smallest = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in _enemies)
        {
            float distance = Vector3.Distance(t.position, enemy.transform.position);

            if (distance < smallest)
            {
                smallest = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private GameObject GetEnemyNearVicinity(Vector3 point)
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

        return nearestEnemy;
    }

}
