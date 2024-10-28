using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Le transform du joueur que la caméra doit suivre
    public Vector3 offset = new Vector3(0, 5, -10); // Offset par rapport au joueur

    void LateUpdate()
    {
        //la cible (le joueur) est définie, si non : 
        if (target == null)
        {
            return;
        }

        // on deplace la caméra pour suivre le joueur avec l'offset
        transform.position = target.position + offset;

        // on fait en sorte que la caméra regarde toujours le joueur
        transform.LookAt(target);
    }
}
