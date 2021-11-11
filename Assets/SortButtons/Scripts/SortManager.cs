using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortManager : MonoBehaviour
{
    [SerializeField] ItemManagementSystem itemSys;
    [SerializeField] Text searchBox;
    [SerializeField] Scrollbar scrollbar;

    float startTime;

    void StartTimer()
    {
        startTime = Time.realtimeSinceStartup;
        Debug.Log("Timer started");
    }

    void EndTimer()
    {
        float timeTaken = Time.realtimeSinceStartup - startTime;
        Debug.Log("Done after " + timeTaken + " seconds");
    }

    public void SortByName(bool version2 = false)
    {
        StartTimer();

        if (version2)
        {
            Bubble(ref itemSys.inventoryItemList);
        }
        else
        {
            Bubble(ref itemSys.inventoryItemList);
        }

        itemSys.InitialiseInventoryItemList();
        EndTimer();
    }

    public void SortByWeight(bool version2 = false)
    {
        StartTimer();
        if (version2)
        {
            Quick(ref itemSys.inventoryItemList, true);
        }
        else
        {
            Bubble(ref itemSys.inventoryItemList, true);
        }

        itemSys.InitialiseInventoryItemList();
        EndTimer();
    }

    public void SearchRemove(bool version2 = false)
    {
        StartTimer();
        string input = searchBox.text.ToLower();

        if (input != "")
        {

            if (version2)
            {
                Binary(input, ref itemSys.inventoryItemList);
            }
            else
            {
                Linear(input, ref itemSys.inventoryItemList);
            }

            itemSys.InitialiseInventoryItemList();
        }

        EndTimer();
    }

    public void ClearInventory()
    {
        itemSys.inventoryItemList = new List<Item>();
        itemSys.ClearInventoryItemList();
        scrollbar.size = 1;
    }

    public void AddRandomItems()
    {
        int max = itemSys.fullItemList.Count;

        for (int i=0; i < 10000; i++)
        {
            int chosenItem = Random.Range(0, max);
            itemSys.AddItemToInventory(chosenItem);
        }

        itemSys.InitialiseInventoryItemList();
    }
    static void Bubble(ref List<Item> list, bool sortByWeight = false)
    {
        int n = list.Count;
        Item temp;

        bool swapped = true;

        while (swapped)
        {
            swapped = false;
            for (int i = 0; i < n - 1; i++)
            {
                if (!sortByWeight)
                {
                    if (list[i].Name[0] > list[i + 1].Name[0])
                    {
                        temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                        swapped = true;
                    }
                    else if (list[i].Name.CompareTo(list[i + 1].Name) > 0)
                    {
                        temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                        swapped = true;
                    }
                } else
                {
                    if (list[i].Weight > list[i+1].Weight)
                    {
                        temp = list[i];
                        list[i] = list[i + 1];
                        list[i + 1] = temp;
                        swapped = true;
                    }
                }
            }
        }
    }

    static void Quick(ref List<Item> list, bool sortByWeight = false)
    {

    }

    void Linear(string input, ref List<Item> list)
    {
        int n = list.Count;

        for (int i = 0; i < n; i++)
        {
            
            if (list[i].Name.ToLower() == input)
            {
                //Debug.Log("  a  ");
                list.RemoveAt(i);
                n--;
                i--;
            }
        }
    }

    void Binary(string input, ref List<Item> list)
    {

    }
}
