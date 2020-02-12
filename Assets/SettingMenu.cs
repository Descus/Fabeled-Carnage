using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer mixerMenu, mixerIngame;
    // Start is called before the first frame update
    public void EnableMusic()
    {
        mixerMenu.SetFloat("volumeMusic", -18f);
        mixerIngame.SetFloat("volumeMusic", -12.50f);
    }
    public void DisableMusic()
    {
        mixerMenu.SetFloat("volumeMusic", -80f);
        mixerIngame.SetFloat("volumeMusic", -80f);
    }
        
    public void EnableSFX()
    {
        mixerMenu.SetFloat("volumeSFX", -12f);
        mixerIngame.SetFloat("volumeSFX", -12f);
    }
    public void DisableSFX()
    {
        mixerMenu.SetFloat("volumeSFX", -80f);
        mixerIngame.SetFloat("volumeSFX", -80f);
    }
}
