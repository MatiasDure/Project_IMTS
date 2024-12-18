using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.Merge.Xml;
using UnityEngine;

public class ItemSwitcher : MonoBehaviour
{
    [SerializeField] private List<WardrobeItem> items = new();

    public void SwitchOn(WardrobeItem itemToSwitchOn)
    {
        if (itemToSwitchOn.itemType == WardrobeItem.ItemType.goggles)
        {
            foreach (var item in items)
            {
                if (item != itemToSwitchOn)
                {
                    item.gameObject.SetActive(false);
                }
            }
        } else
        {
            foreach (var item in items)
            {
                if (item != itemToSwitchOn && item.itemType == itemToSwitchOn.itemType)
                {
                    item.gameObject.SetActive(false);
                }

                if (item != itemToSwitchOn && item.itemType == WardrobeItem.ItemType.goggles)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }


        if (itemToSwitchOn.gameObject.activeSelf)
        {
            itemToSwitchOn.gameObject.SetActive(false);
        }
        else
        {
            itemToSwitchOn.gameObject.SetActive(true);
        }
    }
}
