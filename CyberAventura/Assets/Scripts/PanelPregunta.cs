using UnityEngine;
using System.Collections;

/*
 * Dibuja y controla la ventana de las preguntas
 * */
public class PanelPregunta : MonoBehaviour {

	//--------------------------------------------------------------------------------------------------------
	// Atributos
	//--------------------------------------------------------------------------------------------------------

	private	Bulk		BulkActivo;				//Bulk actual de preguntas
	private	IOPreguntas listaPreguntas;			//Conexion con el script que carga las preguntas del sistema
	private AdminNivel	Control;				//Relacion con el script padre para indicar acciones de control en la BD
	private Rect 		RectPregunta;			//Rectangulo que delimita la ventana de las preguntas
	private Rect		RectConfirmacion;		//Rectangulo que delimita la ventana de confirmacion
	private bool		ventanaActiva;			//Determina si la ventana de preguntas debe dibujarse o no
	private bool		confirmacion;			//Determina si la ventana de confirmacion debe dibujarse o no
	private bool 		respuestaCorrecta;		//Determina si se elgio una respuesta corecta
	private bool		respuestaIncorrecta;	//Determina si se eligio una respuesta errada
	private bool		preguntaActiva;			//Determina si se esta contestando un pregunta
	private bool		termino;				//Determina si se termino todo el bulk de preguntas
	private bool		repetido;				//Deterina si el jugador quiere entrar a un bulk que ya completo
	private string		bulkActual;				//Determina el identificador del bulk de preguntas actual
	private string		textoActual;			//Detemina el enunciado de la pregunta actual
	private string 		AActual;				//Determina la opcion A de la pregunta actual
	private string		BActual;				//Determina la opcion B de la pregunta actual
	private string		CActual;				//Determina la opcion C de la pregunta actual
	private string		DActual;				//Determina la opcion D de la pregunta actual
	private float		guiTime;				//Cronometro de cada pregunta
	private float		tiempoTotal;			//Tiempo total en completar el bulk
	private	float		tiempoRestante;			//Determina el contador de tiempo para la pregunta actual
	private float		tiempoInicio;			//Tiempo de sistema en el que se instancio este script
	private float		cambioTiempo;			//Cambio de tiempo de un frame a otro
	private float		seccion;				//Controla la barra de progreso del contador en interfaz
	private char[]		split;					//Secuencia de chars para identificar el ID del bulk activo
	private string		mensajeConfirmacion;	//Mensaje diferencial dependiendo si hace un bulk por primera vez o no

	//---------------------------------------------------------------------------------------------------------
	// Constructor
	//---------------------------------------------------------------------------------------------------------

	void Start () {
		textoActual = "";
		AActual = "";
		BActual = "";
		CActual = "";
		DActual = "";
		ventanaActiva = false;
		confirmacion = false;
		preguntaActiva = false;
		respuestaCorrecta = false;
		respuestaIncorrecta = false;
		termino = false;
		repetido = false;
		confirmacion = false;
		RectConfirmacion = new Rect((Screen.width/3) - 100,Screen.height/3 ,Screen.width/2,Screen.height/3);
		RectPregunta = new Rect(50,50,Screen.width - 100,Screen.height - 100);
	}

	void OnGUI(){
		if(confirmacion){
			RectConfirmacion = GUI.Window(1,RectConfirmacion,WindowFunction,"");
		}
		else if(ventanaActiva){
			RectPregunta = GUI.Window(0,RectPregunta,WindowFunction,"");
		}
	}

