using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapUI : PopupUIBase
{
    [Header("[Player]")]
    [SerializeField] private Transform _playerPos;
    [SerializeField] private Image _playerIcon;

    [Header("[QuestTarget]")]
    [SerializeField] private GameObject _targetParent;
    [SerializeField] private QuestTargetMarker[] _questTargets; // _targetImage와 순서 맞출 것
    [SerializeField] private Image[] _targetImage;              // _questTargets와 순서 맞출 것
    [SerializeField] private Sprite _questIcon;

    [Header("[NPC]")]
    [SerializeField] private Sprite _default;
    [SerializeField] private Sprite _avaliable;
    [SerializeField] private Sprite _inProgress;
    [SerializeField] private Sprite _completed;

    [Header("[Ping]")]
    [SerializeField] private Image _pingImage;
    [SerializeField] private Ping _ping;
    [SerializeField] private bool _pointerOnPing;   // Ping위에 커서가 있는지 체크

    [Header("[NameText]")]
    [SerializeField] private Text _iconName;                           // Icon 이름표
    [SerializeField] private Vector2 _offsetPos = new Vector2(0, 30);  // Text offset Pos

    private const float _fallHeight = 0f;             // Ping Object 생성 높이
    private const int _realScale = 3;                 // UI 대비 맵 크기 비율
    private const float _scaleFactor = 0.334f;        // 축척비             

    private void Awake()
    {
        _playerPos = Camera.main.transform;
        _playerIcon.sprite = Resources.Load<Sprite>("WorldMapIcon/PlayerIcon");
        _default = Resources.Load<Sprite>("worldMapIcon/Default");
        _avaliable = Resources.Load<Sprite>("worldMapIcon/Avaliable");
        _inProgress = Resources.Load<Sprite>("worldMapIcon/InProgress");
        _completed = Resources.Load<Sprite>("worldMapIcon/Completed");
        _pingImage.sprite = Resources.Load<Sprite>("WorldMapIcon/Ping");
        _questTargets = _targetParent.GetComponentsInChildren<QuestTargetMarker>(true);
    }

    private void OnEnable()
    {
        StartCoroutine(C_UpdateMap());  // Disable시 자동 중단

        QuestTargetUpdate();     // Quest target update
        NPCUpdate();             // NPC update
    }

    private void UpdatePlayerPos()
    {
        _playerIcon.rectTransform.anchoredPosition = new Vector2(
            _playerPos.position.x * _scaleFactor, _playerPos.position.z * _scaleFactor);
    }

    private void TryOnPing()
    {
        // 우클릭 && UI 맵의 사이즈 (1000x1000) 범위 안에 클릭할 경우
        if (Input.GetMouseButtonUp(1)
            && Input.mousePosition.x >= 460 && Input.mousePosition.x <= 1460 
            && Input.mousePosition.y >= 40 && Input.mousePosition.y <= 1040)
        {          
            if (_pingImage.gameObject.activeSelf && _pointerOnPing)
            {   // 현재 Ping이 있고 마우스 위치가 Ping과 같은 경우
                RemovePing();
            }
            else
            {
                SetPingPos();
            }  
        }
    }

    private void SetPingPos()
    {
        _ping.gameObject.SetActive(true);
        _pingImage.gameObject.SetActive(true);
        _pingImage.rectTransform.position = Input.mousePosition;

        // Ping 실제 월드공간 위치지정
        float posX = _pingImage.rectTransform.anchoredPosition.x;
        float posZ = _pingImage.rectTransform.anchoredPosition.y;
        _ping.transform.position = new Vector3(posX * _realScale, _fallHeight, posZ * _realScale);
    }

    public void RemovePing()
    {
        _pointerOnPing = false;
        _ping.gameObject.SetActive(false);
        _pingImage.gameObject.SetActive(false);
        _iconName.gameObject.SetActive(false);
    }

    // 맵을 킬때 동작 <-> 끄면 중단
    private IEnumerator C_UpdateMap()
    {
        while (true)
        {
            UpdatePlayerPos();          // PlayerIcon 위치 업데이트
            TryOnPing();                // Ping 기능 활성화
            yield return null;
        }
    }

    // 퀘스트의 목표가 활성화 되어있다면 맵에 표시
    public void QuestTargetUpdate()
    {
        for(int i = 0; i < _questTargets.Length; i++)
        {
            if (_questTargets[i].gameObject.activeSelf)
            {
                _targetImage[i].gameObject.SetActive(true);
            }
            else
            {
                _targetImage[i].gameObject.SetActive(false);
            }
        }
    }

    private void SetNpcSprite(Image npc, Sprite state)
    {
        npc.sprite = state;
    }

    // 각 NPC가 가진 퀘스트의 상태에 따라 아이콘 이미지 변화
    public void NPCUpdate()
    {
        NPCMarkerUI npcMarkerUI = UIManager._Instance._NPCMarkerUI;

        for (int i = 0; i < npcMarkerUI._Npcs.Length; i++)
        {
            // 퀘스트가 없는 경우 또는 수락 불가상태 - 기본 아이콘
            if (npcMarkerUI._Npcs[i]._CurrentQuest == null || 
                npcMarkerUI._Npcs[i]._CurrentQuest._questState == QuestState.Unvaliable) 
            {
                SetNpcSprite(npcMarkerUI._Npcs[i]._myImage, _default);
                continue;
            }

            switch(npcMarkerUI._Npcs[i]._CurrentQuest._questState)
            {
                case QuestState.Avaliable:  // 퀘스트 수락 가능 상태
                    SetNpcSprite(npcMarkerUI._Npcs[i]._myImage, _avaliable);
                    break;
                case QuestState.Accepted:   // 퀘스트 진행중
                    SetNpcSprite(npcMarkerUI._Npcs[i]._myImage, _inProgress);
                    break;
                case QuestState.Completed:  // 퀘스트 완료
                    SetNpcSprite(npcMarkerUI._Npcs[i]._myImage, _completed);
                    break;
                default:                    
                    Debug.LogWarning(npcMarkerUI._Npcs[i]._npcName + " has wrong state");
                    break;
            }
        }
    }

    // 1. 각 아이콘 이름표 출력 (PointerEnter시)
    // 2. Ping 이미지위 마우스 포인터 감지
    #region Event Trigger
    public void OnText(IconName iconName)
    {
        _iconName.gameObject.SetActive(true);
        _iconName.rectTransform.anchoredPosition = iconName.GetComponent<RectTransform>().anchoredPosition + _offsetPos;
        _iconName.text = iconName._name;
    }

    public void OffText()
    {
        _iconName.gameObject.SetActive(false);
        _iconName.text = " ";
    }

    public void TryRemovePing()
    {
        _pointerOnPing = true;
    }

    public void CancelRemovePing()
    {
        _pointerOnPing = false;
    }
    #endregion

    public override void OpenControllableUI()
    {
        UIManager.CursorVisible(true);
        UIManager._isOpendUI = true;
        gameObject.SetActive(true);
    }

    public override void CloseControllableUI()
    {
        UIManager.CursorVisible(false);
        UIManager._isOpendUI = false;
        gameObject.SetActive(false);
        _pointerOnPing = false;
        OffText();
    }
}