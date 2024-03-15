using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//EnemyFOV ��ũ��Ʈ�� �ڵ����� ���� �ǰ� �����.
[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor //�������� ���̴� GUI�� �����̴�.
{
    void OnSceneGUI() //������ �������� GUI�� �׷��ִ� �Լ� 
    {      //target ��� ���� 
        EnemyFOV fov = (EnemyFOV)target;
                               //������ ������ �������� ��ġ 
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);
        //���� ������  ������� 
        Handles.color = Color.white;
        //������ �׸��� �� (������ǥ 
        Handles.DrawWireDisc(fov.transform.position, //������ǥ
                             Vector3.up, //��� ���� 
                            fov.viewRange); // ���� ������
        //��ä�� ����
        Handles.color = new Color(1f, 1f, 1f, 0.3f);
        //ä���� ��ä���� �׸�
        Handles.DrawSolidArc(fov.transform.position, //������ǥ
                                        Vector3.up, //��ֺ���
                                        fromAnglePos, //��ä�� ������ġ
                                        fov.viewAngle,//����
                                        fov.viewRange); // ������
        //�þ߰��� �ؽ�Ʈ�� ǥ�� 
        Handles.Label(fov.transform.position +(fov.transform.forward *2.0f),
            fov.viewRange.ToString());

    }

}
