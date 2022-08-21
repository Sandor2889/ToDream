using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    #region Singleton
    private static QuestUIManager _instance;
    public static QuestUIManager _Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<QuestUIManager>();

                if (_instance == null)
                {
                    Debug.Log("Did not find QuestUIManager");
                }
            }

            return _instance;
        }
    }
    #endregion



}
