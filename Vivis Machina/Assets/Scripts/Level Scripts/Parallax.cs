using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Vector2 parallaxScale;
    public float sizeX;
    public int num;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(CameraMove.pos.x * parallaxScale.x - sizeX / 2 + sizeX * num, CameraMove.pos.y * parallaxScale.y);
    }
}
