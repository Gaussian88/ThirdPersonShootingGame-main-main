using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip explosionClip;
    [SerializeField] GameObject explosionSpark;
    [SerializeField] private Texture[] textures;
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Mesh[] meshes;
    [SerializeField] private MeshFilter meshFilter;
    public int hitCount = 0;
    private string bulletTag = "BULLET";
    public delegate void EnemyDieHandler();
    public static EnemyDieHandler OnEnemyDie;
    void Start()
    {
        source = GetComponent<AudioSource>();
        renderer = GetComponent<MeshRenderer>();
        explosionClip = Resources.Load<AudioClip>("Sounds/grenade_exp2");
        explosionSpark = Resources.Load<GameObject>("Effects/Exp");
        textures = Resources.LoadAll<Texture>("BarrelTextures");
        int idx = Random.Range(0, textures.Length);
        renderer.material.mainTexture = textures[idx];
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.CompareTag(bulletTag))
        {
            if(++hitCount ==3)
            {
                ExplosionBarrel();
            }

        }
    }
    void ExplosionBarrel()
    {
        GameObject exp = Instantiate(explosionSpark ,transform.position,
            Quaternion.identity);
        Destroy(exp, 1.5f);
        source.PlayOneShot(explosionClip, 1.0f);
        Collider[] Cols = Physics.OverlapSphere(transform.position, 20f,~(1<<6));
                      // �ڱ��ڽ� ��ġ���� 20�ٹ��� �ݶ��̴��� Cols�� �迭 ��´�.
        foreach(Collider col in Cols)
        {
           Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.mass = 1.0f;
                rb.AddExplosionForce(1000f, transform.position, 20f, 800f);
                //������ٵ��������Լ�(���ķ�, ��ġ , �ݰ�  , ���� �ڱ�ġ�� ��
                //col.gameObject.SendMessage("Die",SendMessageOptions.DontRequireReceiver);
                OnEnemyDie();
            }

        }

        Invoke("ShowMeshFilter", 2.0f);
        Camera.main.GetComponent<ShakeCamera>().TurnOn();

    }
    void ShowMeshFilter()
    {
        int idx = Random.Range(0,meshes.Length);
        meshFilter.sharedMesh = meshes[idx];
    }
}
