using UnityEngine;

public class TutorialStepTrigger : MonoBehaviour
{
    [SerializeField] private string tutorialMessage;
    [SerializeField] private RectTransform target;

    [Header("Tutorial Settings")]
    [SerializeField] private TutorialAction requiredAction;
    [SerializeField] private bool showOnStart = false;
    [SerializeField] private bool onlyShowOnce = true;

    private bool hasShown = false;

    private void Start()
    {
        if (showOnStart)
        {
            Show();
        }
    }

    public void Show()
    {
        if (onlyShowOnce && hasShown) return;

        hasShown = true;

        TutorialManager.Instance.ShowTutorial(
            tutorialMessage,
            target,
            requiredAction
        );
    }

    public void Hide()
    {
        TutorialManager.Instance.HideTutorial();
    }
}