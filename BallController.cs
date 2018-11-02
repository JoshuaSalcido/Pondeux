/* BallController.cs
 *
 * Contains functions for ball movement, collision, and other game behaviors. 
 * Also certain public variables to change max speed, speed multplier, sfx, particles, etc.
 */ 

using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C# and for Gamepad Rumble (not mine, will source later)

public class BallController : MonoBehaviour {

	private float xRandom; 
	private float yRandom;
	private Vector2 direction;
	
	public float speed;
    private float OGSpeed;
	public float speedMax;  
	public float speedMultiplier; //KEEP SPEED MULTIPLIER VALUES FROM .1-.5F FOR NOW 

    public float torque = 1000;

    private Rigidbody2D rb; 

	public GameObject particle; 

	private GameManager GM;

    public bool isActive = false;

    private bool rumble_L, rumble_R; // checks if either paddle has triggered a rumble

    public TrailRenderer trailRenderer;
    public GameObject circleRenderers; //

    private SlomoMeter slomoMeter;

    //sfx clips
    public AudioClip paddleImpact;
    public AudioClip ballImpact;
    public AudioClip obstacleImpact;
    public AudioClip wallImpact;
    public AudioClip SFX_BallExit;
    //public AudioClip itemCubeImpact;


    // Use this for initialization
	void Awake () {
		GM = GameObject.Find ("GameManager").GetComponent<GameManager> ();
        slomoMeter = GameObject.Find("GameManager").GetComponent<SlomoMeter>(); 
		rb = GetComponent<Rigidbody2D>();
        OGSpeed = speed;
	}

	void FixedUpdate() {

		rb.velocity = speed * (rb.velocity.normalized);

        float turn = Input.GetAxis("Horizontal");
        float axis = transform.position.x;
        rb.AddTorque(axis * torque);  // to rotate your ball  

		//Check if ball is still in play rea
        if (transform.position.x < -28 && isActive) {
            AudioManager.instance.PlaySingle(SFX_BallExit);
            Destroyed();
        }

        if (transform.position.x > 28 && isActive) {
            AudioManager.instance.PlaySingle(SFX_BallExit);
            Destroyed();
        }
    }

	//Initializes ball trajectory, speed, and linerenderer 
	public void SpawnBall() {
        isActive = true;
        xRandom = Random.Range(-1f, 1f);
        yRandom = Random.Range(-1f, 1f);
        direction = new Vector2(xRandom, yRandom);

        GetComponent<Rigidbody2D> ().velocity = direction * speed;
        trailRenderer.enabled = true;
        circleRenderers.GetComponentInChildren<LineRenderer>().enabled = true; 
        
	}

	//Returns y location from ball/paddle impact point
	float hitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleHeight){
		return (ballPos.y - paddlePos.y) / paddleHeight; 
	}

	void OnCollisionEnter2D (Collision2D col){
		if (col.gameObject.name == "Paddle_R") {
            AudioManager.instance.RandomizeSFX(paddleImpact, paddleImpact);
            SpeedMultiply();

            rumble_R = true;
            StartCoroutine(RumbleEvent());

            float y = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
			Vector2 dir = new Vector2 (-.5f, y).normalized; 

			GetComponent<Rigidbody2D>().velocity = dir * speed;
        } else if (col.gameObject.name == "Paddle_L") {
            //Debug.Log ("I hit L Paddle"); 
            AudioManager.instance.RandomizeSFX(paddleImpact, paddleImpact); 
            SpeedMultiply();

            rumble_L = true;
            StartCoroutine(RumbleEvent());

            float y = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
			Vector2 dir = new Vector2 (.5f, y).normalized; 

			GetComponent<Rigidbody2D>().velocity = dir * speed;
        } else if (col.gameObject.tag == "Obstacle") {
            slomoMeter.ObstacleCollision(col.gameObject.GetComponent<Obstacles>().scoreValue);
			SpeedMultiply();
            StartCoroutine(RumbleEvent());
        } else if (col.gameObject.name == "Ceiling" || col.gameObject.name == "Floor") {
            AudioManager.instance.RandomizeSFX(wallImpact, wallImpact);
            StartCoroutine(RumbleEvent());
        }
    }

	public void Destroyed () {
        circleRenderers.GetComponentInChildren<LineRenderer>().enabled = false;
        trailRenderer.enabled = false; 
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        this.transform.position = GM.resetLocations[0].transform.position; // set this balls location out of game zone
        
        GM.ballCount--;
        speed = OGSpeed;
        isActive = false;
    }

    public void SpeedMultiply() {
		float tempSpeed; 
		
		if (speed != speedMax) {
			tempSpeed = speed + speedMultiplier; 
			speed = tempSpeed;
			tempSpeed = 0;  
		}
	}
	

    //Simple method that triggers the gamepad to rumble. For now it's called whenever the ball collides with the either paddle
	IEnumerator RumbleEvent(){
        if (rumble_L) {
            GamePad.SetVibration(0, 10, 3);
            yield return new WaitForSeconds(.075f);
            GamePad.SetVibration(0, 0, 0);
            rumble_L = false;
        } else if(rumble_R) {
            GamePad.SetVibration(0, 3, 10);
            yield return new WaitForSeconds(.075f);
            GamePad.SetVibration(0, 0, 0);
            rumble_R = false;
        } else {
            GamePad.SetVibration(0,.35f, .35f);
            yield return new WaitForSeconds(.075f);
            GamePad.SetVibration(0, 0, 0);
        }
        StopCoroutine(RumbleEvent());
    }
}
