using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScaleButtons : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button button;
    [SerializeField] private float hoverScale = 1.05f;
    [SerializeField] private float pressScale = 0.92f;
    [SerializeField] private float speed = 10f;

    private Vector3 targetScale;
    private Vector3 originalScale;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * speed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && !button.interactable) return;
        targetScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button != null && !button.interactable) return;
        SoundManager.Instance.PlaySound(SoundType.Click);
        targetScale = originalScale * pressScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button != null && !button.interactable)
        {
            targetScale = originalScale;
            return;
        }

        targetScale = originalScale * hoverScale;
    }

}
