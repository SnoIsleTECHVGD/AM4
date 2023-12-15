using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Vector2 parallaxScale;
    public SpriteRenderer sr;

    float size;

    void Start()
    {
        size = sr.sprite.rect.size.x / 100f;
        Debug.Log(size);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(CameraMove.pos.x * parallaxScale.x - Mathf.RoundToInt(CameraMove.pos.x * parallaxScale.x / size) * -size, CameraMove.pos.y * parallaxScale.y);
        Debug.Log(Mathf.RoundToInt(CameraMove.pos.x / size / size) * -size);
    }
}
