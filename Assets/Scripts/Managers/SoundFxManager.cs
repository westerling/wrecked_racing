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

    public void PlaySoundClip(SoundFxType soundFxType, Transform spawnTransform, float volume)
    {
        var pooledObject = SoundFxPool.Current.GetPooledObjectOfType(soundFxType);

        if (pooledObject == null)
        {
            return;
        }
            
        pooledObject.transform.position = spawnTransform.position;
        pooledObject.SetActive(true);

        if (pooledObject.TryGetComponent(out SoundFx soundFx))
        {

            m_AudioSource.clip = soundFx.AudioClip;
            m_AudioSource.volume = volume;
            m_AudioSource.Play();

            StartCoroutine(ReturnToPoolAfterDelay(pooledObject, m_AudioSource.clip.length));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject pooledObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        SoundFxPool.Current.ReturnObjectToPool(pooledObject);
    }
}
