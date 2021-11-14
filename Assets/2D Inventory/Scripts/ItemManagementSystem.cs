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

    public bool version2;
    public bool sortingByWeight = false;

    int totalPages;
    int pageNumber = 1;
    int itemsPerPage = 12;
    [SerializeField] Text pageCounter;
    [SerializeField] Text itemCounter;

    void Start()
    {
        DefineItems();
        InitialiseFullItemList();
        
    }

    void DefineItems()
    {
        fullItemList.Add(new Item("Axe", 3.0f));
        fullItemList.Add(new Item("Bag of Coins", 4f));
        fullItemList.Add(new Item("Bandage", 0.4f));
        fullItemList.Add(new Item("Bobby Pin", 0.1f));
        fullItemList.Add(new Item("Cheese Wheel", 1f));
        fullItemList.Add(new Item("Crossbow", 4.0f));
        fullItemList.Add(new Item("Dagger", 0.8f));
        fullItemList.Add(new Item("Emerald", 0.2f));
        fullItemList.Add(new Item("EmotionObject", 0.5f));
        fullItemList.Add(new Item("Fish", 2.0f));
        fullItemList.Add(new Item("Fire Medallion", 3.0f));
        fullItemList.Add(new Item("Gems", 0.3f));
        fullItemList.Add(new Item("Gold Ingot", 5.0f));
        fullItemList.Add(new Item("Hat", 0.6f));
        fullItemList.Add(new Item("Heart Refresh", 0.5f));
        fullItemList.Add(new Item("Hookshot", 3f));
        fullItemList.Add(new Item("Iron Ingot", 5.0f));
        fullItemList.Add(new Item("Junk", 1.2f));
        fullItemList.Add(new Item("Letter", 0.1f));
        fullItemList.Add(new Item("Mana Prism", 0.5f));
        fullItemList.Add(new Item("Map", 0.1f));
        fullItemList.Add(new Item("Potion", 0.5f));
        fullItemList.Add(new Item("Sandwich", 0.2f));
        fullItemList.Add(new Item("Sword", 3.5f));
        fullItemList.Add(new Item("Umbrella", 2f));
        fullItemList.Add(new Item("Whole Chicken in a Can", 2f));
    }

    void InitialiseFullItemList() //The Creative Inventory
    {
        GameObject gameObject;

        for (int i = 0; i < fullItemList.Count; i++)
        {
            gameObject = Instantiate(itemPrefab, transform);
            gameObject.transform.GetChild(0).GetComponent<Text>().text = fullItemList[i].Name;
            gameObject.transform.GetChild(1).GetComponent<Text>().text = fullItemList[i].Weight.ToString("f1") + " lbs";
            gameObject.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }
    }

    public void InitialiseInventoryItemList() //The Player Inventory
    {
        ClearInventoryItemList();
        GameObject gameObject;

        totalPages = Mathf.CeilToInt((inventoryItemList.Count - 1) / itemsPerPage) + 1;

        int start = (pageNumber * itemsPerPage) - itemsPerPage;

        for (int i = start; i < Mathf.Min(start+12,inventoryItemList.Count); i++)
        {
            gameObject = Instantiate(itemPrefab, inventoryTransform);
            gameObject.transform.GetChild(0).GetComponent<Text>().text = inventoryItemList[i].Name;
            gameObject.transform.GetChild(1).GetComponent<Text>().text = inventoryItemList[i].Weight.ToString("f1") + " lbs";
            gameObject.GetComponent<Button>().AddEventListener(i, InventoryItemClicked);
        }

        pageCounter.text = pageNumber + " / " + totalPages;
        itemCounter.text = "<b>Items</b>  " + inventoryItemList.Count;
        if (pageNumber > totalPages) SetPage(totalPages);
    }

    public void SetPage(int page)
    {
        pageNumber = page;
        InitialiseInventoryItemList();
    }

    public void NextPage()
    {
        pageNumber++;
        if (pageNumber > totalPages) pageNumber = 1;
        InitialiseInventoryItemList();
    }

    public void PrevPage()
    {
        pageNumber--;
        if (pageNumber <= 0) pageNumber = totalPages;
        InitialiseInventoryItemList();
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
        AddItemToInventory(index);
        InitialiseInventoryItemList();
    }

    void InventoryItemClicked(int index)
    {
        Debug.Log("Item Cicked: " + index + ". " + inventoryItemList[index].Name + " (" + inventoryItemList[index].Weight + ")");
        inventoryItemList.RemoveAt(index);
        InitialiseInventoryItemList();
        
    }

    public void AddItemToInventory(int index)
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
            }
            else if (mid == max || mid == min)
            {
                if (list[mid].Name.ToLower().CompareTo(item.Name) > 0)
                {
                    list.Insert(mid, item);
                }
                else
                {
                    list.Insert(mid + 1, item);
                }
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
        if (list[list.Count - 1].Weight.CompareTo(item.Weight) <= 0f)
        {
            list.Add(item);
            return;
        }
        if (list[0].Weight.CompareTo(item.Weight) >= 0f)
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
                list.Insert(mid, item);
                return;
            }
            else if (mid == max || mid == min)
            {
                if (list[mid].Weight > item.Weight)
                {
                    list.Insert(mid, item);
                }
                else
                {
                    list.Insert(mid + 1, item);
                }
                return;
            }
            // if item is smaller in value than our current one
            else if (list[mid].Weight.CompareTo(item.Weight) > 0f)
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