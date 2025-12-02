using UnityEngine;

public class SistemaDuchaEspejo : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject espejo; //el objeto espejo
    [SerializeField] private SpriteRenderer numeroEnEspejo; //sprite del numero que aparece
    [SerializeField] private GameObject efectoVapor; // Efecto de partículas de vapor para mas adelante 
    [SerializeField] private AudioSource sonidoAgua; // Sonido del agua corriendo

    [Header("Configuración de la Pista")]
    [SerializeField] private string nombrePista = "Tercer Dígito";
    [SerializeField] private string mensajePista = "El vapor revela un número en el espejo: 3";
    [SerializeField] private Sprite iconoPista;

    [Header("Estados")]
    [SerializeField] private float tiempoParaRevelar = 3f; //tiempo que tarda en aparecer el numero
    [SerializeField] private float tiempoVaporVisible = 10f; //cuanto tiempo permanece visible

    [Header("Sonidos")]
    [SerializeField] private AudioClip sonidoActivarDucha;
    [SerializeField] private AudioClip sonidoNumeroAparece;

    private bool duchaActivada = false;
    private bool numeroRevelado = false;
    private bool pistaRecogida = false;
    private float tiempoVapor = 0f;

    private SpriteRenderer spriteRendererNumero;
    private Color colorOriginal;

    void Start()
    {
        //obtener sprite renderer del numero
        if (numeroEnEspejo != null)
        {
            spriteRendererNumero = numeroEnEspejo;
            colorOriginal = spriteRendererNumero.color;

            //hacer invisible al inicio
            Color invisible = colorOriginal;
            invisible.a = 0f;
            spriteRendererNumero.color = invisible;
        }

        //ocultar efecto de vapor al inicio
        if (efectoVapor != null)
        {
            efectoVapor.SetActive(false);
        }

        //detener sonido de agua
        if (sonidoAgua != null)
        {
            sonidoAgua.Stop();
        }
    }

    void Update()
    {
        //si la ducha esta activada, manejar el vapor
        if (duchaActivada && !numeroRevelado)
        {
            tiempoVapor += Time.deltaTime;

            //despues de un tiempo, revelar el numero
            if (tiempoVapor >= tiempoParaRevelar)
            {
                RevelarNumero();
            }
        }

        //si el numero esta revelado, hacer que se desvanezca despues de un tiempo
        if (numeroRevelado && !pistaRecogida)
        {
            tiempoVapor += Time.deltaTime;

            if (tiempoVapor >= tiempoParaRevelar + tiempoVaporVisible)
            {
                DesvanecerNumero();
            }
        }
    }

    //metodo llamado desde InteractionObject de la ducha
    public void ActivarDucha()
    {
        if (duchaActivada)
        {
            //apagar ducha
            DesactivarDucha();
            return;
        }

        duchaActivada = true;
        tiempoVapor = 0f;

        Debug.Log("Ducha activada. El vapor comienza a acumularse...");

        //activar efecto de vapor
        if (efectoVapor != null)
        {
            efectoVapor.SetActive(true);
        }

        //reproducir sonido de agua
        if (sonidoAgua != null)
        {
            sonidoAgua.loop = true;
            sonidoAgua.Play();
        }

        //sonido al activar
        if (sonidoActivarDucha != null)
        {
            AudioSource.PlayClipAtPoint(sonidoActivarDucha, transform.position);
        }

        //mostrar mensaje
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowBubble("Activaste el agua caliente. El vapor comienza a llenar el baño...", transform);
        }
    }

    public void DesactivarDucha()
    {
        duchaActivada = false;

        //detener agua
        if (sonidoAgua != null)
        {
            sonidoAgua.Stop();
        }

        //ocultar vapor gradualmente
        if (efectoVapor != null)
        {
            efectoVapor.SetActive(false);
        }

        Debug.Log("Ducha desactivada.");
    }

    void RevelarNumero()
    {
        if (numeroRevelado) return;

        numeroRevelado = true;
        Debug.Log("¡El número aparece en el espejo!");

        //hacer visible el número gradualmente
        if (spriteRendererNumero != null)
        {
            StartCoroutine(FadeInNumero());
        }

        //sonido al aparecer
        if (sonidoNumeroAparece != null)
        {
            AudioSource.PlayClipAtPoint(sonidoNumeroAparece, espejo.transform.position);
        }

        //mensaje
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowBubble("¡Algo aparece en el espejo con el vapor!", espejo.transform);
        }
    }

    void DesvanecerNumero()
    {
        Debug.Log("El vapor se disipa y el número desaparece...");

        //desvanecer el numero
        if (spriteRendererNumero != null)
        {
            StartCoroutine(FadeOutNumero());
        }

        numeroRevelado = false;
        tiempoVapor = 0f;
    }

    //metodo llamado desde InteractionObject del espejo
    public void ExaminarEspejo()
    {
        if (!numeroRevelado)
        {
            string mensaje = duchaActivada ?
                "El espejo se está empañando con el vapor..." :
                "El espejo está limpio. No ves nada especial.";

            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.ShowBubble(mensaje, espejo.transform);
            }

            Debug.Log(mensaje);
        }
        else if (!pistaRecogida)
        {
            //examinar el numero revelado
            RecogerPista();
        }
        else
        {
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.ShowBubble("Ya anotaste el número que apareció aquí.", espejo.transform);
            }
        }
    }

    void RecogerPista()
    {
        pistaRecogida = true;

        //registrar la pista en el sistema
        if (SistemaPistas.Instancia != null)
        {
            SistemaPistas.Instancia.EncontrarPista(nombrePista);
        }

        //mostrar mensaje
        Debug.Log($"✓ {mensajePista}");

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowBubble(mensajePista, espejo.transform);
        }

        // Mantener el número visible después de recogerlo
    }

    // Coroutine para hacer aparecer el numero gradualmente
    System.Collections.IEnumerator FadeInNumero()
    {
        float duracion = 2f;
        float tiempoTranscurrido = 0f;

        Color colorInvisible = colorOriginal;
        colorInvisible.a = 0f;

        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, colorOriginal.a, tiempoTranscurrido / duracion);

            Color nuevoColor = colorOriginal;
            nuevoColor.a = alpha;
            spriteRendererNumero.color = nuevoColor;

            yield return null;
        }

        spriteRendererNumero.color = colorOriginal;
    }

    // Coroutine para desvanecer el número
    System.Collections.IEnumerator FadeOutNumero()
    {
        float duracion = 2f;
        float tiempoTranscurrido = 0f;

        Color colorActual = spriteRendererNumero.color;

        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float alpha = Mathf.Lerp(colorActual.a, 0f, tiempoTranscurrido / duracion);

            Color nuevoColor = colorActual;
            nuevoColor.a = alpha;
            spriteRendererNumero.color = nuevoColor;

            yield return null;
        }

        Color invisible = colorActual;
        invisible.a = 0f;
        spriteRendererNumero.color = invisible;
    }

    //metodo para resetear (testing)
    public void Resetear()
    {
        duchaActivada = false;
        numeroRevelado = false;
        pistaRecogida = false;
        tiempoVapor = 0f;

        if (spriteRendererNumero != null)
        {
            Color invisible = colorOriginal;
            invisible.a = 0f;
            spriteRendererNumero.color = invisible;
        }

        if (efectoVapor != null)
        {
            efectoVapor.SetActive(false);
        }

        if (sonidoAgua != null)
        {
            sonidoAgua.Stop();
        }
    }

    // Verificar estados (para debugging)
    public bool DuchaEstaActivada() => duchaActivada;
    public bool NumeroEstaRevelado() => numeroRevelado;
    public bool PistaFueRecogida() => pistaRecogida;
}
