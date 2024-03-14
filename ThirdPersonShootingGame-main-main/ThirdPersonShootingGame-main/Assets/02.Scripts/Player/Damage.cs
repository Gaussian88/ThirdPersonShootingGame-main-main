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
    //�븮�� ��ȯ���� void�̰� �Ű������� ���� �Լ��� ��� �ϰڴ�.
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie; //�̺�Ʈ���� ���� ���μ���

    void OnEnable()
    {
        //�̺�Ʈ ����
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
        #region �̺�Ʈ�� ���� �ʰ� �ϴ� ���
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");
        //for(int i =0; i < enemies.Length; i++)
        //{
        //    enemies[i].gameObject.SendMessage("OnPlayerDie",SendMessageOptions.DontRequireReceiver);
        //                                      //�Լ� ��Ÿ�̰ų� �Լ��� ���� ��� ������ �߻� ��Ű�� �ʴ´�.
        //}
        #endregion
        OnPlayerDie();
        GameManager.Instance.isGameOver = true;

    }
    private void PlayerDamage(Collision col)
    {
        col.gameObject.SetActive(false);
        Vector3 hitpos = col.contacts[0].point; //���� ����
        Vector3 _normal = col.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        GameObject blood = Instantiate<GameObject>(BloodEffect, hitpos, rot);
        Destroy(blood, 0.5f);
    }
    void OnDisable()
    {
        GameManager.OnItemChange -= UpdateSetup; //�̺�Ʈ ���� 
    }
}
