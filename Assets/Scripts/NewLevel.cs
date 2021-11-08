using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewLevel : MonoBehaviour
{
    public int iLevelToLoad;
    public string sLevelToLoad;
    public bool ToLoadLevel = false;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;

        if(collisionGameObject.name == "Hamster")
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        if(ToLoadLevel)
        {
            SceneManager.LoadScene(iLevelToLoad); //Integer idetifier of the scence
        }
        else
        {
            SceneManager.LoadScene(sLevelToLoad); //String name of the new scence
        }
    }
}
