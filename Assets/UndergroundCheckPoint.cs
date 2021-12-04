using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundCheckPoint : MonoBehaviour
{
    private Animator animator;
    private PositionController positionController;

    void Start()
    {
        animator = GetComponent<Animator>();
        positionController = GameObject.FindGameObjectWithTag("Position").GetComponent<PositionController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.Play("light");
            positionController.lastCheckpointPosition = transform.position;
        }
    }
}

