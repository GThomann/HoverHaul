using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuRapido : MonoBehaviour
{

    public Image pnlMenuRapido;
    public Button btnContinuar, btnMenuPrincipal;
    public bool menuRapidoAtivado;

    // Use this for initialization
    void Start()
    {
        btnContinuar.onClick = new Button.ButtonClickedEvent();
        btnContinuar.onClick.AddListener(() => Continuar());

        btnMenuPrincipal.onClick = new Button.ButtonClickedEvent();
        btnMenuPrincipal.onClick.AddListener(() => CarregarCena("Menu"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0 && menuRapidoAtivado == false)
        {
            menuRapidoAtivado = true;
            pnlMenuRapido.gameObject.SetActive(true);
            btnContinuar.Select();
        }
    }

    public void Continuar()
    {
        Time.timeScale = 1;
        pnlMenuRapido.gameObject.SetActive(false);
        menuRapidoAtivado = false;
    }

    public void CarregarCena(string cena)
    {
        SceneManager.LoadScene(cena);
        Continuar();
    }
}
