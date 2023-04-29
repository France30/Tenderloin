using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public enum StatType { Damage, Movement, FireRate, Health, Ammo, DamageResist }

    public StatType statType;
    public float value;
}
