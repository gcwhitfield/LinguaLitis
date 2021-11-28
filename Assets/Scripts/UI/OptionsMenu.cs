using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : UnitySingleton<OptionsMenu>
{
    public void MusicVolumeSliderChanged(float amt)
    {
        SetBusVolume("bus:/Music", amt);
    }

    public void SFXVolumeSliderChanged(float amt)
    {
        SetBusVolume("bus:/SFX", amt);
    }

    public void MasterVolumeSliderChanged(float amt)
    {
        SetBusVolume("bus:/Master", amt);
    }

    private void SetBusVolume(string busPath, float amt)
    {
        Debug.Log(20 * Mathf.Log10(amt));
        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus(busPath);
        if (amt == 0)
        {
            // turn off the volume
        }
        else if (amt < 0.0f || amt > 1.0f)
        {
            Debug.LogWarning("Input to MusicVolumeSliderChanged should be between 0 and 1");
        }
        else
        {
            //bus.setVolume(20 * Mathf.Log10(amt));
            bus.setVolume(amt);
        }
    }
}
