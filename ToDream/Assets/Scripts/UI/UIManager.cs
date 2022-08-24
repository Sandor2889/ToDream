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
    private DialogUI _dialogUI;
    private NPCMarkerUI _npcMarkerUI;
    private InteractionUI _interUI;
    private InventoryUI _inventoryUI;
    private ConsumableSlot _consumableSlot;

    public QuestUI _QuestUI => _questUI;
    public QuestListUI _QuestListUI => _questListUI;
    public QButtonPool _QButtonPool => _qButtonPool;
    public DialogUI _DialogUI => _dialogUI;
    public NPCMarkerUI _NPCMarkerUI => _npcMarkerUI;
    public InteractionUI _InterUI => _interUI;
    public InventoryUI _InventoryUI => _inventoryUI;
    public ConsumableSlot _ConsumableSlot => _consumableSlot;


    private void Awake()
    {
        _instance = this;

        _questUI = FindObjectOfType<QuestUI>(true);
        _questListUI = FindObjectOfType<QuestListUI>(true);
        _qButtonPool = FindObjectOfType<QButtonPool>(true);
        _dialogUI = FindObjectOfType<DialogUI>(true);
        _npcMarkerUI = FindObjectOfType<NPCMarkerUI>(true);
        _interUI = FindObjectOfType<InteractionUI>(true);
        _inventoryUI = FindObjectOfType<InventoryUI>(true);
        _consumableSlot = FindObjectOfType<ConsumableSlot>(true);
    }

    private void Update()
    {
        // 소모품 사용 테스트
        if (Input.GetKeyDown(KeyCode.F5))
        {
            _consumableSlot.UpdateItem(_consumableSlot._item, -1);
        }

        ControllQuestList();
        ControllInventory();
    }

    // 퀘스트 창 열고 닫기
    public void ControllQuestList()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _QuestListUI.OpenList();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _questListUI.CloseList();
        }
    }

    // 인벤토리 창 열고 닫기

    public void ControllInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _inventoryUI.OpenInventory();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _inventoryUI.CloseInventory();
        }
    }
}
