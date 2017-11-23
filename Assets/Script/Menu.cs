using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public Button btnJogar, btnOpcoes, btnOpcoesVoltar, btnSair, btnSobre, btnSobreVoltar;
    public Dropdown ddlDificuldade;
    public Slider sldMusVolume, sldSfxVolume, sldMaxVoltas;
    public Image pnlPrincipal, pnlOpcoes, pnlSobre;
	public Text voltasTxt;
	public SoundManager Sm;
	//private Som musicas;
	public AudioSource atual;

    // Use this for initialization
    void Start (){
		//obtem o componente AudioSource do objeto com nome "SoundManager" (responsavel pela musica de fundo)
		atual = GameObject.Find("SoundManager").GetComponent<AudioSource>(); 
		Sm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		//adicionando os eventos aos botoes
        btnJogar.onClick = new Button.ButtonClickedEvent();
        btnJogar.onClick.AddListener(() => Jogar());

        btnOpcoes.onClick = new Button.ButtonClickedEvent();
        btnOpcoes.onClick.AddListener(() => Opcoes(true));

        btnOpcoesVoltar.onClick = new Button.ButtonClickedEvent();
        btnOpcoesVoltar.onClick.AddListener(() => Opcoes(false));

        btnSair.onClick = new Button.ButtonClickedEvent();
        btnSair.onClick.AddListener(() => Sair());

        btnSobre.onClick = new Button.ButtonClickedEvent();
        btnSobre.onClick.AddListener(() => Sobre(true));

        btnSobreVoltar.onClick = new Button.ButtonClickedEvent();
        btnSobreVoltar.onClick.AddListener(() => Sobre(false));

		//preenchimento da lista de dificuldade
        ddlDificuldade.ClearOptions();
        ddlDificuldade.options.Add(new Dropdown.OptionData("Fácil"));
        ddlDificuldade.options.Add(new Dropdown.OptionData("Médio"));
        ddlDificuldade.options.Add(new Dropdown.OptionData("Difícil"));
        ddlDificuldade.value = 1;

        //posiciona os sliders
        sldMusVolume.value = Sm.musicas.volMusic;
        sldSfxVolume.value = Sm.musicas.volSFX;

		//seleciona, carrega musica e comeca a tocar a musica
		atual.clip = Sm.musicas.getMusic ("Menu", Random.Range (0, Sm.musicas.getTotalMusic ("Menu")));
        //atual.volume = Settings.valMusVolume; //justa o volume da musica (pode ser ajustado no menu opcoes)
        atual.volume = Sm.musicas.volMusic; //justa o volume da musica (pode ser ajustado no menu opcoes)
		atual.Play (); //toca a musica selecionada

    } //fim de Start()

    void Update() {
        //se nao estiver tocando, troca a musica e comeca a tocar
        if (!atual.isPlaying) {
            atual.clip = Sm.musicas.getMusic("Menu", Random.Range(0, Sm.musicas.getTotalMusic("Menu")));
            atual.Play(); //toca a musica selecionada
        }
    }

    public void Jogar(){
        //Settings.valMusVolume = sldMusVolume.value; //salva o volume da musica
        //Settings.valSfxVolume = sldSfxVolume.value; //salva o volume dos efeitos
        Settings.valDificuldade = ddlDificuldade.value; //salva o nivel de dificuldade
        //Settings.numVoltas = (int)sldMaxVoltas.value; //salva o numero de voltas da corrida
        CarregarCena("Track1"); //carrega a corrida
    }

    public void Sair(){
        Application.Quit();
    }

    public void CarregarCena(string cena){
        SceneManager.LoadScene(cena);
    }

    public void Opcoes(bool ativo){
		//troca a exibicao para o menu correto
        pnlPrincipal.gameObject.SetActive(!ativo);
        pnlOpcoes.gameObject.SetActive(ativo);

        if (ativo) {
			ddlDificuldade.Select (); //seleciona a dificuldade
			//atualiza o texto no slider
			//o objeto de texto voltas so estara ativo no menu opcoes por isso ele so eh obtido neste ponto
			voltasTxt = GameObject.Find("NumVoltas").GetComponent<Text>();
			voltasTxt.text = sldMaxVoltas.value.ToString();
			sldMaxVoltas.onValueChanged.AddListener(delegate{AtualizaOpcao();});
			sldSfxVolume.onValueChanged.AddListener(delegate{AtualizaOpcao();});
			sldMusVolume.onValueChanged.AddListener(delegate{AtualizaOpcao();});
		}
        else
            btnJogar.Select();
    }
	void AtualizaGUI(){
		//voltas = GameObject.Find("NumVoltas").GetComponent<Text>();
		Settings.numVoltas = (int)sldMaxVoltas.value; //salva o numero de voltas da corrida
		voltasTxt.text = sldMaxVoltas.value.ToString ();
	}
	void AtualizaOpcao(){
		//o objeto de texto voltas so estara ativo no menu opcoes por isso ele so eh obtido neste ponto
		//Text voltasTxt = GameObject.Find("NumVoltas").GetComponent<Text>();
		Settings.valDificuldade = ddlDificuldade.value; //salva o nivel de dificuldade
        Settings.numVoltas = (int)sldMaxVoltas.value; //salva o numero de voltas da corrida
        //Settings.valSfxVolume = sldSfxVolume.value; //salva o volume dos efeitos no Settings
        //Settings.valMusVolume = sldMusVolume.value; //salva o volume da musica no Settings
        Sm.musicas.volMusic = sldMusVolume.value; //salva o volume da musica no SoundManager
        Sm.musicas.volSFX = sldSfxVolume.value; //salva o volume dos efeitos no SoundManager
        voltasTxt.text = sldMaxVoltas.value.ToString (); //atualiza o texto do numero de voltas
		atual.volume = Sm.musicas.volMusic; //justa o volume da musica
	}

    public void Sobre(bool ativo){
		//troca a exibicao para o menu correto
        pnlPrincipal.gameObject.SetActive(!ativo);
        pnlSobre.gameObject.SetActive(ativo);

        if (ativo)
            btnSobreVoltar.Select();
        else
            btnJogar.Select();
    }
} //fim de Menu
