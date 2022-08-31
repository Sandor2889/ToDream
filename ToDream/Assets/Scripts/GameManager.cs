using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager _Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    Debug.Log("Did not find GameManager");
                }
            }

            return _instance;
        }
    }
    #endregion

    private ItemDataBase _itemDataBase;
    public ItemDataBase _ItemDataBase => _itemDataBase;

    private void Awake()
    {
        _instance = this;
        _itemDataBase = FindObjectOfType<ItemDataBase>();
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.3f);
    }

    public static Item GetDicValue(int key)
    {
        Item item;
        _instance._itemDataBase._itemDic.TryGetValue(key, out item);
        return item;
    }
}
