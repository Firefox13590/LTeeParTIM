using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class JoueurMove : MonoBehaviour
{
    [Header("Paramètres de mouvement")]
    public float speed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;
    private Transform cameraTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Récupère automatiquement la caméra principale (doit avoir le tag "MainCamera")
        cameraTransform = Camera.main.transform;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Update()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Récupère les directions de la caméra
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // On met le Y à 0 pour garder le mouvement strictement horizontal
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Déplacement basé sur l'angle de la caméra
        Vector3 move = camRight * moveInput.x + camForward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);

        // Gravité et saut
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}