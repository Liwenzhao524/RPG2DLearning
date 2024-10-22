using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Êý¾ÝÀà
/// </summary>
[System.Serializable]
public class Stats
{
    [SerializeField] float baseValue;

    List<float> _modifiers = new();
    
    public float GetValue() 
    {
        float finalValue = baseValue;
        if (_modifiers != null)
        {
            foreach (float modifier in _modifiers)
            {
                finalValue += modifier;
            }
        }
        return finalValue; 
    }

    public void SetDefaultValue(float value)
    {
        baseValue = value;
    }

    public void AddModifier(float modifier) { _modifiers.Add(modifier);}

    public void RemoveModifier(float modifier) { _modifiers.Remove(modifier);}
}
