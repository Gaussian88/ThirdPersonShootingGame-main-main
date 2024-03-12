using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance =null;
    public bool isGameOver = false;
    [SerializeField] private GameObject enemyPrefab = null;
    [SerializeField] private GameObject swatPrefab = null;
    [SerializeField] private List<GameObject> enemyList = null;
    [SerializeField] private List<Transform>spawnList = new List<Transform>();
    [SerializeField] private Text KillText = null;
    [SerializeField] private CanvasGroup InventoryCG = null;
    public int killScore = 0;
    private WaitForSeconds ws;
    public bool isPause = false;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        KillText = GameObject.Find("Canvas-UI").transform.GetChild(5).GetComponent<Text>();
        InventoryCG = GameObject.Find("Canvas-UI").transform.GetChild(7).GetComponent<CanvasGroup>();
        enemyPrefab = Resources.Load("Enemies/Enemy") as GameObject;
        swatPrefab = Resources.Load<GameObject>("Enemies/swat");
        var spawnPoint = GameObject.Find("SpawnPoints").gameObject;
        if(spawnPoint != null )
        {
            spawnPoint.GetComponentsInChildren<Transform>(spawnList);
        }
        spawnList.RemoveAt(0);
       ws = new WaitForSeconds(Random.Range(3, 6));
        LoadGameData();
    }
    void Start()
    {
        KillScoreNumber(0);
        CreateEnemies();
        StartCoroutine(SpawnEnemies());
    }

    private void LoadGameData()
    {
        killScore = PlayerPrefs.GetInt("KILL_SCORE", 0);
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
            if(isGameOver) yield break;
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
    public  void OnPuase()
    {
        isPause = !isPause;
        Time.timeScale = (isPause) ? 0.0f : 1.0f;
        GameObject playerObj = GameObject.FindWithTag("Player") as GameObject;
        var scripts = playerObj.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = !isPause;
        }

        var canvasGroup = GameObject.Find("Panel-Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPause;

    }
    public void KillScoreNumber(int _score)
    {
        killScore += _score;
        KillText.text = $"KILL :  <color=#ff0000>{killScore.ToString("0000")}</color>";
        PlayerPrefs.SetInt("KILL_SCORE", killScore);
        //예약 혹은 등록된 키명과 저장할 값을 넣는다.
    }
    public void OnInventoryOpen(bool isOpened)
    {
        Time.timeScale = isOpened ? 0.0f : 1.0f;
        InventoryCG.alpha = isOpened ? 1.0f : 0.0f;
        InventoryCG.interactable = isOpened;
        InventoryCG.blocksRaycasts = isOpened;

    }

    void OnDisable()
    {
        PlayerPrefs.DeleteKey("KILL_SCORE");
       
    }
}
