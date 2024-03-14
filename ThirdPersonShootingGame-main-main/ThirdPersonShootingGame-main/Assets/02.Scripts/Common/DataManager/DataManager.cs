using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //���� ����� (������� �Դٰ��� �ϴ� ������ ��Ʈ���̶�� �Ѵ�)
using System.Runtime.Serialization.Formatters.Binary;//����ȭ 
using DataInfo;

public class DataManager : MonoBehaviour
{
    [SerializeField] private string dataPath;
    public void Initialized() //���� ��� �� ���� Ȯ����
    {
        dataPath = Application.persistentDataPath + "gameData.dat";
    }
    public void Save(GameData gameData)
    {    
        // ������ �ϵ��ũ�� �����ϱ� ����  ���������� ��� ����� �Ҵ� 
        BinaryFormatter bf = new BinaryFormatter();
        // ���Ͻ�Ʈ�� ��θ� ���� �����ο� ���� ���� 
        FileStream file = File.Create(dataPath);
        GameData data = new GameData();
        data.killCount = gameData.killCount;
        data.hp = gameData.hp;
        data.speed = gameData.speed;
        data.damage = gameData.damage;
        data.equipItem = gameData.equipItem;
    //���� ����ȭ �ؼ� ���� string,float int�� �����͸� byte�� ��ȯ�ؼ� ����
        bf.Serialize(file, data);
        file.Close(); //���� �ݱ� 
    }
    public GameData Load()
    {       //���ϰ�ο� ������ ���� �Ѵٸ� 
        if(File.Exists(dataPath))
        {     //���������� ����� ���� 
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath,FileMode.Open);
             //���ϰ�θ� ã�Ƽ� ���¸��� �����.
             GameData gameData = (GameData)bf.Deserialize(file);
           //������ȭ( byte�� �ٽ� float�̳� int�� �����ͷ� ��ȯ
            file.Close(); //���� �ݱ� 
             return gameData;

        }
        else
        {   //�׳� ��ȯ ������� 
            GameData data = new GameData();
            return data;
        }
    }

}
