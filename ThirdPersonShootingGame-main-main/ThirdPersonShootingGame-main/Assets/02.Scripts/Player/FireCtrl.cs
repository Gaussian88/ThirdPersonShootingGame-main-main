using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//1.�Ѿ� ������ 2. �߻���ġ 3. �ѼҸ� ������ҽ� ����� Ŭ��
[System.Serializable] // �Ʒ��� ����ü�� �ν����Ϳ� ���̰� �ϴ� ��Ʃ����Ʈ 
public struct PlayerSfx //public ���� �ȴ�. ����ü ���� ������ �Լ���
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
    private float fireRate = 0.1f; //�߻� ���� �ð� 
    private Player_Mecanim player;
    public bool isFire = false;
    private bool isReloding = false;
    public int enemyLayer;
    public int barrelLayer;
    public int layerMask;
    private float nextFire = 0f;
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
        enemyLayer = LayerMask.NameToLayer("ENEMY");
        barrelLayer = LayerMask.NameToLayer("BARREL");
        layerMask = 1<<enemyLayer | 1<<barrelLayer;
    }
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 20f, Color.black);

        if (EventSystem.current.IsPointerOverGameObject()) return;
        //���� UI�� �̺�Ʈ �޴� ���� �ִٸ� ���� ������.
        MouseBtnFire();
        RaycastHit hit;
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, 20f, layerMask))
            isFire = hit.collider.CompareTag("ENEMY");
        else
            isFire = false;
        if(!isReloding&& isFire)
        {
            if(Time.time > nextFire)
            {
               
                Fire();
              
                isReloding = (--remaingBullet % maxBullet) == 0;
                if(isReloding)
                {
                    StartCoroutine(Reloading());
                }
                nextFire = Time.time + fireRate;
            }

        }

    }

    private void MouseBtnFire()
    {
        if (Input.GetMouseButton(0) && Time.time - timePrev > fireRate)
        {
            if (!player.isRun && !isReloding)
            {
                --remaingBullet;
                Fire();
                if (remaingBullet == 0)
                {
                    StartCoroutine(Reloading());
                }
            }

            timePrev = Time.time;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            WeaponEffectDisable();
        }
    }

    private void WeaponEffectDisable()
    {
        muzzleFlash.Stop();
        CartridgeEjectEffect.Stop();
    }

    void Fire()
    {
        Invoke("WeaponEffectDisable", 0.1f);
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
