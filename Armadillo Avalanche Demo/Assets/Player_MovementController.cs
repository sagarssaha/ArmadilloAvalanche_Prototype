using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
public class Player_MovementController : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;

    Rigidbody rb;
    NavMeshAgent player_agent;
    public GameObject playerBody;
    public float speed;
    public float smoothTime;
    float currentVelocity;
    public float dash_cooldown_Time;
    public float dash_force;
    public bool canDash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody>();
        player_agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // Read the "Move" action value, which is a 2D vector
        // and the "Jump" action state, which is a boolean value

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        if (moveValue.magnitude != 0)
        {
            //rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(moveValue.x, 0, moveValue.y) * speed, 1);
            player_agent.Move(new Vector3(moveValue.x, 0, moveValue.y) * speed*Time.deltaTime);
            RotateTowardsInput(moveValue);

            // rb.transform.LookAt(moveValue);
        }

        // your movement code here

        if (jumpAction.IsPressed() && canDash)
        {
            canDash = false;
            Dash(moveValue);
        }
    }

    void RotateTowardsInput(Vector3 dir)
    {
       
        float targetAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(playerBody.transform.eulerAngles.y, targetAngle,ref currentVelocity, smoothTime);
        playerBody.transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    void Dash(Vector3 dir)
    {
        Beat_Conductor.Instance.CheckInputAccuracy();
        //player_agent.Move(new Vector3(dir.x, 0, dir.y) * speed / 2);
        player_agent.velocity = new Vector3(dir.x, 0, dir.y) * dash_force;
        Invoke(nameof(resetDash), dash_cooldown_Time);
    }

    void resetDash()
    {
        canDash = true;
    }
}

