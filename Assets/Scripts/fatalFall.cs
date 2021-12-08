using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fatalFall : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x
            * colliderDistance, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y,
            boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null && hit.transform.tag == "Player")
            player.gameObject.GetComponent<PlayerController>().TakeDamage(10);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
