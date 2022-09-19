using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WorldMapUI : MonoBehaviour
{
    [Header("[Image Prefab]")]
    [SerializeField] private GameObject _ImageObj;
    [SerializeField] private Transform _Instparent;

    [Header("[Player]")]
    [SerializeField] private Transform _player;
    [SerializeField] private Image _playerIcon;

    [Header("[QuestTarget]")]
    [SerializeField] private GameObject _targetParent;
    [SerializeField] private QuestTargetMarker[] _questTargets;
    [SerializeField] private Sprite _questIcon;

    [Header("[NPC]")]
    [SerializeField] private Sprite _default;
    [SerializeField] private Sprite _avaliable;
    [SerializeField] private Sprite _accepted;
    [SerializeField] private Sprite _completed;

    private float _scaleFactor = 0.334f;
    private Coroutine _coroutinMap;

    private void Awake()
    {
        _player = Camera.main.transform;
        _playerIcon.sprite = Resources.Load<Sprite>("WorldMapIcon/PlayerIcon");
        _ImageObj = Resources.Load<GameObject>("WorldMapIcon/ImageObject");

        _questTargets = _targetParent.GetComponentsInChildren<QuestTargetMarker>(true);
        CreateQuestIcons(_questTargets.Length);
    }

    private void OnEnable()
    {
        _coroutinMap = StartCoroutine(CUpdateMap());

        // Quest target update
        QuestTargetUpdate();
        // NPC update
        NPCUpdate();
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutinMap);
    }

    private void PlayerUpdate()
    {
        _playerIcon.rectTransform.anchoredPosition = new Vector2(_player.position.x * _scaleFactor, _player.position.z * _scaleFactor);
    }

    private void QuestTargetUpdate()
    {
        if (QuestManager.CheckHasQuest())
        {
            
        }
    }

    private void NPCUpdate()
    {
        NPCMarkerUI npcMarkerUI = UIManager._Instance._NPCMarkerUI;

        for (int i = 0; i < npcMarkerUI._Npcs.Length; i++)
        {
            if (npcMarkerUI._Npcs[i]._CurrentQuest._questState == QuestState.Avaliable)
            {
                npcMarkerUI._Npcs[i]._myImage.sprite = _avaliable;
            }
            else if (npcMarkerUI._Npcs[i]._CurrentQuest._questState == QuestState.Accepted)
            {
                npcMarkerUI._Npcs[i]._myImage.sprite = _accepted;
            }
            else if (npcMarkerUI._Npcs[i]._CurrentQuest._questState == QuestState.Completed)
            {
                npcMarkerUI._Npcs[i]._myImage.sprite = _completed;
            }
            else
            {
                npcMarkerUI._Npcs[i]._myImage.sprite = _default;
            }
        }
    }


    private void CreateQuestIcons(int size)
    {
        for(int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(_ImageObj, _Instparent);
            obj.GetComponent<Image>().sprite = _questIcon;
        }
    }

    private void NPCIcons(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(_ImageObj, _Instparent);
            obj.GetComponent<Image>().sprite = _default;
        }
    }

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
    private IEnumerator CUpdateMap()
    {
        while(true)
        {
            // Player update
            PlayerUpdate();
            yield return null;
        }
    }
}