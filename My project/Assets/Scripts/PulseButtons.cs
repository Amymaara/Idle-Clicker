using UnityEngine;
using UnityEngine.UI;

public class PulseButtons : MonoBehaviour
{
    [SerializeField] private Button targetButton;
    [SerializeField] private float pulseSpeed = 3f;
    [SerializeField] private float pulseAmount = 0.06f;

    private Vector3 originalScale;
    private bool shouldPulse;

    private void Awake()
    {
        if (targetButton == null)
            targetButton = GetComponent<Button>();

        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (shouldPulse && targetButton != null && targetButton.interactable)
        {
            float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            transform.localScale = originalScale * scale;
        }
        else
        {
            transform.localScale = originalScale;
        }
    }

    public void SetPulse(bool value)
    {
        shouldPulse = value;
    }
}
