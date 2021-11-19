using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : UnitySingleton<OptionsMenu>
{
    public void MusicVolumeSliderChanged(float amt)
    {
        Debug.Log(20 * Mathf.Log10(amt));
        FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        if (amt == 0)
        {
            // turn off the volume
        } else if (amt < 0.0f || amt > 1.0f)
        {
            Debug.LogWarning("Input to MusicVolumeSliderChanged should be between 0 and 1");
        } else
        {
            //bus.setVolume(20 * Mathf.Log10(amt));
            bus.setVolume(amt);
        }
    }

    public void SFXVolumeSliderChanged(int amt)
    {

    }

    public void MasterVolumeSliderChanged(int amt)
    {

    }
}
