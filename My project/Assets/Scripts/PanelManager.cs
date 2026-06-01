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
    [SerializeField] private CanvasGroup prestigePanel;
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
        if (TutorialManager.Instance != null)
            TutorialManager.Instance.CancelCurrentTutorial();

        ShowPanel(mainPanel);
        HidePanel(upgradePanel);
        HidePanel(apprenticePanel);
        HidePanel(charmPanel);
        HidePanel(prestigePanel);

    }

    public void ShowUpgrade()
    {
        if (TutorialManager.Instance != null)
            TutorialManager.Instance.CancelCurrentTutorial();

        HidePanel(mainPanel);
        ShowPanel(upgradePanel);
        HidePanel(apprenticePanel);
        HidePanel(charmPanel);
        HidePanel(prestigePanel);


        if (upgradeMilestoneTutorialController != null)
        {
            upgradeMilestoneTutorialController.OnUpgradePanelOpened();
        }
    }

    public void ShowApprentice()
    {
        if (TutorialManager.Instance != null)
            TutorialManager.Instance.CancelCurrentTutorial();

        HidePanel(mainPanel);
        HidePanel(upgradePanel);
        ShowPanel(apprenticePanel);
        HidePanel(charmPanel);
        HidePanel(prestigePanel);


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
        if (TutorialManager.Instance != null)
            TutorialManager.Instance.CancelCurrentTutorial();

        HidePanel(mainPanel);
        HidePanel(upgradePanel);
        HidePanel(apprenticePanel);
        ShowPanel(charmPanel);
        HidePanel(prestigePanel);


        if (charmTutorialController != null)
        {
            charmTutorialController.OnCharmsTabOpened();
        }
    }

    public void ShowPrestige()
    {
        if (TutorialManager.Instance != null)
            TutorialManager.Instance.CancelCurrentTutorial();

        HidePanel(mainPanel);
        HidePanel(upgradePanel);
        HidePanel(apprenticePanel);
        HidePanel(charmPanel);
        ShowPanel(prestigePanel);
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
