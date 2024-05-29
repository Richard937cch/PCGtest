using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public string playerTag = "Player"; // Tag used to identify the player

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Destroy(gameObject);
        }
    }
}
