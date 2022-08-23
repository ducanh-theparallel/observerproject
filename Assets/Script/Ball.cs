using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoObserver;

public class Ball : MonoBehaviour
{
    static public float high;

    void Update()
    {
        high = transform.position.y + 1;
        this.PostEvent(EventID.HighInSlide);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plane"))
        {
            this.PostEvent(EventID.OnCollisionBall);
        }
    }
}
