using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CandadoCajaFuerte : MonoBehaviour
{
    [Header("Combinación Correcta")]
    [SerializeField] private int digito1Correcto = 5;
    [SerializeField] private int digito2Correcto = 3;
    [SerializeField] private int digito3Correcto = 7;

    [Header("Referencias UI")]
    [SerializeField] private GameObject panelCandado; //panel del candado
    [SerializeField] private TextMeshProUGUI textoDigito1;
    [SerializeField] private TextMeshProUGUI textoDigito2;
    [SerializeField] private TextMeshProUGUI textoDigito3;
    [SerializeField] private GameObject indicadorSeleccion1;
    [SerializeField] private GameObject indicadorSeleccion2;
    [SerializeField] private GameObject indicadorSeleccion3;

    [Header("Items al Abrir")]
    [SerializeField] private string nombreItem = "Llave Maestra";
    [SerializeField] private Sprite iconoItem;
    [SerializeField] private string descripcionItem = "Una llave encontrada dentro de la caja fuerte";

    //para mas adelante si es que me acuerdo
    [Header("Sonidos")]
    [SerializeField] private AudioClip sonidoCambioDigito;
    [SerializeField] private AudioClip sonidoCorrecto;
    [SerializeField] private AudioClip sonidoIncorrecto;
    [SerializeField] private AudioClip sonidoAbrir;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteCaja; //sprite de la caja
    [SerializeField] private Sprite spriteCajaAbierta; //sprite cuando esta abierta

    [Header("Eventos")]
    public UnityEvent onCajaAbierta;

    private int digitoActual1 = 0;
    private int digitoActual2 = 0;
    private int digitoActual3 = 0;
    private int digitoSeleccionado = 0; // 0, 1, 2 (cual dígito estamos editando)
    private bool candadoAbierto = false;
    private bool interfazActiva = false;
    private Player jugador;
    private AudioSource fuenteAudio;
    private InteractionObject interactionObject;

    void Start()
    {
        jugador = FindAnyObjectByType<Player>();
        fuenteAudio = GetComponent<AudioSource>();
        interactionObject = GetComponent<InteractionObject>();

        if (panelCandado != null)
        {
            panelCandado.SetActive(false);
        }

        ActualizarVisualDigitos();
        ActualizarIndicadorSeleccion();
    }

    void Update()
    {
        if (candadoAbierto) return;

        //si la interfaz está activa, manejar controles
        if (interfazActiva)
        {
            ManejarControlesCandado();
        }
    }

    public void AbrirCandado()
    {
        if (!candadoAbierto)
        {
            AbrirInterfazCandado();
        }
    }

    void ManejarControlesCandado()
    {
        // Cerrar con ESC
        if (Input.GetKeyDown(KeyCode.R))
        {
            CerrarInterfazCandado();
            return;
        }

        // Cambiar dígito seleccionado con A/D o Flechas Izquierda/Derecha
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            digitoSeleccionado--;
            if (digitoSeleccionado < 0) digitoSeleccionado = 2;
            ReproducirSonido(sonidoCambioDigito);
            ActualizarIndicadorSeleccion();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            digitoSeleccionado++;
            if (digitoSeleccionado > 2) digitoSeleccionado = 0;
            ReproducirSonido(sonidoCambioDigito);
            ActualizarIndicadorSeleccion();
        }

        // Cambiar valor del dígito seleccionado con W/S o Flechas Arriba/Abajo
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            CambiarDigito(1);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            CambiarDigito(-1);
        }

        // Verificar combinación con Enter o E
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            VerificarCombinacion();
        }
    }

    void CambiarDigito(int cambio)
    {
        switch (digitoSeleccionado)
        {
            case 0:
                digitoActual1 += cambio;
                if (digitoActual1 > 9) digitoActual1 = 0;
                if (digitoActual1 < 0) digitoActual1 = 9;
                break;
            case 1:
                digitoActual2 += cambio;
                if (digitoActual2 > 9) digitoActual2 = 0;
                if (digitoActual2 < 0) digitoActual2 = 9;
                break;
            case 2:
                digitoActual3 += cambio;
                if (digitoActual3 > 9) digitoActual3 = 0;
                if (digitoActual3 < 0) digitoActual3 = 9;
                break;
        }

        ReproducirSonido(sonidoCambioDigito);
        ActualizarVisualDigitos();
    }

    void ActualizarVisualDigitos()
    {
        if (textoDigito1 != null)
            textoDigito1.text = digitoActual1.ToString();

        if (textoDigito2 != null)
            textoDigito2.text = digitoActual2.ToString();

        if (textoDigito3 != null)
            textoDigito3.text = digitoActual3.ToString();
    }

    void ActualizarIndicadorSeleccion()
    {
        // Desactivar todos los indicadores
        if (indicadorSeleccion1 != null) indicadorSeleccion1.SetActive(false);
        if (indicadorSeleccion2 != null) indicadorSeleccion2.SetActive(false);
        if (indicadorSeleccion3 != null) indicadorSeleccion3.SetActive(false);

        // Activar solo el indicador del dígito seleccionado
        switch (digitoSeleccionado)
        {
            case 0:
                if (indicadorSeleccion1 != null) indicadorSeleccion1.SetActive(true);
                break;
            case 1:
                if (indicadorSeleccion2 != null) indicadorSeleccion2.SetActive(true);
                break;
            case 2:
                if (indicadorSeleccion3 != null) indicadorSeleccion3.SetActive(true);
                break;
        }

        // Tambien puedes cambiar el color del texto
        if (textoDigito1 != null)
            textoDigito1.color = (digitoSeleccionado == 0) ? Color.yellow : Color.white;
        if (textoDigito2 != null)
            textoDigito2.color = (digitoSeleccionado == 1) ? Color.yellow : Color.white;
        if (textoDigito3 != null)
            textoDigito3.color = (digitoSeleccionado == 2) ? Color.yellow : Color.white;
    }

    void VerificarCombinacion()
    {
        if (digitoActual1 == digito1Correcto &&
            digitoActual2 == digito2Correcto &&
            digitoActual3 == digito3Correcto)
        {
            //¡CORRECTO!
            CombinacionCorrecta();
        }
        else
        {
            //Incorrecto
            CombinacionIncorrecta();
        }
    }

    void CombinacionCorrecta()
    {
        Debug.Log("¡Combinación correcta! Caja fuerte abierta.");

        ReproducirSonido(sonidoCorrecto);
        ReproducirSonido(sonidoAbrir);

        candadoAbierto = true;

        // Cambiar sprite de la caja
        if (spriteCaja != null && spriteCajaAbierta != null)
        {
            spriteCaja.sprite = spriteCajaAbierta;
        }


        //se agrega objeto a el inventario
        if (!string.IsNullOrEmpty(nombreItem))
        {
            if (Inventario.Instance != null)
            {
                Inventario.Instance.AddItem(nombreItem, iconoItem, descripcionItem);
                Debug.Log("Item agregado al inventario: " + nombreItem);
            }
            else
            {
                Debug.LogError("¡Error! No se encontró Inventario.Instance");
            }
        }
        else
        {
            Debug.LogWarning("No hay item configurado para esta caja fuerte");
        }

        onCajaAbierta?.Invoke();

        // Cerrar interfaz después de un momento
        Invoke("CerrarInterfazCandado", 1.5f);
    }

    void CombinacionIncorrecta()
    {
        Debug.Log("Combinación incorrecta. Intenta de nuevo.");
        ReproducirSonido(sonidoIncorrecto);
    }
    void AbrirInterfazCandado()
    {
        interfazActiva = true;

        if (panelCandado != null)
        {
            panelCandado.SetActive(true);
        }

        if (interactionObject != null)
        {
            interactionObject.ShowPrompt(false);
        }

        // Pausar al jugador
        if (jugador != null)
        {
            jugador.SetCanMove(false);
        }

        digitoSeleccionado = 0;
        ActualizarVisualDigitos();
        ActualizarIndicadorSeleccion();
    }

    void CerrarInterfazCandado()
    {
        interfazActiva = false;

        if (panelCandado != null)
        {
            panelCandado.SetActive(false);
        }

        // Reanudar al jugador
        if (jugador != null)
        {
            jugador.SetCanMove(true);
        }
    }

    void ReproducirSonido(AudioClip clip)
    {
        if (fuenteAudio != null && clip != null)
        {
            fuenteAudio.PlayOneShot(clip);
        }
    }

    public bool EstaAbierta()
    {
        return candadoAbierto;
    }
}
