using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Vector2 parallaxScale; 
    public float yOffset;
    public SpriteRenderer sr;

    float size;

    void Start()
    {
        size = sr.sprite.rect.size.x / 100f;
    }

    void Update()
    {
        transform.position = new Vector2(CameraMove.pos.x * (parallaxScale.x - 1) - Mathf.RoundToInt(CameraMove.pos.x * (parallaxScale.x - 1) / size) * size + CameraMove.pos.x, CameraMove.pos.y * parallaxScale.y - yOffset);
    }
}
