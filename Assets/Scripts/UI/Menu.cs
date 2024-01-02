using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public abstract class Menu : MonoBehaviour
{
    [SerializeField]
    private bool m_EnabledOnStart = false;

    [SerializeField]
    private GameObject m_SelectedOnStart;

    private Menu m_ParentMenu;

    protected Menu ParentMenu
    {
        get => m_ParentMenu;
        set => m_ParentMenu = value;
    }

    protected abstract void OnPlayerJoined(Player playerJoining);
    protected abstract void OnPlayerLeft(Player playerLeaving);

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameManager.Current.PlayerStatusChanged += OnPlayerStatusChanged;
    }

    protected abstract void OnPlayerStatusChanged(Player player, bool playerStatus);

    private void OnEnable()
    {
        if (m_EnabledOnStart)
        {
            EnableMenu();
        }
    }

    public virtual void EnterButtonPerformed(Player player)
    {
        if (EventSystem.current.currentSelectedGameObject.TryGetComponent(out Button button))
        {
            button.ButtonSelected();
        }
    }

    public virtual void EnableMenu()
    {
        if (m_SelectedOnStart == null)
        {
            return;
        }

        SelectFirstItem();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameObject.SetActive(m_EnabledOnStart);
    }

    private void SelectFirstItem()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_SelectedOnStart);
    }

    protected void OpenParentMenu()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (ParentMenu == null)
        {
            return;
        }

        gameObject.SetActive(false);
        ParentMenu.gameObject.SetActive(true);

        if (ParentMenu.TryGetComponent(out Menu menu))
        {
            menu.EnableMenu();
        }
    }

    protected void OpenMenu(Menu newMenu)
    {
        newMenu.ParentMenu = this;
        newMenu.gameObject.SetActive(true);
        newMenu.EnableMenu();

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
