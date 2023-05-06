using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Space]
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 10f;
    //[SerializeField] private float rotateSpeed = 5f;
    [Space]
    [Header("Sounds")]
    [SerializeField] protected string shotSoundReference = "player_shoot";
    protected AudioClip shotSound;

    [Space]
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform pivot;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CustomParticles particles;

    private bool canMove = true;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isFiring = false;
    [SerializeField] private float fireRate = 0.5f;
    private float nextFireTime = 0f;
    private Vector3 lastLookPos;

    public bool CanMove { set => canMove = value; }
    public Rigidbody Rb { get => rb; }

    private void OnMove(InputValue value) => moveInput = value.Get<Vector2>();

    private void OnLook(InputValue value) => lookInput = value.Get<Vector2>();

    private void Start()
    {
        shotSound = AudioManager.instance.sounds.GetAudioClip(shotSoundReference);
    }
    private void FixedUpdate()
    {
        if (!canMove)
            return;

        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        Rb.AddForce(movement * moveSpeed, ForceMode.Impulse);

        particles.isMoving = moveInput != Vector2.zero;
    }
    private void LateUpdate()
    {
        if (!canMove)
        {
            pivot.LookAt(lastLookPos);
            return;
        }
            

        var mousePos = Mouse.current.position.ReadValue();
        Ray cameraRay = Camera.main.ScreenPointToRay(mousePos);
        Plane groundPlane = new(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(cameraRay, out float rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            lastLookPos = new Vector3(pointToLook.x, pivot.position.y, pointToLook.z);
            pivot.LookAt(lastLookPos);
        }

    }

    private void Update()
    {
        particles.alive = canMove;

        if (!canMove)
            return;

        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            AudioManager.instance.PlaySound(shotSound, .6f);
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }

        isFiring = Mouse.current.leftButton.isPressed;
    }
}
