using UnityEngine;
using UnityEngine.UI;

public class FIX_Scrollbar : MonoBehaviour
{
    public ScrollRect Rect;
    public Slider ScrollSlider;

    private void OnEnable()
    {
        ScrollSlider.onValueChanged.AddListener(UpdateScrollPosition);
        Rect.onValueChanged.AddListener(UpdateSliderValue);
    }

    private void OnDisable()
    { 
        ScrollSlider.onValueChanged.RemoveListener(UpdateScrollPosition);
        Rect.onValueChanged.RemoveListener(UpdateSliderValue);
    }

    private void UpdateScrollPosition(float value)
    {
        Rect.verticalNormalizedPosition = 1 - value;
    }

    private void UpdateSliderValue(Vector2 scrollPosition)
    {
        ScrollSlider.SetValueWithoutNotify(1 - scrollPosition.y);
    }
}