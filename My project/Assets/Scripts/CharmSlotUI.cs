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

    [Header("Tutorial")]
    [SerializeField] private bool canTriggerCharmSlotTutorial = false;
    [SerializeField] private RectTransform charmSlotButtonTutorialTarget;
    [SerializeField] private CharmTutorialController charmTutorialController;

    private bool hasShownCharmSlotTutorial = false;

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

        if (charmTutorialController != null)
        {
            charmTutorialController.OnCharmSlotBought();
        }
    }

    public void TryShowCharmSlotTutorial()
    {
        if (TutorialManager.Instance == null) return;
        if (!TutorialManager.Instance.HasCompletedOpenCharmTab) return;
        if (!canTriggerCharmSlotTutorial) return;
        if (hasShownCharmSlotTutorial) return;
        if (CharmManager.Instance.UnlockedSlots > 0) return;
        if (!CurrencyManager.Instance.CanAfford(CharmManager.Instance.SlotUnlockCost)) return;

        hasShownCharmSlotTutorial = true;

        TutorialManager.Instance.ShowTutorial(
            "Buy a charm slot first. Charms only work when placed in an active slot.",
            charmSlotButtonTutorialTarget,
            TutorialAction.UnlockCharmSlot
        );
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
