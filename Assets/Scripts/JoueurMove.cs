using UnityEngine;
using UnityEngine.InputSystem;
using Unity;
using Unity.Netcode;

[RequireComponent(typeof(CharacterController))]
public class JoueurMove : NetworkBehaviour
{
    [Header("Paramètres de mouvement")]
    public float vitesse = 5f;
    public float hauteurSaut = 1.5f;
    public float gravite = -9.81f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocite;
    private Transform cameraTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Récupère automatiquement la caméra principale
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
            velocite.y = Mathf.Sqrt(hauteurSaut * -2f * gravite);
        }
    }

    void Update()
    {
        if (controller.isGrounded && velocite.y < 0)
        {
            velocite.y = -2f;
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
        controller.Move(move * vitesse * Time.deltaTime);

        // Saut
        velocite.y += gravite * Time.deltaTime;
        controller.Move(velocite * Time.deltaTime);
    }

    // Détecte les collisions lorsque le joueur touche la zone de mort
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Vérification pour la collision
        if (hit.gameObject.CompareTag("Mort"))
        {
            Destroy(gameObject);
        }
    }
}