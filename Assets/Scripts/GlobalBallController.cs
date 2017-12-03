using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GlobalBallController : MonoBehaviour {

    public static GlobalBallController Instance;

    public Transform[] ballSpawnPos;

    public GameObject ballPrefab;
    public int ballCount;

    Ball[] balls;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        balls = new Ball[Mathf.Max(ballCount, ballSpawnPos.Length)];

        var i = 0;
        foreach(var t in ballSpawnPos)
        {
            var go = Instantiate(ballPrefab, t.position, t.rotation, null);
            balls[i++] = go.GetComponent<Ball>();
        }
        FindObjectOfType<AI>().balls = new List<Ball>(balls);
    }


    // -1 is negative z, 1 is positive z
    public int GetBallCountOnSide(int side)
    {
        if (balls == null) return 1;
        int c = 0;
        foreach(Ball b in balls)
        {
            if (b.transform.position.z * side > 0)
            {
                c++;
            }
        }
        return c;
    }
}
