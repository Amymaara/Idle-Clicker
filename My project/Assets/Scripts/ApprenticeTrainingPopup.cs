using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ApprenticeTrainingPopup : MonoBehaviour
{
    public static ApprenticeTrainingPopup Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text masteryPointsText;

    [Header("Upgrade Rows")]
    [SerializeField] private ApprenticeTrainingUpgradeRow speedRow;
    [SerializeField] private ApprenticeTrainingUpgradeRow profitRow;
    [SerializeField] private ApprenticeTrainingUpgradeRow batchRow;

    private ApprenticeCard currentApprentice;

    private void Awake()
    {
        Instance = this;

        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    private void Start()
    {
        closeButton.onClick.AddListener(ClosePopup);
    }

    public void OpenPopup(ApprenticeCard apprentice)
    {
        Debug.Log("OpenPopup called");

        if (popupPanel == null)
        {
            Debug.LogError("popupPanel is not assigned.");
            return;
        }

        currentApprentice = apprentice;

        if (titleText != null)
            titleText.text = apprentice.ApprenticeName + "'s Training";

        UpdateMasteryPointText();

        batchRow.Setup(currentApprentice, ApprenticeTrainingType.Batch);

        if (speedRow != null)
            speedRow.Setup(currentApprentice, ApprenticeTrainingType.Speed);

        if (profitRow != null)
            profitRow.Setup(currentApprentice, ApprenticeTrainingType.Profit);

        popupPanel.SetActive(true);

        Debug.Log("Popup panel set active.");
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        currentApprentice = null;
    }

    public void UpdateMasteryPointText()
    {
        if (currentApprentice == null || masteryPointsText == null) return;

        masteryPointsText.text =
            "Mastery Points: " +
            currentApprentice.AvailableMasteryPoints +
            " / " +
            currentApprentice.TotalMasteryPoints;
    }
}