using UnityEngine;
using System.Collections;

public class AdminNivel : MonoBehaviour {
	public	GUISkin			skinConfirmacion;
	public 	GameObject 		personaje;
	private	AdminSQL		SQL;
	private PanelPregunta	panel; 
	private Movimiento 		mov;
	private string 			bulkActual;
	private string			usuarioActual;
	private Run				RunActual;
	private Rect			RectConfirmacion;
	private bool			salvar;
	private bool			gano;
	private int				totalEdificios;
	private int				totalPuntos;
	private float			totalTiempo;
	private string			MensajeSalida = "\t\t\tSalvaremos su progreso, puede cargarlo despues desde\n\t\t\t\tel menu inicial\n\t\t\t\t\t¿Desea volver al menu inicial?";


	
	void Start (){
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
	}

	void Update (){
	
	}

	void OnGUI(){
		GUI.skin = skinConfirmacion;
		GUI.Label(new Rect(Screen.width/10 - 20,Screen.height/20,Screen.width/5,Screen.height/12),"Bienvenido(a) "+ usuarioActual);
		GUI.Label(new Rect(Screen.width*3/10,Screen.height/20,Screen.width*3/10,Screen.height/12),"Haz completado "+ totalEdificios + " edificios, te faltan " + (12 - totalEdificios));
		GUI.Label(new Rect(Screen.width*6/10,Screen.height/20,Screen.width*2/10,Screen.height/12),"Puntos: "+ totalPuntos + "     Tiempo: " + totalTiempo);

		if(salvar)
			RectConfirmacion = GUI.Window(1,RectConfirmacion,WindowFunction,"");

		if(gano){
			RectConfirmacion = GUI.Window(2,RectConfirmacion,WindowFunction,"");
		}

		if(GUI.Button(new Rect(Screen.width*8/10,Screen.height/20,Screen.width/5,Screen.height/12), "Menu Principal")){
			salvar = true;
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

	public void CompletarBulk(int ID, int puntos, float tiempo){
		RunActual.completarBulk(ID);
		RunActual.SumarPuntos(puntos);
		RunActual.SumarTiempo(tiempo);
		totalPuntos = RunActual.DarPuntuacionTotal();
		totalTiempo = RunActual.DarTiempoTotal();
		totalEdificios = RunActual.DarBulksCompletos();
		SQL.GuardarBulk(ID, int.Parse(SQL.DarLlaveActual()),puntos,tiempo);

		if(totalEdificios == 12){
			gano = true;
			SQL.primerRun = true;
		}
	}

	public bool EstaCompleto(int IDBulk){
		return RunActual.DarFlag(IDBulk);
	}
}