/* PlayerController.cs
*
* Contains functions for controlling paddles. 
*
*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public bool isL_Paddle;
    public float minY, maxY; 

    private float v;

    public bool canMove;

    private Rigidbody2D r_RB, l_RB;
    private Transform r_Pos, l_Pos;

    //particles for boost
    ParticleSystem particle_LTop, particle_LBottom, particle_RTop, particle_R_Bottom; 


    // Use this for initialization
    void Awake () {

		//need to make these public variables so particle gameobjects can be 
		//assigned in Inspector. 
        particle_LTop = GameObject.Find("blue star_top").GetComponent<ParticleSystem>();
        particle_LBottom = GameObject.Find("blue star_Bottom").GetComponent<ParticleSystem>();
        particle_RTop = GameObject.Find("blue star_top1").GetComponent<ParticleSystem>();
        particle_R_Bottom = GameObject.Find("blue star_Bottom1").GetComponent<ParticleSystem>();

        canMove = true;

        if (!isL_Paddle) {
            r_RB = GetComponent<Rigidbody2D>();
            r_Pos = GetComponent<Transform>();

        } else {
            l_RB = GetComponent<Rigidbody2D>();
            l_Pos = GetComponent<Transform>();
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (canMove) {
            if (!isL_Paddle) {
                if (Input.GetAxis("RightTrigger") > 0) {
                    //Apply velocity to RIGHT Paddle based on float output from joystick
                    v = Input.GetAxisRaw("RightJoyStickVertical");
                    r_RB.velocity = new Vector2(0, v) * (speed * 1.75f);

                    if (v > .01f) {
                        particle_R_Bottom.Play();
                    } else if (v < 0) {
                        particle_RTop.Play();
                    } else { 
						//Do nothing
					}
                } else {
                    particle_R_Bottom.Stop();
                    particle_RTop.Stop();
                    //Apply velocity to RIGHT Paddle based on float output from joystick
                    v = Input.GetAxisRaw("RightJoyStickVertical");
                    r_RB.velocity = new Vector2(0, v) * speed;
                }

            
                //** IM STILL ADJUSTING THE 4.95F AND MINY AND MAXY VALUES SO THAT THE 
                //PADDLE DOESNT JUMP A UNITS WHEN THE STICK IS LET GO (REFER TO NOTES **//
                if (transform.position.y > 9f || transform.position.y <= -9f) {
                    //Freeze y position when paddle is about to leave game screen 
                    r_Pos.position = new Vector3(r_Pos.position.x, Mathf.Clamp(r_Pos.position.y, minY, maxY), 0);
                }
            } else {
                if (transform.position.y > 9f || transform.position.y <= -9f) {
                    // print("Beyond gamezone");
                    //Freeze y position when paddle is about to leave game screen 
                    l_Pos.position = new Vector3(l_Pos.position.x, Mathf.Clamp(l_Pos.position.y, minY, maxY), 0);
                }

                if (Input.GetAxis("LeftTrigger") > 0) {
                    //-print("pulling left trigger");
                    //Apply velocity to LEFT Paddle based on float output from joystick
                    v = Input.GetAxisRaw("Vertical");
                    l_RB.velocity = new Vector2(0, v) * (speed * 1.75f); //1,75-2.25 apeed mulitplier
                    
					if (v > .01f) {
                        particle_LBottom.Play();
                    } else if(v < 0) {
                        particle_LTop.Play(); 
                    } else { 
					//Do nothing
					}
                } else {
                    particle_LBottom.Stop();
                    particle_LTop.Stop();
                    //Apply velocity to LEFT Paddle based on float output from joystick
                    v = Input.GetAxisRaw("Vertical");
                    l_RB.velocity = new Vector2(0, v) * speed;
                }
            }
        } else {
            //Stick should not move paddles
        }
    }
}
