using UnityEngine;
using UnityEngine.InputSystem;
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

   public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (controller == null)
            controller = GetComponent<CharacterController>();

        // Récupérer le PlayerInput
        PlayerInput playerInput = GetComponent<PlayerInput>();

        if (IsOwner)
        {
            // Activer les contrôles uniquement pour le joueur local
            if (playerInput != null) playerInput.enabled = true;

            controller.enabled = false;
            
            Vector3[] positionsSpawn = new Vector3[]
            {
                new Vector3(-17.4599991f, 11.3339996f, -14.8900003f), 
                new Vector3(-3f, 11.3190002f, -29.4699993f),          
                new Vector3(-2.00999999f, 11.335f, 0.699999988f),     
                new Vector3(12.9099998f, 11.309f, -14.1400003f)       
            };

            int index = (int)(OwnerClientId % (ulong)positionsSpawn.Length); 
            transform.position = positionsSpawn[index];
            
            controller.enabled = true;
        }
        else
        {
            // Désactiver le PlayerInput pour les personnages des autres joueurs
            if (playerInput != null) playerInput.enabled = false;
        }
    }
    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
            
        cameraTransform = Camera.main.transform;
    }

    public void OnMove(InputValue value) { moveInput = value.Get<Vector2>(); }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && controller.isGrounded)
            velocite.y = Mathf.Sqrt(hauteurSaut * -2f * gravite);
    }

    void Update()
    {
        if (!IsOwner) return; // Empêcher les autres joueurs de contrôler votre personnage

        if (controller.isGrounded && velocite.y < 0)
            velocite.y = -2f;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * moveInput.x + camForward * moveInput.y;
        controller.Move(move * vitesse * Time.deltaTime);

        velocite.y += gravite * Time.deltaTime;
        controller.Move(velocite * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Mort"))
            Destroy(gameObject);
    }
}