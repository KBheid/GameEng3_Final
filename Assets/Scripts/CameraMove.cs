using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float sensitivity;

    public Vector2 minPos;
    public Vector2 maxPos;

    private Vector3 start;
    private bool dragging = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
		{
            dragging = true;
            start = Input.mousePosition;
		}

        if (Input.GetMouseButtonUp(2))
		{
            dragging = false;
		}

        if (dragging)
		{
            Vector3 diff = sensitivity * Time.deltaTime * (Input.mousePosition - start).normalized;
            diff.z = 0;

            transform.position += diff;

            float posX = Mathf.Clamp(transform.position.x, minPos.x, maxPos.x);
            float posY = Mathf.Clamp(transform.position.y, minPos.y, maxPos.y);

            transform.position = new Vector3(posX, posY, -10);
		}
    }
}
