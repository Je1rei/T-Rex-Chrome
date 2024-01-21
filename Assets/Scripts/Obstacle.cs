using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float leftEdge;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;
    }

    private void Update()
    {
        transform.position += Vector3.left * GameManager.Instance.GameSpeed * Time.deltaTime;

        if(transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }
}