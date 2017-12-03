using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {

    Ball holding;
    bool isHolding;
    Camera main;
    public Slider hpS;
    public Slider charge;
    public bool isAi;
    public ImageEffect imgE;
    public bool canGetDunked;
    public float deflectorDepleteTime;
    public float deflectorChargeTime;
    public LayerMask rayLayers;
    public float ballSpd;
    public GameObject ballPos;
    public GameObject deflector;
    public GameObject graphics;
    public int maxHp;
    public int hp;
    public RigidbodyFirstPersonController fpsc;
    float deflectorCharge;
    bool canDeflect;
    public bool gettingDunked;

	// Use this for initialization
	void Start () {
        hp = maxHp;
        main = GetComponentInChildren<Camera>();
        fpsc = GetComponent<RigidbodyFirstPersonController>();
        imgE = GetComponentInChildren<ImageEffect>();

        canGetDunked = true;
        gettingDunked = false;

        imgE.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        hpS.maxValue = maxHp;
        hpS.value = hp;
        if (isAi) return;
        charge.value = deflectorCharge;
		if (!isHolding && Input.GetMouseButton(0))
        {
            Debug.Log("ye");
            RaycastHit h;
            if (Physics.Raycast(main.transform.position, main.transform.forward, out h, 2, rayLayers))
            {
                if (h.transform.gameObject.CompareTag("Ball"))
                {
                    Debug.Log("boi");
                    holding = h.transform.GetComponent<Ball>();
                    holding.transform.SetParent(ballPos.transform);
                    holding.transform.localPosition = Vector3.zero;
                    holding.GetComponent<Rigidbody>().isKinematic = true;
                    isHolding = true;
                }
            }
            
        }
        else if (isHolding && Input.GetMouseButtonUp(0))
        {
            holding.transform.SetParent(null);
            holding.transform.position = main.transform.position + main.transform.forward;
            holding.GetComponent<Rigidbody>().isKinematic = false;
            holding.Throw(main.transform.forward, ballSpd, 2, rayLayers, true);
            holding.canHitPlayer = true;
            isHolding = false;
        }

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    ScoreKeeper.Instance.Punish(this);
        //}

        if (Input.GetMouseButtonDown(1) && deflectorCharge > 0.5f)
        {
            canDeflect = true;
        }
        if (deflectorCharge < 0.05f) canDeflect = false;

        if (!isHolding && deflectorCharge > 0.01f && Input.GetMouseButton(1) && canDeflect)
        {
            canGetDunked = false;
            deflector.SetActive(true);
            deflectorCharge -= Time.deltaTime / deflectorDepleteTime;
        }
        else if (!isHolding)
        {
            canGetDunked = true;
            deflector.SetActive(false);
            deflectorCharge += Time.deltaTime / deflectorChargeTime;
        }
        deflectorCharge = Mathf.Clamp01(deflectorCharge);
	}
}
