using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwatDamage : MonoBehaviour
{
    [SerializeField] private GameObject BloodEffect;
    public float hp = 100f;
    public float hpMax = 100f;
    private  string bulletTag ="BULLET";
    public GameObject hpBar;
    public Image hpBarImg;
    public Vector3 hpBarOffset = new Vector3(0f, 2.2f, 0f);
    void OnEnable()
    {
       StartCoroutine(enemyHpbarSetting());
    }
    IEnumerator enemyHpbarSetting()
    {
        yield return new WaitForSeconds(0.3f);
        hpBar = ObjPoolingManager.P_instance.GetEnemyHpbar();
        hpBar.gameObject.SetActive(true);
        hpBarImg = hpBar.GetComponentsInChildren<Image>()[1];
        hpBarImg.color = Color.green;
        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffset;
    }
    void Start()
    {

       BloodEffect = Resources.Load<GameObject>("Effects/GoopSpray");

    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(bulletTag))
        {
            col.gameObject.SetActive(false);
            hp -= col.gameObject.GetComponent<BulletCtrl>().damage;
            hpBarImg.fillAmount  = hp / hpMax;
            ContactPoint cp = col.GetContact(0);
            Vector3 _normal = col.GetContact(0).normal;
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
            var blood = Instantiate(BloodEffect, cp.point, rot);
            Destroy(blood, 0.5f);
            if(hp<=0f)
            {
                hpBarImg.GetComponentsInParent<Image>()[1].color = Color.clear;
                GetComponent<SwatAI>().Die();
            }
        }
        
    }

}
