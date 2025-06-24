using UnityEngine;

public class WaterRippleEffect : MonoBehaviour
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
        var rigidBody = other.attachedRigidbody;
        if (rigidBody != null)
        {
            Debug.Log("Snopp");
            var impactPoint = other.ClosestPoint(transform.position);
            m_WaterMaterial.SetVector(m_RippleOriginID, new Vector4(impactPoint.x, impactPoint.y, impactPoint.z, 1));
        }
    }
}
