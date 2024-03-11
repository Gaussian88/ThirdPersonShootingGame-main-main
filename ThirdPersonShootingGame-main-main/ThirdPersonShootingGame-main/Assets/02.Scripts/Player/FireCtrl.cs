using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//1.총알 프리팹 2. 발사위치 3. 총소리 오디오소스 오디오 클립
[System.Serializable] // 아래의 구조체가 인스펙터에 보이게 하는 애튜리뷰트 
public struct PlayerSfx //public 선언만 된다. 구조체 안의 변수나 함수는
{
  public  AudioClip[] fireClips;
  public  AudioClip[] reloadClips;
                      
}

public class FireCtrl : MonoBehaviour
{
    public enum WeaponType
    {
        RIFLE =0,
        SHOTGUN
    }
    public WeaponType curWeaponType = WeaponType.RIFLE;
    public PlayerSfx playerSfx;
   
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioSource source;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem CartridgeEjectEffect;
    [SerializeField] private Image magazineImg;
    [SerializeField] private Text magazineText;
    [SerializeField] private Animator animator;
    [SerializeField] private Sprite[] weaponIcons;
    [SerializeField] private Image weaponImg;
    private readonly int hashReload = Animator.StringToHash("ReloadTrigger");
    private readonly int hashFire = Animator.StringToHash("FireTrigger");
    private int remaingBullet = 0;
    private readonly int maxBullet = 10;
    private float timePrev;
    private float fireRate = 0.1f; //발사 간격 시간 
    private Player_Mecanim player;
    private bool isReloding = false;
    void Start()
    {
        animator = GetComponent<Animator>();
       
        firePos = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Transform>();
        player = GetComponent<Player_Mecanim>();
        source = GetComponent<AudioSource>();
        magazineImg = GameObject.Find("Panel-Magazine").transform.GetChild(2).GetComponent<Image>();
        magazineText = GameObject.Find("Panel-Magazine").transform.GetChild(0).GetComponent<Text>();
        muzzleFlash = firePos.GetChild(0).GetComponent<ParticleSystem>();
        CartridgeEjectEffect = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<ParticleSystem>();
        weaponImg = GameObject.Find("Image-weapon").GetComponent<Image>();
        for (int i = 0;  i< 2; i++)
        {
            playerSfx.fireClips = Resources.LoadAll<AudioClip>("Sounds/Fires");
        }
        for (int i = 0; i < 2; i++)
        {
            playerSfx.reloadClips = Resources.LoadAll<AudioClip>("Sounds/Reloads");
        }

        muzzleFlash.Stop();
        remaingBullet = maxBullet;
        remaingBullet = Mathf.Clamp(remaingBullet, 0, 10);
    }
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
              //현재 UI에 이벤트 받는 것이 있다면 빠져 나간다.

        if(Input.GetMouseButton(0)&&Time.time - timePrev >fireRate)
        {
             if(!player.isRun &&!isReloding)
             {
                --remaingBullet;
                 Fire();
                if(remaingBullet ==0)
                {
                    StartCoroutine(Reloading());
                }
             }
                 
            timePrev = Time.time;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            muzzleFlash.Stop();
            CartridgeEjectEffect.Stop();
        }
    }
    void Fire()
    {
       
          var _bullet = ObjPoolingManager.P_instance.GetPlayerBullet();
          _bullet.transform.position = firePos.position;
         _bullet.transform.rotation = firePos.rotation;
         _bullet.SetActive(true);

        source.PlayOneShot(playerSfx.fireClips[(int)curWeaponType], 1.0f);
        animator.SetTrigger(hashFire);
        muzzleFlash.Play() ;
        CartridgeEjectEffect.Play();
        magazineImg.fillAmount = (float)remaingBullet / (float)maxBullet;
        
        magazineTextShow();
    }
    void magazineTextShow()
    {
        magazineText.text = "<color=#ff0000>" + remaingBullet.ToString() + "</color>" + "/" + maxBullet.ToString();
    }
    IEnumerator Reloading()
    {
        magazineTextShow();
        isReloding = true;
        animator.SetTrigger(hashReload);
        muzzleFlash.Stop();
        CartridgeEjectEffect.Stop();
        source.PlayOneShot(playerSfx.reloadClips[(int)curWeaponType], 1.0f);
        yield return new WaitForSeconds(1.5f);

        magazineImg.fillAmount = 1.0f;
        isReloding = false;
        remaingBullet = maxBullet;
        magazineTextShow();

    }
    public void OnChangeWeapon()
    {
        curWeaponType = (WeaponType)((int)++curWeaponType % 2);
        weaponImg.sprite = weaponIcons[(int)curWeaponType];
    }
}
