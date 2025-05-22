using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Equipable,
    Consumable,
    Resource
}

public enum  ConsumableType
{
    Health,
    Hunger
}


[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public int value;
}



[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType itemType;
    public Sprite icon;
    public GameObject dropprefab;


    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;


    [Header("Consumable")]
    public ItemDataConsumable[] consumable;

}
