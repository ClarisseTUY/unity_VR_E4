using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextOnLight : MonoBehaviour
{
    public Light spotlight;
    public GameObject textObject;

    void Start()
    {
        if (textObject != null)
            textObject.SetActive(false);

        if (spotlight == null)
            Debug.LogError("⚠️ Spotlight non assignée !");
    }

    void Update()
    {
        if (spotlight == null || textObject == null) return;

        Vector3 rotation = spotlight.transform.eulerAngles;

        float rotX = NormalizeAngle(rotation.x);
        float rotY = NormalizeAngle(rotation.y);

        // Logs pour vérifier en temps réel
        Debug.Log($"Rotation X: {rotX}, Rotation Y: {rotY}");

        if (rotX >= -5f && rotX <= -1f && rotY >= 75f && rotY <= 80f)
        {
            if (!textObject.activeSelf)
                Debug.Log("✅ Texte activé !");
            textObject.SetActive(true);
        }
        else
        {
            if (textObject.activeSelf)
                Debug.Log("❌ Texte désactivé !");
            textObject.SetActive(false);
        }
    }

    float NormalizeAngle(float angle)
    {
        return (angle > 180f) ? angle - 360f : angle;
    }
}