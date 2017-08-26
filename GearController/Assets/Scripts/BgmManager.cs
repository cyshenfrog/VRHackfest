using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class BgmManager : MonoBehaviour
{
    private bool isfading = false;
    private float fadingTime;
    private float originalVolme;
    private int i;
    public AudioClip[] Bgms;
    public AudioSource bgmSource;
    public AudioSource[] AudioSources;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;
    private Action onFadeoutFinished;

    public enum Channel
    {
        bgmSource,
    }

    public static BgmManager instance = null;

    // SoundManager won't be destroy
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Debug.Log("there is two BgmManager instance");
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        PlayBgm(Bgms[level]);
    }

    private void Start()
    {
        PlayBgm(Bgms[0]);

        //AudioSources = new AudioSource[] { bgmSource, };
    }

    private void FixedUpdate()
    {
        if (isfading)
        {
            AudioSources[i].volume -= (1 / fadingTime) * Time.deltaTime;
            if (AudioSources[i].volume < 0.01)
            {
                AudioSources[i].Stop();
                AudioSources[i].volume = originalVolme;
                isfading = false;
                if (onFadeoutFinished != null)
                {
                    onFadeoutFinished();
                    onFadeoutFinished = null;
                }
            }
        }
    }

    /// <summary>
    /// Stop the clip of selected sound channel
    /// </summary>
    /// <param name="SoundChannel">sound channel</param>
    public void Stop(Channel SoundChannel)
    {
        i = (int)SoundChannel;
        AudioSources[i].Stop();
    }

    /// <summary>
    /// Fadeout volume to 0 in FadeTime of Target SoundChannel, then stop the clip, return original Volumn
    /// </summary>
    /// <param name="FadeTime"></param>
    /// <param name="SoundChannel"></param>
    public void VolumeFadeout(float FadeTime, Channel SoundChannel)
    {
        fadingTime = FadeTime;
        i = (int)SoundChannel;
        originalVolme = AudioSources[i].volume;
        isfading = true;
    }

    /// <summary>
    /// Play BGM Music of Chennal 1
    /// </summary>
    /// <param name="clip"></param>
    public void PlayBgm(AudioClip clip)
    {
        if (isfading)
            onFadeoutFinished = new Action(() => { PlayBgm(clip); });
        else
        {
            if (bgmSource.clip != null)
            {
                if (bgmSource.clip.name != clip.name || !bgmSource.isPlaying)
                {
                    bgmSource.clip = clip;
                    bgmSource.Play();
                }
            }
            else
            {
                bgmSource.clip = clip;
                bgmSource.Play();
            }
        }
    }
}