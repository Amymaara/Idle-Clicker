using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RandomEventManager : MonoBehaviour
{
    public static RandomEventManager Instance { get; private set; }

    [Header("Event Timing")]
    [SerializeField] private float timeBetweenEventRolls = 90f;
    [SerializeField] private float eventChance = 0.25f;
    [SerializeField] private float eventDuration = 60f;

    [Header("UI")]
    [SerializeField] private CanvasGroup eventBanner;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text effectText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image bannerBackground;

    public float ProfitMultiplier { get; private set; } = 1f;
    public float ProductionSpeedMultiplier { get; private set; } = 1f;
    public float ApprenticeSpeedMultiplier { get; private set; } = 1f;

    private bool hasShownEventTutorial = false;

    private float rollTimer;
    private float eventTimer;
    private bool eventActive = false;

    private RandomEventType activeEventType;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rollTimer = timeBetweenEventRolls;
        HideBanner();
    }

    private void Update()
    {
        if (eventActive)
        {
            UpdateActiveEvent();
        }
        else
        {
            RollForEventTimer();
        }
    }

    private void RollForEventTimer()
    {
        rollTimer -= Time.deltaTime;

        if (rollTimer > 0) return;

        rollTimer = timeBetweenEventRolls;

        float roll = Random.value;

        if (roll <= eventChance)
        {
            StartRandomEvent();
        }
    }

    private void StartRandomEvent()
    {
        int randomIndex = Random.Range(0, 6);
        activeEventType = (RandomEventType)randomIndex;

        eventTimer = eventDuration;
        eventActive = true;
        ApplyEventEffect();

        UpdateBannerText();
        ShowBanner();

        if (!hasShownEventTutorial)
        {
            hasShownEventTutorial = true;

            TutorialManager.Instance.ShowInfoTutorial(
                "Random Event! Events can temporarily boost or hinder your workshop.",
                eventBanner.GetComponent<RectTransform>(),
                5f
            );
        }

        Debug.Log("Random Event Started: " + activeEventType);
    }

    private void UpdateActiveEvent()
    {
        eventTimer -= Time.deltaTime;

        timerText.text = Mathf.CeilToInt(eventTimer) + "s";

        if (eventTimer <= 0)
        {
            EndEvent();
        }
    }

    private void EndEvent()
    {
        eventActive = false;
        ClearEventEffects();
        HideBanner();

        Debug.Log("Random Event Ended: " + activeEventType);
    }

    private void UpdateBannerText()
    {
        switch (activeEventType)
        {
            case RandomEventType.ProfitBoost:
                titleText.text = "Rare Ingredients";
                effectText.text = "+50% Profit";
                bannerBackground.color = Color.green;
                break;

            case RandomEventType.ProfitPenalty:
                titleText.text = "Ingredient Shortage";
                bannerBackground.color = Color.red;
                effectText.text = "-25% Profit";
                break;

            case RandomEventType.ProductionBoost:
                titleText.text = "Arcane Surge";
                effectText.text = "+50% Production Speed";
                bannerBackground.color = Color.green;
                break;

            case RandomEventType.ProductionPenalty:
                titleText.text = "Faulty Cauldrons";
                bannerBackground.color = Color.red;
                effectText.text = "-25% Production Speed";
                break;

            case RandomEventType.ApprenticeBoost:
                titleText.text = "Apprentice Inspiration";
                effectText.text = "+50% Apprentice Speed";
                bannerBackground.color = Color.green;
                break;

            case RandomEventType.ApprenticePenalty:
                titleText.text = "Distracted Apprentices";
                effectText.text = "-25% Apprentice Speed";
                bannerBackground.color = Color.red;
                break;
        }

        timerText.text = Mathf.CeilToInt(eventDuration) + "s";
    }

    private void ShowBanner()
    {
        eventBanner.alpha = 1;
        eventBanner.interactable = false;
        eventBanner.blocksRaycasts = false;
    }

    private void HideBanner()
    {
        eventBanner.alpha = 0;
        eventBanner.interactable = false;
        eventBanner.blocksRaycasts = false;
    }

    private void ApplyEventEffect()
    {
        ClearEventEffects();

        switch (activeEventType)
        {
            case RandomEventType.ProfitBoost:
                ProfitMultiplier = 1.5f;
                break;

            case RandomEventType.ProfitPenalty:
                ProfitMultiplier = 0.75f;
                break;

            case RandomEventType.ProductionBoost:
                ProductionSpeedMultiplier = 1.5f;
                break;

            case RandomEventType.ProductionPenalty:
                ProductionSpeedMultiplier = 0.75f;
                break;

            case RandomEventType.ApprenticeBoost:
                ApprenticeSpeedMultiplier = 1.5f;
                break;

            case RandomEventType.ApprenticePenalty:
                ApprenticeSpeedMultiplier = 0.75f;
                break;
        }
    }

    private void ClearEventEffects()
    {
        ProfitMultiplier = 1f;
        ProductionSpeedMultiplier = 1f;
        ApprenticeSpeedMultiplier = 1f;
    }
}