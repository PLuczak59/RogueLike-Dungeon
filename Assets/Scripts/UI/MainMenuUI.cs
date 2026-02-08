using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuUI : MonoBehaviour
{
    private const string StartGameScene = "DungeonRoom";

    public void OnStartGame()
    {
        if (!Application.CanStreamedLevelBeLoaded(StartGameScene))
            return;
            
        SceneManager.LoadScene(StartGameScene);
    }

    public void OnQuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
