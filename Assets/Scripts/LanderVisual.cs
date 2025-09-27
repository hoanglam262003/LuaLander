using System;
using UnityEngine;

public class LanderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftParticleSystem;
    [SerializeField] private ParticleSystem middleParticleSystem;
    [SerializeField] private ParticleSystem rightParticleSystem;

    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;

        SetEnabledThrusterParticleSystem(middleParticleSystem, false);
        SetEnabledThrusterParticleSystem(leftParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightParticleSystem, false);
    }

    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        SetEnabledThrusterParticleSystem(middleParticleSystem, false);
        SetEnabledThrusterParticleSystem(leftParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightParticleSystem, false);
    }

    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        SetEnabledThrusterParticleSystem(middleParticleSystem, true);
        SetEnabledThrusterParticleSystem(leftParticleSystem, true);
        SetEnabledThrusterParticleSystem(rightParticleSystem, true);
    }

    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        SetEnabledThrusterParticleSystem(rightParticleSystem, true);
    }

    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        SetEnabledThrusterParticleSystem(leftParticleSystem, true);
    }

    private void SetEnabledThrusterParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule em = particleSystem.emission;
        em.enabled = enabled;
    }
}
