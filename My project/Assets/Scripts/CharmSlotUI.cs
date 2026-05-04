using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharmSlotUI : MonoBehaviour
{
    [Header("Slot Purchase UI")]
    [SerializeField] private TMP_Text slotsAvailableText;
    [SerializeField] private TMP_Text unlockSlotButtonText;
    [SerializeField] private Button unlockSlotButton;

    [Header("Active Charm Slot Texts")]
    [SerializeField] private TMP_Text charmSlot1Text;
    [SerializeField] private TMP_Text charmSlot2Text;
    [SerializeField] private TMP_Text charmSlot3Text;

    [SerializeField] private PulseButtons pulseButton;

    private void Start()
    {
        unlockSlotButton.onClick.AddListener(UnlockSlot);
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void UnlockSlot()
    {
        CharmManager.Instance.UnlockCharmSlot();
        UpdateUI();
    }

    private void UpdateUI()
    {
        bool canAffordSlot =
    CurrencyManager.Instance.CanAfford(CharmManager.Instance.SlotUnlockCost);

        slotsAvailableText.text =
            "Charm Slots\nAvailable\n" +
            CharmManager.Instance.UnlockedSlots + " / " +
            CharmManager.Instance.MaxSlots;

        unlockSlotButtonText.text =
      "Unlock Slot\n" +
      NumberFormatter.FormatMoney(CharmManager.Instance.SlotUnlockCost);

        unlockSlotButton.interactable =
            CharmManager.Instance.UnlockedSlots < CharmManager.Instance.MaxSlots &&
            CurrencyManager.Instance.CanAfford(CharmManager.Instance.SlotUnlockCost);

        UpdateSlotText(charmSlot1Text, 0);
        UpdateSlotText(charmSlot2Text, 1);
        UpdateSlotText(charmSlot3Text, 2);

        unlockSlotButton.image.color =
    CharmManager.Instance.UnlockedSlots < CharmManager.Instance.MaxSlots && canAffordSlot
        ? new Color(0.6f, 1f, 0.6f)
        : new Color(0.6f, 0.6f, 0.6f);

        pulseButton.SetPulse(
    CharmManager.Instance.UnlockedSlots < CharmManager.Instance.MaxSlots && canAffordSlot
);
    }

    private void UpdateSlotText(TMP_Text slotText, int index)
    {
        if (index >= CharmManager.Instance.UnlockedSlots)
        {
            slotText.text = "Locked";
            return;
        }

        if (index < CharmManager.Instance.ActiveCharms.Count)
        {
            slotText.text = CharmManager.Instance.ActiveCharms[index].CharmName;
        }
        else
        {
            slotText.text = "Empty Slot";
        }
    }
}
