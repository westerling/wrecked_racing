using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Checkpoint : MonoBehaviour
{
    public event Action<Checkpoint> CheckpointPassed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Car car))
        {
            CheckpointPassed?.Invoke(this);
        }
    }
}
