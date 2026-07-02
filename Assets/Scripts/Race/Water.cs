using UnityEngine;

public class Water : MonoBehaviour
{
    private Material m_WaterMaterial;
    private int m_RippleOriginID;

    void Start()
    {
        m_WaterMaterial = GetComponent<Renderer>().material;
        m_RippleOriginID = Shader.PropertyToID("_RippleOrigin");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            var impactPoint = other.ClosestPoint(transform.position);
            m_WaterMaterial.SetVector(m_RippleOriginID, new Vector4(impactPoint.x, impactPoint.y, impactPoint.z, 1));
        }

        if (other.TryGetComponent(out Buoyancy buoyancy))
        {
            buoyancy.SetBuoyancy(true);
        }

        if (other.TryGetComponent(out Car car))
        {
            if (car is PlayerCar playerCar)
            {
                playerCar.Health.Damage(float.MaxValue);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Buoyancy buoyancy))
        {
            buoyancy.SetBuoyancy(false);
        }
    }
}
