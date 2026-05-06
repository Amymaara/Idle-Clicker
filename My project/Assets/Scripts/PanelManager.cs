using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{

    [SerializeField] private CanvasGroup mainPanel;
    [SerializeField] private CanvasGroup upgradePanel;
    [SerializeField] private CanvasGroup apprenticePanel;
    [SerializeField] private CanvasGroup charmPanel;
    [SerializeField] private RectTransform basicApprenticeBuyButtonTarget;
    [SerializeField] private CharmCard tutorialCharmCard;
    [SerializeField] private CharmSlotUI charmSlotUI;
    [SerializeField] private CharmTutorialController charmTutorialController;
    [SerializeField] private UpgradeMilestoneController upgradeMilestoneTutorialController;
    [SerializeField] private BuyModeTutorialController buyModeTutorialController;
    private bool hasShownBuyApprenticeTutorial = false;


    private void Start()
    {
        ShowMain();
    }

    private void Update()
    {
        if (charmTutorialController != null)
        {
            charmTutorialController.TryShowOpenCharmTabTutorial();
        }

        if (upgradeMilestoneTutorialController != null)
        {
            upgradeMilestoneTutorialController.TryShowOpenUpgradePanelTutorial();
        }

        if (buyModeTutorialController != null)
        {
            buyModeTutorialController.TryShowBuyModeTutorial();
        }
    }

    public void ShowMain()
    {
        ShowPanel(mainPanel);
        HidePanel(upgradePanel);
        HidePanel(apprenticePanel);
        HidePanel(charmPanel);
    }

    public void ShowUpgrade()
    {
        HidePanel(mainPanel);
        ShowPanel(upgradePanel);
        HidePanel(apprenticePanel);
        HidePanel(charmPanel);

        if (upgradeMilestoneTutorialController != null)
        {
            upgradeMilestoneTutorialController.OnUpgradePanelOpened();
        }
    }

    public void ShowApprentice()
    {
        HidePanel(mainPanel);
        HidePanel(upgradePanel);
        ShowPanel(apprenticePanel);
        HidePanel(charmPanel);

        Button apprenticeButton =
    basicApprenticeBuyButtonTarget.GetComponent<Button>();

        if (!hasShownBuyApprenticeTutorial &&
            apprenticeButton != null &&
            apprenticeButton.interactable)
        {
            hasShownBuyApprenticeTutorial = true;

            TutorialManager.Instance.ShowTutorial(
                "Buy an apprentice to automate Basic Brew.",
                basicApprenticeBuyButtonTarget,
                TutorialAction.BuyApprentice
            );
        }
    }

    public void ShowCharms()
    {
        HidePanel(mainPanel);
        HidePanel(upgradePanel);
        HidePanel(apprenticePanel);
        ShowPanel(charmPanel);

        if (charmTutorialController != null)
        {
            charmTutorialController.OnCharmsTabOpened();
        }
    }

    private void ShowPanel(CanvasGroup panel)
    {
        panel.alpha = 1;
        panel.interactable = true;
        panel.blocksRaycasts = true;
    }

    private void HidePanel(CanvasGroup panel)
    {
        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;
    }

}
