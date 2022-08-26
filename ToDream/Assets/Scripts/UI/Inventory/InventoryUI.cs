using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum Category
{
    Car,
    Air,
    Boat,
    Consumable
}

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private VehicleRegistration _vehicleResgistration;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Text _goldText;
    private Category _category;
    private int _gold;

    public List<Item> _items = new List<Item>();    // 습득한 아이템 리스트
    public Slot[] _slots;

    private void Awake()
    {
        UpdateGold(0);
        _slots = GetComponentsInChildren<Slot>();
    }

    // 골드 업데이트
    public void UpdateGold(int gold)
    {
        if (_gold + gold < 0 && gold < 0)
        {
            Debug.Log("소지골드가 부족합니다.");
            return;
        }

        _gold += gold;
        _goldText.text = _gold.ToString();
    }

    // 슬롯의 탈것 등록 상태
    public void UpdateRegisteredSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].SetRegisteredText(VehicleWheelController._currentVehicles[(int)_category + 1]);
        }
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

        SortByCategory(_category);
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

            if (item == null) { break; }

            _slots[i].SetItem(item._key);
            
            copyList.Remove(item);
        }

        UpdateRegisteredSlot();
    }

    // 카테고리 버튼 색 변경 (Color 값 조절)
    private void SetButtonColor(Button button, float r, float g, float b)
    {
        Color color = new Color(r / 255f, g / 255f, b / 255f);
        button.GetComponent<Image>().color = color;
    }

    // 카테고리 버튼 색 변경 (버튼 누를 때)
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
        _vehicleResgistration.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
