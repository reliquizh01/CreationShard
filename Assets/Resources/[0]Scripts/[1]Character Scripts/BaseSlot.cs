using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barebones.Items;

public class BaseSlot : MonoBehaviour {
    
    public List<EquipType> holdItemType;
    public EquipType currentItemType;
    public ItemBase currentItem;
    

    public bool IsSlotOccupied
    {
        get
        {
            if (currentItem != null)
                return false;
            else return false;
        }
    }
    public bool PlaceItemToSlot(ItemBase thisItem)
    {
        if(currentItem != null)
        {
            return false;
        }
        
        currentItem = thisItem;
        currentItemType = thisItem.equipType;
        SetCurrentItemParent();
        return true;
    }
    
    public void SetCurrentItemParent()
    {
        currentItem.transform.parent = this.transform;
        currentItem.transform.rotation = this.transform.rotation;
        currentItem.transform.localPosition = Vector3.zero;
    }

    public void DropCurrentItem()
    {
        if(currentItem != null)
        {
            currentItem.transform.parent = null;
            currentItem.DropItem();
            currentItem = null;
        }
    }
    public void NewSetOfHoldItem(EquipType thisType, bool toAdd = true)
    {
        if(toAdd)
        {
            holdItemType.Add(thisType);
        }
        else
        {
            if (holdItemType.Contains(thisType))
            {
                holdItemType.Remove(thisType);
            }
        }
    }

    public void AdjustSlotPosition(Vector3 newLocalVector)
    {
        if(newLocalVector == null)
        {
            return;
        }
        this.transform.localPosition = newLocalVector;
    }
}
