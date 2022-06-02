using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, -55.5f);
    }
}