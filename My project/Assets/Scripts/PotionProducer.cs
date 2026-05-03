using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionProducer : MonoBehaviour
{
    [Header("Potion Settings")]
    [SerializeField] private string potionName = "Basic Potion";
    [SerializeField] private double profit = 5;
    [SerializeField] private float productionTime = 2f;

    [Header("UI")]
    [SerializeField] private TMP_Text potionNameText;
    [SerializeField] private TMP_Text profitText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Button produceButton;

    private bool isProducing = false;

    private void Start()
    {
        produceButton.onClick.AddListener(StartProduction);

        progressSlider.value = 0;
        UpdateUI();
    }

    private void StartProduction()
    {
        if (!isProducing)
        {
            StartCoroutine(ProducePotion());
        }
    }

    private IEnumerator ProducePotion()
    {
        isProducing = true;
        produceButton.interactable = false;

        float timer = 0f;

        while (timer < productionTime)
        {
            timer += Time.deltaTime;

            progressSlider.value = timer / productionTime;
            timerText.text = (productionTime - timer).ToString("F1") + "s";

            yield return null;
        }

        CurrencyManager.Instance.AddCoins(profit);

        progressSlider.value = 0;
        timerText.text = productionTime.ToString("F1") + "s";

        produceButton.interactable = true;
        isProducing = false;
    }

    private void UpdateUI()
    {
        potionNameText.text = potionName;
        profitText.text = "+$" + profit.ToString("F0");
        timerText.text = productionTime.ToString("F1") + "s";
    }

}
