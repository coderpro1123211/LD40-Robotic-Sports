using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class StartTimer : MonoBehaviour {

    public float timeToStart;
    float t;

    public RigidbodyFirstPersonController player;
    public AI ai;

    public Image crosshair;
    public Text text;
    int lastInt;

    public AudioClip low;
    public AudioClip high;
    public AudioSource music;

	void Start ()
    {
        t = timeToStart;
        lastInt = Mathf.CeilToInt(t);
        crosshair.enabled = false;
        player.enabled = false;
        ai.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        music.Stop();
        Cursor.visible = false;
	}

	void Update ()
    {
        t -= Time.deltaTime;
        if (Mathf.CeilToInt(t) != lastInt)
        {
            lastInt = Mathf.CeilToInt(t);
            if (lastInt == 0)
            {
                AudioSource.PlayClipAtPoint(high, Camera.main.transform.position);

                music.Play();
            }
            else
            {
                AudioSource.PlayClipAtPoint(low, Camera.main.transform.position);
            }
        }
        text.text = Mathf.CeilToInt(t) + "";
        if (t < 0)
        {
            crosshair.enabled = true;
            player.enabled = true;
            ai.enabled = true;
            text.text = "";
            Destroy(this);
        }
	}
}
