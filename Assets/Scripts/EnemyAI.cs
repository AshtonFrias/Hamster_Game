using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public Transform target;

    [Header ("Agro Range Settings")]
    [SerializeField] public float chaseRadius;
    [SerializeField] public float speed;

    [Header ("Attack Settings")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float damage;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private float targetDistance;
    [Header("Hit Box Settings")]
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;



    Rigidbody2D rb;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, target.position);

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
        else
        {
            anim.SetBool("attack", false);

            if (targetDistance < chaseRadius)
                chasePlayer();
            else
                idleAround();
        }
    }

    void chasePlayer()
    {
        if (transform.position.x < target.position.x)
        {
            rb.velocity = new Vector2(speed, 0);

            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1,
                transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);

            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1,
                transform.localScale.y, transform.localScale.z);
            }
        }

        anim.SetBool("moving", true);
    }

    void idleAround()
    {
        rb.velocity = new Vector2(0, 0);
        anim.SetBool("moving", false);
    }

    private bool PlayerInSight()
    {
        rb.velocity = new Vector2(0, 0);

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
