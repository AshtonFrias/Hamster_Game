using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed, jumpSpeed;
    [SerializeField] private LayerMask ground;
    private PlayerMovementControls playerMovementControls;
    private Collider2D col;
    private Rigidbody2D rb;

    //for setting/getting hamster's max health and current health
    public int maxHealth = 10;
    public int currentHealth;
    public HealthBar healthBar;

    //gets/sets the hamster's lives
    public Lives lives;

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
        
        currentHealth = maxHealth;  //setting health to 10
        healthBar.SetMaxHealth(maxHealth);

        //TakeDamage(5);     //was using this for testing since no damage is in game yet
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
        currentHealth = currentHealth - damage;  //if hamster touches enemy/obstacle, minus 1 health

        if(currentHealth==0)
        {
            lives.LoseLife();
        }
    }

    public void landedOnSlipperySurface(float _surfaceacceleration, float _surfacedecceleration, float maxSpeed)
    {
        // pass over the surfaces "slipperiness"
        surfaceacceleration = _surfaceacceleration;
        surfacedecceleration = _surfacedecceleration;
        surfaceMaxSpeed = maxSpeed;

        // set the speed/direction of landing on surface
        float movementInput = playerMovementControls.Land.Move.ReadValue<float>();
        isOnSlipperySurface = true;
        accelerationSpeed = movementInput * speed;
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
