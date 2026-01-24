using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuUI : MonoBehaviour
{
    private const string DungeonMapSceneName = "DungeonMap";

    // Méthode appelée par le bouton Start
    public void OnStartGame()
    {
        if (!Application.CanStreamedLevelBeLoaded(DungeonMapSceneName))
        {
            Debug.LogError($"[MainMenuUI] Scène introuvable ou non incluse: {DungeonMapSceneName}");
            return;
        }
        SceneManager.LoadScene(DungeonMapSceneName);
    }

    // Méthode appelée par le bouton Quit
    public void OnQuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
