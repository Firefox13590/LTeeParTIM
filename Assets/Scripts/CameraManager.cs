using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Valeurs a ajuster dans l'inspecteur")]
    public GameObject[] Cameras;
    public int setCameraiIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // abonnement aux évènements
        Case.OnTileChoiceStart += SetCameraChoixCase;
        PlayerControls.OnTileChoiceEnd += SetCameraPlayer;

        UpdateActiveCamera();
    }

    private void OnDestroy()
    {
        // désabonnement aux évènements
        Case.OnTileChoiceStart -= SetCameraChoixCase;
        PlayerControls.OnTileChoiceEnd -= SetCameraPlayer;
    }



    /// <summary>
    /// Met à jour la cmaéra active en fonction de l'index défini.
    /// </summary>
    void UpdateActiveCamera()
    {
        for(int i = 0; i< Cameras.Length; i++)
        {
            if(i == setCameraiIndex)
            {
                Cameras[i].SetActive(true);
            }
            else
            {
                Cameras[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Change l'index de caméra pour la vue de la map.
    /// </summary>
    void SetCameraMap()
    {
        setCameraiIndex = 0;
        UpdateActiveCamera();
    }

    /// <summary>
    /// Change l'index de caméra pour la vue du joueur.
    /// </summary>
    void SetCameraPlayer()
    {
        setCameraiIndex = 1;
        UpdateActiveCamera();
    }

    /// <summary>
    /// Change l'index de caméra pour la vue de choix de case.
    /// </summary>
    /// <param name="options">Les options de case</param>
    void SetCameraChoixCase(Transform[] options)
    {
        setCameraiIndex = 2;
        //Cameras[2].transform.position = options[0].position + options[1].position + new Vector3(-5, 10);
        Cameras[2].transform.position = Vector3.Lerp(options[0].position, options[1].position, .5f) + new Vector3(-10, 15);
        UpdateActiveCamera();
    }
}
