using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public enum MoveDirection
    {
        LeftRight,
        UpDown
    }

    public MoveDirection direction = MoveDirection.LeftRight;
    public float moveDistance = 3f;   
    public float moveSpeed = 2f;      

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        float movement = Mathf.PingPong(Time.time * moveSpeed, moveDistance);

        if (direction == MoveDirection.LeftRight)
        {
            transform.position = new Vector3(
                startPos.x + movement,
                startPos.y,
                startPos.z
            );
        }
        else if (direction == MoveDirection.UpDown)
        {
            transform.position = new Vector3(
                startPos.x,
                startPos.y + movement,
                startPos.z
            );
        }
    }
}
