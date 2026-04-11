using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 10f;
    
    void Update()
    {
        // PLAYER MOVEMENT
        // input and move paddle

        float input = Input.GetAxis("Vertical");

        Vector3 move = new Vector3 (0, input, 0) * speed * Time.deltaTime;  
        transform.position += move;

        float clampedY = Mathf.Clamp(transform.position.y, -4.5f, 4.5f);

        transform.position = new Vector3 (transform.position.x, clampedY, transform.position.z);
    }
}
