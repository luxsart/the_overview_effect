using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class behaviour : MonoBehaviour
{
    private Transform player;
    public float smoothTime = 10.0f;
    private Vector3 smoothVelocity = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        print(player);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        transform.position = Vector3.SmoothDamp(transform.position, player.position, ref smoothVelocity, smoothTime) * Time.deltaTime;
    }
}
