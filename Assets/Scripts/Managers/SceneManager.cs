using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Current;

    private int m_CurrentLoadedScene = 0;
    private float m_TotalSceneProgress;

    private List<AsyncOperation> m_ScenesLoading = new List<AsyncOperation>();

    private void Awake()
    {
        Current = this;

        SceneManager.LoadSceneAsync((int)SceneIndexes.Title_Screen, LoadSceneMode.Additive);
    }

    public void LoadTrack(int levelIndex)
    {
        m_ScenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Title_Screen));
        m_ScenesLoading.Add(SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive));

        m_CurrentLoadedScene = levelIndex;

        StartCoroutine(GetSceneLoadProgress());
    }

    public void UnloadTrack()
    {
        m_ScenesLoading.Add(SceneManager.UnloadSceneAsync(m_CurrentLoadedScene));
        m_ScenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.Title_Screen, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public IEnumerator GetSceneLoadProgress()
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

                //m_ProgressBar.value = Mathf.RoundToInt(m_TotalSceneProgress);

                yield return null;
            }
        }
    }
}
