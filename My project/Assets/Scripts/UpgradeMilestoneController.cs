using UnityEngine;

public class UpgradeMilestoneController : MonoBehaviour
{
    [Header("Tutorial Targets")]
    [SerializeField] private RectTransform upgradeTabTarget;

    [Header("Milestone Upgrades")]
    [SerializeField] private PotionMilestonUpgrade[] milestoneUpgrades;

    private bool hasShownOpenUpgradePanelTutorial = false;
    private bool hasShownBuyMilestoneTutorial = false;

    public void TryShowOpenUpgradePanelTutorial()
    {
        if (hasShownOpenUpgradePanelTutorial) return;

        PotionMilestonUpgrade readyUpgrade = GetFirstReadyUpgrade();
        if (readyUpgrade == null) return;

        hasShownOpenUpgradePanelTutorial = true;

        TutorialManager.Instance.ShowTutorial(
            "You unlocked a milestone upgrade! Open the Upgrade tab to view it.",
            upgradeTabTarget,
            TutorialAction.OpenUpgradePanel
        );
    }

    public void OnUpgradePanelOpened()
    {
        TutorialManager.Instance.TryCompleteTutorial(TutorialAction.OpenUpgradePanel);

        if (hasShownBuyMilestoneTutorial) return;

        PotionMilestonUpgrade readyUpgrade = GetFirstReadyUpgrade();
        if (readyUpgrade == null) return;

        hasShownBuyMilestoneTutorial = true;

        TutorialManager.Instance.ShowTutorial(
            "Milestone upgrades become available when a potion reaches certain levels. Buy this upgrade to increase how many potions are made per round.",
            readyUpgrade.UpgradeButtonTarget,
            TutorialAction.BuyMilestoneUpgrade
        );
    }

    public void OnMilestoneUpgradeBought()
    {
        TutorialManager.Instance.TryCompleteTutorial(TutorialAction.BuyMilestoneUpgrade);
    }

    private PotionMilestonUpgrade GetFirstReadyUpgrade()
    {
        foreach (PotionMilestonUpgrade upgrade in milestoneUpgrades)
        {
            if (upgrade != null && upgrade.IsMilestoneReady())
            {
                return upgrade;
            }
        }

        return null;
    }
}