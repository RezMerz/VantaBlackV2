﻿using UnityEngine;
using System.Collections;

public class UnityPhysics : MonoBehaviour {
    private Rigidbody2D rb;
    private float hoverHeight = 0.5f;
    private float hoverForce = 50;
    private float powerInput;
    private float speed = 50;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        powerInput = Input.GetAxis("Horizontal");
    }


    void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            Vector2 input = new Vector2(powerInput * speed, 0);

            Ray2D ray = new Ray2D(transform.position, -transform.up);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + new Vector2(0, -0.5f), -transform.up, 0.5f);
            RaycastHit2D hit2 = Physics2D.Raycast((Vector2)transform.position + new Vector2(0.5f, 0), transform.right, 0.5f);
            if (hit)
            {
                float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
                Vector2 force = hit.normal;
                Vector3 appliedHoverForce = force * proportionalHeight * hoverForce;
                rb.AddForce(appliedHoverForce, ForceMode2D.Force);
            }

            if (powerInput >= 0.4f)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if(powerInput <= -0.4f)
                transform.rotation = Quaternion.Euler(0, 180, 0);

            rb.AddRelativeForce(input);

        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ramp")
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(powerInput , powerInput );
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        
    }
}