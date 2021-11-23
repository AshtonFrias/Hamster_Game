using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrolEnemyAI : MonoBehaviour
{
    [Header ("Attack Settings")]
    [SerializeField] public Transform target;

   [SerializeField] private float attackCooldown;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header ("Hit Box Settings")]
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Patrol Behavior Settings")]
    [SerializeField] private Transform enemy;
    [SerializeField] private float patrolSpeed;
    private Vector3 initialScale;
    private bool movingLeft;
    [SerializeField] private float idleTime;
    private float idleTimer;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
       anim = GetComponent<Animator>();
       initialScale = enemy.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

       if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetBool("attack", true);
            }
            else
                anim.SetBool("attack", false);
        }
        else if (movingLeft)
        {
            anim.SetBool("attack", false);
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else if(!movingLeft)
        {
            anim.SetBool("attack", false);
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
            {
                DirectionChange();
            }
        }
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);
        enemy.localScale = new Vector3(Mathf.Abs(initialScale.x) * _direction, initialScale.y, initialScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * patrolSpeed,
            enemy.position.y, enemy.position.z);
    }

    private void DirectionChange()
    {
        if(idleTime > 0)
            anim.SetBool("moving", false);

        idleTimer += Time.deltaTime;

        if(idleTimer > idleTime)
            movingLeft = !movingLeft;
    }
   
    private bool PlayerInSight()
    {
       RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x
            * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y,
            boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null && hit.transform.tag == "Player")
            return true;
        else
            return false;        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void attackPlayer()
    {
        if (PlayerInSight())
        {
            target.gameObject.GetComponent<PlayerController>().TakeDamage(1);
        }
    }
}

