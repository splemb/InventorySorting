using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManagementSystem : MonoBehaviour
{
    [SerializeField]
    public List<Item> fullItemList = new List<Item>();
    [SerializeField]
    public List<Item> inventoryItemList = new List<Item>();

    [SerializeField]
    GameObject itemPrefab;

    [SerializeField]
    Transform inventoryTransform;

    [SerializeField]
    bool version2;
    public bool sortingByWeight = false;

    void Start()
    {
        DefineItems();
        InitialiseFullItemList();
    }

    void Update()
    {

    }

    void DefineItems()
    {
        fullItemList.Add(new Item("Axe", 3.0f));
        fullItemList.Add(new Item("Bandage", 0.4f));
        fullItemList.Add(new Item("Cheese Wheel", 1f));
        fullItemList.Add(new Item("Crossbow", 4.0f));
        fullItemList.Add(new Item("Dagger", 0.8f));
        fullItemList.Add(new Item("Emerald", 0.2f));
        fullItemList.Add(new Item("Fish", 2.0f));
        fullItemList.Add(new Item("Gems", 0.3f));
        fullItemList.Add(new Item("Hat", 0.6f));
        fullItemList.Add(new Item("Ingot", 5.0f));
        fullItemList.Add(new Item("Junk", 1.2f));
        fullItemList.Add(new Item("Map", 0.1f));
        fullItemList.Add(new Item("Potion", 0.5f));
        fullItemList.Add(new Item("Sandwich", 0.2f));
        fullItemList.Add(new Item("Sword", 3.5f));


    }

    void InitialiseFullItemList()
    {
        GameObject gameObject;

        for (int i = 0; i < fullItemList.Count; i++)
        {
            gameObject = Instantiate(itemPrefab, transform);
            gameObject.transform.GetChild(0).GetComponent<Text>().text = fullItemList[i].Name;
            gameObject.transform.GetChild(1).GetComponent<Text>().text = fullItemList[i].Weight.ToString() + " lbs";
            gameObject.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }
    }

    public void InitialiseInventoryItemList()
    {
        ClearInventoryItemList();
        GameObject gameObject;

        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            gameObject = Instantiate(itemPrefab, inventoryTransform);
            gameObject.transform.GetChild(0).GetComponent<Text>().text = inventoryItemList[i].Name;
            gameObject.transform.GetChild(1).GetComponent<Text>().text = inventoryItemList[i].Weight.ToString() + " lbs";
            gameObject.GetComponent<Button>().AddEventListener(i, InventoryItemClicked);
        }
    }

    public void ClearInventoryItemList()
    {
        foreach (Transform child in inventoryTransform)
        {
            GameObject.Destroy(child.gameObject);
        }


    }

    void ItemClicked(int index)
    {

        Debug.Log("Item Cicked: " + index + ". " + fullItemList[index].Name + " (" + fullItemList[index].Weight + ")");
        AddItemToInventory(index, version2);
        InitialiseInventoryItemList();

    }

    void InventoryItemClicked(int index)
    {
        Debug.Log("Item Cicked: " + index + ". " + inventoryItemList[index].Name + " (" + inventoryItemList[index].Weight + ")");
    }

    public void AddItemToInventory(int index, bool version2)
    {
        Item item = new Item(fullItemList[index].Name, fullItemList[index].Weight);
        if (!version2)
        {
            inventoryItemList.Add(item);
        }
        else
        {
            if (!sortingByWeight) BinaryInsert(item, ref inventoryItemList);
            else BinaryInsertWeight(item, ref inventoryItemList);
        }

        //InitialiseInventoryItemList();
    }

    void BinaryInsert(Item item, ref List<Item> list)
    {
        if (list.Count == 0)
        {
            list.Add(item);
            return;
        }
        if (list[list.Count - 1].Name.CompareTo(item.Name) <= 0)
        {
            list.Add(item);
            return;
        }
        if (list[0].Name.CompareTo(item.Name) >= 0)
        {
            list.Insert(0, item);
            return;
        }

        //define minimum and maximum bounds
        int min = 0;
        int max = list.Count - 1;

        while (min <= max)
        {
            // defines a middle value using provided bounds
            int mid = (min + max) / 2;
            // checks if item corresponds to middle value
            if (list[mid].Name == item.Name)
            {
                // if it does, insert item
                list.Insert(mid + 1, item);
                return;
                //Console.WriteLine("\n Binary Search: {0} is at index {1} \n", value, mid);
            }
            else if (mid == max || mid == min)
            {
                list.Insert(mid, item);
                return;
            }
            // if item is smaller in value than our current one
            else if (list[mid].Name.ToLower().CompareTo(item.Name) > 0)
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

    void BinaryInsertWeight(Item item, ref List<Item> list)
    {
        if (list.Count == 0)
        {
            list.Add(item);
            return;
        }
        if (list[list.Count - 1].Weight.CompareTo(item.Weight) <= 0)
        {
            list.Add(item);
            return;
        }
        if (list[0].Weight.CompareTo(item.Weight) >= 0)
        {
            list.Insert(0, item);
            return;
        }

        //define minimum and maximum bounds
        int min = 0;
        int max = list.Count - 1;

        while (min <= max)
        {
            // defines a middle value using provided bounds
            int mid = (min + max) / 2;
            // checks if item corresponds to middle value
            if (list[mid].Weight == item.Weight)
            {
                // if it does, insert item
                list.Insert(mid + 1, item);
                return;
                //Console.WriteLine("\n Binary Search: {0} is at index {1} \n", value, mid);
            }
            else if (mid == max || mid == min)
            {
                list.Insert(mid, item);
                return;
            }
            // if item is smaller in value than our current one
            else if (list[mid].Weight.CompareTo(item.Weight) > 0)
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

public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
	{
        button.onClick.AddListener(delegate() { OnClick(param); });
    }
}

public class Item
{
    public string Name { get; set; }
    public float Weight { get; set; }

    public Item(string name, float weight)
    {
        Name = name;
        Weight = weight;
    }
}