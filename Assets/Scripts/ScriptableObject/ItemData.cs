using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Equipable,
    Consumable,
}

public enum ConsumableType
{
    Speed,
    Jump
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float time;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public GameObject dropPrefab;

    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

}
