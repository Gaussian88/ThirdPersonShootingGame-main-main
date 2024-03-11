using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPoolingManager : MonoBehaviour
{
    //싱글톤 기법 : 각종 매니저 클래스에 쉽게 접근 하기 위해 전(지)역변수 선연
    public static ObjPoolingManager P_instance =null;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject E_bullet;
    [SerializeField] private List<GameObject> bulletList = new List<GameObject>();
    [SerializeField] private List<GameObject> E_bulletList = new List<GameObject>();
    private int maxbullet = 10;
    private int E_maxbullet = 20;
    void Awake() //비활성화 되어도 호출  제일 빠르게 호출 Awake OnEnable Start()
    {
        if (P_instance == null)  //무분별한 객체생성을 막고 딱 한번만 생성 시킴
            P_instance = this;
        else if (P_instance != this)
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
        bulletPrefab = Resources.Load("Weapons/Bullet") as GameObject;
        E_bullet = (GameObject)Resources.Load("Weapons/E_Bullet");
        CreatePlayerBullet();
        CreateE_Bullet();
    }
    void CreatePlayerBullet()
    {
        GameObject playerbulletGroup = new GameObject("PlayerBulletGroup");
        for(int i = 0; i< maxbullet; i++)
        {
            GameObject _bullet = Instantiate(bulletPrefab,playerbulletGroup.transform);
            _bullet.name = (i+1).ToString()+"발";
            _bullet.gameObject.SetActive(false);
            bulletList.Add(_bullet);
        }
    }
    void CreateE_Bullet()
    {
        GameObject EnemybulletGroup = new GameObject("EnemyBulletGroup");
        for (int i = 0; i < E_maxbullet; i++)
        {
            GameObject _bullet = Instantiate(E_bullet, EnemybulletGroup.transform);
            _bullet.name = (i + 1).ToString() + "발";
            _bullet.gameObject.SetActive(false);
            E_bulletList.Add(_bullet);
        }
    }
    public GameObject GetPlayerBullet()
    {
        foreach (GameObject _bullet in bulletList)
        {
            if(_bullet.activeSelf ==false)
                return _bullet;

        }
        return null;
    }
    public GameObject GetE_Bullet()
    {
        foreach (GameObject e_bullet in E_bulletList)
        {
            if (e_bullet.activeSelf==false)
                return e_bullet;

        }
        return null;
    }

}
