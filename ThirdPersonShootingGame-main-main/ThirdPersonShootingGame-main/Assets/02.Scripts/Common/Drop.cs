using DataInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour,IDropHandler
{
  
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount ==0)
        {
            Drag.DraggingItem.transform.SetParent(this.transform,false);
            Item item = Drag.DraggingItem.GetComponent<ItemInfo>().itemData;
            GameManager.Instance.AddItem(item);
        }
    }

}
