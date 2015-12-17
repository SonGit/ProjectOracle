using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    #region Singleton
    static GameController __sharedInstance;
    public static GameController Instance
    {
        get
        {
            return __sharedInstance;
        }
    }
    #endregion

    public Transform[] _totalLevelSpawnPoint;
    public Transform[] _totalLevelStartPoint;
    public Transform[] _totalLevelCameraPoint;
    public GameObject[] _Blockades;
    public GameObject _enemyPrefab;
    public int _currentLevel;
    public Player _player;
    public CameraMover _camera;

    private int _maxLevel;
    private List<GameObject> _currentEnemyList;
    private int _totalEnemy;

    void Awake()
    {
        __sharedInstance = this;
    }
	// Use this for initialization
	void Start () {
        _maxLevel = _totalLevelSpawnPoint.Length;
        _currentLevel = 0;
        _currentEnemyList = new List<GameObject>();

        StartLevel();
	}
	
	public void StartLevel()
    {
        if(_currentLevel >= _maxLevel)
        {
            return;
        }

        Transform[] spawnPoints =  _totalLevelSpawnPoint[_currentLevel].GetComponentsInChildren<Transform>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemy = (GameObject)Instantiate(_enemyPrefab, spawnPoint.position, _enemyPrefab.transform.rotation);
            _currentEnemyList.Add(enemy);
        }

        _totalEnemy = _currentEnemyList.Count;

        BlockAllEntrances();
    }

    public void OnEnemyKilled()
    {
        _totalEnemy--;
        if(_totalEnemy <= 0)
        {
            StartCoroutine(TransitionToAnotherLevel());
        }
    }

    IEnumerator TransitionToAnotherLevel()
    {
        OpenAllEntrances();
        yield return new WaitForSeconds(2);
        _currentLevel++;
        _player.GoToAnotherLevel(_totalLevelStartPoint[_currentLevel]);
        _camera.TransitioningLevel(_player.gameObject,_totalLevelCameraPoint[_currentLevel].position);
    }

    //So nememies cannot pathfinding to other areas
    private void BlockAllEntrances()
    {
        foreach(GameObject blockade in _Blockades)
        {
            blockade.SetActive(true);
        }
    }

    private void OpenAllEntrances()
    {
        foreach (GameObject blockade in _Blockades)
        {
            blockade.SetActive(false);
        }
    }
}
