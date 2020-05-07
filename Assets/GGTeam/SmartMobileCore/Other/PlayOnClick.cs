using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource), typeof(Button))]
public class PlayOnClick : MonoBehaviour
{
    [SerializeField] AudioClip audioClip = null;
    AudioSource audioSource;
    Button button;
    bool inited = false;
    
    void Awake()
    {
        if (inited) return;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (button == null) button = GetComponent<Button>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioClip;
        button.onClick.AddListener(() => Play());
        inited = true;
    }

    public void Play()
    {
        audioSource.PlayOneShot(audioClip);
    }

}
