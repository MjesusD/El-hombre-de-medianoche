using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleTuberias : MonoBehaviour
{
    [Header("Configuración del Puzzle")]
    [SerializeField] private int filas = 5;
    [SerializeField] private int columnas = 5;
    [SerializeField] private float tamañoCelda = 100f;

    [Header("Llave Requerida")]
    [SerializeField] private string nombreLlaveRequerida = "Llave Inglesa";
    [SerializeField] private bool consumirLlave = false;

    [Header("Referencias UI")]
    [SerializeField] private GameObject panelPuzzle;
    [SerializeField] private Transform contenedorGrid;
    [SerializeField] private GameObject prefabTuberia;
    [SerializeField] private TextMeshProUGUI textoEstado;
    [SerializeField] private GameObject botonVerificar;

    [Header("Sprites de Tuberías")]
    [SerializeField] private Sprite spriteRecta; // Tubería recta |
    [SerializeField] private Sprite spriteCurva; // Tubería curva L
    [SerializeField] private Sprite spriteT; // Tubería T
    [SerializeField] private Sprite spriteCruz; // Tubería + (cruz)
    [SerializeField] private Sprite spriteTapada; // Sin conexión

    [Header("Puntos de Inicio y Fin")]
    [SerializeField] private Vector2Int posicionInicio = new Vector2Int(0, 2); // Entrada
    [SerializeField] private Vector2Int posicionFin = new Vector2Int(4, 2); // Salida

    [Header("Mensajes")]
    [SerializeField] private string mensajeNecesitaLlave = "Necesitas una Llave Inglesa para reparar las tuberías.";
    [SerializeField] private string mensajeCorrecto = "¡Perfecto! Las tuberías están conectadas.";
    [SerializeField] private string mensajeIncorrecto = "Las tuberías no están conectadas correctamente.";

    [Header("Sonidos")]
    [SerializeField] private AudioClip sonidoRotar;
    [SerializeField] private AudioClip sonidoCorrecto;
    [SerializeField] private AudioClip sonidoIncorrecto;

    private Tuberia[,] grid;
    private bool puzzleResuelto = false;
    private AudioSource audioSource;
    private Player jugador;

    // Clase para cada tubería
    [System.Serializable]
    public class Tuberia
    {
        public GameObject objeto;
        public Image imagen;
        public Button boton;
        public TipoTuberia tipo;
        public int rotacion; // 0, 90, 180, 270
        public bool[] conexiones = new bool[4]; // Norte, Este, Sur, Oeste
        public Vector2Int posicion;

        public void Rotar()
        {
            rotacion = (rotacion + 90) % 360;
            imagen.transform.rotation = Quaternion.Euler(0, 0, -rotacion);
            ActualizarConexiones();
        }

        public void ActualizarConexiones()
        {
            bool[] base_conexiones = ObtenerConexionesBase(tipo);
            int pasos = rotacion / 90;

            for (int i = 0; i < 4; i++)
            {
                conexiones[i] = base_conexiones[(i - pasos + 4) % 4];
            }
        }

        private bool[] ObtenerConexionesBase(TipoTuberia tipo)
        {
            switch (tipo)
            {
                
                case TipoTuberia.Recta:
                    return new bool[] { true, false, true, false }; // Norte y Sur
                case TipoTuberia.Curva:
                    return new bool[] { true, false, false, true }; // Norte y Este
                case TipoTuberia.T:
                    return new bool[] { true, false, true, true }; // Norte, Este, Oeste
                case TipoTuberia.Cruz:
                    return new bool[] { true, true, true, true }; // Todas
                case TipoTuberia.Tapada:
                    return new bool[] { false, false, false, false }; // Ninguna
                default:
                    return new bool[] { false, false, false, false };
            }
        }
    }

    public enum TipoTuberia
    {
        Recta,
        Curva,
        T,
        Cruz,
        Tapada
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        jugador = FindAnyObjectByType<Player>();

        if (panelPuzzle != null)
        {
            panelPuzzle.SetActive(false);
        }
    }

    // Método llamado desde InteractionObject
    public void IntentarAbrirPuzzle()
    {
        if (puzzleResuelto)
        {
            MostrarMensaje("Ya reparaste las tuberías.");
            return;
        }

        // Verificar si tiene la llave
        if (Inventario.Instance == null || !Inventario.Instance.HasItem(nombreLlaveRequerida))
        {
            MostrarMensaje(mensajeNecesitaLlave);
            return;
        }

        // Abrir puzzle
        AbrirPuzzle();
    }

    public void AbrirPuzzle()
    {
        if (panelPuzzle != null)
        {
            panelPuzzle.SetActive(true);
        }

        // Pausar jugador
        if (jugador != null)
        {
            jugador.SetCanMove(false);
        }

        // Generar el puzzle
        GenerarPuzzle();

        if (textoEstado != null)
        {
            textoEstado.text = "Rota las tuberías para conectar desde la entrada hasta la salida.";
        }
    }

    void GenerarPuzzle()
    {
        // Limpiar grid anterior
        if (grid != null)
        {
            foreach (var tuberia in grid)
            {
                if (tuberia != null && tuberia.objeto != null)
                {
                    Destroy(tuberia.objeto);
                }
            }
        }

        grid = new Tuberia[filas, columnas];


        // Solo asegurar que existe el componente
        if (contenedorGrid != null)
        {
            GridLayoutGroup gridLayout = contenedorGrid.GetComponent<GridLayoutGroup>();
            if (gridLayout == null)
            {
                gridLayout = contenedorGrid.AddComponent<GridLayoutGroup>();
                // Configuración por defecto solo si no existe
                gridLayout.cellSize = new Vector2(tamañoCelda, tamañoCelda);
                gridLayout.spacing = new Vector2(5, 5);
                gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayout.constraintCount = columnas;
            }
            // Si ya existe, no tocar su configuración
        }

        // Crear las tuberías (nivel 3 - difícil)
        for (int fila = 0; fila < filas; fila++)
        {
            for (int col = 0; col < columnas; col++)
            {
                GameObject celda = Instantiate(prefabTuberia, contenedorGrid);
                Tuberia tuberia = new Tuberia();
                tuberia.objeto = celda;
                tuberia.imagen = celda.GetComponent<Image>();
                tuberia.boton = celda.GetComponent<Button>();
                tuberia.posicion = new Vector2Int(fila, col);

                // Asignar tipo según el nivel (más difícil)
                tuberia.tipo = AsignarTipoTuberia(fila, col);

                // Sprite según tipo
                tuberia.imagen.sprite = ObtenerSprite(tuberia.tipo);

                // Rotación aleatoria para dificultad
                tuberia.rotacion = Random.Range(0, 4) * 90;
                tuberia.imagen.transform.rotation = Quaternion.Euler(0, 0, -tuberia.rotacion);

                tuberia.ActualizarConexiones();

                // Añadir listener al botón
                int f = fila;
                int c = col;
                tuberia.boton.onClick.AddListener(() => RotarTuberia(f, c));

                grid[fila, col] = tuberia;
            }
        }

        // Marcar inicio y fin con color diferente
        if (grid[posicionInicio.x, posicionInicio.y] != null)
        {
            grid[posicionInicio.x, posicionInicio.y].imagen.color = Color.green;
        }
        if (grid[posicionFin.x, posicionFin.y] != null)
        {
            grid[posicionFin.x, posicionFin.y].imagen.color = Color.red;
        }
    }

    TipoTuberia AsignarTipoTuberia(int fila, int col)
    {
        // Solución predefinida para nivel 3 (puedes cambiarla)
        // Esta es una configuración que tiene solución

        // Fila del medio (camino principal)
        if (fila == 2)
        {
            return TipoTuberia.Recta;
        }

        // Algunas curvas
        if ((fila == 1 || fila == 3) && (col % 2 == 0))
        {
            return TipoTuberia.Curva;
        }

        // Algunas T
        if (Random.value < 0.2f)
        {
            return TipoTuberia.T;
        }

        // Más curvas para dificultad
        if (Random.value < 0.5f)
        {
            return TipoTuberia.Curva;
        }

        return TipoTuberia.Recta;
    }

    Sprite ObtenerSprite(TipoTuberia tipo)
    {
        switch (tipo)
        {
            case TipoTuberia.Recta:
                return spriteRecta;
            case TipoTuberia.Curva:
                return spriteCurva;
            case TipoTuberia.T:
                return spriteT;
            case TipoTuberia.Cruz:
                return spriteCruz;
            case TipoTuberia.Tapada:
                return spriteTapada;
            default:
                return spriteRecta;
        }
    }

    void RotarTuberia(int fila, int col)
    {
        if (grid[fila, col] != null)
        {
            grid[fila, col].Rotar();
            ReproducirSonido(sonidoRotar);

            // Debug para ver la rotación
            Debug.Log($"Tubería rotada en [{fila},{col}] - Nueva rotación: {grid[fila, col].rotacion}°");

            // Mostrar conexiones
            bool[] conex = grid[fila, col].conexiones;
            Debug.Log($"Conexiones: Norte={conex[0]}, Este={conex[1]}, Sur={conex[2]}, Oeste={conex[3]}");
        }
        else
        {
            Debug.LogWarning($"No hay tubería en [{fila},{col}]");
        }
    }

    public void VerificarSolucion()
    {
        Debug.Log("========== VERIFICANDO SOLUCIÓN ==========");
        Debug.Log($"Inicio: [{posicionInicio.x},{posicionInicio.y}]");
        Debug.Log($"Fin: [{posicionFin.x},{posicionFin.y}]");

        bool correcto = VerificarCamino();

        Debug.Log($"Resultado: {(correcto ? "CORRECTO " : "INCORRECTO ")}");
        Debug.Log("==========================================");

        if (correcto)
        {
            // ¡Puzzle resuelto!
            PuzzleCompletado();
        }
        else
        {
            // Incorrecto
            if (textoEstado != null)
            {
                textoEstado.text = mensajeIncorrecto;
                textoEstado.color = Color.red;
            }
            ReproducirSonido(sonidoIncorrecto);
            Invoke("RestablecerTexto", 2f);
        }
    }

    bool VerificarCamino()
    {
        // Algoritmo BFS para verificar si hay camino desde inicio a fin
        bool[,] visitado = new bool[filas, columnas];
        return BFS(posicionInicio, posicionFin, visitado);
    }

    bool BFS(Vector2Int inicio, Vector2Int fin, bool[,] visitado)
    {
        Debug.Log($"BFS: Visitando [{inicio.x},{inicio.y}]");

        if (inicio == fin)
        {
            Debug.Log("¡Llegamos al final!");
            return true;
        }

        visitado[inicio.x, inicio.y] = true;

        // Direcciones: Norte, Este, Sur, Oeste
        Vector2Int[] direcciones = new Vector2Int[]
        {
            new Vector2Int(-1, 0), // Norte
            new Vector2Int(0, 1),  // Este
            new Vector2Int(1, 0),  // Sur
            new Vector2Int(0, -1)  // Oeste
        };

        string[] nombresDirecciones = { "Norte", "Este", "Sur", "Oeste" };

        for (int i = 0; i < 4; i++)
        {
            // Verificar si la tubería actual tiene conexión en esta dirección
            if (!grid[inicio.x, inicio.y].conexiones[i])
            {
                Debug.Log($"  {nombresDirecciones[i]}: No hay conexión desde aquí");
                continue;
            }

            Vector2Int siguiente = inicio + direcciones[i];

            // Verificar límites
            if (siguiente.x < 0 || siguiente.x >= filas ||
                siguiente.y < 0 || siguiente.y >= columnas)
            {
                Debug.Log($"  {nombresDirecciones[i]}: Fuera de límites");
                continue;
            }

            // Si ya visitado, saltar
            if (visitado[siguiente.x, siguiente.y])
            {
                Debug.Log($"  {nombresDirecciones[i]}: Ya visitado");
                continue;
            }

            // Verificar si la tubería siguiente tiene conexión de vuelta
            int direccionOpuesta = (i + 2) % 4;
            if (!grid[siguiente.x, siguiente.y].conexiones[direccionOpuesta])
            {
                Debug.Log($"  {nombresDirecciones[i]}: La tubería siguiente no conecta de vuelta");
                continue;
            }

            Debug.Log($"  {nombresDirecciones[i]}: ✓ Conexión válida a [{siguiente.x},{siguiente.y}]");

            // Recursión
            if (BFS(siguiente, fin, visitado))
                return true;
        }

        Debug.Log($"No hay más caminos desde [{inicio.x},{inicio.y}]");
        return false;
    }

    void PuzzleCompletado()
    {
        puzzleResuelto = true;

        if (textoEstado != null)
        {
            textoEstado.text = mensajeCorrecto;
            textoEstado.color = Color.green;
        }

        ReproducirSonido(sonidoCorrecto);

        // Consumir llave si está configurado
        if (consumirLlave && Inventario.Instance != null)
        {
            Inventario.Instance.RemoveItem(nombreLlaveRequerida);
        }

        // Cerrar puzzle después de un momento
        Invoke("CerrarPuzzle", 2f);
    }

    void RestablecerTexto()
    {
        if (textoEstado != null)
        {
            textoEstado.text = "Rota las tuberías para conectar desde la entrada hasta la salida.";
            textoEstado.color = Color.white;
        }
    }

    public void CerrarPuzzle()
    {
        if (panelPuzzle != null)
        {
            panelPuzzle.SetActive(false);
        }

        // Reanudar jugador
        if (jugador != null)
        {
            jugador.SetCanMove(true);
        }
    }

    void MostrarMensaje(string mensaje)
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowBubble(mensaje, transform);
        }
        else
        {
            Debug.Log(mensaje);
        }
    }

    void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public bool EstáResuelto()
    {
        return puzzleResuelto;
    }
}