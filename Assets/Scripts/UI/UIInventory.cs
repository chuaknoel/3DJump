using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;      // item ���� �� �ʿ��� ��ġ

    [Header("Selected Item")]           // ������ ������ ������ ���� ǥ�� ���� UI
    public ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private int curEquipIndex;

    private PlayerController controller;
    private PlayerCondition condition;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // Action ȣ�� �� �ʿ��� �Լ� ���
        controller.inventory += Toggle;      // inventory Ű �Է� ��
        CharacterManager.Instance.Player.addItem += AddItem;  // ������ �Ĺ� ��

        // Inventory UI �ʱ�ȭ ������
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        ClearSelectedItemWindow();
    }

    // ������ ������ ǥ���� ����â Clear �Լ�
    void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // Inventory â Open/Close �� ȣ��
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }


    public void AddItem()
    {
        // 10�� ItemObject �������� Player�� �Ѱ��� ������ ������ ��
        ItemData data = CharacterManager.Instance.Player.itemData;


        if (data == null)
        {
            Debug.LogWarning("[UIInventory] ������ �����Ͱ� null�Դϴ�!");
            return;
        }

        Debug.Log($"[UIInventory] {data.displayName} �߰� �õ�");

        // ������ ���� �� �ִ� �������̶��
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        // �� ���� ã��
        ItemSlot emptySlot = GetEmptySlot();

        // �� ������ �ִٸ�
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        // �� ���� ���� ���� ��
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    // UI ���� ���ΰ�ħ
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // ���Կ� ������ ������ �ִٸ�
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    // ������ ���� �� �ִ� �������� ���� ã�Ƽ� return
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    // ������ item ������ ����ִ� ���� return
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    // Player ��ũ��Ʈ ���� ����
    // ������ ������ (������ �Ű������� ���� �����Ϳ� �ش��ϴ� ������ ����)
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropprefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }


    // ItemSlot ��ũ��Ʈ ���� ����
    // ������ ������ ����â�� ������Ʈ ���ִ� �Լ�
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        // 1. ���� ���� ������ outline ��Ȱ��ȭ
        if (selectedItem != null)
        {
            selectedItem.equipped = false;
            selectedItem.GetComponent<Outline>().enabled = false;
        }

        // 2. �� ���� ���� �Ҵ�
        selectedItem = slots[index];
        selectedItemIndex = index;

        // 3. �� ���� ���Կ� outline Ȱ��ȭ
        selectedItem.equipped = true;
        selectedItem.GetComponent<Outline>().enabled = true;

        // 4. ���� �ؽ�Ʈ ����
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.item.consumable.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumable[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumable[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.itemType == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.itemType == ItemType.Equipable && !selectedItem.equipped);
        unEquipButton.SetActive(selectedItem.item.itemType == ItemType.Equipable && selectedItem.equipped);
        dropButton.SetActive(true);
    }


    public void OnUseButton()
    {
        if (selectedItem.item.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.item.consumable.Length; i++)
            {
                switch (selectedItem.item.consumable[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.item.consumable[i].value); break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.item.consumable[i].value); break;
                }
            }
            RemoveSelctedItem();
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelctedItem();
    }

    void RemoveSelctedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

            UpdateUI();
    }

    // Added missing UnEquip method
    void UnEquip(int index)
    {
        slots[index].equipped = false;
        // Additional logic for unequipping can be added here
    }

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }
}
