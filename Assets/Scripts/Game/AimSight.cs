using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AimSight : MonoBehaviour
{

    [SerializeField]
    private RectTransform m_TopLeft;

    [SerializeField]
    private RectTransform m_TopRight;

    [SerializeField]
    private RectTransform m_BottomLeft;

    [SerializeField]
    private RectTransform m_BottomRight;

    [SerializeField]
    private RectTransform m_Circle;

    [SerializeField]
    private UnityEngine.UI.Image[] m_ColorImages;

    private RectTransform m_RectTransform;
    private float m_OpenDistance = 20f;
    private float m_ClosedDistance = 5f;

    private float m_AimMaxSpeed = 350f;

    private Vector2 m_AimInput;
    private Vector2 m_AimPosition;

    private bool m_LockedIn;

    private PlayerCar m_Target;
    private float m_TargetTimer;
    private const float LOCK_ON_TIME = 1f;

    public bool LockedIn
    {
        get => m_LockedIn;
        private set => m_LockedIn = value;
    }
    public Vector2 AimInput
    {
        get => m_AimInput;
        set => m_AimInput = value;
    }

    public PlayerCar Target
    {
        get => m_Target;
        private set => m_Target = value;
    }

    private void Awake()
    {
        if (gameObject.TryGetComponent(out RectTransform rectTransform))
        {
            m_RectTransform = rectTransform;
        }
        else
        {
            Debug.LogError("No Rect Transform found on Gameobject");
        }

        ResetTransforms();
    }

    private void ResetTransforms()
    {
        SetAimProgress(0);
        Target = null;
        LockedIn = false;
        ToggleSights();
        m_TargetTimer = 0f;
    }

    private void Update()
    {
        MoveAim();
        CheckTarget();
        UpdateSights();
    }

    public void SetColor(Color color)
    {
        foreach (var image in m_ColorImages)
        {
            image.color = color;
        }
    }

    private void ToggleSights()
    {
        m_TopLeft.gameObject.SetActive(!LockedIn);
        m_TopRight.gameObject.SetActive(!LockedIn);
        m_BottomLeft.gameObject.SetActive(!LockedIn);
        m_BottomRight.gameObject.SetActive(!LockedIn);
        m_Circle.gameObject.SetActive(LockedIn);
    }

    private void MoveAim()
    {
        var input = Vector2.ClampMagnitude(AimInput, 1f);

        m_AimPosition += m_AimMaxSpeed * Time.deltaTime * input;

        m_AimPosition.x = Mathf.Clamp(m_AimPosition.x, 0f, Screen.width);
        m_AimPosition.y = Mathf.Clamp(m_AimPosition.y, 0f, Screen.height);

        transform.position = m_AimPosition;
    }

    private void CheckTarget()
    {
        if (Target == null)
        {
            CheckForNewTarget();
        }
        else
        {
            CheckIfTargetStillActive();
        }
    }

    private void CheckIfTargetStillActive()
    {
        var screenPosition = RectTransformUtility.WorldToScreenPoint(null, m_RectTransform.position);
        var ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, LayerMasks.CarLayerMask))
        {
            if (hit.transform.gameObject.TryGetComponent(out PlayerCar playerCar))
            {
                if (playerCar == Target)
                {
                    return;
                }
            }
        }

        ResetTransforms();
    }

    private void CheckForNewTarget()
    {
        var screenPosition = RectTransformUtility.WorldToScreenPoint(null, m_RectTransform.position);
        var ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out var hit, 1000f, LayerMasks.CarLayerMask))
        {
            if (hit.transform.gameObject.TryGetComponent(out PlayerCar playerCar))
            {
                Target = playerCar;
                m_TargetTimer = 0f;
            }
        }
    }

    private void UpdateSights()
    {
        if (LockedIn)
        {
            return;
        }

        if (Target != null)
        {
            m_TargetTimer += Time.deltaTime;

            if (m_TargetTimer >= LOCK_ON_TIME)
            {
                LockedIn = true;
                ToggleSights();
            }
            else
            {
                SetAimProgress(m_TargetTimer / LOCK_ON_TIME);
            }
        }
    }

    private void SetAimProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);
        
        var distance = Mathf.Lerp(
            m_OpenDistance,
            m_ClosedDistance,
            progress
        );

        m_TopLeft.anchoredPosition =
            new Vector2(-distance, distance);

        m_TopRight.anchoredPosition =
            new Vector2(distance, distance);

        m_BottomLeft.anchoredPosition =
            new Vector2(-distance, -distance);

        m_BottomRight.anchoredPosition =
            new Vector2(distance, -distance);
    }
}
