using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataInfo;
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
    private DataManager dataManager = null;
    //public GameData gameData;
    public GameDataObject gameData;
    //�κ��丮�� �������� ���� ���� �� �߻� ��ų �̺�Ʈ ���� 
    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;

    [SerializeField] private GameObject slotList;
    [SerializeField] private GameObject[] itemObjects;
    [SerializeField] private List<Transform>slotsList= new List<Transform>();
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        KillText = GameObject.Find("Canvas-UI").transform.GetChild(5).GetComponent<Text>();
        InventoryCG = GameObject.Find("Canvas-UI").transform.GetChild(7).GetComponent<CanvasGroup>();
       
        dataManager = GetComponent<DataManager>();
        dataManager.Initialized();

        enemyPrefab = Resources.Load("Enemies/Enemy") as GameObject;
        swatPrefab = Resources.Load<GameObject>("Enemies/swat");
        var spawnPoint = GameObject.Find("SpawnPoints").gameObject;
        if(spawnPoint != null )
        {
            spawnPoint.GetComponentsInChildren<Transform>(spawnList);
        }
        spawnList.RemoveAt(0);
       ws = new WaitForSeconds(Random.Range(3, 6));
       StartCoroutine(LoadGameData());
    }
    void Start()
    {
        slotList = GameObject.Find("Image-SlotList").gameObject;
        if(slotList != null )
        {
            slotList.GetComponentsInChildren<Transform>(slotsList);
        }
        slotsList.RemoveAt(0);
        OnInventoryOpen(false);
        KillScoreNumber(0);
       StartCoroutine(CreateEnemies());
       StartCoroutine(SpawnEnemies());
    }

    IEnumerator LoadGameData()
    {
        yield return null;
        // killScore = PlayerPrefs.GetInt("KILL_SCORE", 0);
        //GameData data = dataManager.Load();
        //gameData.hp = data.hp;
        //gameData.damage = data.damage;
        //gameData.speed = data.speed;
        //gameData.killCount = data.killCount;
        //gameData.equipItem = data.equipItem;
        //�������� �ϳ��̻� �Ծ��ٸ�
        if(gameData.equipItem.Count >0)
        {
           StartCoroutine( InventorySetup());
        }
    }
    void SaveGame()
    {
        //dataManager.Save(gameData);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
        //���α׷� ��� �߿� Dirty ǥ���� �صд�.
        //.asset ���Ͽ� ������ ����
    }
    IEnumerator InventorySetup()
    {
        yield return null;
        
        for(int i =0; i< gameData.equipItem.Count; i++)
        {
            for(int j =1;j<=slotsList.Count;j++)
            {   //Slot������ �ٸ� ������ �ִٸ� ���� �ε����� �Ѿ 
                if (slotsList[j].childCount > 0) continue;

                int itemIdx = (int)gameData.equipItem[i].itemType;
                itemObjects[itemIdx].GetComponent<Transform>().SetParent(slotsList[j]);
                itemObjects[itemIdx].GetComponent<ItemInfo>().itemData = gameData.equipItem[i];
                break;

            }
        }
    }
    public void AddItem(Item item)
    {      // ���� �����ۿ� ���� �������� ������ ��������
        if (gameData.equipItem.Contains(item)) return;
       
         //���� �������� ������ Add()�ִ´�
        gameData.equipItem.Add(item);
        //�������� ������ ���� �б� ó�� 
        switch(item.itemType)
        {
            case Item.ItemType.HP:
                if (item.itemCalc == Item.ItemCalc.VALUE) //������ ��� ����� �����¶��
                    gameData.hp += item.value;
                else
                    gameData.hp += gameData.hp * item.value;

                break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed += item.value;
                else
                    gameData.speed += gameData.speed * item.value;

                break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage += item.value;
                else
                    gameData.damage += gameData.damage * item.value;

                break;
            case Item.ItemType.GRENADE:


                break;

        }

        OnItemChange();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
    }
    public void RemoveItem(Item item)
    {
        gameData.equipItem.Remove(item); //������ ���� 
        switch(item.itemType)
        {
            case Item.ItemType.HP:
              if(item.itemCalc == Item.ItemCalc.VALUE)
                   gameData.hp -= item.value;
              else
                  gameData.hp = gameData.hp/(1.0f +item.value);

                break;
            case Item.ItemType.SPEED:
                if(item.itemCalc== Item.ItemCalc.VALUE)
                    gameData.speed -= item.value;
                else
                    gameData.speed = gameData.speed/(1.0f +item.value);

                break;
            case Item.ItemType.DAMAGE:

                if(item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage -= item.value;
                else
                    gameData.damage = gameData.damage/(1.0f +item.value);

                break;
            case Item.ItemType.GRENADE:

                break;

        }


        OnItemChange();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(gameData);
#endif
    }
    IEnumerator CreateEnemies()
    {
        yield return new WaitForSeconds(0.3f);
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
        gameData.killCount += _score;
        KillText.text = $"KILL :  <color=#ff0000>{gameData.killCount.ToString("0000")}</color>";

        //PlayerPrefs.SetInt("KILL_SCORE", killScore);
        //���� Ȥ�� ��ϵ� Ű��� ������ ���� �ִ´�.
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
        //PlayerPrefs.DeleteKey("KILL_SCORE");
       
    }
    void OnApplicationQuit()
    {
        SaveGame();
    }
}
