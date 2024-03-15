using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Transform camPivotTr;
    public Transform camTr;
    public float height = 5f;
    public float distance = 10f;
    public float rotDamping = 10f;
    public float moveDamping = 15f;
    public readonly float targetOffset = 2.0f;
    public float origin = 0f;
    public float maxheight = 15f;
    void Awake()
    {
        origin = height;
      
    }
    private void Update()
    {
        
         if(target == null&& camPivotTr==null) return;
        #region 카메라 에서 광선을 쏘아서 맞은 위치랑 카메라위치의 거리 만큼 카메라가 뒤에 위치
        //RaycastHit hit;
        //if (Physics.Raycast(camPivotTr.position,  camTr.position- camPivotTr.position, out hit,
        // 20f,1<<8 | 1<<12))
        //{
        //    if(!hit.collider.CompareTag("Player"))
        //    camTr.localPosition = Vector3.back * hit.distance;
        //     else
        //        camTr.localPosition = Vector3.back * distance;
        //}

        #endregion
        RaycastHit casthit;
        Vector3 castdir = (target.position - transform.position).normalized;
        if(Physics.Raycast(transform.position,castdir,out casthit ,20f))
        {
           if(!casthit.collider.CompareTag("Player"))
            {
                height = Mathf.Lerp(height,maxheight,Time.deltaTime *3f);
            }
           else
            {
                height = Mathf.Lerp(height, origin, Time.deltaTime * 3f);
            }
        }

    }
    void LateUpdate()
    {
        var camPos = target.position - (target.forward * distance) + (target.up * height);
        camPivotTr.position = Vector3.Slerp(camPivotTr.position, camPos, Time.deltaTime * moveDamping);
        camPivotTr.rotation = Quaternion.Slerp(camPivotTr.rotation, target.rotation, Time.deltaTime * rotDamping);
        camPivotTr.LookAt(target.transform.position +(target.up * targetOffset));
    }
    private void OnDrawGizmos() //씬화면에 라인(선)이나 색깔을 넣어주는 함수 콜백 함수 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target.position + (target.up * targetOffset), 0.1f);

        Gizmos.DrawLine(target.position + (target.up * targetOffset), camPivotTr.position);
    }
    void OnValidate()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        camPivotTr = GetComponent<Transform>();
        camTr = transform.GetChild(0).transform;

    }
}
