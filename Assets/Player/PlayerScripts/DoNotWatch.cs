using UnityEngine;

public class CircleMove : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private float speed = 1f;

    [SerializeField] private int index;   // 何番目か
    [SerializeField] private int count = 7;  // 全体の数

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float phase = Mathf.PI * 2f * index / count;
        float angle = Time.time * speed + phase;

        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        transform.position = startPos + new Vector3(x, y, 0);
    }
}