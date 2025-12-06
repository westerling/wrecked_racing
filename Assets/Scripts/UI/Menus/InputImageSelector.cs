using UnityEngine;

public class InputImageSelector : MonoBehaviour
{

    [SerializeField]
    private GameObject m_KeyboardSprite;

    [SerializeField]
    private GameObject m_ControllerSprite;

    public void SetSpriteInvisible()
    {
        m_KeyboardSprite.SetActive(false);
        m_ControllerSprite.SetActive(false);
    }

    public void SetSpriteVisible(InputType inputType)
    {
        switch (inputType)
        {
            case InputType.Keyboard:
                m_KeyboardSprite.SetActive(true);
                m_ControllerSprite.SetActive(false);
                break;
            case InputType.Controller:
                m_KeyboardSprite.SetActive(false);
                m_ControllerSprite.SetActive(true);
                break;
        }
    }
}
