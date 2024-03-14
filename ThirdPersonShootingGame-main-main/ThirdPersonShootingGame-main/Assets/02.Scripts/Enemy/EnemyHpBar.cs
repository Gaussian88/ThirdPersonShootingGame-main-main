using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField] private Camera ui_Camera; //ui카메라
    [SerializeField] private Canvas canvas; //캔버스 부모
    [SerializeField] private RectTransform rectParent; //부모가 이디인지
    [SerializeField] private RectTransform rectHp; // hp가 줄어드는 실제 이미지 
    public Vector3 offset = Vector3.zero; //타겟위치를 보정할 위치
    public Transform targetTr; //타겟 이 될 위치 
    void Start()
    {
        canvas = transform.parent.GetComponent<Canvas>();
        ui_Camera = canvas.worldCamera;
        rectParent = transform.parent.GetComponent<RectTransform>();
        rectHp = GetComponentsInChildren<RectTransform>()[1];
    }
    void LateUpdate()
    {
         //월드좌표 --> 스크린좌표 - 캔버스 로컬좌표 변환 해야 한다.
        var ScreenPos = Camera.main.WorldToScreenPoint(targetTr.position+offset);
                         //월드에서 스크린좌표 좌표 변환
        //주인공 카메라 등지고 있는 데도 월드좌표에서 스크린좌표로 넘어올때 z값도 
        //같이 넘어와서 등지고 있는 EnemyUI를 바라보는 것을 막을려면 밑에 로직을 쓴다.
        if (ScreenPos.z <0.01f) 
            ScreenPos *= -1;
        var locaPos = Vector2.zero;
        //렉트 트랜스폼 유틸리티(클래스)안에  스크린좌표에서 캔버스 좌표 변환 하는 함수 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, ScreenPos,ui_Camera , out locaPos);
        //스크린좌표에서 로컬(캔버스)좌표로 변환
        rectHp.localPosition = locaPos;

    }
}
