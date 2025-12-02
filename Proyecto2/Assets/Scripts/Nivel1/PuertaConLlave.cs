using UnityEngine;

public class PuertaConLlave : MonoBehaviour
{
    [Header("Llave Requerida")]
    [SerializeField] private string nombreLlaveRequerida = "Llave Maestra";
    [SerializeField] private bool consumirLlave = false; // Si la llave se gasta al usarla
    [SerializeField] private bool requiereSeleccionarLlave = true;

    [Header("Referencias")]
    [SerializeField] private SceneTransition sceneTransition; // Script de transición

    [Header("Mensajes")]
    [SerializeField] private string mensajePuertaCerrada = "La puerta está cerrada. Necesitas una llave.";
    [SerializeField] private string mensajePuertaAbierta = "Usaste la llave. La puerta se abre.";
    [SerializeField] private string mensajeNoTienesLlave = "No tienes la llave correcta.";

    [Header("Sonidos (Opcional)")]
    [SerializeField] private AudioClip sonidoPuertaCerrada;
    [SerializeField] private AudioClip sonidoPuertaAbierta;
    [SerializeField] private AudioClip sonidoUsarLlave;

    public bool puertaAbierta = false;
    private AudioSource audioSource;
    private string llaveSeleccionada = "";

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //si no se asigno el SceneTransition, buscarlo en este objeto
        if (sceneTransition == null)
        {
            sceneTransition = GetComponent<SceneTransition>();
        }
    }

    //metodo llamado desde InteractionObject cuando el jugador presiona E
    public void IntentarAbrirPuerta()
    {
        if (puertaAbierta)
        {
            //la puerta ya esta abierta
            CargarEscena();
            return;
        }

        //verificar si el jugador tiene la llave
        if (Inventario.Instance != null && Inventario.Instance.HasItem(nombreLlaveRequerida))
        {
            // ¡Tiene la llave! Abrir puerta
            PuertaCerrada();
            return;
        }
        if (requiereSeleccionarLlave)
        {
            //verificar si la llave esta seleccionada
            if (llaveSeleccionada != nombreLlaveRequerida)
            {
                //tiene la llave pero no la seleccionó
                DebeUsarLlave();
                return;
            }
        }
        AbrirPuerta();
    }

    public void UsarLlaveEnPuerta(string nombreLlave)
    {
        llaveSeleccionada = nombreLlave;
        Debug.Log($"Llave seleccionada: {nombreLlave}");

        // Si está cerca de esta puerta y tiene la llave correcta, intentar abrir
        if (nombreLlave == nombreLlaveRequerida && !puertaAbierta)
        {
            AbrirPuerta();
        }
        else if (nombreLlave != nombreLlaveRequerida)
        {
            MostrarMensaje("Esta llave no abre esta puerta.");
            llaveSeleccionada = "";
        }
    }

    private void AbrirPuerta()
    {
        puertaAbierta = true;

        Debug.Log($"Puerta abierta con {nombreLlaveRequerida}");

        //consumir la llave si esta configurado
        if (consumirLlave && Inventario.Instance != null)
        {
            Inventario.Instance.RemoveItem(nombreLlaveRequerida);
            Debug.Log($"La {nombreLlaveRequerida} se gastó.");
        }

        //mostrar mensaje
        MostrarMensaje(mensajePuertaAbierta);

        //reproducir sonidos
        ReproducirSonido(sonidoUsarLlave);
        ReproducirSonido(sonidoPuertaAbierta);

        //cargar escena después de un momento
        Invoke("CargarEscena", 1f);
    }

    private void PuertaCerrada()
    {
        Debug.Log("La puerta está cerrada. Se requiere: " + nombreLlaveRequerida);

        // Mostrar mensaje
        MostrarMensaje(mensajePuertaCerrada);

        // Sonido de puerta cerrada
        ReproducirSonido(sonidoPuertaCerrada);
    }

    private void DebeUsarLlave()
    {
        Debug.Log("Tienes la llave pero debes seleccionarla en el inventario primero.");
        ReproducirSonido(sonidoPuertaCerrada);
    }

    private void CargarEscena()
    {
        if (sceneTransition != null)
        {
            sceneTransition.LoadScene();
        }
        else
        {
            Debug.LogWarning("No hay SceneTransition asignado en esta puerta.");
        }
    }

    private void MostrarMensaje(string mensaje)
    {
        // Mostrar en DialogueManager si existe
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowBubble(mensaje, transform);
        }
        else
        {
            Debug.Log(mensaje);
        }
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public bool EstaAbierta()
    {
        return puertaAbierta;
    }

    public static PuertaConLlave puertaCercana;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            puertaCercana = this;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && puertaCercana == this)
        {
            puertaCercana = null;
            llaveSeleccionada = "";
        }
    }
}
