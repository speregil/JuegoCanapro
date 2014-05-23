using UnityEngine;
using System.Collections;

public class AdminNivel : MonoBehaviour {
	public	GUISkin			skinConfirmacion;
	public	GUISkin			skinBtnVolver;
	public 	GameObject 		personaje;
	public	Camera			camaraPrincipal;
	public	Camera			camaraAnimacion;
	public	GameObject		AnimCorrecto;
	public	GameObject		AnimIncorrecto;
	private	AdminSQL		SQL;
	private PanelPregunta	panel; 
	private Movimiento 		mov;
	private string 			bulkActual;
	private string			usuarioActual;
	private Run				RunActual;
	private Rect			RectConfirmacion;
	private bool			salvar;
	private bool			gano;
	private bool			onMapa;
	private int				totalEdificios;
	private int				totalPuntos;
	private float			totalTiempo;
	private string			MensajeSalida = "\t\t\tSalvaremos su progreso, puede cargarlo despues desde\n\t\t\t\tel menu inicial\n\t\t\t\t\t¿Desea volver al menu inicial?";
	public	Texture			imgBarraInfo;
	private	Texture2D		imagenEdActual;
	private	string			nombreEdActual;
	private string			descripcionEdActual;

	
	void Start (){
		//camaraPrincipal.enabled = false;
		camaraAnimacion.enabled = false;
		activarCorrecto(false);
		activarIncorrecto(false);
		panel = (PanelPregunta)GetComponent(typeof(PanelPregunta));
		mov = (Movimiento)personaje.GetComponent(typeof(Movimiento));
		GameObject adSQL = GameObject.Find("AdminSQL");
		SQL = (AdminSQL)adSQL.GetComponent(typeof(AdminSQL));
		usuarioActual = SQL.DarUsuarioActual();
		RunActual = SQL.DarRunActual();
		totalEdificios = RunActual.DarBulksCompletos();
		totalPuntos = RunActual.DarPuntuacionTotal();
		totalTiempo = RunActual.DarTiempoTotal();
		RectConfirmacion = new Rect((Screen.width/3) - 100,Screen.height/3 ,Screen.width/2,Screen.height/3);
		salvar = false;
		gano = false;
		onMapa = true;
	}

	void Update (){
	
	}

	void OnGUI(){
		if(onMapa){
			GUI.skin = skinConfirmacion;
			GUI.Label(new Rect(Screen.width*3/10,Screen.height/20,Screen.width*7/10,Screen.height/12), imgBarraInfo);
			GUI.Label(new Rect(Screen.width*15/40 - 60,Screen.height/20+Screen.height*78/10000,Screen.width/3,Screen.height/12),"Hola "+ usuarioActual);
			GUI.Label(new Rect(Screen.width*27/40,Screen.height/20+Screen.height*78/10000,Screen.width*3/10,Screen.height/12),"Edificios: "+ totalEdificios + "/12");
			GUI.Label(new Rect(Screen.width*8/10,Screen.height/20+Screen.height*78/10000,Screen.width*2/10,Screen.height/12),"Puntos: "+ totalPuntos + "     Tiempo: " + totalTiempo);

			if(salvar)
				RectConfirmacion = GUI.Window(1,RectConfirmacion,WindowFunction,"");

			if(gano){
				RectConfirmacion = GUI.Window(2,RectConfirmacion,WindowFunction,"");
			}
			//GUI.Box(new Rect((0),(0),(Screen.width),(Screen.height)), "");
			GUI.skin = skinBtnVolver;
			Rect rect = new Rect((Screen.width*113/128), (Screen.height*57/64)+(Screen.height*1/128), (Screen.width*7/64), (Screen.height*6/78));
			if(GUI.Button( rect, "Regresar"))
			{
				salvar = true;
			}
		}
	}

	void WindowFunction(int WindowID){

		if(WindowID == 1){
			GUI.Label(new Rect(10,50,RectConfirmacion.width - 10,RectConfirmacion.height - 50), MensajeSalida);
			if(GUI.Button(new Rect(RectConfirmacion.width/4,RectConfirmacion.height/3 + 50,RectConfirmacion.width/4,RectConfirmacion.height/3), "SI")){
				//Salvar
				Application.LoadLevel("Intro");
			}
		
			if(GUI.Button(new Rect(2*(RectConfirmacion.width/4)+5,RectConfirmacion.height/3 + 50,RectConfirmacion.width/4,RectConfirmacion.height/3), "NO")){
				salvar = false;
			}
		}

		if(WindowID == 2){
			GUI.Label(new Rect(10,50,RectConfirmacion.width - 10,RectConfirmacion.height - 50), MensajeSalida);
			if(GUI.Button(new Rect(RectConfirmacion.width/2 - 50,RectConfirmacion.height/3 + 50,RectConfirmacion.width/3,RectConfirmacion.height/3), "CONTINUAR")){
				//Bajar Run

				//Cargar nivel Estadisticas
				Application.LoadLevel("Intro");
			}
		}
	}

	public string darBulkActual(){
		return bulkActual;
	}

	public void nuevoDestino(Vector3 destino, string IDBulk){
		mov.IniciarMovimiento(destino);
		bulkActual = IDBulk;
	}

	public void IniciarPreguntas(){
		panel.PedirConfirmacion(bulkActual, this);
	}

	public void mostrarGUI(bool param){
		onMapa = param;
	}

	public void activarCorrecto(bool param){
		SpriteRenderer a = (SpriteRenderer)AnimCorrecto.GetComponent(typeof(SpriteRenderer));
		a.enabled = param;
		Animator an = (Animator)AnimCorrecto.GetComponent(typeof(Animator));
		an.SetBool("activo",param);
	}

	public void activarIncorrecto(bool param){
		SpriteRenderer a = (SpriteRenderer)AnimIncorrecto.GetComponent(typeof(SpriteRenderer));
		a.enabled = param;
		Animator an = (Animator)AnimIncorrecto.GetComponent(typeof(Animator));
		an.SetBool("activo",param);
	}

	public void CompletarBulk(int ID, int puntos, float tiempo, Bulk bulkActivo){
		RunActual.completarBulk(ID);
		RunActual.SumarPuntos(puntos);
		RunActual.SumarTiempo(tiempo);
		totalPuntos = RunActual.DarPuntuacionTotal();
		totalTiempo = Mathf.CeilToInt(RunActual.DarTiempoTotal());
		totalEdificios = RunActual.DarBulksCompletos();
		SQL.GuardarBulk(ID, int.Parse(SQL.DarLlaveActual()),puntos,tiempo, bulkActivo);

		if(totalEdificios == 12){
			gano = true;
			SQL.primerRun = true;
		}
	}

	public bool EstaCompleto(int IDBulk){
		return RunActual.DarFlag(IDBulk);
	}

	public void setDatosEdificio(Texture2D imagen, string nombre, string descripcion){
		imagenEdActual = imagen;
		nombreEdActual = nombre;
		descripcionEdActual = descripcion;
	}

	public Texture2D darImagen(){
		return imagenEdActual;
	}

	public string darNombre(){
		return nombreEdActual;
	}

	public string darDescripcion(){
		return descripcionEdActual;
	}
	public void guardarPregunta(string formato){
		SQL.guardarPregunta(formato);
	}

	public string preguntas()
	{
		return SQL.preguntas();
	}
}