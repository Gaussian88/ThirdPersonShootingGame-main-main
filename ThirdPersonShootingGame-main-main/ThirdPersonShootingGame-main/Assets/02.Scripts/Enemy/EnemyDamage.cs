using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private GameObject BloodEffect;
    private readonly string playerbulletTag = "BULLET";
    private float hp = 0f;
    private float maxhp = 100f;
    public GameObject hpBar;
    public Image hpbarImg;
    public Vector3 hpbarOffset = new Vector3(0f, 2.2f, 0f);
    void OnEnable()
    {
        StartCoroutine(HpBarSetting());
    }
    IEnumerator HpBarSetting()
    {
        yield return null;
        hpBar = ObjPoolingManager.P_instance.GetEnemyHpbar();
        hpBar.gameObject.SetActive(true);
       
        hpbarImg = hpBar.GetComponentsInChildren<Image>()[1];
        hpbarImg.color = Color.green;
        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = this.gameObject.transform;
        _hpBar.offset = hpbarOffset;
       
    }

    void Start()
    {
        BloodEffect = Resources.Load<GameObject>("Effects/GoopSpray");
        hp = maxhp;
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(playerbulletTag))
        {
            col.gameObject.SetActive(false);
            hp -= col.gameObject.GetComponent<BulletCtrl>().damage;
            hpbarImg.fillAmount = hp / maxhp;
            if(hp <= 0f)
            {
                hpbarImg.GetComponentsInParent<Image>()[1].color = Color.clear;
                //GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
                GetComponent<EnemyAI>().Die();
            }

            ShowBloodEffect(col);

        }

    }

    private void ShowBloodEffect(Collision col)
    {
        Vector3 hitpos = col.contacts[0].point; //맞은 지점
        Vector3 _normal = col.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        GameObject blood = Instantiate<GameObject>(BloodEffect, hitpos, rot);
        Destroy(blood, 0.5f);
    }
}
