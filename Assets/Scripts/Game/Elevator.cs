using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] 
    private Rigidbody m_Rigidbody;
    
    [SerializeField] 
    private Transform m_TopPosition;
    
    [SerializeField] 
    private Transform m_BottomPosition;

    [SerializeField]
    private Cog[] m_Cogs;

    [SerializeField] 
    private float m_Speed = 5f;

    private List<Car> m_CarsOnElevator = new List<Car>();

    private ElevatorState m_State;

    private void FixedUpdate()
    {
        switch (m_State)
        {
            case ElevatorState.MovingUp:
                MoveElevator(m_TopPosition.position);

                if (IsAtPosition(m_TopPosition.position))
                {
                    m_State = ElevatorState.Top;
                    RotateCogs(false, false);
                }
                break;

            case ElevatorState.MovingDown:
                MoveElevator(m_BottomPosition.position);

                if (IsAtPosition(m_BottomPosition.position))
                {
                    m_State = ElevatorState.Bottom;
                    RotateCogs(false, false);
                }
                    

                break;
        }
    }

    private bool IsAtPosition(Vector3 target)
    {
        return (m_Rigidbody.position - target).sqrMagnitude < 0.1f;
    }

    private void MoveElevator(Vector3 targetPosition)
    {
        Vector3 newPosition = Vector3.MoveTowards(
            m_Rigidbody.position,
            targetPosition,
            m_Speed * Time.fixedDeltaTime
        );

        m_Rigidbody.MovePosition(newPosition);
    }

    public void CarEntered(Car car)
    {
        m_CarsOnElevator.Add(car);

        if (m_State == ElevatorState.Bottom)
        {
            StartCoroutine(ChangeState(ElevatorState.MovingUp, 1f));
            RotateCogs(true, false);
        }
    }

    private IEnumerator ChangeState(ElevatorState state, float delay)
    {
        yield return new WaitForSeconds(delay);

        m_State = state;
    }

    public void CarExited(Car car)
    {
        m_CarsOnElevator.Remove(car);

        if (m_CarsOnElevator.Count == 0)
        {
            StartCoroutine(ChangeState(ElevatorState.MovingDown, 0.5f));
            RotateCogs(true, true);
        }
    }

    private void RotateCogs(bool rotate, bool clockwise)
    {
        foreach (var cog in m_Cogs)
        {
            cog.Rotate = rotate;
            cog.RotateClockwise = clockwise;
        }
    }
}
