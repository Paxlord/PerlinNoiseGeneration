using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

    CharacterController cc;
    public Camera characterCamera;

    float rotationX;
    float sensitivityX = 2.0f;
    float speed = 10;

    Vector3 moving = new Vector3();

	// Use this for initialization
	void Start () {
        cc = this.GetComponent<CharacterController>();
        this.transform.position = new Vector3(0, 100, 0);
	}
	
	// Update is called once per frame
	void Update () {

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        rotationX += horizontal * sensitivityX;

        Quaternion rotation = Quaternion.AngleAxis(rotationX, Vector3.up);

        transform.rotation = rotation;

        if (cc.isGrounded)
        {
            moving = transform.forward * vertical * speed;

            if (Input.GetButtonDown("Jump"))
            {
                moving.y = 60;
            }
        }
       

        
        moving.y -= 200.0f * Time.deltaTime;

        cc.Move(moving * Time.deltaTime);

	}
}
