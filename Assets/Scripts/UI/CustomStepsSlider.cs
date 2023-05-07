using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomStepsSlider : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private int minValue = 0;
    [SerializeField] private int maxValue = 10;
    [SerializeField] private Image handle;
    [SerializeField] private RectTransform handleArea;
    [SerializeField] private Image[] steps;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Enums.CustomSliderType sliderType;
    [SerializeField] private UnityEvent onValueChanged;

    private float currentValue = 10;
    private float stepSize = 1f;
    private float totalSteps;

    private AudioClip stepSound;

    public float NormalizedValue { get => currentValue / 10; }

    private void Start()
    {
        stepSound = AudioManager.instance.sounds.GetAudioClip("button_select");
        totalSteps = steps.Length;
        stepSize = (maxValue - minValue) / totalSteps;

        currentValue = PlayerPrefs.GetFloat(sliderType == Enums.CustomSliderType.BGM ? Consts.BGM_VOLUME : Consts.SFX_VOLUME, 1) * 10;
        UpdateHandlePosition();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateValue(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateValue(eventData);
    }

    private void UpdateValue(PointerEventData eventData)
    {
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out localCursor
        ))
        {
            return;
        }

        float percent = Mathf.InverseLerp(
            GetComponent<RectTransform>().rect.xMin,
            GetComponent<RectTransform>().rect.xMax,
            localCursor.x
        );

        int newValue = Mathf.RoundToInt(Mathf.Lerp(minValue, maxValue, percent));
        if (newValue != currentValue)
        {
            currentValue = newValue;
            UpdateHandlePosition();
            UpdateStepImages();
            OnStep();
        }
    }

    private void UpdateHandlePosition()
    {
        float stepWidth = handleArea.rect.width / totalSteps;
        float handleX = handleArea.rect.xMin + stepWidth * currentValue;
        Vector2 handlePosition = new Vector2(
            handleX,
            handle.GetComponent<RectTransform>().anchoredPosition.y
        );
        handle.GetComponent<RectTransform>().anchoredPosition = handlePosition;
    }

    private void UpdateStepImages()
    {
        bool hasActiveStep = (currentValue > 0);

        if (hasActiveStep)
        {
            steps[0].sprite = activeSprite;
            steps[0].rectTransform.sizeDelta = new Vector2(
                steps[0].rectTransform.sizeDelta.x,
                21.49f
            );
        }
        else
        {
            steps[0].sprite = inactiveSprite;
            steps[0].rectTransform.sizeDelta = new Vector2(
                steps[0].rectTransform.sizeDelta.x,
                4
            );
        }

        for (int i = 1; i < steps.Length; i++)
        {
            if (i <= (currentValue - 1))
            {
                steps[i].sprite = activeSprite;
                steps[i].rectTransform.sizeDelta = new Vector2(
                    steps[i].rectTransform.sizeDelta.x,
                    21.49f
                );
                hasActiveStep = true;
            }
            else
            {
                steps[i].sprite = inactiveSprite;
                steps[i].rectTransform.sizeDelta = new Vector2(
                    steps[i].rectTransform.sizeDelta.x,
                    4
                );
            }
        }

        if (!hasActiveStep)
            currentValue = 0;
    }

    private void OnStep()
    {
        AudioManager.instance.PlaySound(stepSound, .5f);
        switch (sliderType)
        {
            case Enums.CustomSliderType.Master:
                PlayerPrefs.SetFloat(Consts.MASTER_VOLUME, NormalizedValue);
                break;
            case Enums.CustomSliderType.BGM:
                PlayerPrefs.SetFloat(Consts.BGM_VOLUME, NormalizedValue);
                break;
            case Enums.CustomSliderType.SFX:
                PlayerPrefs.SetFloat(Consts.SFX_VOLUME, NormalizedValue);
                break;
        }
        
        PlayerPrefs.Save();
        onValueChanged?.Invoke();
    }
}
