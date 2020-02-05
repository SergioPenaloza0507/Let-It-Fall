using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem system;
    ParticleSystem.MainModule main;

    private ParticleSystem.EmissionModule emissionModule;
    // Start is called before the first frame update
    void Awake()
    {
        system = GetComponent<ParticleSystem>();
        main = system.main;
        emissionModule = system.emission;
    }

    public void SetParticleCount(float val)
    {
        int toint = Mathf.CeilToInt(val);
        main.maxParticles = toint;
        emissionModule.rateOverTime = toint;
    }

    public void Play()
    {
        system.Play();
        Debug.LogFormat("{0} played?, {1}",system.name,system.isPlaying);
    }

    public void Stop()
    {
        system.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
        Debug.LogFormat("{0} Stopped?, {1}",system.name,!system.isPlaying);
    }
}
