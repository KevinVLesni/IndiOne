using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float speed;
    void FixedUpdate()
    {
        if (target != null)
        {
            float interp = speed * Time.deltaTime;
            Vector3 position = this.transform.position;
            position.y = Mathf.Lerp(this.transform.position.y, target.position.y, interp);
            position.x = Mathf.Lerp(this.transform.position.x, target.position.x, interp);
            this.transform.position = position;
        }
    }
}
