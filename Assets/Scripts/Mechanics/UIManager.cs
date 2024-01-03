using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Current;

    [Header("Screens")]
    [SerializeField]
    private GameObject m_SplashScreen;

    [SerializeField]
    private GameObject m_LoadingScreen;

    private void Awake()
    {
        Current = this;

        InstantiateScreens();
    }

    public void SetLoadingScreenActivity(bool active)
    {
        m_LoadingScreen.SetActive(active);
    }

    private void InstantiateScreens()
    {
        _ = Instantiate(m_SplashScreen);
        _ = Instantiate(m_LoadingScreen);
    }
}
