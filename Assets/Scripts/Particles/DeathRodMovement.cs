using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathRodMovement : MonoBehaviour
{
    public float speed = 5f; 
    public float acceleration = 1f;
    public Vector3 move;
    float time;

    private void OnEnable()
    {
        time = 0;
    }
    void Update()
    {
        time = Time.deltaTime;
        speed += acceleration * Time.deltaTime; // Aumenta a velocidade gradualmente
        transform.Translate(move * speed * Time.deltaTime); 
        transform.localScale -= new Vector3(0f, Time.deltaTime, 0f); 
    }
}
