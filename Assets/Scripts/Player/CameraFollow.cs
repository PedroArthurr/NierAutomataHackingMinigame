using UnityEngine;

public class CameraFollow : MonoBehaviour {
    
    public Transform target; // O objeto que a câmera seguirá
    public float smoothSpeed = 0.125f; // A velocidade com que a câmera se moverá para seguir o objeto
    public Vector3 offset; // A posição relativa da câmera em relação ao objeto
    
    void FixedUpdate () {
        // Define a posição desejada da câmera com base na posição do objeto e no offset
        Vector3 desiredPosition = target.position + offset;
        // Suavemente move a câmera em direção à posição desejada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Define a posição da câmera como a posição suavizada
        transform.position = smoothedPosition;
    }

}
