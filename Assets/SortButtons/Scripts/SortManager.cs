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
    [SerializeField] bool version2;

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
            Quick(ref itemSys.inventoryItemList);
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
                SortByName(true);
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
            itemSys.AddItemToInventory(chosenItem, version2);
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
                if (list[i].Weight < pivot.Weight)
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
        //define minimum and maximum bounds
        int min = 0;
        int max = list.Count - 1;

        while (min <= max)
        {
            // defines a middle value using provided bounds
            int mid = (min + max) / 2;
            // checks if item corresponds to middle value
            if (list[mid].Name == input)
            {
                // if it does, return item
                list.RemoveAt(mid);
                //Console.WriteLine("\n Binary Search: {0} is at index {1} \n", value, mid);
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
}
