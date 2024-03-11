using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance =null;
    public bool isGameOver = false;
    [SerializeField] private GameObject enemyPrefab = null;
    [SerializeField] private GameObject swatPrefab = null;
    [SerializeField] private List<GameObject> enemyList = null;
    [SerializeField] private List<Transform>spawnList = new List<Transform>();
    private WaitForSeconds ws;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        enemyPrefab = Resources.Load("Enemies/Enemy") as GameObject;
        swatPrefab = Resources.Load<GameObject>("Enemies/swat");
        var spawnPoint = GameObject.Find("SpawnPoints").gameObject;
        if(spawnPoint != null )
        {
            spawnPoint.GetComponentsInChildren<Transform>(spawnList);
        }
        spawnList.RemoveAt(0);
       ws = new WaitForSeconds(Random.Range(3, 6));
    }
    void Start()
    {
        CreateEnemies();
        StartCoroutine(SpawnEnemies());
    }
    void CreateEnemies()
    {
        GameObject EnemyGroup = new GameObject("EnemyGroup");
        for(int i=0; i< 5; i++)
        {
            var _enemy = Instantiate(enemyPrefab,EnemyGroup.transform);
            var _swat = Instantiate(swatPrefab,EnemyGroup.transform);
            _enemy.name = "enemy " + (i+1).ToString();
            _swat.name = "swat " +(i+1).ToString(); 
            _enemy.gameObject.SetActive(false);
            _swat.gameObject.SetActive(false);
            enemyList.Add(_enemy);
            enemyList.Add(_swat);
        }
    }
    IEnumerator SpawnEnemies()
    {
       
        while (!isGameOver)
        {
            yield return ws;

            foreach (var enemy in enemyList)
            {
                if (enemy.activeSelf == false)
                {
                    int idx = Random.Range(0, spawnList.Count);
                    enemy.transform.position = spawnList[idx].position;
                    enemy.transform.rotation = spawnList[idx].rotation;
                    enemy.gameObject.SetActive(true);
                    break;
                }
                else
                    yield return null;

               
            }
        }

    }

}
