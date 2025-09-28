using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    public float scaleAmount = 1.05f;   // how much to scale up
    public float speed = 10f;           // how fast to scale

    private Vector3 originalScale;
    private Vector3 targetScale;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // Smoothly move towards target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleAmount;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}

