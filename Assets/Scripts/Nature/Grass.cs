using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField]
    private float m_Position = 0f;

    private bool m_ListIsEmpty = true;

    private List<Transform> m_Entities = new List<Transform>();
    private Material m_GrassMaterial;

    private void Awake()
    {
        if (TryGetComponent(out MeshRenderer meshRenderer))
        {
            m_GrassMaterial = meshRenderer.material;
        }
    }

    private void FixedUpdate()
    {
        if (m_Entities.Count <= 0)
        {
            if (!m_ListIsEmpty)
            {
                Vector3 currentGrassMatPos = m_GrassMaterial.GetVector("_Pos");
                m_GrassMaterial.SetVector("_Position", currentGrassMatPos + new Vector3(0f, m_Position, 0f));
                
                if (m_Position >= 50f)
                {
                    m_ListIsEmpty = true;
                }

                m_Position += .0025f;
            }
            return;
        }
        if (m_Entities[m_Entities.Count - 1] == null)
        {
            return;
        }

        m_GrassMaterial.SetVector("_Pos", m_Entities[m_Entities.Count - 1].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        m_Entities.Add(other.transform);
        m_ListIsEmpty = false;
        m_Position = 0f;
    }

    private void OnTriggerExit(Collider other)
    {
        m_Entities.Remove(other.transform);
    }
}
