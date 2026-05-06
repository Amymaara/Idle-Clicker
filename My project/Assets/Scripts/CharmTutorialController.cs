using UnityEngine;

public class CharmTutorialController : MonoBehaviour
{
    [SerializeField] private RectTransform charmsTabTarget;
    [SerializeField] private CharmSlotUI charmSlotUI;
    [SerializeField] private CharmCard tutorialCharmCard;

    private bool hasShownOpenCharmTabTutorial = false;
    private bool hasShownSlotTutorial = false;
    private bool hasShownBuyCharmTutorial = false;

    public void TryShowOpenCharmTabTutorial()
    {
        if (hasShownOpenCharmTabTutorial) return;
        if (!CurrencyManager.Instance.CanAfford(CharmManager.Instance.SlotUnlockCost)) return;

        hasShownOpenCharmTabTutorial = true;

        TutorialManager.Instance.ShowTutorial(
            "Open the Charms tab to unlock charm slots.",
            charmsTabTarget,
            TutorialAction.OpenCharmTab
        );
    }

    public void OnCharmsTabOpened()
    {
        TutorialManager.Instance.TryCompleteTutorial(TutorialAction.OpenCharmTab);
        TutorialManager.Instance.CompleteOpenCharmTab();

        if (!hasShownSlotTutorial && charmSlotUI != null)
        {
            hasShownSlotTutorial = true;
            charmSlotUI.TryShowCharmSlotTutorial();
        }
    }

    public void OnCharmSlotBought()
    {
        TutorialManager.Instance.TryCompleteTutorial(TutorialAction.UnlockCharmSlot);

        if (!hasShownBuyCharmTutorial && tutorialCharmCard != null)
        {
            hasShownBuyCharmTutorial = true;
            tutorialCharmCard.ShowBuyCharmTutorial();
        }
    }
}
