using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBob : MonoBehaviour
{
    public float speed;
    public float height;
    public float startTime;
    public float offsetY;

    float t;

    private void Start()
    {
        t = startTime;
    }

    void Update()
    {
        t += Time.deltaTime * speed;
        transform.position = new Vector2(transform.position.x, Mathf.Sin(t) * height + CameraMove.pos.y + offsetY);
    }
}
