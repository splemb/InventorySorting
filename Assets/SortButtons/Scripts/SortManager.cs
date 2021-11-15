using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortManager : MonoBehaviour
{
    [SerializeField] ItemManagementSystem itemSys;
    [SerializeField] InputField searchBox;

    [SerializeField] Button nameButton;
    [SerializeField] Button weightButton;

    [SerializeField] Text version1Desc;
    [SerializeField] Text version2Desc;

    

    float startTime;
    bool version2;

    private void Start()
    {
        version2 = itemSys.version2;
        if (version2) nameButton.interactable = false;
        version1Desc.enabled = !version2;
        version2Desc.enabled = version2;
        
    }

    void StartTimer()
    {
        startTime = Time.realtimeSinceStartup;
    }

    void EndTimer()
    {
        float timeTaken = Time.realtimeSinceStartup - startTime;
        Debug.Log("Done after " + timeTaken + " seconds");
    }

    public void SortByName(bool bypassUpdate = false)
    {
        StartTimer();
        itemSys.sortingByWeight = false;
        if (version2)
        {
            nameButton.interactable = false;
            weightButton.interactable = true;
            Quick(ref itemSys.inventoryItemList);
        }
        else
        {
            Bubble(ref itemSys.inventoryItemList);
        }

        if (!bypassUpdate) 
        { 
            itemSys.InitialiseInventoryItemList();
            itemSys.SetPage(1);
        }
        EndTimer();
    }

    public void SortByWeight(bool bypassUpdate = false)
    {
        StartTimer();
        itemSys.sortingByWeight = true;
        if (version2)
        {
            nameButton.interactable = true;
            weightButton.interactable = false;
            Quick(ref itemSys.inventoryItemList, true);
        }
        else
        {
            Bubble(ref itemSys.inventoryItemList, true);
        }

        if (!bypassUpdate)
        {
            itemSys.InitialiseInventoryItemList();
            itemSys.SetPage(1);
        }
        EndTimer();
    }

    public void SearchRemove(bool removeOne = false)
    {
        StartTimer();
        string input = searchBox.text.ToLower();

        if (input != "")
        {

            if (version2)
            {
                if (itemSys.sortingByWeight)
                {
                    SortByName(true);
                    Binary(input, ref itemSys.inventoryItemList, removeOne);
                    SortByWeight();
                }
                
                else Binary(input, ref itemSys.inventoryItemList, removeOne);
            }
            else
            {
                Linear(input, ref itemSys.inventoryItemList, removeOne);
            }

            itemSys.InitialiseInventoryItemList();
        }

        EndTimer();
    }

    public void ClearInventory()
    {
        itemSys.inventoryItemList = new List<Item>();
        itemSys.SetPage(1);
        
    }

    public void AddRandomItems(int amt = 10000)
    {
        int max = itemSys.fullItemList.Count;

        for (int i=0; i < amt; i++)
        {
            int chosenItem = Random.Range(0, max);
            itemSys.AddItemToInventory(chosenItem);
        }

        if (version2)
        {
            if (itemSys.sortingByWeight) SortByWeight();
            else SortByName();
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

    public void Quick(ref List<Item> list, bool sortByWeight = false)
    {
        // alternative to Quick sort where the bounds dont have to be pased in
        // bounds are automatically assigned to lower and upper ends of the array
        Quick(ref list, 0, list.Count - 1, sortByWeight);
    }

    void Quick(ref List<Item> list, int low, int high, bool sortByWeight)
    {
        // checks if the lower bound of the array is actually of a lower value
        if (low < high)
        {
            // calls the "Partition" function which returns a value into the pivot integer
            int pivot = Partition(list, low, high, sortByWeight);

            if (pivot > 1)
            {
                // pivot passed in as higher bound to sort the left hand side
                Quick(ref list, low, pivot - 1, sortByWeight);
            }
            if (pivot + 1 <= high)
            {
                // pivot passed in as lower bound to sort the right hand side
                Quick(ref list, pivot + 1, high, sortByWeight);
            }
        }
    }

    private int Partition(List<Item> list, int low, int high, bool sortByWeight)
    {
        Item pivot = list[high];
        int lowIndex = (low - 1);

        if (!sortByWeight)
        {
            // sorts the array based on values lower than the pivot
            for (int i = low; i < high; i++)
            {
                // only sorts if current point is smaller than the pivot
                if (list[i].Name.CompareTo(pivot.Name) < 0)
                {
                    lowIndex++;

                    Item temp = list[lowIndex];
                    list[lowIndex] = list[i];
                    list[i] = temp;
                }
            }
        }
        else
        {
            // sorts the array based on values lower than the pivot
            for (int i = low; i < high; i++)
            {
                // only sorts if current point is smaller than the pivot 
                if (list[i].Weight == pivot.Weight)
                {
                    if (list[i].Name.CompareTo(pivot.Name) < 0)
                    {
                        lowIndex++;

                        Item temp = list[lowIndex];
                        list[lowIndex] = list[i];
                        list[i] = temp;
                    }
                }
                else if (list[i].Weight < pivot.Weight)
                {
                    lowIndex++;

                    Item temp = list[lowIndex];
                    list[lowIndex] = list[i];
                    list[i] = temp;
                }
            }
        }

        // puts the current pivot into its correct position in the array 
        Item _temp = list[lowIndex + 1];
        list[lowIndex + 1] = list[high];
        list[high] = _temp;

        // returns the value ended up with to determine what to loop through next
        return lowIndex + 1;
    }

    void Linear(string input, ref List<Item> list, bool removeOne=false)
    {
        int n = list.Count;

        for (int i = 0; i < n; i++)
        {
            if (list[i].Name.ToLower() == input) // searches are not case sensitive
            {
                list.RemoveAt(i);
                if (removeOne) return; // If only set to remove one, stop searching
                n--; // Reduce recorded size of list
                i--; // Set counter back one
            }
        }
    }

    void Binary(string input, ref List<Item> list, bool removeOne = false)
    {
        //define minimum and maximum bounds
        int min = 0;
        int max = list.Count - 1;

        while (min <= max)
        {
            // defines a middle value using provided bounds
            int mid = Mathf.FloorToInt((min + max) / 2);
            // checks if item corresponds to middle value
            if (list[mid].Name.ToLower() == input)
            {
                // if it does, remove item
                list.RemoveAt(mid);
                if (removeOne) return;
                max--;
            }
            // if item is smaller in value than our current one
            else if (list[mid].Name.ToLower().CompareTo(input) > 0)
            {
                // discard the bottom half of the array
                // search again
                max = mid - 1;
            }
            else
            {
                // discard the top half of the array
                // loop back around
                min = mid + 1;
            }

        }
    }

    public void SwitchVersion()
    {
        itemSys.version2 = !itemSys.version2;
        version2 = itemSys.version2;
        ClearInventory();
        nameButton.interactable = true;
        weightButton.interactable = true;
        if (version2) nameButton.interactable = false;
        version1Desc.enabled = !version2;
        version2Desc.enabled = version2;
        searchBox.text = "";
        itemSys.sortingByWeight = false;
    }
}