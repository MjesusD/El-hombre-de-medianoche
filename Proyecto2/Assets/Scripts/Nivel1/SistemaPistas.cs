using System.Collections.Generic;
using UnityEngine;

public class SistemaPistas : MonoBehaviour
{
    [System.Serializable]
    public class Pista
    {
        public string nombrePista;      // Ej: "Primer Dígito"
        public int digito;              // El número que representa (0-9)
        public bool encontrada = false;
    }

    [Header("Pistas de la Combinación")]
    [SerializeField] private List<Pista> pistas = new List<Pista>();

    [Header("UI de Seguimiento")]
    [SerializeField] private GameObject panelPistas; // Panel que muestra las pistas encontradas

    // Singleton
    public static SistemaPistas Instancia { get; private set; }

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (panelPistas != null)
        {
            panelPistas.SetActive(false);
        }
    }

    //marca una pista como encontrada
    public void EncontrarPista(string nombrePista)
    {
        Pista pista = pistas.Find(p => p.nombrePista == nombrePista);

        if (pista != null && !pista.encontrada)
        {
            pista.encontrada = true;
            Debug.Log($"Pista encontrada: {nombrePista} - Dígito: {pista.digito}");

            // Agregar al inventario como nota
            if (Inventario.Instance != null)
            {
                Inventario.Instance.AddItem(
                    nombrePista,
                    null,
                    $"Has descubierto un digito de la combinacion: {pista.digito}"
                );
            }

            VerificarTodasLasPistas();
        }
    }

    //verifica si todas las pistas han sido encontradas
    private void VerificarTodasLasPistas()
    {
        bool todasEncontradas = true;
        foreach (Pista pista in pistas)
        {
            if (!pista.encontrada)
            {
                todasEncontradas = false;
                break;
            }
        }

        if (todasEncontradas)
        {
            Debug.Log("¡Todas las pistas encontradas! Ya puedes abrir la caja fuerte.");
            MostrarCombinacionCompleta();
        }
    }

    //mostrar la combinacion completa cuando se encuentren todas las pistas
    private void MostrarCombinacionCompleta()
    {
        string combinacion = "";
        foreach (Pista pista in pistas)
        {
            combinacion += pista.digito;
        }

        Debug.Log($"Combinacion completa: {combinacion}");

        // Opcional: Agregar nota final al inventario
        if (Inventario.Instance != null)
        {
            Inventario.Instance.AddItem(
                "Combinación Completa",
                null,
                $"La combinación de la caja fuerte es: {combinacion}"
            );
        }
    }

    //verificar si una pista especifica fue encontrada
    public bool PistaEncontrada(string nombrePista)
    {
        Pista pista = pistas.Find(p => p.nombrePista == nombrePista);
        return pista != null && pista.encontrada;
    }

    //obtener el digito de una pista
    public int ObtenerDigito(string nombrePista)
    {
        Pista pista = pistas.Find(p => p.nombrePista == nombrePista);
        return pista != null ? pista.digito : -1;
    }

    //verifica si todas las pistas fueron encontradas
    public bool TodasLasPistasEncontradas()
    {
        foreach (Pista pista in pistas)
        {
            if (!pista.encontrada)
                return false;
        }
        return true;
    }

    //obtener cantidad de pistas encontradas
    public int CantidadPistasEncontradas()
    {
        int contador = 0;
        foreach (Pista pista in pistas)
        {
            if (pista.encontrada)
                contador++;
        }
        return contador;
    }

    //obtener la combinacion como string (solo si todas fueron encontradas)
    public string ObtenerCombinacion()
    {
        if (!TodasLasPistasEncontradas())
            return "???";

        string combinacion = "";
        foreach (Pista pista in pistas)
        {
            combinacion += pista.digito.ToString();
        }
        return combinacion;
    }
}
