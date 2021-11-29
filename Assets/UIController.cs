using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private static UIController uiController;

    void Awake()
    {
        if (uiController == null)
        {
            uiController = this;
            DontDestroyOnLoad(uiController);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
