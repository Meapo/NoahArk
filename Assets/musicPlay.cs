using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicPlay : MonoBehaviour
{
    VolumeManager manager;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        manager = VolumeManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        audio.volume = manager.BGVolume;
    }
}
