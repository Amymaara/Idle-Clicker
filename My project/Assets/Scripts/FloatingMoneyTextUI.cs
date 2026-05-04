using TMPro;
using UnityEngine;

public class FloatingMoneyTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private float floatSpeed = 60f;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float popScale = 1.2f;
    [SerializeField] private float popDuration = 0.1f;

    private Vector3 originalScale;
    private CanvasGroup canvasGroup;
    private float timer;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        originalScale = transform.localScale;
        transform.localScale = originalScale * popScale;
    }

    public void Setup(double amount)
    {
        moneyText.text = "+" + NumberFormatter.FormatMoney(amount);
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        transform.localPosition += Vector3.up * floatSpeed * Time.deltaTime;

        canvasGroup.alpha = 1 - (timer / lifetime);

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }

        float t = timer / popDuration;
        transform.localScale = Vector3.Lerp(originalScale * popScale, originalScale, t);
    }
}
