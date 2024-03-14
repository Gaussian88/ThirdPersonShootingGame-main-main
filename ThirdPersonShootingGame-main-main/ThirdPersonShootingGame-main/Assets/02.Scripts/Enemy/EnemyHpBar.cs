using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    [SerializeField] private Camera ui_Camera; //uiī�޶�
    [SerializeField] private Canvas canvas; //ĵ���� �θ�
    [SerializeField] private RectTransform rectParent; //�θ� �̵�����
    [SerializeField] private RectTransform rectHp; // hp�� �پ��� ���� �̹��� 
    public Vector3 offset = Vector3.zero; //Ÿ����ġ�� ������ ��ġ
    public Transform targetTr; //Ÿ�� �� �� ��ġ 
    void Start()
    {
        canvas = transform.parent.GetComponent<Canvas>();
        ui_Camera = canvas.worldCamera;
        rectParent = transform.parent.GetComponent<RectTransform>();
        rectHp = GetComponentsInChildren<RectTransform>()[1];
    }
    void LateUpdate()
    {
         //������ǥ --> ��ũ����ǥ - ĵ���� ������ǥ ��ȯ �ؾ� �Ѵ�.
        var ScreenPos = Camera.main.WorldToScreenPoint(targetTr.position+offset);
                         //���忡�� ��ũ����ǥ ��ǥ ��ȯ
        //���ΰ� ī�޶� ������ �ִ� ���� ������ǥ���� ��ũ����ǥ�� �Ѿ�ö� z���� 
        //���� �Ѿ�ͼ� ������ �ִ� EnemyUI�� �ٶ󺸴� ���� �������� �ؿ� ������ ����.
        if (ScreenPos.z <0.01f) 
            ScreenPos *= -1;
        var locaPos = Vector2.zero;
        //��Ʈ Ʈ������ ��ƿ��Ƽ(Ŭ����)�ȿ�  ��ũ����ǥ���� ĵ���� ��ǥ ��ȯ �ϴ� �Լ� 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, ScreenPos,ui_Camera , out locaPos);
        //��ũ����ǥ���� ����(ĵ����)��ǥ�� ��ȯ
        rectHp.localPosition = locaPos;

    }
}
