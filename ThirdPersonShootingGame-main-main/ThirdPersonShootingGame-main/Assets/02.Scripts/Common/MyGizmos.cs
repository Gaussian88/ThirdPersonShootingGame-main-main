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
    void OnDrawGizmos() //선이나 색상 그려주는 유니티 지원 함수 콜백함수 
    {
        if (type == Type.NORMAL)
        {
            Gizmos.color = _color; //컬러 
            Gizmos.DrawSphere(transform.position, _radius); // 모양이나 도형(위치, 반경)
        }
        else
        {
            Gizmos.color = _color;
            Gizmos.DrawIcon(transform.position + Vector3.up * 1f, SpawnFile, true);
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

    }

}
