using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour
{
    private static PositionController positionController;
    public Vector2 lastCheckpointPosition;

    void Awake()
    {
        if (positionController == null)
        {
            positionController = this;
            DontDestroyOnLoad(positionController);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
