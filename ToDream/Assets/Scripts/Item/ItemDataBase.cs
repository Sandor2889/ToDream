using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    public List<TKey> g_InspectorKeys;
    public List<TValue> g_InspectorValues;

    public SerializableDictionary()
    {
        g_InspectorKeys = new List<TKey>();
        g_InspectorValues = new List<TValue>();
        SyncInspectorFromDictionary();
    }

    public new void Add(TKey key, TValue vaule)
    {
        base.Add(key, vaule);
        SyncInspectorFromDictionary();
    }
    public new void Remove(TKey key)
    {
        base.Remove(key);
        SyncInspectorFromDictionary();
    }
    public void OnBeforeSerialize() { }
    public void SyncInspectorFromDictionary()
    {
        g_InspectorKeys.Clear();
        g_InspectorValues.Clear();
        foreach (var pair in this)
        {
            g_InspectorKeys.Add(pair.Key);
            g_InspectorValues.Add(pair.Value);
        }
    }
    public void SyncDictionaryFromInspector()
    {
        foreach (var key in Keys.ToList())
        {
            base.Remove(key);
        }
        for (int i = 0; i < g_InspectorKeys.Count; i++)
        {
            if (this.ContainsKey(g_InspectorKeys[i]))
            {
                Debug.LogError("�ߺ��� Ű�� �ֽ��ϴ�.");
                break;
            }
            base.Add(g_InspectorKeys[i], g_InspectorValues[i]);
        }
    }
    public void OnAfterDeserialize() // �⺻ (������ �ҷ��� ��)
    {
        if (g_InspectorKeys.Count == g_InspectorValues.Count)
        {
            SyncDictionaryFromInspector();
        }
    }
}


public class ItemDataBase : MonoBehaviour
{
    public SerializableDictionary<int, Item> _itemDic;

    public Item _inputItemData;
    [HideInInspector] public bool _isFolded;        // "Show data" GUI ������

    // Item data ���
    public void RegisterItem(int _key, Item item)
    {
        _itemDic.Add(_key, item);
        _inputItemData = null;
    }
}
