using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldMapUI : MonoBehaviour
{
    [Header("[Player]")]
    [SerializeField] private Transform _player;
    [SerializeField] private Image _playerIcon;

    [Header("[QuestTarget]")]
    [SerializeField] private GameObject _targetParent;
    [SerializeField] private QuestTargetMarker[] _questTargets;     // _targetImage와 순서 맞출 것
    [SerializeField] private Image[] _targetImage;                  // _questTargets와 순서 맞출 것
    [SerializeField] private Sprite _questIcon;

    [Header("[NPC]")]
    [SerializeField] private Sprite _default;
    [SerializeField] private Sprite _avaliable;
    [SerializeField] private Sprite _inProgress;
    [SerializeField] private Sprite _completed;

    [Header("[Ping]")]
    [SerializeField] private Image _pingImage;
    [SerializeField] private Ping _ping;

    [Header("[Others]")]
    [SerializeField] private Text _iconName;                            // Icon 이름표
    [SerializeField] private Vector2 _offsetPos = new Vector2(0, 30);   // Text offset Pos
    private float _fallHeight = 0f;
    private int _realScale = 3;

    private float _scaleFactor = 0.334f;        // 축척비
    private Coroutine _coroutinMap;             

    private void Awake()
    {
        _player = Camera.main.transform;
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
        _coroutinMap = StartCoroutine(CUpdateMap());

        QuestTargetUpdate();     // Quest target update
        NPCUpdate();             // NPC update
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutinMap);
    }

    private void SetPlayerPos()
    {
        _playerIcon.rectTransform.anchoredPosition = new Vector2(_player.position.x * _scaleFactor, _player.position.z * _scaleFactor);
    }

    private void TryOnPing()
    {
        // 우클릭 && 맵의 사이즈 (1000x1000) 범위 안에 클릭할 경우
        if (Input.GetMouseButtonUp(1)
            && Input.mousePosition.x >= 460 && Input.mousePosition.x <= 1460 
            && Input.mousePosition.y >= 40 && Input.mousePosition.y <= 1040)
        {
            _pingImage.gameObject.SetActive(true);
            _pingImage.rectTransform.position = Input.mousePosition;
            _ping.gameObject.SetActive(true);
            
            // Ping 실제 위치
            float posX = _pingImage.rectTransform.anchoredPosition.x;
            float posZ = _pingImage.rectTransform.anchoredPosition.y;
            _ping.transform.position = new Vector3(posX * _realScale, _fallHeight, posZ * _realScale);
        }
    }

    private IEnumerator CUpdateMap()
    {
        while (true)
        {
            SetPlayerPos();
            TryOnPing();
            yield return null;
        }
    }

    public void RemovePing()
    {
        _pingImage.gameObject.SetActive(false);
        _iconName.gameObject.SetActive(false);
        _ping.gameObject.SetActive(false);
    }

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

    public void NPCUpdate()
    {
        NPCMarkerUI npcMarkerUI = UIManager._Instance._NPCMarkerUI;

        for (int i = 0; i < npcMarkerUI._Npcs.Length; i++)
        {
            if (npcMarkerUI._Npcs[i]._CurrentQuest == null || npcMarkerUI._Npcs[i]._CurrentQuest._questState == QuestState.Unvaliable)
            {
                npcMarkerUI._Npcs[i]._myImage.sprite = _default;
            }
            else if (npcMarkerUI._Npcs[i]._CurrentQuest._questState == QuestState.Avaliable)
            {
                npcMarkerUI._Npcs[i]._myImage.sprite = _avaliable;
            }
            else if (npcMarkerUI._Npcs[i]._CurrentQuest._questState == QuestState.Accepted)
            {
                npcMarkerUI._Npcs[i]._myImage.sprite = _inProgress;
            }
            else if (npcMarkerUI._Npcs[i]._CurrentQuest._questState == QuestState.Completed)
            {
                npcMarkerUI._Npcs[i]._myImage.sprite = _completed;
            }
        }
    }

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
    #endregion

    public void OpenMap()
    {
        UIManager.CursorVisible(true);
        UIManager._isOpendUI = true;
        gameObject.SetActive(true);
    }

    public void CloseMap()
    {
        UIManager.CursorVisible(false);
        UIManager._isOpendUI = false;
        gameObject.SetActive(false);
    }
}