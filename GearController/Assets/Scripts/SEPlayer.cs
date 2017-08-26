using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SEPlayer : MonoBehaviour
{
    public bool PlayOnEnable = true;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;
    public AudioClip[] SEs;

    private AudioSource FxSoundSource;

    private void OnEnable()
    {
        if (PlayOnEnable)
        {
            PlayFxSound(SEs);
        }
    }

    // Use this for initialization
    private void Awake()
    {
        FxSoundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// Play Fx Sound of Special Fx (Completely)
    /// </summary>

    public void PlayFxSound(AudioClip[] clips)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        int randomIndex = Random.Range(0, clips.Length);

        FxSoundSource.pitch = randomPitch;
        FxSoundSource.clip = clips[randomIndex];
        FxSoundSource.Play();
    }

    public void PlayFxSound(AudioClip clip)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        FxSoundSource.pitch = randomPitch;
        FxSoundSource.clip = clip;
        FxSoundSource.Play();
    }
}