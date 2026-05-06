using TMPro;
using UnityEngine;

public enum TutorialAction
{
    None,
    BrewPotion,
    UpgradePotion,
    UnlockPotion,
    OpenApprenticeTab,
    BuyApprentice,
    OpenCharmTab,
    BuyCharm
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Header("Tutorial UI")]
    [SerializeField] private GameObject tutorialOverlay;
    [SerializeField] private RectTransform highlightBox;
    [SerializeField] private RectTransform arrowImage;
    [SerializeField] private RectTransform tutorialBubble;
    [SerializeField] private TMP_Text tutorialText;


    [Header("Settings")]
    [SerializeField] private Vector2 highlightPadding = new Vector2(30, 20);
    [SerializeField] private Vector2 arrowOffset = new Vector2(0, -90);

    private RectTransform currentTarget;
    private bool isShowing;
    public bool HasCompletedFirstBrew { get; private set; }
    public bool HasCompletedFirstUpgrade { get; private set; }
    public bool HasCompletedPotionUnlock { get; private set; }
    public bool IsTutorialShowing { get; private set; }
    private TutorialAction requiredAction = TutorialAction.None;

    private void Awake()
    {
        Instance = this;

        if (tutorialOverlay != null)
            tutorialOverlay.SetActive(false);
    }

    private void Update()
    {
        if (isShowing && currentTarget != null)
        {
            PositionTutorial(currentTarget);
        }
    }

    public void CompleteFirstBrew()
    {
        HasCompletedFirstBrew = true;
    }

    public void CompleteFirstUpgrade()
    {
        HasCompletedFirstUpgrade = true;
    }

    public void CompletePotionUnlock()
    {
        HasCompletedPotionUnlock = true;
    }

    public void ShowTutorial(string message, RectTransform target, TutorialAction action)
    {
        if (tutorialOverlay == null || target == null) return;

        currentTarget = target;
        requiredAction = action;
        isShowing = true;
        IsTutorialShowing = true;

        tutorialText.text = message;
        tutorialOverlay.SetActive(true);

        PositionTutorial(target);
    }

    public void TryCompleteTutorial(TutorialAction action)
    {
        if (!isShowing) return;

        if (action == requiredAction)
        {
            HideTutorial();
        }
    }

    public void HideTutorial()
    {
        isShowing = false;
        currentTarget = null;

        if (tutorialOverlay != null)
            tutorialOverlay.SetActive(false);

        IsTutorialShowing = false;

    }

    private void PositionTutorial(RectTransform target)
    {
        Vector3 targetWorldPos = target.position;

        highlightBox.position = targetWorldPos;
        highlightBox.sizeDelta = target.rect.size + highlightPadding;

        arrowImage.position = targetWorldPos + (Vector3)arrowOffset;
    }
}