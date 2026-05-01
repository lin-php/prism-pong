using UnityEngine;

public class CircleRotate : MonoBehaviour
{

    [SerializeField] private float speed = 20f;

    void Update()
    {
        transform.Rotate(0f,0f, speed * Time.deltaTime);
    }

}
