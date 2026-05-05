using TMPro;
using UnityEngine;

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

    public void ShowTutorial(string message, RectTransform target)
    {
        if (tutorialOverlay == null || target == null) return;

        currentTarget = target;
        isShowing = true;

        tutorialText.text = message;
        tutorialOverlay.SetActive(true);

        PositionTutorial(target);
    }

    public void HideTutorial()
    {
        isShowing = false;
        currentTarget = null;

        if (tutorialOverlay != null)
            tutorialOverlay.SetActive(false);
    }

    private void PositionTutorial(RectTransform target)
    {
        Vector3 targetWorldPos = target.position;

        highlightBox.position = targetWorldPos;
        highlightBox.sizeDelta = target.rect.size + highlightPadding;

        arrowImage.position = targetWorldPos + (Vector3)arrowOffset;
    }
}