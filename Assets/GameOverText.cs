using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GameOverText : MonoBehaviour {
    public float val;
    public Text text;
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
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
	}

    public void EndGame(string state)
    {
        text.text = "YOU " + state + "\nplay AGAIN?\n\n[ENTER]yes    [ESC]no";
        anim.SetTrigger("gameend");

        gameEnd = true;
    }
}
