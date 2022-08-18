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

    [SerializeField] private ObjectInteraction _objInter;
    [SerializeField] private QButtonPool _qButtonPool;
    [SerializeField] private DialogUI _dialogUI;
    [SerializeField] private QuestUI _questUI;
    [SerializeField] private QuestListUI _questListUI;
    [SerializeField] private InteractionUI _interUI;
    [SerializeField] private NPCMarkerUI _npcMarkerUI;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private ConsumableSlot _consumableSlot;

    public ObjectInteraction _ObjInter => _objInter;
    public QButtonPool _QButtonPool => _qButtonPool;
    public DialogUI _DialogUI => _dialogUI;
    public QuestUI _QuestUI => _questUI;
    public QuestListUI _QuestListUI => _questListUI;
    public InteractionUI _InterUI => _interUI;
    public NPCMarkerUI _NPCMarkerUI => _npcMarkerUI;
    public InventoryUI _InventoryUI => _inventoryUI;
    public ConsumableSlot _ConsumableSlot => _consumableSlot;


    private void Awake()
    {
        _instance = this;

        _objInter = FindObjectOfType<ObjectInteraction>();
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
