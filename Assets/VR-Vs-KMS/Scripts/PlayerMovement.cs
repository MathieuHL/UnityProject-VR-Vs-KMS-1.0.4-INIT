using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerMovement : MonoBehaviour
{
    CharacterController player;
    private Vector3 movement;
    private float Speed = 10.0f;
    private float gravity = -25.0f;
    private float verticalForce = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded)
        {
            verticalForce = 0;
            if (Input.GetButton("Jump"))
            {
                verticalForce = 8;
            }
        }
        else
        {
            verticalForce += gravity * Time.deltaTime;
        }

        movement = new Vector3(Input.GetAxis("Horizontal") * Speed, verticalForce, Input.GetAxis("Vertical") * Speed);
        movement = transform.TransformDirection(movement);
        player.Move(movement * Time.deltaTime);
    }
}
