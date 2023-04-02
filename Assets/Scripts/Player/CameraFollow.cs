using UnityEngine;

public class CameraFollow : MonoBehaviour {
    
    public Transform target; // O objeto que a c�mera seguir�
    public float smoothSpeed = 0.125f; // A velocidade com que a c�mera se mover� para seguir o objeto
    public Vector3 offset; // A posi��o relativa da c�mera em rela��o ao objeto
    
    void FixedUpdate () {
        // Define a posi��o desejada da c�mera com base na posi��o do objeto e no offset
        Vector3 desiredPosition = target.position + offset;
        // Suavemente move a c�mera em dire��o � posi��o desejada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Define a posi��o da c�mera como a posi��o suavizada
        transform.position = smoothedPosition;
    }

}
