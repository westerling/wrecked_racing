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

    public void PlaySoundClip(Sound sound, Transform origin)
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
            StartCoroutine(ReturnToPoolAfterDelay(pooledObject, audioSource.clip.length));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject pooledObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        pooledObject.SetActive(false);
    }
}
