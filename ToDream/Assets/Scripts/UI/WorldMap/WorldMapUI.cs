using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapUI : MonoBehaviour
{
    [Header("[Player]")]
    [SerializeField] private Transform _player;
    [SerializeField] private Image _playerIcon;

    [Header("[QuestTarget]")]
    [SerializeField] private GameObject _targetParent;
    [SerializeField] private QuestTargetMarker[] _questTargets;     // _targetImage�� ���� ���� ��
    [SerializeField] private Image[] _targetImage;                  // _questTargets�� ���� ���� ��
    [SerializeField] private Sprite _questIcon;

    [Header("[NPC]")]
    [SerializeField] private Sprite _default;
    [SerializeField] private Sprite _avaliable;
    [SerializeField] private Sprite _inProgress;
    [SerializeField] private Sprite _completed;

    [Header("[Ping]")]
    [SerializeField] private Image _pingImage;
    [SerializeField] private Ping _ping;
    [SerializeField] private bool _pointerOnPing;

    [Header("[Others]")]
    [SerializeField] private Text _iconName;                            // Icon �̸�ǥ
    [SerializeField] private Vector2 _offsetPos = new Vector2(0, 30);   // Text offset Pos

    private const float _fallHeight = 0f;             // Ping Object ���� ����
    private const int _realScale = 3;                 // UI ��� �� ũ�� ����
    private const float _scaleFactor = 0.334f;        // ��ô��
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

    private void UpdatePlayerPos()
    {
        _playerIcon.rectTransform.anchoredPosition = new Vector2(_player.position.x * _scaleFactor, _player.position.z * _scaleFactor);
    }

    private void TryOnPing()
    {
        // ��Ŭ�� && ���� ������ (1000x1000) ���� �ȿ� Ŭ���� ���
        if (Input.GetMouseButtonUp(1)
            && Input.mousePosition.x >= 460 && Input.mousePosition.x <= 1460 
            && Input.mousePosition.y >= 40 && Input.mousePosition.y <= 1040)
        {          
            if (_pingImage.gameObject.activeSelf && _pointerOnPing)
            {   // ���� Ping�� �ְ� ���콺 ��ġ�� Ping�� ���� ���
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

        // Ping ���� ������� ��ġ����
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

    // ���� ų�� ���� <-> ���� �ߴ�
    private IEnumerator CUpdateMap()
    {
        while (true)
        {
            UpdatePlayerPos();          // PlayerIcon ��ġ ������Ʈ
            TryOnPing();                // Ping ��� Ȱ��ȭ
            yield return null;
        }
    }

    // ����Ʈ�� ��ǥ�� Ȱ��ȭ �Ǿ��ִٸ� �ʿ� ǥ��
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

    // �� NPC�� ���� ����Ʈ�� ���¿� ���� ������ �̹��� ��ȭ
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

    // 1. �� ������ �̸�ǥ ��� (PointerEnter��)
    // 2. Ping �̹����� ���콺 ������ ����
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
        _pointerOnPing = false;
        OffText();
    }
}