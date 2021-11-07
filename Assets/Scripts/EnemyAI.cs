using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public float attackRadius;
    [SerializeField] public float speed;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float targetDistance = Vector2.Distance(transform.position, target.position);

        if(targetDistance < attackRadius)
            chasePlayer();
        else
            idleAround();
    }

    void chasePlayer()
    {
        if (transform.position.x < target.position.x)
            rb.velocity = new Vector2(speed, 0);
        else
            rb.velocity = new Vector2(-speed, 0);
    }

    void idleAround()
    {
        rb.velocity = new Vector2(0, 0);
    }
}
