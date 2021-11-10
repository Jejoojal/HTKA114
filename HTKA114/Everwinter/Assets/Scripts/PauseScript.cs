using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour {

    public static bool GameIsPaused;
    public GameObject pauseMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);
        GameIsPaused = false;
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) Resume();
            else Pause();
        }
    }

    public void Resume () {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; //TODO: ei toimi >:/ 
        GameIsPaused = false;
        Cursor.visible = false;
        AudioListener.volume = 1f;

    }

    void Pause ()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; //TODO: ei toimi >:/
        GameIsPaused = true;
        Cursor.visible = true;
        AudioListener.volume = 0f; //ajan pysäytyksen sijaan laitan äänet pois

    }

    public void Restart()
    {
        AudioListener.volume = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //ei toimi editorin puolella, mutta .exe:ssä kyllä
    }

}
