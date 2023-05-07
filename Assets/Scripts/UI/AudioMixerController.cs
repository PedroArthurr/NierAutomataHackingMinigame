using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    private AudioMixerGroup sfx, bgm;
    [SerializeField] private Slider sfxSlider, bgmSlider;

    private void Start()
    {
        sfx = AudioManager.instance.sfxGroup;
        bgm = AudioManager.instance.bgmGroup;
    }

    public void UpdateSfxVolume() => sfx?.audioMixer?.SetFloat(sfx.name + "Volume", Mathf.Log10(sfxSlider.value) * 20);
    public void UpdateBgmVolume() => bgm?.audioMixer?.SetFloat(bgm.name + "Volume", Mathf.Log10(bgmSlider.value) * 20);
}
