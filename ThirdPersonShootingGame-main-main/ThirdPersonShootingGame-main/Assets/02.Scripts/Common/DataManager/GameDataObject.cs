using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataInfo;
//æ÷∆©∏Æ∫‰∆Æ 
[CreateAssetMenu(fileName ="GameDataSO",menuName ="Create GameData",order =1)]
public class GameDataObject : ScriptableObject
{
    public int killCount = 0;
    public float hp = 120f;
    public float damage = 25.0f;
    public float speed = 6.0f;
    public List<Item>equipItem = new List<Item>();
  
}


