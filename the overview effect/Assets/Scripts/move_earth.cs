using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_earth : MonoBehaviour
{
    public float smooth = 1f;
    private Quaternion targetRotation;
    void Start()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * (smooth * Time.deltaTime));
    }
}
