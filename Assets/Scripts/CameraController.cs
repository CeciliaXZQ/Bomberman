using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }

    }

    public void AssignTarget(PlayerController player) {
        target = player.transform;
    }
}
