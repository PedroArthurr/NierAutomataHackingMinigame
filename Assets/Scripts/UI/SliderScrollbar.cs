using UnityEngine;
using UnityEngine.UI;

public class SliderScrollbar : MonoBehaviour
{

    [SerializeField] private ScrollRect rect;
    [SerializeField] private Slider scrollSlider;

    [SerializeField] private Image upperDot, bottomDot;

    private void OnEnable()
    {
        scrollSlider.onValueChanged.AddListener(UpdateScrollPosition);
        rect.onValueChanged.AddListener(UpdateSliderValue);
    }

    private void OnDisable()
    { 
        scrollSlider.onValueChanged.RemoveListener(UpdateScrollPosition);
        rect.onValueChanged.RemoveListener(UpdateSliderValue);
    }

    private void UpdateScrollPosition(float value)
    {
        rect.verticalNormalizedPosition = 1 - value;
    }

    private void UpdateSliderValue(Vector2 scrollPosition)
    {
        scrollSlider.SetValueWithoutNotify(1 - scrollPosition.y);
        
    }
    public void SetDotsColor()
    {
        float scrollValue = scrollSlider.value;
        Color upperDotColor = upperDot.color;
        Color bottomDotColor = bottomDot.color;

        if (scrollValue == 0)
        {
            var u = upperDotColor;
            u.a = 1f;
            upperDot.color = u;
            var b = bottomDotColor;
            b.a = .6f;
            bottomDot.color = b;
        }
        if (scrollValue == 1)
        {
            var u = upperDotColor;
            u.a = .6f;
            upperDot.color = u;
            var b = bottomDotColor;
            b.a = 1f;
            bottomDot.color = b;
        }
    }
}