
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void SelectLevel(int levelNumber)
    {
        string sceneName = "Nivel" + levelNumber;

        SceneManager.LoadScene(sceneName);
    }
}
