using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private RectTransform buttonTransform;

    private void Start() {
        // Get the button's RectTransform
        buttonTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        // Animate the button's scale when hovered
        buttonTransform.DOScale(1.1f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData) {
        // Animate the button's scale when the pointer exits
        buttonTransform.DOScale(1f, 0.2f);
    }
}