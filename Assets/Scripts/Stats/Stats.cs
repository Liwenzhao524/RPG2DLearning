using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Êý¾ÝÀà
/// </summary>
[System.Serializable]
public class Stats
{
    [SerializeField] float _baseValue;

    [SerializeField]List<float> _modifiers = new();
    
    public float GetValue() 
    {
        float finalValue = _baseValue;
        if (_modifiers != null)
        {
            foreach (float modifier in _modifiers)
            {
                finalValue += modifier;
            }
        }
        finalValue = (float)Math.Round(finalValue, 2);
        return finalValue; 
    }

    public void SetDefaultValue(float value)
    {
        _baseValue = value;
    }

    public void AddModifier(float modifier) { _modifiers.Add(modifier);}

    public void RemoveModifier(float modifier) { _modifiers.Remove(modifier);}
}
