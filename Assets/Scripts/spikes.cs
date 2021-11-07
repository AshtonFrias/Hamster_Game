using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikes : MonoBehaviour
{
    public int spikeBounceBack = 15;
    public int damage = 2;

    [SerializeField] private PlayerController player;

    // Triggered when Hamster touches spikes, can add damage here
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(damage);
            Rigidbody2D rb = collision.gameObject.GetComponent("Rigidbody2D") as Rigidbody2D;
            rb.AddForce(new Vector2(0, spikeBounceBack), ForceMode2D.Impulse);
        }
    }
}
