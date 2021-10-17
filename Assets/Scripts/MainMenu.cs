using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject creditosCanvas;
    public GameObject ComoJugarCanvas;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Juego()
    {
        SceneManager.LoadScene("Juego");
    }

    public void CanvasCreditos()
    {
        creditosCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }
    public void CanvasComoJugar()
    {
        ComoJugarCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }

    public void CanvasMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        creditosCanvas.SetActive(false);
    }
}
