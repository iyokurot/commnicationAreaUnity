using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    CharacterController characterController = default;
    public float speed = 3.0f;
    float moveX = 0f;
    float moveZ = 0f;

    void Update () {
        moveX = Input.GetAxis ("Horizontal") * speed;
        moveZ = Input.GetAxis ("Vertical") * speed;
        Vector3 direction = new Vector3 (moveX, 0, moveZ);
        characterController.SimpleMove (direction);
        if (moveX != 0 || moveZ != 0) {
            SocketClient.Instance.EmitMove (this.transform.position);
        }
    }
}