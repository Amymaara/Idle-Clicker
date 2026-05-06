using UnityEngine;
using UnityEngine.UI;

public class SideTabNotifier : MonoBehaviour
{
    [SerializeField] private Image glowImage;
    [SerializeField] private float pulseSpeed = 3f;
    [SerializeField] private float minAlpha = 0.25f;
    [SerializeField] private float maxAlpha = 0.9f;

    private bool hasNotification;
    private bool isSelected;

    private void Awake()
    {
        HideGlow();
    }

    private void Update()
    {
        if (glowImage == null) return;

        bool shouldGlow = hasNotification && !isSelected;

        if (!shouldGlow)
        {
            HideGlow();
            return;
        }

        glowImage.gameObject.SetActive(true);

        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, pulse);

        Color color = glowImage.color;
        color.a = alpha;
        glowImage.color = color;
    }

    public void SetNotification(bool value)
    {
        hasNotification = value;

        if (!value)
            HideGlow();
    }

    public void SetSelected(bool value)
    {
        isSelected = value;

        if (value)
            HideGlow();
    }

    public void Clear()
    {
        hasNotification = false;
        HideGlow();
    }

    private void HideGlow()
    {
        if (glowImage == null) return;

        Color color = glowImage.color;
        color.a = 0f;
        glowImage.color = color;

        glowImage.gameObject.SetActive(false);
    }
}
