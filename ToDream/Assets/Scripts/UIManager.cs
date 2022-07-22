using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _instance;
    public static UIManager _Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                
                if (_instance == null)
                {
                    Debug.Log("Did not find UIManager");
                }
            }

            return _instance;
        }
    }
    #endregion

    [SerializeField] private DialogUI _dialogUI;
    [SerializeField] private QuestUI _questUI;
    [SerializeField] private ObjectInteraction _objInter;

    public DialogUI _DialogUI => _dialogUI;
    public QuestUI _QuestUI => _questUI;
    public ObjectInteraction _ObjInter => _objInter;

    private void Awake()
    {
        _instance = this;

        _objInter = FindObjectOfType<ObjectInteraction>();
    } 
}
