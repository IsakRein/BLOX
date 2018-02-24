using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void Load(string scene)
    {
        if (scene == "Gameover") {
            Manager.NextTimeDontLoadLevel();
            Manager.loadColors = false;
        }

        Manager.sceneOrder.Add(SceneManager.GetActiveScene().name);

        SceneManager.LoadScene(scene);
    }

    public void LoadLastScene() {
        string sceneToLoad = Manager.sceneOrder[Manager.sceneOrder.Count-1];
        Manager.sceneOrder.RemoveAt(Manager.sceneOrder.Count-1);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ReloadScene(string scene) {
		if (scene == "Game") {
            Manager.loadColors = false;
  		}

        SceneManager.LoadScene(scene);
    }
}