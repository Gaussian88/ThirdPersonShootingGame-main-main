using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [SerializeField] private Transform tr;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TrailRenderer trailRenderer;
    public float speed = 1500f;
    public float damage;
    void Awake()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
       
        //Destroy(gameObject, 3.0f);
    }
    void OnEnable()
    {
        damage = GameManager.Instance.gameData.damage;
        rb.AddForce(tr.forward * speed);
        GameManager.OnItemChange += UpdateSetup;
    }
    void UpdateSetup()
    {
        damage = GameManager.Instance.gameData.damage;
    }
    void OnDisable()
    {
        trailRenderer.Clear();
        rb.Sleep();
        tr.position = Vector3.zero;

    }

}
