using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour {

    bool isThrowing;
    int l;
    float spd;
    float gravityScale;
    Vector3 dir;
    Rigidbody b;
    public bool canHitPlayer;
    bool pThrow;

    private void Start()
    {
        b = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 dir, float spd, float gravityScale, LayerMask layer, bool playerThrow)
    {
        this.dir = dir;
        this.spd = spd;
        this.gravityScale = gravityScale;
        this.l = layer;
        canHitPlayer = true;
        pThrow = playerThrow;

        b.velocity = dir * spd;
        b.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(pThrow ? "AI" : "Player") && canHitPlayer)
        {
            ScoreKeeper.Instance.Punish(collision.gameObject.GetComponent<Player>());
        }
        canHitPlayer = false;
    }

    void Update()
    {
        //if (isThrowing)
        //{
        //    dir.y -= Time.deltaTime * gravityScale;
        //    RaycastHit hit;
        //    if (Physics.Raycast(transform.position, dir, out hit, spd*Time.deltaTime, l))
        //    {
        //        Debug.Log("Ball hit something");
        //        if (hit.transform.gameObject.CompareTag("Player"))
        //        {
        //            ScoreKeeper.Instance.Punish(hit.transform.gameObject.GetComponent<Player>());
        //        }
        //    }
        //}
    }
}
