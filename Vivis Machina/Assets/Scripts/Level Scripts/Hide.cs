using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public SpriteRenderer sr;

    void Start()
    {
        sr.enabled = false;
    }
}
