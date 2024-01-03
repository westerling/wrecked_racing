using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Current;

    public event Action<Player, bool> PlayerStatusChanged;

    [SerializeField]
    private GameObject m_Camera;

    private int m_CurrentLoadedScene;
    private float m_TotalSceneProgress;

    private List<Player> m_Players;
    private List<AsyncOperation> m_ScenesLoading = new List<AsyncOperation>();
    private RaceSettings m_RaceSettings;


    public List<Player> Players
    {
        get => m_Players;
        private set => m_Players = value;
    }

    public RaceSettings RaceSettings
    {
        get => m_RaceSettings;
        private set => m_RaceSettings = value;
    }

    public GameObject Camera 
    {
        get => m_Camera;
    }
    
    private void Awake()
    {
        Current = this;

        SceneManager.LoadSceneAsync((int)SceneIndexes.Title_Screen, LoadSceneMode.Additive);
    }

    public void LoadTrack(RaceSettings raceSettings)
    {
        RaceSettings = raceSettings;

        UIManager.Current.SetLoadingScreenActivity(true);

        m_ScenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Title_Screen));
        m_ScenesLoading.Add(SceneManager.LoadSceneAsync(raceSettings.SceneIndex, LoadSceneMode.Additive));

        m_CurrentLoadedScene = raceSettings.SceneIndex;

        StartCoroutine(GetSceneLoadProgress());
    }

    public void UnloadTrack()
    {
        m_ScenesLoading.Add(SceneManager.UnloadSceneAsync(m_CurrentLoadedScene));
        m_ScenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Title_Screen, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.TryGetComponent(out Player player))
        {
            if (!Players.Contains(player))
            {
                Players.Add(player);
                PlayerStatusChanged?.Invoke(player, true);
            }
        }
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

    private IEnumerator GetSceneLoadProgress()
    {
        for (var i = 0; i < m_ScenesLoading.Count; i++)
        {
            while (!m_ScenesLoading[i].isDone)
            {
                m_TotalSceneProgress = 0;

                foreach (var operation in m_ScenesLoading)
                {
                    m_TotalSceneProgress += operation.progress;
                }

                m_TotalSceneProgress = (m_TotalSceneProgress / m_ScenesLoading.Count) * 100;

                yield return null;
            }
        }

        UIManager.Current.SetLoadingScreenActivity(false);
    }
}
