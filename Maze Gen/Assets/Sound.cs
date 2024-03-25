using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{

    public AudioSource wow;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            wow.Play();
        }
    }

}