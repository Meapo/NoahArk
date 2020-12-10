using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeAdjust : MonoBehaviour
{
    // Start is called before the first frame update
    VolumeManager volumeManager;
    public Slider BGVolumeSlider;
    public Slider EFVolumeSlider;
    private void Start()
    {
        volumeManager = VolumeManager.GetInstance();
        BGVolumeSlider.value = volumeManager.BGVolume;
        EFVolumeSlider.value = volumeManager.EFVolume;
    }

    public void AdjustBGVolume()
    {
        volumeManager.BGVolume = BGVolumeSlider.value;
    }
    public void AdjustEFVolume()
    {
        volumeManager.EFVolume = EFVolumeSlider.value;
    }
}
