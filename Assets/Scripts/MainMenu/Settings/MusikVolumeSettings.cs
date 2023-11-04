using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusikVolumeSettings : MonoBehaviour
{
    private float volume;
    private AudioSource audioSrc;
    [SerializeField]
    private GameObject musikRegulator;
    [SerializeField]
    private GameObject soundRegulator;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.volume = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        volume = musikRegulator.transform.rotation.z;
        audioSrc.volume = volume;
    }
}
