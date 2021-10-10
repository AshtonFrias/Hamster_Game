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
        //Mobe the player
        Vector3 currentPosition = transform.position;
        currentPosition.x += movementInput * speed * Time.deltaTime;
        transform.position = currentPosition;
    }
}
