using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    [System.Serializable]
    public class SkyboxZone
    {
        public string zoneName;
        public Vector3 position;
        public float radius = 10f;
        public Material skyboxMaterial;

        [Header("Ambient Sound")]
        public AudioClip ambientSound;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [Header("Zones")]
    public SkyboxZone[] skyboxZones;
    public Material defaultSkybox;

    [Header("Default Sound")]
    public AudioClip defaultAmbientSound;
    [Range(0f, 1f)] public float defaultVolume = 1f;

    private Skybox cameraSkybox;
    private Material currentSkybox;
    private AudioSource audioSource;

    void Start()
    {
        cameraSkybox = Camera.main.GetComponent<Skybox>();

        if (cameraSkybox == null)
            Debug.LogError("No Skybox component found on Main Camera!");

        // สร้าง AudioSource อัตโนมัติ
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.spatialBlend = 0f; // 2D Sound

        currentSkybox = defaultSkybox;
        cameraSkybox.material = defaultSkybox;
        PlaySound(defaultAmbientSound, defaultVolume);
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
                    PlaySound(zone.ambientSound, zone.volume);
                }
                return;
            }
        }

        if (currentSkybox != defaultSkybox)
        {
            cameraSkybox.material = defaultSkybox;
            currentSkybox = defaultSkybox;
            PlaySound(defaultAmbientSound, defaultVolume);
        }
    }

    void PlaySound(AudioClip clip, float volume)
    {
        if (audioSource.clip == clip) return; // เล่นอยู่แล้ว ไม่ต้องเปลี่ยน

        audioSource.clip = clip;
        audioSource.volume = volume;

        if (clip != null)
            audioSource.Play();
        else
            audioSource.Stop();
    }

    void OnDrawGizmos()
    {
        if (skyboxZones == null) return;
        Gizmos.color = Color.cyan;
        foreach (SkyboxZone zone in skyboxZones)
            Gizmos.DrawWireSphere(zone.position, zone.radius);
    }
}