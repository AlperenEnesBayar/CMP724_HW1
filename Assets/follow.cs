using UnityEngine;

public class follow : MonoBehaviour
{
    public Transform target;
    public float speed = 2.0f;
    public float yOffSet = 1.3f;


    void Update()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + yOffSet, -10f);
        transform.position = Vector3.Slerp(transform.position, targetPosition, speed * Time.deltaTime);    
    }
}
