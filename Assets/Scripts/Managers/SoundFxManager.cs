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

    public void PlaySoundClip(SoundFxType soundFxType, Transform spawnTransform, float volume = 1f)
    {
        var pooledObject = SoundFxPool.Current.GetPooledObjectOfType(soundFxType);
        if (pooledObject == null)
        {
            return;
        }

        pooledObject.transform.position = spawnTransform.position;
        pooledObject.SetActive(true);

        if (pooledObject.TryGetComponent(out AudioSource source))
        {
            source.volume = volume;
            source.Play();
            StartCoroutine(ReturnToPoolAfterDelay(pooledObject, source.clip.length));
        }
        else
        {
            Debug.LogWarning("Pooled object missing AudioSource");
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject pooledObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        pooledObject.SetActive(false);
    }
}
