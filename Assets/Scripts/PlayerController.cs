using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed, jumpSpeed;
    [SerializeField] private LayerMask ground;
    private PlayerMovementControls playerMovementControls;
    private Collider2D col;
    private Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;

    //saves the position of hamster for respawns when scene is loaded
    private PositionController positionController;

    //saves the hamster's health bar and lives when scene is loaded
    private UIController uiController;

    //for setting/getting hamster's max health and current health
    public int maxHealth = 10;
    public int currentHealth;
    public HealthBar healthBar;

    //gets/sets the hamster's lives
    public Lives lives;

    private bool isGrounded=false;

    // triggered when on slippery surface
    public bool isOnSlipperySurface;
    private float accelerationSpeed, surfaceacceleration, surfacedecceleration, surfaceMaxSpeed;

    private void Awake()
    {
        playerMovementControls = new PlayerMovementControls();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        playerMovementControls.Enable();
    }

    private void OnDisable()
    {
        playerMovementControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovementControls.Land.Jump.performed += _ => Jump();

        lives = GameObject.FindGameObjectWithTag("Lives").GetComponent<Lives>();
        lives.SetHearts();

        healthBar = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<HealthBar>();
        healthBar.SetMaxHealth();

        spriteRenderer = GetComponent<SpriteRenderer>();

        positionController = GameObject.FindGameObjectWithTag("Position").GetComponent<PositionController>();
        transform.position = positionController.lastCheckpointPosition;

        uiController = GameObject.FindGameObjectWithTag("UIOverlay").GetComponent<UIController>();
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        if(isGrounded)
        {
            return true;
        }
        Vector2 topLeftPoint = transform.position;
        topLeftPoint.x -= col.bounds.extents.x;
        topLeftPoint.y += col.bounds.extents.y;

        Vector2 bottomRight = transform.position;
        bottomRight.x += col.bounds.extents.x;
        bottomRight.y -= col.bounds.extents.y;

        return Physics2D.OverlapArea(topLeftPoint, bottomRight, ground);
    }
    // Update is called once per frame
    void Update()
    {
        //Read the movement value
        float movementInput = playerMovementControls.Land.Move.ReadValue<float>();
        //Move the player
        if (!isOnSlipperySurface)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x += movementInput * speed * Time.deltaTime;
            transform.position = currentPosition;
        }
        else 
        {
            slipAcceleration(movementInput);
        }
    }

    public void TakeDamage(int damage)
    {
        healthBar.currentHealth = healthBar.currentHealth - damage;  //if hamster touches enemy/obstacle, minus 1 health

        healthBar.SetHealth();

        if (healthBar.currentHealth <= 0)    //if health bar reaches 0, player loses a life
        {
            //lives.LoseLife();
            lives.livesRemaining--;
            lives.SetHearts();

            if (lives.livesRemaining != 0)    //if player still has lives, respawn player at start of level or checkpoint (if a checkpoint was activated)
            {
                //go back to checkpoint/start
                healthBar.reset();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                //LOSE
                positionController.lastCheckpointPosition = new Vector2(0.5f,-0.6f);    //resetting to starting position of level
                healthBar.reset();
                lives.reset();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    bool GainHealth(int health) //hamster gains health if food item is picked up and health is not full
    {
        if (healthBar.currentHealth == healthBar.maxHealth)
        {
            return false;
        }
        healthBar.currentHealth = healthBar.currentHealth + health;

        healthBar.SetHealth();

        return true;
    }

    bool GainLife(int life) //hamster gains health if food item is picked up and health is not full
    {
        if (lives.livesRemaining==3)
        {
            return false;
        }
        lives.AddLife();
        return true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Health Item"))
        {
            if (GainHealth(1))  //if hamster's health is full, the health item cannot be picked up and no health is gained
            {
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Life Collectible"))
        {
            if (GainLife(1))  //if hamster's lives are full, this cannot be picked up and no life is gained
            {
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Saw")) //if hamster reaches checkpoint, the respawn position is changed to the position of the checkpoint
        {
            TakeDamage(10);
        }
        else if (collision.gameObject.CompareTag("Checkpoint")) //if hamster reaches checkpoint, the respawn position is changed to the position of the checkpoint
        {
            positionController.lastCheckpointPosition = transform.position;
        }

        else if (collision.gameObject.CompareTag("Underground"))
        {
            positionController.lastCheckpointPosition = new Vector2(0.06f, -1.4f);    //resetting to starting position of the underground level
            Debug.Log("Going to Underground...");
            SceneManager.LoadScene("Underground", LoadSceneMode.Single);  //Tag the exit then just replace "2" with whatever level you want the scene to go to next
        }

        else if (collision.gameObject.CompareTag("Campground"))
        {
            positionController.lastCheckpointPosition = new Vector2(0.53f, -0.56f);    //resetting to starting position of the underground level
            Debug.Log("Going to Campground...");
            SceneManager.LoadScene("Campground Level", LoadSceneMode.Single);  //Tag the exit then just replace "2" with whatever level you want the scene to go to next
        }

        else if (collision.gameObject.CompareTag("Home"))
        {
            positionController.lastCheckpointPosition = new Vector2(-.54f, 0.2f);    //resetting to starting position of the underground level
            SceneManager.LoadScene("Home Level", LoadSceneMode.Single);  //Tag the exit then just replace "2" with whatever level you want the scene to go to next
        }

        else if (collision.gameObject.CompareTag("Back to Level 1"))
        {
            SceneManager.LoadScene(1);  //The exit in Level 2 goes back to 1
        }

        else if (collision.gameObject.CompareTag("Game Finish"))
        {
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);  //Go back to Main Menu
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) //if hamster reaches checkpoint, the respawn position is changed to the position of the checkpoint
        {
            isGrounded = true;
        }
        else if (collision.gameObject.CompareTag("Rock")) //if hamster reaches checkpoint, the respawn position is changed to the position of the checkpoint
        {
            if(collision.gameObject.GetComponent<FallingRock>().damage)
                TakeDamage(10);
        }
        
        
        if (!collision.gameObject.CompareTag("Slippy")) //if hamster touches a non slippy surface, switch to non-accelerated movement
        {
            isOnSlipperySurface = false;
        }
    }

        public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) //if hamster reaches checkpoint, the respawn position is changed to the position of the checkpoint
        {
            isGrounded = false;
        }
    }


    public void landedOnSlipperySurface(float _surfaceacceleration, float _surfacedecceleration, float maxSpeed)
    {
        // pass over the surfaces "slipperiness"
        surfaceacceleration = _surfaceacceleration;
        surfacedecceleration = _surfacedecceleration;
        surfaceMaxSpeed = maxSpeed;

        // set the speed/direction of landing on surface, IF it is a newly touched slippery surface.
        if (!isOnSlipperySurface) { 
            float movementInput = playerMovementControls.Land.Move.ReadValue<float>();
            isOnSlipperySurface = true;
            accelerationSpeed = movementInput * speed;
        }
    }

    public void slipAcceleration(float movementInput)
    {
        // left acceleration
        if (movementInput < 0)
        {
            accelerationSpeed = accelerationSpeed - surfaceacceleration * Time.deltaTime;
        }
        // right acceleration
        else if (movementInput > 0)
        {
            accelerationSpeed = accelerationSpeed + surfaceacceleration * Time.deltaTime;
        }
        // no input, deceleration
        else
        {
            if (accelerationSpeed > 10 * Time.deltaTime)
            {
                accelerationSpeed = accelerationSpeed - surfacedecceleration * Time.deltaTime;
            }
            else if (accelerationSpeed < -10 * Time.deltaTime)
            {
                accelerationSpeed = accelerationSpeed + surfacedecceleration * Time.deltaTime;
            }
            else
            {
                accelerationSpeed = 0;
            }
        }

        // move player with calculated speed
        Vector3 currentPosition = transform.position;

        // If the player hits the speed limit for slippery surface
        if((accelerationSpeed > surfaceMaxSpeed || accelerationSpeed < -surfaceMaxSpeed) && surfaceMaxSpeed >= 0)
        {
            accelerationSpeed = surfaceMaxSpeed * movementInput;
        }

        currentPosition.x += accelerationSpeed * Time.deltaTime;
        transform.position = currentPosition;
    }
}
