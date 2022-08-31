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

    private QuestUI _questUI;
    private QuestListUI _questListUI;
    private QButtonPool _qButtonPool;
    private QuestNavigation _questNav;
    private DialogUI _dialogUI;
    private NPCMarkerUI _npcMarkerUI;
    private InteractionUI _interUI;
    private InventoryUI _inventoryUI;
    private ConsumableSlot _consumableSlot;
    private WorldMapUI _worldMapUI;
    private CursorManager _cursorManager;

    public QuestUI _QuestUI => _questUI;
    public QuestListUI _QuestListUI => _questListUI;
    public QButtonPool _QButtonPool => _qButtonPool;
    public QuestNavigation _QuestNav => _questNav;
    public DialogUI _DialogUI => _dialogUI;
    public NPCMarkerUI _NPCMarkerUI => _npcMarkerUI;
    public InteractionUI _InterUI => _interUI;
    public InventoryUI _InventoryUI => _inventoryUI;
    public ConsumableSlot _ConsumableSlot => _consumableSlot;
    public WorldMapUI _WorldMapUI => _worldMapUI;
    public CursorManager _CursorManager => _cursorManager;

    public static bool _isOpendUI;


    private void Awake()
    {
        _instance = this;

        _questUI = FindObjectOfType<QuestUI>(true);
        _questListUI = FindObjectOfType<QuestListUI>(true);
        _qButtonPool = FindObjectOfType<QButtonPool>(true);
        _questNav = FindObjectOfType<QuestNavigation>(true);
        _dialogUI = FindObjectOfType<DialogUI>(true);
        _npcMarkerUI = FindObjectOfType<NPCMarkerUI>(true);
        _interUI = FindObjectOfType<InteractionUI>(true);
        _inventoryUI = FindObjectOfType<InventoryUI>(true);
        _consumableSlot = FindObjectOfType<ConsumableSlot>(true);
        _worldMapUI = FindObjectOfType<WorldMapUI>(true);
        _cursorManager = GetComponentInChildren<CursorManager>();
    }

    private void Update()
    {
        ControlUI();
    }


    // UI 컨트롤러
    public void ControlUI()
    {
        // 인벤토리 창
        if (Input.GetKeyDown(KeyCode.I))
        {
            _inventoryUI.OpenInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _inventoryUI.gameObject.activeSelf)
        {
            _inventoryUI.CloseInventory();
        }
        
        // 퀘스트 창
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _QuestListUI.OpenList();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _questListUI.gameObject.activeSelf)
        {
            _questListUI.CloseList();
        }

        // Map
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    _worldMapUI.OpenMap();
        //}
        //else if (Input.GetKeyDown(KeyCode.Escape) && _worldMapUI.gameObject.activeSelf)
        //{
        //    _worldMapUI.CloseMap();
        //}

        // Quest Navigation
        if (Input.GetKeyDown(KeyCode.F) && !_questNav.gameObject.activeSelf)
        {
            _questNav.OnNav();
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            _questNav.OffNav();
        }
    }

    public static bool IsTalking()
    {
        return _instance._dialogUI.gameObject.activeSelf;
    }

    public static void CursorVisible(bool visible)
    {
        if (_Instance._questUI.gameObject.activeSelf) { return; }
        _instance._cursorManager.CursorVisible(visible);
    }
}
