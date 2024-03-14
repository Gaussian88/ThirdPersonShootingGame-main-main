using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //이벤트 관련기능
public class Drag : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [SerializeField] RectTransform inventoryTr;
    [SerializeField] RectTransform SlotListTr;
    [SerializeField] RectTransform ItemListTr;
    [SerializeField] RectTransform itemTr;
    [SerializeField] CanvasGroup canvasGroup;
    public static GameObject DraggingItem = null;

   
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemTr = GetComponent<RectTransform>();
        ItemListTr = GameObject.Find("Image-ItemList").GetComponent<RectTransform>();
        SlotListTr = GameObject.Find("Image-SlotList").GetComponent<RectTransform>();
        inventoryTr = GameObject.Find("Image-Inventory").GetComponent<RectTransform>() ;

    }
    public void OnDrag(PointerEventData eventData)
    {
        itemTr.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DraggingItem = this.transform.gameObject;
        itemTr.SetParent(inventoryTr);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DraggingItem = null;
        canvasGroup.blocksRaycasts = true;
        if(itemTr.parent == inventoryTr)
        {
            itemTr.SetParent(ItemListTr);
            GameManager.Instance.RemoveItem(GetComponent<ItemInfo>().itemData);
        }
    }
}
