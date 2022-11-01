using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class PlayerInventory : NetworkBehaviour
{

    //Dictionary of items
    [SerializeField] private Dictionary<string, Item> playerItemDict = new Dictionary<string, Item>();
    public Dictionary<string, Item> PlayerItemDict{
        get{ return playerItemDict; }
    }

    //List for the network
    [SerializeField] private List<string> networkItemList = new List<string>();
    public List<string> NetworkItemList{
        get{ return networkItemList; }
    }

    //Scripts
    public PlayerStats pStats;
    
    void Awake(){
        pStats = GetComponent<PlayerStats>();  
    }
    
    //Adds player Item to a dictionary
    void AddItem(Item item){
        playerItemDict.Add(item.name, item);
        item.Equip(pStats, this.gameObject);
    }

     //Add item to player Inventory
    public void UpdateItemNetwork(string itemName, int add){

        //if Removing items
        if(add == 0) networkItemList.Remove(itemName);

        //if adding items
        else if(add == 1 )networkItemList.Add(itemName);

        //if nothing is being updated
        else Debug.Log("Nothing updated");
    }

    //Remove Item player inv
    public void RemoveItem(Item item){
        //if item removal was successful
        if(playerItemDict.Remove(item.name)){

            //Modify player stats
            item.Unequip(pStats, this.gameObject);
        }
    }

    //Updates Inventory to Add item to player list
    public int UpdateInventory(Item item, bool ableToAdd){

        //if player can add the item
        if(!playerItemDict.ContainsKey(item.name) && ableToAdd){
            Debug.Log("Item Added");
            AddItem(item);
            return 1;
        }
        else if(!ableToAdd && !playerItemDict.ContainsKey(item.name)){
            //Item cannot be added
            Debug.Log("Item Cannot Be Added");
            return -1;
        }
        else{
            //If player already has the item it removes it
            Debug.Log("Item Removed");
            RemoveItem(item);
            return 0;
        }
    }

    //Equips All items in list at start of next scene
    public void UpdateEquips(List<string> items, Dictionary<string, Item> allItems){
        if (items.Count <= 0) { return; }

        foreach(string itemName in items){
            if (itemName == "" || itemName == " ") { continue; }

            if(!playerItemDict.ContainsKey(itemName)){
                AddItem(allItems[itemName]);
            }
            else{
                Debug.Log("Attempted To Equip Item Already in Inventory");
            }
                
        }
    }

    public void ClearInventory(){
        foreach(KeyValuePair<string, Item> item in PlayerItemDict){
            RemoveItem(item.Value);
        }
    }
}
