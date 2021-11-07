using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperySurface : MonoBehaviour
{
    public float acceleration, deceleration, maxSpeed;
    [SerializeField] private PlayerController player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.landedOnSlipperySurface(acceleration, deceleration, maxSpeed);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.isOnSlipperySurface = false;
        }
    }
}
