using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int sceneId)
    {
        Cursor.visible = true;
        SceneManager.LoadScene(sceneId);
    }
}
