using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMainCamera : MonoBehaviour
{
    // Allegedly, main camera as a child of a rigidbody can be weird.
    public Transform cameraPosition;

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
