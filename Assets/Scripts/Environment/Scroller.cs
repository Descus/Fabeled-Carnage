using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;

public class Scroller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.timeScale != 0.0f)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        GameObject[] movables = GameObject.FindGameObjectsWithTag("Movable");
        foreach (GameObject movable in movables)
        {
            IsMovable mov = movable.GetComponent<IsMovable>();
            mov.Move(5 * Time.deltaTime);
            if (movable.transform.position.x <= -11)
            {
                Destroy(movable);
            }
        }
    }
}
