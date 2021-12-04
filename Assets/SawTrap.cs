using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] public Transform target;

    [SerializeField] private float damage;
    [SerializeField] private LayerMask playerLayer;

    [Header("Hit Box Settings")]
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Patrol Behavior Settings")]
    [SerializeField] private float patrolSpeed;
    private Vector3 initialScale;
    private bool movingLeft;
    [SerializeField] private float idleTime;
    private float idleTimer;

    private Transform saw;


    // Start is called before the first frame update
    void Start()
    {
        saw = GetComponent<Transform>();
        initialScale = saw.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        if (movingLeft)
        {
            if (saw.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else if (!movingLeft)
        {
            if (saw.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        saw.localScale = new Vector3(Mathf.Abs(initialScale.x) * _direction, initialScale.y, initialScale.z);

        saw.position = new Vector3(saw.position.x + Time.deltaTime * _direction * patrolSpeed,
            saw.position.y, saw.position.z);
    }

    private void DirectionChange()
    {

        idleTimer += Time.deltaTime;

        if (idleTimer > idleTime)
            movingLeft = !movingLeft;
    }
}
