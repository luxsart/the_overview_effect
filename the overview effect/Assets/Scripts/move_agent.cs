using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class move_agent : MonoBehaviour
{
    private Transform goal;
    private NavMeshAgent agent;
    private float distance;
    private bool is_following = false;
    private bool found_home = false;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    Animator anim;

    public string spaceman_goal = "City A";
    public float rotationSpeed = 5.0f;
    public float start_follow_distance = 4.0f;
    public float stop_follow_distance = 30.0f;
    public float stop_waving_distance = 20.0f;


    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        velocity = agent.velocity;
        bool shouldMove = velocity.magnitude > 0.1f;// && agent.remainingDistance > agent.radius;

        // Update animation parameters
        anim.SetBool("move", shouldMove);
        anim.SetFloat("velx", velocity.x);
        anim.SetFloat("vely", velocity.y);

        // GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
        // agent.destination = goal.position;

        //agent.isStopped = true; agent.ResetPath();
        distance = Vector3.Distance(transform.position, goal.position);
        if (found_home == false)
        {
            if (is_following == false)
            {
                if (distance < start_follow_distance)
                {
                    anim.Play("Wave");
                    is_following = true;
                } else if (distance > stop_waving_distance && distance < stop_follow_distance * 1.5f) {
                    anim.Play("BigWave");
                }
                if (distance < stop_follow_distance * 1.5f)
                {
                    RotateTowards(goal.transform);
                }
            }
            else if (distance < start_follow_distance)
            {
                RotateTowards(goal.transform);
            }
            else if (distance > start_follow_distance && distance < stop_follow_distance)
            {
                agent.destination = goal.position;
            }
            else
            {
                agent.isStopped = true;
                agent.ResetPath();
                is_following = false;
            }
        }
        else if (distance < stop_follow_distance*0.5f)
        {
            anim.Play("BigWave");
            RotateTowards(goal.transform);
        }
        print(found_home);
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == spaceman_goal)
            found_home = true;
    }
}
