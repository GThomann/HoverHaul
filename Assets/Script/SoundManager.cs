using UnityEngine;
using System.Collections;

/*
 * 	Este script deve ser colocado em um empty Object
 * 	com um componente AudioSource.
 * 	O objeto deve estar na tela inicial do projeto (jogo)
 */

public class SoundManager : MonoBehaviour {
	//para ver no ambiente de cena a localizacao do objeto
	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (transform.position, 1.5f);
	}
    //atributo de verificacao de instancia
    public static SoundManager instancia;

	//atributos iniciais gerais
	public AudioClip[] resourceRaceMusic, resourceMenuMusic; //selecao de musicas do menu e da corrida
	public AudioClip[] resourceFx;
	public Som musicas;
	private int indice;

	//classe de sons
	public class Som{
		//atributos
		private AudioClip[] m_Race; //musicas de fundo ou menu
		private AudioClip[] m_Menu;
		private AudioClip[] efeitos; //efeitos especiais
        public float volMusic, volSFX;
		//construtor
		public Som(int numMenuMusic, int numRaceMusic, int numFx){
			this.m_Race = new AudioClip[numRaceMusic]; //cria um array com todas as checkpoints, na ordem que aparecem na hierarquia
			this.m_Menu = new AudioClip[numMenuMusic]; //cria um array com todas as checkpoints, na ordem que aparecem na hierarquia
			this.efeitos = new AudioClip[numFx];
            this.volMusic = 0.9f; //valor inicial
            this.volSFX = 0.6f; //valor inicial
		}
		
		//adiciona as musicas na lista
		public void addMusica(AudioClip outra, int index, string destino){
			if (destino == "Race") {
				this.m_Race[index] = outra;;
			} else {
				if (destino == "Menu") {
					this.m_Menu[index] = outra;;
				}
			}
		}

		public void addFx(AudioClip outro, int index){
			this.efeitos[index] = outro;
		}
        //disponibiliza musica
		public AudioClip getMusic(string destino, int indice) {
			AudioClip ret = null;
			if (destino == "Race") {
				ret = this.m_Race [indice];
			} else {
				if (destino == "Menu") {
					ret =  this.m_Menu [indice];
				}
			}
			return ret;
        }

        public AudioClip getFx(int indice) {
            return this.efeitos[indice];
        }

		public int getTotalMusic(string destino) {
			int ret = 0;
			if (destino == "Race") {
				ret = this.m_Race.Length;
			}else 
			if(destino == "Menu") {
				ret = this.m_Menu.Length;
			}
			return ret;
        }

        public int getTotalFx() {
            return this.efeitos.Length;
        }
    }//fim da classe Som

    //executa quando o script eh chamado (antes de ser iniciado)
	private void Awake(){
        if (instancia==null) { //garante uma unica instancia
            //lista todas as musicas(na ordem que aparece no projeto) no caminho especificado dentro da pasta "Resources"
            //se a pasta "resources nao existir, deve ser criada.
            resourceRaceMusic = Resources.LoadAll<AudioClip>("Audio/Race/Background");
            resourceMenuMusic = Resources.LoadAll<AudioClip>("Audio/Menu");
            //lista todos os efeitos sonoros no caminho especificado (na ordem que aparece no projeto)
            resourceFx = Resources.LoadAll<AudioClip>("Audio/Race/SFX");

            musicas = new Som(resourceMenuMusic.Length, resourceRaceMusic.Length, resourceFx.Length); //instancia um objeto musica do tipo Som
                                                                                                      //*
            int i = 0;
            //popula a lista de musicas
            foreach (AudioClip clip in resourceMenuMusic) {
                musicas.addMusica(clip, i, "Menu");
                i++;
            }
            i = 0;
            foreach (AudioClip clip in resourceRaceMusic) {
                musicas.addMusica(clip, i, "Race");
                i++;
            }
            //Debug.Log (string.Format ("Musicas: {0}", musicas.getTotalMusic("Race")));
            //popula a lista de efeitos especiais
            i = 0;
            foreach (AudioClip clip in resourceFx) {
                musicas.addFx(clip, i);
                i++;
            }
            DontDestroyOnLoad(this); //mantem ativo entre as cenas
            //*/
            instancia = this; //atribui na primeira vez que instanciar
        }

    } //fim de Awake()
	/*
	private void Start(){
        //seleciona a musica a ser tocada (eh escutada no listener que esta anexado na camera do jogo)
        atual.clip = musicas.getMusic("Race", Random.Range(0, musicas.getTotalMusic("Race")));
        atual.volume = 0.5f; //ajusta o volume da musica (pode ser ajustado no menu???)
        atual.volume = Settings.valMusVolume; //modifica o volume da musica
		atual.Play (); //toca a musica selecionada
	}
	//*/
} //fim de SoundManager
