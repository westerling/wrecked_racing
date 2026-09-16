using System;
using System.Collections;
using UnityEngine;

public class SoundFxManager : MonoBehaviour
{
    public static SoundFxManager Current;

    [SerializeField]
    private AudioSource m_AudioSource;

    private void Awake()
    {
        Current = this;
    }

    private void Update()
    {
        
    }

    public void PlaySoundClip(Sound sound, Transform origin, Func<bool> stopCondition = null)
    {
        var pooledObject = AudioSourcePool.Current.GetPooledObject();
        if (pooledObject == null)
        {
            return;
        }

        pooledObject.transform.position = origin.position;
        pooledObject.SetActive(true);

        if (pooledObject.TryGetComponent(out AudioSource audioSource))
        {
            audioSource.volume = sound.Volume;
            audioSource.pitch = sound.Pitch;
            audioSource.clip = sound.AudioClip;
            audioSource.spatialBlend = 1;
            audioSource.Play();

            if (sound.Loop && stopCondition != null)
            {
                StartCoroutine(ReturnAfterConditionMet(
                pooledObject,
                stopCondition));
            }
            else
            {
                StartCoroutine(ReturnToPoolAfterDelay(pooledObject, audioSource.clip.length));
            }
        }
    }

    private IEnumerator ReturnAfterConditionMet(
    GameObject pooledObject,
    Func<bool> condition)
    {
        yield return new WaitUntil(condition);

        var audioSource = pooledObject.GetComponent<AudioSource>();
        audioSource.Stop();

        pooledObject.SetActive(false);
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject pooledObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        pooledObject.SetActive(false);
    }
}
