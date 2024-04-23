using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    private float speed;
    public float boundaryZ = 10f; 
    void Start()
    {
        if (gameObject.CompareTag("obstacle"))
        {
            speed = Mathf.RoundToInt(Random.Range(4f, 12f)); 
        }
        else if (gameObject.CompareTag("reward"))
        {
            speed = Mathf.RoundToInt(Random.Range(8f, 10f));
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}