	void WindowFunction(int WindowID){
		// Ventana de las preguntas
		if(WindowID == 0){
			if(preguntaActiva){

				//Timer
				guiTime = Time.time - tiempoInicio;
				cambioTiempo = Mathf.CeilToInt(tiempoRestante - guiTime);
				seccion = ((tiempoRestante - cambioTiempo)/tiempoRestante);
				if(seccion < 1){
					int ancho = Mathf.CeilToInt(seccion*((RectPregunta.width/2) - 100));
					GUI.Box(new Rect(50, RectPregunta.height - 50,ancho,RectPregunta.height/4),"");
				}
				else{
					tiempoTotal += tiempoRestante;
					Debug.Log(tiempoTotal);
					preguntaActiva = false;
					respuestaIncorrecta = true;
				}

				// Dibujar la pregunta
				GUI.Label(new Rect(50,50,(RectPregunta.width/2) - 50,RectPregunta.height - 50),textoActual);
				if(GUI.Button(new Rect(RectPregunta.width/2,50,RectPregunta.width/2 - 50,RectPregunta.height/8),AActual))
					ValidarRespuesta(Pregunta.OPCIONA);
				if(GUI.Button(new Rect(RectPregunta.width/2,50 + (RectPregunta.height/4),RectPregunta.width/2 - 50,RectPregunta.height/8),BActual))
					ValidarRespuesta(Pregunta.OPCIONB);
				if(GUI.Button(new Rect(RectPregunta.width/2,50 + (RectPregunta.height/2),RectPregunta.width/2 - 50,RectPregunta.height/8),CActual))
					ValidarRespuesta(Pregunta.OPCIONC);
				if(GUI.Button(new Rect(RectPregunta.width/2,50 + 3*(RectPregunta.height/4),RectPregunta.width/2 - 50,RectPregunta.height/8),DActual))
					ValidarRespuesta(Pregunta.OPCIOND);


			}
			else if(respuestaCorrecta){
				if(GUI.Button(new Rect(RectPregunta.width/3,RectPregunta.height/3,RectPregunta.width/3,RectPregunta.height/3), "¡CORRECTO!")){
					if(listaPreguntas.AvanzarPregunta()){
						respuestaCorrecta = false;
						MostrarPregunta();
					}
					else{
						respuestaCorrecta = false;
						termino = true;
					}
				}
			}
			else if(respuestaIncorrecta){
				string respuesta = "La respuesta correcta es: " + listaPreguntas.DarRespuesta();
				GUI.Label(new Rect(RectPregunta.width/3,RectPregunta.height/2 + 50,RectPregunta.width/3,RectPregunta.height/5),respuesta);
				if(GUI.Button(new Rect(RectPregunta.width/3,RectPregunta.height/3 - 50,RectPregunta.width/3,RectPregunta.height/3), "ESO NO ES CORRECTO")){
					if(listaPreguntas.AvanzarPregunta()){
						respuestaIncorrecta = false;
						MostrarPregunta();
					}
					else{
						respuestaIncorrecta = false;
						termino = true;
					}
				}
			}
			else if(termino){
				if(GUI.Button(new Rect(RectPregunta.width/3,RectPregunta.height/3,RectPregunta.width/3,RectPregunta.height/3), "¡TERMINASTE!")){
					BulkActivo.AsignarTiempo(tiempoTotal);
					int id = (int.Parse(BulkActivo.DarID().Substring(4))) - 1;
					Control.CompletarBulk(id,BulkActivo.DarPuntuacion(),BulkActivo.DarTiempo());
					termino = false;
					ventanaActiva = false;

					//Bajar el bulk a la base de datos
				}
			}
		}

		// Ventana de confirmacion
		if(WindowID == 1){
			GUI.Label(new Rect(RectConfirmacion.width/3,50,(RectConfirmacion.width/2) - 10,RectConfirmacion.height - 50),mensajeConfirmacion);
			if(repetido){
				if(GUI.Button(new Rect(RectConfirmacion.width/3 - 50,RectConfirmacion.height/3 + 50,RectConfirmacion.width/2,RectConfirmacion.height/3), "CONTINUAR")){
					confirmacion = false;
					repetido = false;
				}
			}
			else{
				if(GUI.Button(new Rect(RectConfirmacion.width/4,RectConfirmacion.height/3 + 50,RectConfirmacion.width/4,RectConfirmacion.height/3), "SI")){
					confirmacion = false;
					CargarLista(bulkActual);
				}
					
				if(GUI.Button(new Rect(2*(RectConfirmacion.width/4)+5,RectConfirmacion.height/3 + 50,RectConfirmacion.width/4,RectConfirmacion.height/3), "NO")){
					confirmacion = false;
				}
			}
		}
	}

	//-------------------------------------------------------------------------------------------------
	// Metodos
	//-------------------------------------------------------------------------------------------------

	/*
	 * Genera la conexion con el script IO e indica la construccion de la ventana
	 * */
	void CargarLista(string NombreLista){
		listaPreguntas = new IOPreguntas(NombreLista);
		BulkActivo = new Bulk(NombreLista);
		ventanaActiva = true;
		MostrarPregunta();
	}

	/*
	 * Carga la informacion de la pregunta e inicia la visualizacion
	 * */
	void MostrarPregunta(){
		textoActual = listaPreguntas.DarTexto();
		AActual = listaPreguntas.DarOpcionA();
		BActual = listaPreguntas.DarOpcionB();
		CActual = listaPreguntas.DarOpcionC();
		DActual = listaPreguntas.DarOpcionD();
		tiempoRestante = listaPreguntas.DarTiempo();
		seccion = 0.0f;
		cambioTiempo = 0.0f;
		tiempoInicio = Time.time;
		preguntaActiva = true;
	}

	/*
	 * Valida si la respuesta dada por el usuario es correcta y actua consecuentemente
	 * */
	void ValidarRespuesta(string Opcion){
		if(listaPreguntas.ValidarRespuesta(Opcion)){
			BulkActivo.SumarPunto();
			tiempoTotal += guiTime;
			preguntaActiva = false;
			respuestaCorrecta = true;
		}
		else{
			preguntaActiva = false;
			respuestaIncorrecta = true;
			tiempoTotal += guiTime;
		}
	}

	/*
	 * Pide al usuario confirmacion para iniciar al bulk actual
	 * */
	public void PedirConfirmacion(string IDBulk, AdminNivel padre){
		bulkActual = IDBulk;
		Control = padre;

		if(Control.EstaCompleto((int.Parse(IDBulk.Substring(4)))- 1)){
			mensajeConfirmacion = "Ya has completado este edificio previamente\nSi deseas mejorar tu tiempo debes hacerlo en un nuevo juego.";
			repetido = true;
		}
		else{
			mensajeConfirmacion = "¿Desea entrar a este edificio?";
		}

		confirmacion = true;
	}
}