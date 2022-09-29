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

    private List<PopupUIBase> _popupUI = new List<PopupUIBase>();


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
    private void ControlUI()
    {
        // 인벤토리 창
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenOrClose(_inventoryUI);
        }

        // 퀘스트 창
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OpenOrClose(_questListUI);
        }

        // 월드 맵
        if (Input.GetKeyDown(KeyCode.M))
        {
            OpenOrClose(_worldMapUI);
        }

        // esc 기능 - 가장 마지막에 킨 UI부터 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            int lastIdx = _popupUI.Count - 1;   // 가장 최근에 Open한 UI 인덱스
            if (lastIdx < 0) { return; }        // 예외처리 - 열려있는 UI가 없을 때

            PopupUIBase lastUI = _popupUI[lastIdx];
            lastUI.CloseControllableUI();
            SetOrder(lastUI, false);
            _popupUI.RemoveAt(lastIdx);
        }

        // 퀘스트 네비게이션
        if (Input.GetKeyDown(KeyCode.F) && !_questNav.gameObject.activeSelf)
        {
            _questNav.OnNav();
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            _questNav.OffNav();
        }
    }

    // PopupUI - 열었을 때 단축키로 다시 닫기 기능
    private void OpenOrClose(PopupUIBase popupUI)
    {
        if (popupUI.gameObject.activeSelf)  // 닫기
        {
            _popupUI.Remove(popupUI);
            SetOrder(popupUI, false);
            popupUI.CloseControllableUI();
        }
        else   // 열기
        {
            _popupUI.Add(popupUI);
            SetOrder(popupUI, true);
            popupUI.OpenControllableUI();
        }
    }

    // PopupUI 순서
    private void SetOrder(PopupUIBase popupUI, bool active)
    {
        if (active)
        {
            // 켜져있는 모든 UI에대해 재정렬
            for (int i = 0; i < _popupUI.Count; i++)
            {
                _popupUI[i]._canvas.sortingOrder = _popupUI.IndexOf(_popupUI[i]) + 1;
            }
        }
        else { popupUI._canvas.sortingOrder = 0; } 
    }

    public static bool IsTalking()
    {
        return _instance._dialogUI.gameObject.activeSelf;
    }

    public static void CursorVisible(bool visible)
    {
        if (_instance._questUI.gameObject.activeSelf) { return; }
        _instance._cursorManager.CursorVisible(visible);
    }
}
