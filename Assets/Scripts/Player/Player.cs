using UnityEngine;

public class Player : MonoBehaviour
{
    private string m_Name;

    public string Name
    {
        get => m_Name;
        set => m_Name = value;
    }
}
