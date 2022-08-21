using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum Category
{
    Car,
    Boat,
    Air,
    Consumable
}

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Text _goldText;
    private int _gold;

    [HideInInspector] public Category _category;
    public List<Item> _items = new List<Item>();    // 습득한 아이템 리스트
    public Slot[] _slots;

    private void Awake()
    {
        UpdateGold(0);
        _slots = GetComponentsInChildren<Slot>();
    }

    public void UpdateGold(int gold)
    {
        _gold += gold;
        _goldText.text = _gold.ToString();
    }

    // 아이템 습득
    public void AcquireItem(Item item)
    {
        // Item이 소모품이면 ConsumableSlot에 등록
        if (item._category == Category.Consumable)
        {
            UIManager._Instance._ConsumableSlot.UpdateItem(item, 1);
        }
        else
        {
            _items.Add(item);
        }
    }

    // 카테고리에 따른 정렬
    public void SortByCategory(Category category)
    {
        ClearSlotData();
        PressedCategory(category);

        List<Item> copyList = _items.ToList<Item>();   // Item List 복제하여 활용

        for (int i = 0; i < _slots.Length; i++)
        {
            Item item = copyList.Find(x => x._category == _category);

            if (item == null) { return; }

            _slots[i].SetItem(item);
            copyList.Remove(item);
        }
    }

    // 카테고리 버튼 색 변경
    private void SetButtonColor(Button button, float r, float g, float b)
    {
        Color color = new Color(r / 255f, g / 255f, b / 255f);
        button.GetComponent<Image>().color = color;
    }

    // 카테고리 버튼 눌렀을때
    private void PressedCategory(Category category)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            // Hierarchy의 각 카테고리 버튼 명 정확히 맞출 것
            if (_buttons[i].name.GetHashCode() == category.ToString().GetHashCode())
            {
                SetButtonColor(_buttons[i], 255f, 141f, 255f);
            }
            else
            {
                SetButtonColor(_buttons[i], 255f, 255f, 255f);
            }
        }
    }

    // 현재 카테고리 변환
    #region Change Category
    public void Car()
    {
        _category = Category.Car;
        SortByCategory(_category);
    }

    public void Boat()
    {
        _category = Category.Boat;
        SortByCategory(_category);
    }

    public void Air()
    {
        _category = Category.Air;
        SortByCategory(_category);
    }
    #endregion


    public void ClearSlotData()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].Clear();
        }
    }

    public void OpenInventory()
    {
        gameObject.SetActive(true);
        SortByCategory(_category);
    }

    public void CloseInventory()
    {
        gameObject.SetActive(false);
    }
}
