using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar2 : MonoBehaviour
{
    private Transform health_bar;

    private void Start()
    {
        health_bar = transform.Find("Fill2");
    }

    public void SetMaxHealth(int health)
    {
        health_bar = transform.Find("Fill2");
        health_bar.localScale = new Vector3((float)health / (float)10, 1f);
    }

    // Update is called once per frame
    public void SetHealth(int health)
    {
        health_bar.localScale = new Vector3((float)health / (float)10, 1f);
    }
}
