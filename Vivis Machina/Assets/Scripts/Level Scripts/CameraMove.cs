using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float ease;
    public Vector2 xParams;
    public Vector2 yParams;

    public static Vector2 pos;

    void Update()
    {
        transform.position -= new Vector3((transform.position.x - Mathf.Min(Mathf.Max(Player.pos.x, xParams.x), xParams.y)) * Time.deltaTime * ease, (transform.position.y - Mathf.Min(Mathf.Max(Player.pos.y, yParams.x), yParams.y)) * Time.deltaTime * ease, 0);
        pos = transform.position;
    }
}
