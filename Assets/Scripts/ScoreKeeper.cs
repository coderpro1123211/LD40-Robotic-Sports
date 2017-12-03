using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.PostProcessing;

public class ScoreKeeper : MonoBehaviour {

    public int p1Score;
    public int p2Score;

    public Texture2D angry;
    public Texture2D neutral;
    public Texture2D happy;
    public Material face;

    public static ScoreKeeper Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(this); return; }
        Instance = this;
    }

    public void Punish(Player player)
    {
        var p = player;
        int area = (int)Mathf.Sign(p.transform.position.z);
        float ballsNormalized = (float)GlobalBallController.Instance.GetBallCountOnSide(area) / GlobalBallController.Instance.ballCount;
        if (!(player.gettingDunked || !player.canGetDunked)) p.hp -= Mathf.CeilToInt(ballsNormalized) * 3;
        if (p.hp <= 0)
        {
            Debug.Log("Game over for player " + p.name);
            // TODO: Game over here :(
            if (player.isAi)
            {
                FindObjectOfType<GameOverText>().EndGame("won");
                player.GetComponent<AI>().GoFuckingDie();
            }
            else
            {
                FindObjectOfType<GameOverText>().EndGame("lost");
            }
            return;
        }
        var f = p.fpsc.movementSettings.ForwardSpeed;
        var b = p.fpsc.movementSettings.BackwardSpeed;
        var sp = p.fpsc.movementSettings.StrafeSpeed;
        Debug.Log("geeeeeeeeeeetttt dunkkkkked onn");
        if (player.isAi)
        {
            face.SetTexture("_EmissionMap", angry);
            p1Score++;
            float origSpd = player.GetComponent<AI>().speed;
            player.GetComponent<AI>().EnableShield();
            player.GetComponent<AI>().speed = origSpd * Mathf.Clamp01(1.1f - ballsNormalized);
            StartCoroutine(ResetPlayer(null, null, p, ballsNormalized * 10, origSpd, b, sp, true));
            return;
        }
        else
        {
            face.SetTexture("_EmissionMap", happy);
            p2Score++;
        }
        if (player.gettingDunked || !player.canGetDunked) return;

        var pp = p.GetComponentInChildren<PostProcessingBehaviour>().profile;

        var nf = Mathf.Clamp01(1.1f - ballsNormalized) * f;
        var nb = Mathf.Clamp01(1.1f - ballsNormalized) * b;
        var nsp = Mathf.Clamp01(1.1f - ballsNormalized) * sp;

        p.fpsc.movementSettings.ForwardSpeed = nf;
        p.fpsc.movementSettings.BackwardSpeed = nb;
        p.fpsc.movementSettings.StrafeSpeed = nsp;

        var s = pp.vignette.settings;
        s.intensity = 0.2f + (ballsNormalized-0.2f);
        pp.vignette.settings = s;

        var s2 = pp.grain.settings;
        s2.intensity = 0.18f + (ballsNormalized-0.18f);
        pp.grain.settings = s2;
        p.gettingDunked = true;
        p.GetComponentInChildren<ImageEffect>().enabled = true;

        StartCoroutine(ResetPlayer(pp, p.GetComponentInChildren<ImageEffect>(), p, ballsNormalized * 10, f, b, sp, false));
    }

    public IEnumerator ResetPlayer(PostProcessingProfile p, ImageEffect eff, Player pl, float time, float f, float b, float sp, bool ai)
    {
        yield return new WaitForSecondsRealtime(time);
        if (p != null)
        {
            var s = p.vignette.settings;
            s.intensity = 0.2f;
            p.vignette.settings = s;

            var s2 = p.grain.settings;
            s2.intensity = 0.18f;
            p.grain.settings = s2;

            pl.fpsc.movementSettings.ForwardSpeed = f;
            pl.fpsc.movementSettings.BackwardSpeed = b;
            pl.fpsc.movementSettings.StrafeSpeed = sp;
        }
        if (ai)
        {
            pl.GetComponent<AI>().speed = f;
            pl.GetComponent<AI>().DisableShield();
        }
        yield return new WaitForSecondsRealtime(1);
        pl.gettingDunked = false;
        face.SetTexture("_EmissionMap", neutral);
        if (eff != null) eff.enabled = false;
    }
}
