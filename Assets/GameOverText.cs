using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GameOverText : MonoBehaviour {
    public float val;
    public Text text;
    public Image crosshair;
    Animator anim;
    bool gameEnd;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update ()
    {
        text.lineSpacing = val;

        if (!gameEnd) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
	}

    public void EndGame(string state)
    {
        text.text = "YOU " + state + "\nplay AGAIN?\n\n[ENTER]yes    [ESC]no";
        anim.SetTrigger("gameend");
        crosshair.enabled = false;
        FindObjectOfType<ScoreKeeper>().ResetPlayerNow();

        gameEnd = true;
    }
}
