using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableMenuItem : MonoBehaviour
{
    [SerializeField] private Enums.ChallengeButtonState state;
    [Space]

    [SerializeField] private GameObject[] selectedObjects;
    [SerializeField] private TMP_Text challengeName;
    [SerializeField] private Image squareIcon;
    [SerializeField] private Color white, black;

    private AudioClip selection;

    private void Start()
    {
        selection = AudioManager.instance.sounds.GetAudioClip("button_select");
    }
    public void SetCurrentState(bool selected)
    {
        state = selected ? Enums.ChallengeButtonState.Selected : Enums.ChallengeButtonState.Unselected;
        SetUI();
    }

    private void SetUI()
    {
        switch (state)
        {
            case Enums.ChallengeButtonState.Selected:
                foreach(var o in selectedObjects)
                {
                    o.SetActive(true);
                    challengeName.color = white;
                    squareIcon.color = white;
                }
                break;
            case Enums.ChallengeButtonState.Unselected:
                foreach(var o in selectedObjects)
                {
                    o.SetActive(false);
                    challengeName.color = black;
                    squareIcon.color = black;
                }
                break;
        }
    }

    public void PlaySelectAudio()
    {
        AudioManager.instance.PlaySound(selection);
    }
}