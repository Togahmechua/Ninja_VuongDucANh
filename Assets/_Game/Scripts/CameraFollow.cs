using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float speed = 20f;

    void Start()
    {
        target = FindObjectOfType<Player>().transform;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position , target.position + offset, speed * Time.deltaTime);
    }
}
