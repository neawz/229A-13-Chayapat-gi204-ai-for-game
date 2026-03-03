using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    [System.Serializable]
    public class SkyboxZone
    {
        public Vector3 position;
        public float radius = 10f;
        public Material skyboxMaterial;
    }

    public SkyboxZone[] skyboxZones;
    public Material defaultSkybox;

    private Skybox cameraSkybox;
    private Material currentSkybox;

    void Start()
    {
        // Get the Skybox component on the Main Camera
        cameraSkybox = Camera.main.GetComponent<Skybox>();

        if (cameraSkybox == null)
        {
            Debug.LogError("No Skybox component found on Main Camera!");
        }
    }

    void Update()
    {
        foreach (SkyboxZone zone in skyboxZones)
        {
            float distance = Vector3.Distance(transform.position, zone.position);

            if (distance <= zone.radius)
            {
                if (currentSkybox != zone.skyboxMaterial)
                {
                    cameraSkybox.material = zone.skyboxMaterial;
                    currentSkybox = zone.skyboxMaterial;
                }
                return;
            }
        }

        // Revert to default if outside all zones
        if (currentSkybox != defaultSkybox)
        {
            cameraSkybox.material = defaultSkybox;
            currentSkybox = defaultSkybox;
        }
    }

    void OnDrawGizmos()
    {
        if (skyboxZones == null) return;
        Gizmos.color = Color.cyan;
        foreach (SkyboxZone zone in skyboxZones)
        {
            Gizmos.DrawWireSphere(zone.position, zone.radius);
        }
    }
}