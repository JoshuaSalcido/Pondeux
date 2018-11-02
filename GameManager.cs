using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int totalScore = 0;
    public int currentGameSessions = 0;
    public int totalGameSessions;

    public bool currentGameActive = false; 

    private Messages messages;

    private SlomoMeter slomoMeter;
    

	//Obstacle Spawner variables

    public bool canSpawn = false;

    public ObstacleSpawner ObsSpwnr;
    public GameObject[] obstacles; //
    public AreaChecker sCollider;
    public AreaChecker lCollider;

    public bool isChecking = false;

    float timer; // keep track of how much time it's been since the last spawn 
    public bool canCount = false; //this bool turns HUD timer on or off


    public Text timerText;

    public int secCount = 0;
    public string S_secCount;
    public int minCount = 0;
    public string S_minCount;
    public float milliCount = 0f;
    public string S_milliCount;


    public int objIndex;
    public int obstacleCount = 0; // keep track of total obstacles in the scene
    public int objLimit = 0;

    /// <summary>
    /// Ball parameters 
    /// </summary>
    public GameObject[] _ballPool;
    public bool spawnBall;
    public int ballCount = 0;
    public int ballLimit = 2;

    public float particleDestroyTime;
    public GameObject ball;
    public GameObject[] balls;
    public GameObject spawnPoint;
    public GameObject[] resetLocations;
    private GameObject[] particles;


    public bool spaceAvailable = false;


    //HUD/GUI variables
    public GameObject MainMenu; 
    public GameObject MainHUD;
    public GameObject PauseMenu;
    public GameObject[] confirmationMenus;
    public bool confirmationListActive = false; 

    public int OG_Count = 0;

    public float spawnLevel;
    public float timeFactor, scoreFactor;

    int temp;

    public int spawnTime = 3;
    public int lastSecCount;

    public Transform itemCube;
	private GameObject _GO_itemCube;

    
    public AudioClip SFX_FailState;


	// Use this for initialization
    void Awake() {
        slomoMeter = GetComponent<SlomoMeter>();

        messages = GameObject.Find("Canvas_Messages").GetComponent<Messages>();

		itemCube = GameObject.Find("ItemCube").GetComponent<Transform>();


        sCollider = GameObject.Find("sCollider").GetComponent<AreaChecker>();
        lCollider = GameObject.Find("lCollider").GetComponent<AreaChecker>();


        ObsSpwnr = GameObject.Find("GameManager").GetComponent<ObstacleSpawner>();
        balls = GameObject.FindGameObjectsWithTag("Ball");

        
        _GO_itemCube = GameObject.Find("ItemCube"); // **Temp** this Game Object should be called from a public variable the designer can
		//assign in the inspector. 


        Time.timeScale = 1f;
    }
		
 
	// Update is called once per frame
	void FixedUpdate () {

        if (!MainMenu.activeSelf && !PauseMenu.activeSelf) {
            if (!currentGameActive && !CheckConfirmationMenus()) {
                currentGameActive = true;
                StartCoroutine(Init_Game());
            }
        }
        
		if (PauseMenu.activeSelf) {
            currentGameActive = false;
        }
        
        timer += Time.deltaTime;
     
        milliCount = (int)(Time.timeSinceLevelLoad * 1000f) % 1000;
      
        S_minCount = minCount.ToString();
        S_minCount = S_minCount.Insert(1, " ");

        S_secCount = secCount.ToString();
        S_secCount = S_secCount.Insert(1, " ");

        S_milliCount = milliCount.ToString();
        S_milliCount = S_milliCount.Insert(1," ");

       
        if (canCount) {
            if (minCount < 10) {
                if (secCount < 10) {
                    if (milliCount < 100) {
                        timerText.text = "0 " + S_minCount + "  0 " + S_secCount + "  0 " + S_milliCount.Remove(S_milliCount.Length - 1);
                    } else {
                        timerText.text = "0 " + S_minCount + "  0 " + S_secCount + "  " + S_milliCount.Remove(S_milliCount.Length - 1);
                    }
                } else {
                    if (milliCount < 100) {
                        timerText.text = "0 " + S_minCount + "  " + S_secCount + "   0 " + S_milliCount.Remove(S_milliCount.Length - 1);
                    } else {
                        timerText.text = "0 " + S_minCount + "  " + S_secCount + "   " + S_milliCount.Remove(S_milliCount.Length - 1);
                    }
                }
            } else {
                if (secCount < 10){
                    if (milliCount < 100) {
                        timerText.text = S_minCount + "   0 " + S_secCount + "  0 " + S_milliCount.Remove(S_milliCount.Length - 1);
                    } else {
                        timerText.text = S_minCount + "   0 " + S_secCount + "  " + S_milliCount.Remove(S_milliCount.Length - 1);
                    }
                } else {
                    if (milliCount < 100) {
                        timerText.text = S_minCount + "   " + S_secCount + "   0 " + S_milliCount.Remove(S_milliCount.Length - 1);
                    } else {
                        timerText.text = S_minCount + "   " + S_secCount + "   " + S_milliCount.Remove(S_milliCount.Length - 1);
                    }
                }
            }
        }
        
        obstacleCount = GameObject.FindGameObjectsWithTag("Obstacle").Length;
     
		//Destory any remaining partcle effects 
		particles = GameObject.FindGameObjectsWithTag ("Particle");
		for (int i = 0; i < particles.Length; i++) {
			Destroy (particles [i], particleDestroyTime); 
		}
	}

    public void BeginGame() {
        if (currentGameActive && currentGameSessions == 0) {
            messages.StartGameMessage();
        } else if (currentGameActive && currentGameSessions > 0) {
            canCount = true;
            currentGameSessions++;
            StartCoroutine(OneSecondTimer());  
            SpawnBall();
        }
    }

    public bool CheckConfirmationMenus() {
		for (int i = 0; i < confirmationMenus.Length; i++) {
            print("going through menus");

            if (!confirmationMenus[i].activeSelf) {
                confirmationListActive = false;
            }
            if (confirmationMenus[i].activeSelf) {
                confirmationListActive = true;
                break;
            }
        }
        return confirmationListActive;
    }

    public void SpawnBall() {
		
        if (currentGameActive && currentGameSessions > 0) {
            for (int i = 0; i <= 2; i++) {
                GameObject tempGO = balls[i] as GameObject;

				if (tempGO.GetComponent<BallController>().isActive == false) {
                    _ballPool[i].transform.position = spawnPoint.transform.position;
                    _ballPool[i].GetComponent<BallController>().SpawnBall();
                    ballCount++;
                    break; 
                }
            }
        }
    }
 

	public IEnumerator Init_Game() {
		yield return new WaitForSeconds(1.5f); // Delay HUD timer so it starts when ball is spawned
        BeginGame();

        print("Delay finished");
        StopCoroutine(Init_Game());
    }

    public IEnumerator Delay() {
        yield return new WaitForSeconds(1.5f);
        print("Delay finished");
        StopCoroutine(Delay());
    }
		
	public IEnumerator OneSecondTimer() {
        while (canCount && 1 == 1) {
			
            //TRIGGER FAILSTATE: NO ACTIVE BALLS ON SCREEN
            if (currentGameSessions > 0 && messages.messageFinished && ballCount <= 0) {
                if (currentGameActive == true) {
                    print("no balls");
                    AudioManager.instance.PlaySingle(SFX_FailState);
                    ResetGame();
                }
            }


            yield return new WaitForSeconds(1.0f);
            milliCount = 0;
            secCount++;
         
            timeFactor = spawnLevel * .0015f;
        
            scoreFactor = .015f * (spawnLevel + totalScore);
            spawnLevel += (secCount * timeFactor) + scoreFactor;
            temp = (int)spawnLevel;
		
            ObsSpwnr.ObjectGenerator();

            if (secCount == 60) {
                secCount = 0;
                minCount++;
            }

            if (secCount == lastSecCount + spawnTime) {
                lastSecCount = secCount;
                canSpawn = true;
            }
        }
	}

	//Reset When current game session is failed or player quits from Pause Menu. 
    public void ResetGame() {
        canCount = false;
        canSpawn = false;
        ObsSpwnr.canMoveAreaChkr = false; 
        StopCoroutine(OneSecondTimer());
        StopCoroutine(messages.Delay());

        lastSecCount = 0; 
        secCount = 0;
        minCount = 0;
        milliCount = 0;

        spawnLevel = 0;
        timeFactor = 0;
        scoreFactor = 0;

        timerText.text = "0 0   0 0   0 0";

        totalScore = 0;

        for (int i = 0; i < slomoMeter.sliders.Length; i++) {
            slomoMeter.sliders[i].value = 0; 
        }

        GetComponent<Bomb>().ResetBomb();
        GetComponent<Restart>().DestoryAllObjects();

        for (int i = 0; i < _ballPool.Length; i++) {
            _ballPool[i].GetComponent<BallController>().Destroyed();
        }
        ballCount = 0;
        if (MainMenu.activeSelf && GetComponent<PauseMenu>().isPaused == true) {
            GetComponent<PauseMenu>().TogglePause();
        }
        StartCoroutine(Init_Game()); 
    }
}
