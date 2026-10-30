using System.Collections;
using UnityEngine;
using Unity;
using Unity.Netcode;

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

    void Start()
    {
        // Accélère toutes les 10s
        InvokeRepeating(nameof(Accelerer), intervalleAcceleration, intervalleAcceleration);

        // Lance la gestion de l'inversion
        StartCoroutine(GererInversionAleatoire());
    }

    void Update()
    {
        // Rotation selon la vitesse et le sens
        transform.Rotate(0f, vitesse * sensRotation * Time.deltaTime, 0f, Space.Self);
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
    