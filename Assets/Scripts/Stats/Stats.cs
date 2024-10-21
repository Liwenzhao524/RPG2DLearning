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

    List<float> _modifiers = new List<float>();
    
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

    public void AddModifer(float modifier) { _modifiers.Add(modifier);}

    public void RemoveModifer(float modifier) { _modifiers.Remove(modifier);}
}
