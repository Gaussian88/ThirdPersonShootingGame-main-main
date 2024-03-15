using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//EnemyFOV 스크립트에 자동으로 포함 되게 만든다.
[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor //씬에서만 보이는 GUI용 도구이다.
{
    void OnSceneGUI() //에디터 씬에서만 GUI를 그려주는 함수 
    {      //target 대상 지정 
        EnemyFOV fov = (EnemyFOV)target;
                               //원주의 각도의 시작점에 위치 
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);
        //원의 색상은  흰색으로 
        Handles.color = Color.white;
        //원반을 그리는 데 (원점좌표 
        Handles.DrawWireDisc(fov.transform.position, //원점좌표
                             Vector3.up, //노멀 벡터 
                            fov.viewRange); // 원의 반지름
        //부채꼴 색상
        Handles.color = new Color(1f, 1f, 1f, 0.3f);
        //채워진 부채꼴을 그림
        Handles.DrawSolidArc(fov.transform.position, //원점좌표
                                        Vector3.up, //노멀벡터
                                        fromAnglePos, //부채꼴 시작위치
                                        fov.viewAngle,//각도
                                        fov.viewRange); // 반지름
        //시야각을 텍스트에 표시 
        Handles.Label(fov.transform.position +(fov.transform.forward *2.0f),
            fov.viewRange.ToString());

    }

}
