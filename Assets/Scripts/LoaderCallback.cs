using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    public bool firstFrame = true;
    void Update()
    {
        if (firstFrame)
        {
            firstFrame = false;
            Loader.LoaderCallback();
        }
    }
}
