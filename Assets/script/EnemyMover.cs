using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float speed = 5f; // Speed of movement
    public float boundaryZ = 10f; // Boundary to switch direction

    void Update()
    {
        // Move the object
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Check if the object is beyond the boundary
        if (transform.position.z >= boundaryZ || transform.position.z <= -boundaryZ)
        {
            // Reverse the direction
            speed *= -1;
        }
    }
}