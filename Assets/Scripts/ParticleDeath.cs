using UnityEngine;

public class ParticleDeath : MonoBehaviour
{
    private ParticleSystemRenderer particleRenderer;

    private void Awake()
    {
        particleRenderer = GetComponent<ParticleSystemRenderer>();
    }

    public void SetColor(Color color, Color emissionColor)
    {
        particleRenderer.material.color = color;
        particleRenderer.material.SetColor("_Emission", emissionColor);
    }


}
