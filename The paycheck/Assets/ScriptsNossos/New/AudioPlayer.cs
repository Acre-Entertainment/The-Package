using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioInfo[] audios;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(string audioID, bool loop, float volume = .8f)
    {
        AudioClip clipToPlay = GetClipFromString(audioID);

        if(clipToPlay == null)
        {
            Debug.LogWarning("NOTE: Tried to play " + audioID + " on " + this.gameObject.name + " but it does not have a audioClip with this ID");
            return;
        }

        if(loop)
        {
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.clip = clipToPlay;
            audioSource.Play();
            return;
        }

        audioSource.PlayOneShot(clipToPlay, volume);
    }

    public void StopAnyClip()
    {
        audioSource.clip = null;
        audioSource.Stop();
    }

    AudioClip GetClipFromString(string audioID)
    {
        foreach(AudioInfo audioInfo in audios)
        {
            if (audioInfo.audioID == audioID)
            {
                if (audioInfo.audioClip.Length < 1)
                    return null;

                return GetRandomIndex<AudioClip>(audioInfo.audioClip);
            }
        }

        return null;
    }

    T GetRandomIndex<T>(T[] array)
    {
        int max = array.Length;
        return array[Random.Range(0, max)]; ;
    }

}

[System.Serializable]
public class AudioInfo
{
    public string audioID;
    public AudioClip[] audioClip;
}
