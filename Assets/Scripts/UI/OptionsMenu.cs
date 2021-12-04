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
        SetBusVolume("bus:/", amt);
    }

    private void SetBusVolume(string busPath, float amt)
    {
        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus(busPath);
        if (Mathf.Approximately(amt, 0))
        {
            bus.setVolume(0);
            // turn off the volume
        }
        else if (amt < 0.0f || amt > 1.0f)
        {
            Debug.LogWarning("Input to MusicVolumeSliderChanged should be between 0 and 1");
        }
        else
        {  
            bus.setVolume(amt); // bus.setlvolume is LINEAR
        }
    }
}
