using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Current;

    public event Action<Player, bool> PlayerStatusChanged;
    public event Action<GameState> GameStateChanged;

    [SerializeField]
    private bool m_DebugMode = false;

    [SerializeField]
    private GameObject m_RaceCamera;

    [SerializeField]
    private GameObject m_FollowCamera;

    [SerializeField]
    private List<GameObject> m_Cars = new List<GameObject>();

    [SerializeField]
    private List<TrackInfo> m_Tracks = new List<TrackInfo>();

    [SerializeField]
    private GameObject[] m_Pools;

    private int m_CurrentLoadedScene;
    private float m_TotalSceneProgress;

    private List<Player> m_Players = new List<Player>();
    private List<Player> m_ActivePlayers = new List<Player>();
    private List<AsyncOperation> m_LoadingScenes = new List<AsyncOperation>();
    private RaceSettings m_RaceSettings;

    public List<Player> Players
    {
        get => m_Players;
        private set => m_Players = value;
    }

    public List<Player> ActivePlayers
    {
        get => m_ActivePlayers;
        private set => m_ActivePlayers = value;
    }

    public RaceSettings RaceSettings
    {
        get => m_RaceSettings;
        private set => m_RaceSettings = value;
    }

    public GameObject RaceCamera 
    {
        get => m_RaceCamera;
    }

    public GameObject FollowCamera
    {
        get => m_FollowCamera;
    }

    public List<GameObject> Cars
    {
        get => m_Cars;
    }
    
    public List<TrackInfo> Tracks 
    {
        get => m_Tracks; 
    }

    public GameObject[] Pools
    {
        get => m_Pools;
    }

    public bool DebugMode
    {
        get => m_DebugMode;
    }

    private void Awake()
    {
        Current = this;

        //StartCoroutine(ShowSplashScreen());
        SceneManager.LoadSceneAsync((int)SceneIndexes.Title_Screen, LoadSceneMode.Additive);
    }

    public void SetGameState(GameState gameState)
    {
        GameStateChanged?.Invoke(gameState);
    }

    public void SetActivePlayers(List<Player> activePlayers)
    {
        ActivePlayers.Clear();
        ActivePlayers.AddRange(activePlayers);
    }

    public void LoadTrack(RaceSettings raceSettings)
    {
        RaceSettings = raceSettings;

        UIManager.Current.SetScreenActive(Screens.LoadingScreen, true);

        m_LoadingScenes.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Title_Screen));
        m_LoadingScenes.Add(SceneManager.LoadSceneAsync(raceSettings.SceneIndex, LoadSceneMode.Additive));

        m_CurrentLoadedScene = raceSettings.SceneIndex;

        StartCoroutine(GetSceneLoadProgress());

        SetGameState(GameState.Race);
    }

    public void UnloadTrack()
    {
        m_LoadingScenes.Add(SceneManager.UnloadSceneAsync(m_CurrentLoadedScene));
        m_LoadingScenes.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Title_Screen, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());

        SetGameState(GameState.Menu);
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.TryGetComponent(out Player player))
        {
            if (!Players.Contains(player))
            {
                //var availableColor = GivePlayerColor();

                //if (!Enum.IsDefined(typeof(PlayerColor), availableColor))
                //{
                //    Console.WriteLine("No available colors!");
                //    return;
                //}

                //player.Color = availableColor;

                if (playerInput.currentControlScheme == "Controller")
                {
                    player.InputType = InputType.Controller;
                }
                else
                {
                    player.InputType = InputType.Keyboard;
                }

                Players.Add(player);
                PlayerStatusChanged?.Invoke(player, true);
            }
        }
    }

    private PlayerColor GivePlayerColor()
    {
        var allColors = Enum.GetValues(typeof(PlayerColor)).Cast<PlayerColor>();
        var takenColors = Players.Select(p => p.Color).ToHashSet();

        return allColors.FirstOrDefault(color => !takenColors.Contains(color));
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        if (playerInput.TryGetComponent(out Player player))
        {
            if (Players.Contains(player))
            {
                Players.Remove(player);
                PlayerStatusChanged?.Invoke(player, false);
            }
        }
    }

    private IEnumerator ShowSplashScreen()
    {
        SetGameState(GameState.Loading);
        UIManager.Current.SetScreenActive(Screens.SplashScreen, true);
        yield return new WaitForSeconds(3f);
        UIManager.Current.SetScreenActive(Screens.SplashScreen, false);
        SetGameState(GameState.Menu);
    }

    private IEnumerator GetSceneLoadProgress()
    {
        for (var i = 0; i < m_LoadingScenes.Count; i++)
        {
            while (!m_LoadingScenes[i].isDone)
            {
                m_TotalSceneProgress = 0;

                foreach (var operation in m_LoadingScenes)
                {
                    m_TotalSceneProgress += operation.progress;
                }

                m_TotalSceneProgress = (m_TotalSceneProgress / m_LoadingScenes.Count) * 100;

                yield return null;
            }
        }

        UIManager.Current.SetScreenActive(Screens.LoadingScreen, false);
    }
}
