using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Camera eyes;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3.0f;
    public float sprintMultiplier = 2f;
    public float crouchHeightPercent = 0.5f;
    public Transform cylinder;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private float standardHeight;
    private float crouchSpeed = 1.0f;

    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        standardHeight = controller.height;
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        // Crouching
        if (Input.GetButton("Fire1"))
        {
            if (controller.height > standardHeight * crouchHeightPercent)
                controller.height -= Time.deltaTime * crouchSpeed;
        }
        else
        {
            if (controller.height < standardHeight)
                controller.height += Time.deltaTime * crouchSpeed;
        }

        // Move slower while smaller
        float ratio = controller.height / standardHeight;
        move *= ratio * ratio;

        controller.center = new Vector3(0f, -(3.9f - controller.height) / 2, 0f);
        eyes.transform.localPosition = new Vector3(0f, (controller.height) - 2.1f, 0f);

        // IRRELEVANT FOR LATER JUST CHANGE CYLINDER APPEARANCE FOR NOW, REALLY NO FUNCTIONALITY
        cylinder.localScale = new Vector3(1f, 1.8f * ratio, 1f);
        cylinder.localPosition = new Vector3(0f, -(3.9f - controller.height) / 2, 0f);

        // Sprinting
        if (!Input.GetButton("Fire1") && Input.GetButton("Fire3"))
        {
            move *= sprintMultiplier;
        }
        controller.Move(move * speed * Time.deltaTime);


        // FUN CROUCH INCREASE JUMP LIKE SPRING
        float jumpMultiplier = 1/(ratio*ratio);
        
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity * jumpMultiplier);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }
}
