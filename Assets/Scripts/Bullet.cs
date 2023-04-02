using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // velocidade da bala
    private Rigidbody rb;

    void Start()
    {
        // obter a referência do Rigidbody
        rb = GetComponent<Rigidbody>();

        // mover a bala na direção do eixo Z
        rb.velocity = transform.forward * speed;

        //Destroy(this.gameObject, 3);
    }

    void Update()
    {
        // verificar se a bala saiu da tela e destruí-la
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        // destruir a bala quando colidir com outro objeto
        if (other.gameObject.layer != 3 || other.gameObject.layer != 6)
            Destroy(gameObject);
    }
}