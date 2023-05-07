using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    private AudioMixerGroup master, sfx, bgm;
    [SerializeField] private CustomStepsSlider sfxSlider, bgmSlider;

    private void Start()
    {
        master = AudioManager.instance.master;
        sfx = AudioManager.instance.sfxGroup;
        bgm = AudioManager.instance.bgmGroup;
    }

    public void UpdateMasterVolume() => master.audioMixer.SetVolume(master.name + "Volume", PlayerPrefs.GetFloat(Consts.MASTER_VOLUME));

    public void UpdateSfxVolume() => sfx.audioMixer.SetVolume(sfx.name + "Volume", PlayerPrefs.GetFloat(Consts.SFX_VOLUME));

    public void UpdateBgmVolume() => bgm.audioMixer.SetVolume(bgm.name + "Volume", PlayerPrefs.GetFloat(Consts.BGM_VOLUME));
}
