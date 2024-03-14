using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    [SerializeField] private GameObject BloodEffect;
    private readonly string e_bulletTag = "E_BULLET";
    [SerializeField] private Image hpBar;
    [SerializeField] private Animator animator;
    public int hp;
    public int hpMax = 120;
    private readonly int hashDie = Animator.StringToHash("DIeTrigger");
    //대리자 반환형이 void이고 매개변수가 없는 함수를 대신 하겠다.
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie; //이벤트명을 전역 으로선언

    void OnEnable()
    {
        //이벤트 연결
        GameManager.OnItemChange += UpdateSetup;
    }
    void UpdateSetup()
    {
        hpMax = (int)GameManager.Instance.gameData.hp;
        hp += (int)GameManager.Instance.gameData.hp - hp;
    }
    void Start()
    {
        hpMax = (int)GameManager.Instance.gameData.hp;
        hp = hpMax;
        animator = GetComponent<Animator>();
        hpBar = GameObject.Find("Panel-HP").transform.GetChild(1).GetComponent<Image>();
        BloodEffect = Resources.Load<GameObject>("Effects/GoopSpray");
        hpBar.color = Color.green;
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(e_bulletTag))
        {
            PlayerDamage(col);
            hp -= 5;
            hp = Mathf.Clamp(hp, 0, hpMax);
            hpBar.fillAmount = (float)hp / (float)hpMax;

            HpBarColor();
            if (hp <= 0)
                PlayerDie();
        }

    }

    private void HpBarColor()
    {
        if (hpBar.fillAmount <= 0.3f)
            hpBar.color = Color.red;
        else if (hpBar.fillAmount <= 0.5f)
            hpBar.color = Color.yellow;
    }

    void PlayerDie()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        animator.SetTrigger(hashDie);
        #region 이벤트를 쓰지 않고 하는 방법
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");
        //for(int i =0; i < enemies.Length; i++)
        //{
        //    enemies[i].gameObject.SendMessage("OnPlayerDie",SendMessageOptions.DontRequireReceiver);
        //                                      //함수 오타이거나 함수가 없는 경우 오류를 발생 시키지 않는다.
        //}
        #endregion
        OnPlayerDie();
        GameManager.Instance.isGameOver = true;

    }
    private void PlayerDamage(Collision col)
    {
        col.gameObject.SetActive(false);
        Vector3 hitpos = col.contacts[0].point; //맞은 지점
        Vector3 _normal = col.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        GameObject blood = Instantiate<GameObject>(BloodEffect, hitpos, rot);
        Destroy(blood, 0.5f);
    }
    void OnDisable()
    {
        GameManager.OnItemChange -= UpdateSetup; //이벤트 해제 
    }
}
