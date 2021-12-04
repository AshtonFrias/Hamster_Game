using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    private static Lives lives;
    public Image[] hearts;
    public int livesRemaining = 3;

    public void SetHearts()
    {
        if (livesRemaining == 0)
        {
            //Debug.Log("Game Lost: ran out of lives!");
        }
        else
        {
            for (int i = 2; i > livesRemaining - 1; i--)
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void AddLife()
    {
        livesRemaining++;
        hearts[livesRemaining-1].enabled = true;
    }

    public void reset()
    {
        livesRemaining=3;

        for (int i = 0; i < livesRemaining; i++)
        {
            hearts[i].enabled = true;
        }
    }
}
