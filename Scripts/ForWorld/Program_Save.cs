using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

[CreateAssetMenu(fileName = "Program_Save", menuName = "Program_Save")]
public class Program_Save : ScriptableObject
{
    public List<Weapon> Weapon_Inventory = new List<Weapon>();
}
