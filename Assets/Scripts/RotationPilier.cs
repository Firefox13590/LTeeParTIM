using System.Collections;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody))] // S'assure qu'un Rigidbody est présent
public class RotationPilier : NetworkBehaviour
{
    [Header("Vitesse et Accélération")]
    public float vitesse = 90f;
    public float incrementVitesse = 30f;
    public float intervalleAcceleration = 10f;

    [Header("Inversion Aléatoire")]
    [Range(0f, 1f)]
    public float chanceInversion = 0.25f;
    public float delaiCooldown = 3f;

    private float sensRotation = 1f; // 1 = normal, -1 = inversé
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        rb.isKinematic = true; 
        rb.useGravity = false;
        
        // AJOUT CRUCIAL : Force le centre de masse exactement sur le pivot (0, 0, 0)
        rb.centerOfMass = Vector3.zero; 
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Seul le serveur doit calculer les changements de vitesse et les inversions aléatoires
        if (IsServer)
        {
            InvokeRepeating(nameof(Accelerer), intervalleAcceleration, intervalleAcceleration);
            StartCoroutine(GererInversionAleatoire());
        }
    }

    void FixedUpdate()
    {
        // Seul le serveur déplace physiquement le pilier
        if (!IsServer) return;

        // On utilise MoveRotation dans FixedUpdate avec fixedDeltaTime pour la physique
        Quaternion deltaRotation = Quaternion.Euler(0f, vitesse * sensRotation * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    void Accelerer()
    {
        vitesse += incrementVitesse;
    }

    IEnumerator GererInversionAleatoire()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Test chaque seconde

            if (Random.value < chanceInversion)
            {
                sensRotation *= -1f; // Inverse le sens

                // Attend le reste du cooldown
                if (delaiCooldown > 1f)
                {
                    yield return new WaitForSeconds(delaiCooldown - 1f);
                }
            }
        }
    }
}