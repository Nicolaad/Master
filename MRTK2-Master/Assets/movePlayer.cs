using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayer : MonoBehaviour
{
     public float speed = 5;
 public float gravity = -5;
 
 float velocityY = 0; 
 
 CharacterController controller;
 
 void Start()
 {
     controller = GetComponent<CharacterController>();
 }
 
 void Update()
 {
     velocityY += gravity * Time.deltaTime;
 
     Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
     input = input.normalized;
 
     Vector3 temp = Vector3.zero;
     if (input.z == 1)
     {
         temp += transform.forward;
     }
     else if (input.z == -1)
     {
         temp += transform.forward * -1;
     }
 
     if (input.x == 1)
     {
         temp += transform.right;
     }
     else if (input.x == -1)
     {
         temp += transform.right * -1;
     }
 
     Vector3 velocity = temp * speed;
     velocity.y = velocityY;
     
     controller.Move(velocity * Time.deltaTime);
 
     if (controller.isGrounded)
     {
         velocityY = 0;
     }
 }
}
