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

	void Start ()
    {
        t = timeToStart;
        crosshair.enabled = false;
        player.enabled = false;
        ai.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}

	void Update ()
    {
        t -= Time.deltaTime;
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
