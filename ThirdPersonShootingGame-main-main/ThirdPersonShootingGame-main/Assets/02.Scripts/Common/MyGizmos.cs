using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public enum Type { NORMAL,SPAWNPOINT}
    private const string SpawnFile = "Enemy";
    public Type type = Type.NORMAL;
    [SerializeField] private Color _color = Color.red;
    [SerializeField] private float _radius = 0.3f;
    void Start()
    {
        
    }
    void OnDrawGizmos() //���̳� ���� �׷��ִ� ����Ƽ ���� �Լ� �ݹ��Լ� 
    {
        if (type == Type.NORMAL)
        {
            Gizmos.color = _color; //�÷� 
            Gizmos.DrawSphere(transform.position, _radius); // ����̳� ����(��ġ, �ݰ�)
        }
        else
        {
            Gizmos.color = _color;
            Gizmos.DrawIcon(transform.position + Vector3.up * 1f, SpawnFile, true);
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

    }

}
