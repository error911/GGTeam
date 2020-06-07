using GGTeam.Tools.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    public AudioClip clip1;
    public AudioClip clip2;
    int id;

    IMusicPlayer imp;
    IAudioPlayer iap;
    void Start()
    {
        var ac = new AudioController();
        //IAudioController iac = ac;
        imp = ac;
        iap = ac;
        id = imp.PlayMusicClip(clip1);
        iap.PlayAudioClip2D(clip2);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) imp.PausePlayingClip(id);
        if (Input.GetKeyDown(KeyCode.DownArrow)) imp.ResumeClipIfInPause(id);
        if (Input.GetKey(KeyCode.Alpha1)) iap.PlayAudioClip2D(clip2);
        if (Input.GetKey(KeyCode.Alpha2)) iap.PlayAudioClip2D(clip2);
        if (Input.GetKey(KeyCode.Alpha3)) iap.PlayAudioClip2D(clip2);
    }

    
}
