using UnityEngine;

public class Particulas : MonoBehaviour
{

    public ParticleSystem notas_p;
    public void CrearParticulas()   // al abrir el panel
    {
        if (notas_p != null)
            notas_p.Play();
    }

    public void DetenerParticulas() // al cerrar el panel
    {
        if (notas_p != null)
            notas_p.Stop();
    }
}
