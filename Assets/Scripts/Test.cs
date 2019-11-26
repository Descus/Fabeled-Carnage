using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("geht");
        
        
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("nicht");
        }
    }
}
