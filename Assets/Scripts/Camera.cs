using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private float speed = 4;

    void Update()
    {
        var movementVector = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            movementVector.z = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movementVector.z = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movementVector.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movementVector.x = -1;
        }
        transform.position = transform.position + movementVector * Time.deltaTime * speed;
    }
}