using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //파일 입출력 (입출력이 왔다갔다 하는 공간을 스트림이라고 한다)
using System.Runtime.Serialization.Formatters.Binary;//직렬화 
using DataInfo;

public class DataManager : MonoBehaviour
{
    [SerializeField] private string dataPath;
    public void Initialized() //파일 경로 및 저장 확장자
    {
        dataPath = Application.persistentDataPath + "gameData.dat";
    }
    public void Save(GameData gameData)
    {    
        // 파일을 하드디스크에 저장하기 위해  이진데이터 양식 선언및 할당 
        BinaryFormatter bf = new BinaryFormatter();
        // 파일스트림 통로를 거쳐 저장경로에 파일 저장 
        FileStream file = File.Create(dataPath);
        GameData data = new GameData();
        data.killCount = gameData.killCount;
        data.hp = gameData.hp;
        data.speed = gameData.speed;
        data.damage = gameData.damage;
        data.equipItem = gameData.equipItem;
    //파일 직렬화 해서 저장 string,float int형 데이터를 byte로 변환해서 저장
        bf.Serialize(file, data);
        file.Close(); //파일 닫기 
    }
    public GameData Load()
    {       //파일경로에 파일이 존재 한다면 
        if(File.Exists(dataPath))
        {     //이진데이터 양식을 선언 
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath,FileMode.Open);
             //파일경로를 찾아서 오픈모드로 만든다.
             GameData gameData = (GameData)bf.Deserialize(file);
           //역직렬화( byte를 다시 float이나 int형 데이터로 변환
            file.Close(); //파일 닫기 
             return gameData;

        }
        else
        {   //그냥 반환 원래대로 
            GameData data = new GameData();
            return data;
        }
    }

}
