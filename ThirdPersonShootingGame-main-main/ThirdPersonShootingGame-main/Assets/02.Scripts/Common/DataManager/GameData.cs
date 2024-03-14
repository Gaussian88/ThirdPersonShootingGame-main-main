using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataInfo
{
    [System.Serializable]
    public class GameData
    {
        public int killCount = 0;
        public float hp = 120f;
        public float damage = 25f;
        public float speed = 6.0f;
        public List<Item> equipItem = new List<Item>();
        
    }
    [System.Serializable]
    public class Item
    {
        public enum ItemType { HP=1,SPEED,GRENADE,DAMAGE}
        public enum ItemCalc { VALUE,PERSENT} //아이템 계산 방식 값, 퍼센트 
        public ItemType itemType;
        public ItemCalc itemCalc;

        public string name; //이름 
        public string desc; //설명
        public float value; //값
    }

}
