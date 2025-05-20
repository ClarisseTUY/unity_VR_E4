using UnityEngine;

public class DropZone3 : MonoBehaviour
{
    public GameObject objectToActivate;
    public Material newMaterial;

    public string expectedTag = "tweezer";

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag(expectedTag))
        {
            Debug.Log("Objet ignoré : " + other.name + " (tag = " + other.tag + ")");
            return;
        }


        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }


        Renderer zoneRenderer = GetComponent<Renderer>();
        if (zoneRenderer != null && newMaterial != null)
        {
            zoneRenderer.material = newMaterial;
        }
        else
        {
            Debug.LogWarning("DropZone n'a pas de Renderer ou de Matériau assigné !");
        }
    }
}
