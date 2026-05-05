using UnityEngine;

public class Targeter : CarComponent
{
    [SerializeField]
    private LayerMask m_LaserLayerMask;

    [SerializeField]
    private Transform m_RayOrigin;

    [SerializeField]
    private LineRenderer m_LineRenderer;

    private float m_MaxDistance = 20f;
    private bool m_HasHit = false;

    private Transform m_LaserTransform;
    private GameObject m_MovingObject;
    private GameObject m_CurrentTarget;

    private RaycastHit m_Hit;

    public GameObject CurrentTarget
    {
        get => m_CurrentTarget;
        set => m_CurrentTarget = value;
    }
    
    private void OnEnable()
    {
        if (Car is PlayerCar playerCar)
        {
            if (playerCar.WeaponManager.Weapon == null)
            {
                return;
            }

            if (playerCar.WeaponManager.Weapon is TargetWeapon targetWeapon)
            {
                m_LaserTransform = targetWeapon.LaserTransform;
                m_MovingObject = targetWeapon.MovingObject;
                m_LineRenderer.enabled = true;
                m_LineRenderer.material = m_LineRenderer.materials[0];
            }
        }
    }

    private void OnDisable()
    {
        m_LaserTransform = null;
        m_MovingObject = null;
        m_LineRenderer.enabled = false;
    }

    private void Update()
    {
        UpdateGraphics();
        UpdateMaterials();
    }

    private void FixedUpdate()
    {
        TargetEnemy();
    }

    private void TargetEnemy()
    {
        if (Physics.Raycast(m_RayOrigin.position, m_RayOrigin.forward, out m_Hit, m_MaxDistance, LayerMasks.TargeterLayerMask))
        {
            m_HasHit = true;

            if (m_Hit.collider.TryGetComponent(out Car car))
            {
                CurrentTarget = car.gameObject;
            }
            else
            {
                CurrentTarget = null;
            }
        }
        else
        {
            m_HasHit = false;
            CurrentTarget = null;
        }
    }

    private void UpdateGraphics()
    {
        m_LineRenderer.SetPosition(0, m_LaserTransform.position);

        if (!m_HasHit)
        {
            var hitPoint = m_RayOrigin.position + m_RayOrigin.forward * m_MaxDistance;

            m_LineRenderer.SetPosition(1, hitPoint);
            m_MovingObject.transform.LookAt(hitPoint);
        }
        else
        {
            if (CurrentTarget == null)
            {
                m_LineRenderer.SetPosition(1, m_Hit.point);
                m_MovingObject.transform.LookAt(m_Hit.point);
            }
            else
            {
                m_LineRenderer.SetPosition(1, CurrentTarget.transform.position);
                m_MovingObject.transform.LookAt(CurrentTarget.transform.position);
            }
        }
    }

    private void UpdateMaterials()
    {
        if (CurrentTarget == null)
        {
            if (m_LineRenderer.material == m_LineRenderer.materials[1])
            {
                m_LineRenderer.material = m_LineRenderer.materials[0];
            }
        }
        else
        {
            if (m_LineRenderer.material == m_LineRenderer.materials[0])
            {
                m_LineRenderer.material = m_LineRenderer.materials[1];
            }
        }
    }
}
