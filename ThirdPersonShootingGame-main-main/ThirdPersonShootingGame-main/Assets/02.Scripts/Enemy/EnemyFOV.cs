using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRange = 15f;
    [Range(0f,360f)]
    public float viewAngle = 120f;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform tr;
    private int PlayerLayer;
    private int barrelLayer;
    private int layerMask;
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        tr = GetComponent<Transform>();
        PlayerLayer = LayerMask.NameToLayer("PLAYER");
        barrelLayer = LayerMask.NameToLayer("BARREL");
        layerMask = 1<<PlayerLayer | 1<<barrelLayer;
    }
    public Vector3 CirclePoint(float angle) //���������� ȸ������ ���ϴ� �Լ�
    {                                       //���� y�� ȸ���� ���� �޾�
                                            // 3d���� �������� ������ ���Ѵ�.
        //���� y�� ȸ���� ���� angle ������ ����
        angle += transform.eulerAngles.y;
         //3D ��������  ������ ���Ҷ� ���� = x: Sin ,0f,z: Cos
        return new Vector3(Mathf.Sign(angle * Mathf.Deg2Rad), 0f,
            Mathf.Cos(angle * Mathf.Deg2Rad));
                           //�Ϲݰ��� ����Ȱ���
    }
    public bool IsTracePlayer()
    {
        bool isTracePlayer = false;
        Collider[] cols = Physics.OverlapSphere(tr.position, viewRange, 1 << PlayerLayer);
        if(cols.Length == 1)
        {
            Vector3 dir = (playerTr.position - tr.position).normalized;
            if(Vector3.Angle(tr.forward,dir)< viewAngle *0.5f)
            {
               return  isTracePlayer = true;
            }
                
        }
        return isTracePlayer;

    }
    public bool IsViewPlayer()
    {
        bool isview = false;
        RaycastHit hit;
        Vector3 dir = (playerTr.position - tr.position).normalized;
        if(Physics.Raycast(tr.position, dir, out hit,viewRange,layerMask))
        {
            return isview = hit.collider.CompareTag("Player");

        }

        return isview;
    }
}
