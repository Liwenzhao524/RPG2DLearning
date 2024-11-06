using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DictionarySerialized<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] List<TKey> _keys = new();
    [SerializeField] List<TValue> _values = new();

    public void OnBeforeSerialize ()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var item in this)
        {
            _keys.Add(item.Key);
            _values.Add(item.Value);
        }
    }

    public void OnAfterDeserialize ()
    {
        Clear ();

        if(_keys.Count != _values.Count)
        {
            Debug.Log("keys.Count != values.Count");
        }

        for (int i = 0; i < _keys.Count; i++)
        {
            Add(_keys[i], _values[i]);
        }
    }

} 
