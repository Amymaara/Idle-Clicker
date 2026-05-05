using UnityEngine;

public class SidebarNotificationManager : MonoBehaviour
{
    [Header("Tab Notifiers")]
    [SerializeField] private SideTabNotifier upgradeTab;
    [SerializeField] private SideTabNotifier apprenticeTab;
    [SerializeField] private SideTabNotifier charmsTab;

    [Header("Systems To Check")]
    [SerializeField] private PotionMilestonUpgrade[] upgradeCards;
    [SerializeField] private ApprenticeCard[] apprenticeCards;
    [SerializeField] private CharmCard[] charmCards;
    [SerializeField] private CharmSlotUI charmSlotUI;

    private void Update()
    {
        bool upgradeAvailable = HasAvailableUpgrade();

        upgradeTab.SetNotification(upgradeAvailable);
        apprenticeTab.SetNotification(HasAvailableApprentice());
        charmsTab.SetNotification(HasAvailableCharm());
    }

    private bool HasAvailableUpgrade()
    {
        foreach (PotionMilestonUpgrade card in upgradeCards)
        {
            if (card != null && card.CanCurrentlyUpgrade())
                return true;
        }

        return false;
    }

    private bool HasAvailableApprentice()
    {
        foreach (ApprenticeCard card in apprenticeCards)
        {
            if (card != null && card.IsAvailableToBuy())
                return true;
        }

        return false;
    }

    private bool HasAvailableCharm()
    {
        foreach (CharmCard card in charmCards)
        {
            if (card != null && card.IsAvailableToBuy())
                return true;
        }

        return false;
    }

}
