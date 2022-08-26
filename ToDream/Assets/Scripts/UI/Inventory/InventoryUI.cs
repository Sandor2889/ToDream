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

    public List<Item> _items = new List<Item>();    // ������ ������ ����Ʈ
    public Slot[] _slots;

    private void Awake()
    {
        UpdateGold(0);
        _slots = GetComponentsInChildren<Slot>();
    }

    // ��� ������Ʈ
    public void UpdateGold(int gold)
    {
        if (_gold + gold < 0 && gold < 0)
        {
            Debug.Log("������尡 �����մϴ�.");
            return;
        }

        _gold += gold;
        _goldText.text = _gold.ToString();
    }

    // ������ Ż�� ��� ����
    public void UpdateRegisteredSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].SetRegisteredText(VehicleWheelController._currentVehicles[(int)_category + 1]);
        }
    }

    // ������ ����
    public void AcquireItem(Item item)
    {
        // Item�� �Ҹ�ǰ�̸� ConsumableSlot�� ���
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

    // ī�װ��� ���� ����
    public void SortByCategory(Category category)
    {
        ClearSlotData();
        PressedCategory(category);

        List<Item> copyList = _items.ToList<Item>();   // Item List �����Ͽ� Ȱ��
        
        for (int i = 0; i < _slots.Length; i++)
        {
            Item item = copyList.Find(x => x._category == _category);

            if (item == null) { break; }

            _slots[i].SetItem(item._key);
            
            copyList.Remove(item);
        }

        UpdateRegisteredSlot();
    }

    // ī�װ� ��ư �� ���� (Color �� ����)
    private void SetButtonColor(Button button, float r, float g, float b)
    {
        Color color = new Color(r / 255f, g / 255f, b / 255f);
        button.GetComponent<Image>().color = color;
    }

    // ī�װ� ��ư �� ���� (��ư ���� ��)
    private void PressedCategory(Category category)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            // Hierarchy�� �� ī�װ� ��ư �� ��Ȯ�� ���� ��
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

    // ���� ī�װ� ��ȯ
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
