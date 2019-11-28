using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float _startZ = 0f;
    void Start()
    {
        _startZ = transform.position.z;
    }
    
    void Update()
    {
        var playerPosition = player.transform.position;
        transform.position = new Vector3(playerPosition.x, playerPosition.y, _startZ);
    }
}
