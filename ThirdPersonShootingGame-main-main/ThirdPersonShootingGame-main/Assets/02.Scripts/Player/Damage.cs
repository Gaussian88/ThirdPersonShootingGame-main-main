using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    [SerializeField] private GameObject BloodEffect;
    private readonly string e_bulletTag = "E_BULLET";
    [SerializeField] private Image hpBar;
    void Start()
    {
        hpBar = GameObject.Find("Panel-HP").transform.GetChild(1).GetComponent<Image>();
        BloodEffect = Resources.Load<GameObject>("Effects/GoopSpray");
        hpBar.color = Color.green;
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(e_bulletTag))
        {
            col.gameObject.SetActive(false);

            Vector3 hitpos = col.contacts[0].point; //맞은 지점
            Vector3 _normal = col.contacts[0].normal;
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
            GameObject blood = Instantiate<GameObject>(BloodEffect, hitpos, rot);
            Destroy(blood, 0.5f);
        }    

    }

}
