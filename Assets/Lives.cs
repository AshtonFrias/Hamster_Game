using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    public Image[] lives;
    public int livesRemaining;

    public void LoseLife()  //if hamster's health reaches 0, the player loses a life or loses if none are left
    {
        livesRemaining = livesRemaining - 1;
        lives[livesRemaining].enabled = false;  //this hides a heart to represent the player lost a life

        if(livesRemaining==0)
        {
            Debug.Log("Game Lost: ran out of lives");
        }
    }
}
