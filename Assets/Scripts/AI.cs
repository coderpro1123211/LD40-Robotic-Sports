using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

    public List<Ball> balls;
    public Transform ballPos;
    public GameObject shield;
    Ball tracked;
    public float speed = 6;

    public float timeBetweenTicks;
    float timeToNext;
    bool hasPickup;
    public Player p;
    LaunchCalculator.LaunchData cur;
    Rigidbody r;

    public GameObject graphics;
    public GameObject dead;

    bool isFuckingDead;

    private void Start()
    {
        isFuckingDead = false;
        Debug.Log(shield == null);
        shield.SetActive(false);
        r = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        if (isFuckingDead) return;
        r.velocity = Vector3.zero;
        timeToNext -= Time.deltaTime;
        if (timeToNext < 0)
        {
            timeToNext = timeBetweenTicks;
            if (hasPickup)
            {
                cur = LaunchCalculator.CalculateLaunchData(-9.81f, tracked.transform, p.transform, 5);
                StartCoroutine(ResetCollisionState(tracked.GetComponent<Collider>()));
                tracked.transform.SetParent(null);
                tracked.GetComponent<Rigidbody>().isKinematic = false;
                tracked.Throw(cur.initialVelocity, 1, 1, 0, false);
                hasPickup = false;
                return;
            }
            if (!hasPickup)
            {
                balls = OrderBalls();
                if (tracked == null) tracked = balls[0];
                var ball = balls[0];
                for (int i = 1; i < balls.Count; i++)
                {
                    if (ball.transform.position.z > 0/* && ball != tracked*/) break;
                    ball = balls[i];
                }
                tracked = ball;
            }

            if (Vector3.Distance(transform.position, tracked.transform.position) < 2)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), tracked.GetComponent<Collider>(), true);
                tracked.transform.SetParent(ballPos);
                tracked.transform.localPosition = Vector3.zero;
                tracked.GetComponent<Rigidbody>().isKinematic = true;
                transform.LookAt(new Vector3(p.transform.position.x, transform.position.y, p.transform.position.z));
                hasPickup = true;
            }
        }
        if (hasPickup) return;
        transform.LookAt(new Vector3(tracked.transform.position.x, transform.position.y, tracked.transform.position.z));
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    public void EnableShield()
    {
        shield.SetActive(true);
        GetComponent<Player>().canGetDunked = false;
    }

    public void DisableShield()
    {
        shield.SetActive(false);
        GetComponent<Player>().canGetDunked = true;
    }

    public void GoFuckingDie()
    {
        graphics.SetActive(false);
        dead.SetActive(true);
        isFuckingDead = true;
        DisableShield();
        dead.transform.SetParent(null);
        gameObject.SetActive(false);
    }

    //private void OnDrawGizmos()
    //{
    //    Vector3 previousDrawPoint = tracked.transform.position;

    //    int resolution = 30;
    //    for (int i = 1; i <= resolution; i++)
    //    {
    //        float simulationTime = i / (float)resolution * cur.timeToTarget;
    //        Vector3 displacement = cur.initialVelocity * simulationTime + Vector3.up * -9.81f * simulationTime * simulationTime / 2f;
    //        Vector3 drawPoint = tracked.transform.position + displacement;
    //        Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
    //        previousDrawPoint = drawPoint;
    //    }
    //}

    List<Ball> OrderBalls()
    {
        var q = from b in balls orderby Vector3.Distance(b.transform.position, transform.position) select b;
        return q.ToList();
    }

    IEnumerator ResetCollisionState(Collider c)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Physics.IgnoreCollision(GetComponent<Collider>(), c, false);
    }
}

public class LaunchCalculator
{
    public static LaunchData CalculateLaunchData(float gravity, Transform ball, Transform target, float h)
    {
        float displacementY = target.position.y - ball.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - ball.position.x, 0, target.position.z - ball.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    public struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }
}
