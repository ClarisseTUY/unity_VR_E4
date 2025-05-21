using UnityEngine;

public class DropZone : MonoBehaviour
{
    [Header("Objectif")]
    public string expectedObjectName; 

    [Header("Action à déclencher")]
    public GameObject objectToActivate;

    [Header("Changement visuel")]
    public Material newMaterial; 

    private void OnTriggerEnter(Collider other)
    {
        
        if (!other.name.Contains(expectedObjectName))
        {
            //Debug.Log($"[DropZone] {other.name} ignoré (attendait : {expectedObjectName})");
            return;
        }

        
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        
        Renderer objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null && newMaterial != null)
        {
            objectRenderer.material = newMaterial;
        }
        else
        {
            Debug.LogWarning($"[DropZone] Aucun Renderer ou Material trouvé pour {other.name}");
        }

        Destroy(other.gameObject);
    }
}
