using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform pivot;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CustomParticles particles;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isFiring = false;
    [SerializeField] private float fireRate = 0.5f; // taxa de disparo (em segundos)
    private float nextFireTime = 0f; // momento do próximo disparo

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        // Move the player in X and Z axis based on input
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.AddForce(movement * moveSpeed, ForceMode.Impulse);

        particles.isMoving = moveInput != Vector2.zero;
    }
    private void LateUpdate()
    {
        // Rotate the player towards the position of the mouse
        var mousePos = Mouse.current.position.ReadValue();
        Ray cameraRay = Camera.main.ScreenPointToRay(mousePos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(cameraRay, out float rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            pivot.LookAt(new Vector3(pointToLook.x, pivot.position.y, pointToLook.z));
        }

    }

    private void Update()
    {
        // Disparar a bala
        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }

        // Atualizar o estado de disparo
        isFiring = Mouse.current.leftButton.isPressed;
    }
}
