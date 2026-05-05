using UnityEngine;
using UnityEngine.UI;

public class SideTabNotifier : MonoBehaviour
{
    [SerializeField] private Image glowImage;
    [SerializeField] private float pulseSpeed = 3f;
    [SerializeField] private float minAlpha = 0.25f;
    [SerializeField] private float maxAlpha = 0.9f;

    private bool hasNotification;
    private bool previousNotification;
    private bool isSelected;
    private bool showGlow;

    private void Update()
    {
        if (glowImage == null) return;

        // If nothing is currently available, fully reset the glow
        if (!hasNotification)
        {
            showGlow = false;
            previousNotification = false;
            glowImage.gameObject.SetActive(false);
            return;
        }

        // Detect NEW availability
        if (hasNotification && !previousNotification)
        {
            showGlow = true;
        }

        previousNotification = hasNotification;

        if (showGlow && !isSelected)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, pulse);

            Color color = glowImage.color;
            color.a = alpha;
            glowImage.color = color;

            glowImage.gameObject.SetActive(true);
        }
        else
        {
            glowImage.gameObject.SetActive(false);
        }
    }

    public void SetNotification(bool value)
    {
        hasNotification = value;
    }

    public void SetSelected(bool value)
    {
        isSelected = value;

        if (isSelected)
        {
            showGlow = false; // player has seen it
        }
    }


}
