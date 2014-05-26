using UnityEngine;
using System.Collections;

/*
 * Dibuja y controla la ventana de las preguntas
 * */
public class PanelPregunta : MonoBehaviour {

	//--------------------------------------------------------------------------------------------------------
	// Atributos
	//--------------------------------------------------------------------------------------------------------

	public	Camera		camaraPrincipal;
	public	Camera		camaraAnimacion;
	public 	GUISkin		skinPreguntasA;			//Skin de las preguntas tipo A
	public 	GUISkin		skinPreguntasB;			//Skin de las preguntas tipo B
	public 	GUISkin		skinPreguntasC;			//Skin de las preguntas tipo C
	public  GUISkin		skinPreguntasD;			//Skin de las preguntas tipo D
	public	GUISkin		skinVacio;
	public	GUISkin		skinTextoPreguntas;
	public	GUISkin		skinConfirmacion;		//Skin del cuadro de confirmacion
	private	GUISkin		skinTemp;				//Temporal de control para los cambios de skin
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
	public	GameObject	personaje;				//Personaje de la escena necesario para la animacion
	private	int 		IDPreguntaActual;		//IDPreguntaActual
	private string 		preguntas;				//Preguntas respondidas
	private bool		paso;					//Variable provisional para guardar bulk

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
		paso = false;
		RectConfirmacion = new Rect(Screen.width/100,Screen.height/100 ,1024,551);
		RectPregunta = new Rect(0,0,Screen.width,Screen.height);
		skinTemp = skinTextoPreguntas;
		Fader.Instance.FadeOut(1);
	}

	void OnGUI(){
		if(confirmacion){
			GUI.skin = skinConfirmacion;
			RectConfirmacion = GUI.Window(1,RectConfirmacion,WindowFunction,"");
		}
		else if(ventanaActiva){
			GUI.skin = skinPreguntasA;
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
					// If que se volvio inutil al quitar la barra de tiempo pero que no quiero quitar
					// para no tener que reestructurar todo el condicional
				}
				else{
					tiempoTotal += tiempoRestante;
					Debug.Log(tiempoTotal);
					preguntaActiva = false;
					respuestaIncorrecta = true;
					ValidarRespuesta("");
				}

				// Dibujar la pregunta
				GUI.skin = skinTextoPreguntas;
				GUI.Label(new Rect(50,50,RectPregunta.width - 150,RectPregunta.height/2),textoActual);
				GUI.skin = skinPreguntasA;
				GUI.Box(new Rect(5,RectPregunta.height/2 + Screen.height*39/1000,RectPregunta.width*5/6+Screen.width*256/10000,RectPregunta.height/20-Screen.height*195/10000),"");
				skinTemp = GUI.skin;
				GUI.skin = skinPreguntasB;
				GUI.Label(new Rect(RectPregunta.width/3 + Screen.width*549/10000,RectPregunta.height/2 - Screen.height*195/10000,RectPregunta.width/11,RectPregunta.height/11),cambioTiempo + "");
				GUI.skin = skinPreguntasA;

				if(GUI.Button(new Rect(5,Screen.height*84/1000 + RectPregunta.height/2,RectPregunta.width/2 - Screen.width*768/10000, Screen.height*7/60),""))
					ValidarRespuesta(Pregunta.OPCIONA);
				skinTemp = GUI.skin;
				GUI.skin = skinPreguntasC;
				GUI.Label(new Rect(Screen.width*366/10000,Screen.height*781/10000 + RectPregunta.height/2,RectPregunta.width/3,RectPregunta.height/8),AActual);
				GUI.skin = skinPreguntasB;

				if(GUI.Button(new Rect(RectPregunta.width/2 - Screen.width*62/1000,Screen.height*84/1000 + (RectPregunta.height/2),RectPregunta.width/2 - Screen.width*79/1000,RectPregunta.height*7/60),""))
					ValidarRespuesta(Pregunta.OPCIONB);
				GUI.skin = skinPreguntasC;
				GUI.Label(new Rect(RectPregunta.width/2 - Screen.width*47/1000,60 + (RectPregunta.height/2),RectPregunta.width/3,RectPregunta.height/8),BActual);
				GUI.skin = skinPreguntasC;

				if(GUI.Button(new Rect(Screen.width*58/1000,Screen.height*91/1000 + (RectPregunta.height/2) + (RectPregunta.height/8),RectPregunta.width/2 - Screen.width*131/1000,Screen.height*7/60),""))
					ValidarRespuesta(Pregunta.OPCIONC);
				GUI.Label(new Rect(Screen.width*84/1000,Screen.height*84/1000 + (RectPregunta.height/2) + (RectPregunta.height/8),RectPregunta.width/3 - Screen.width*146/10000,RectPregunta.height/8),CActual);
				GUI.skin = skinPreguntasD;

				if(GUI.Button(new Rect(RectPregunta.width/2 - Screen.width*62/1000,Screen.height*91/1000 + (RectPregunta.height/2) + (RectPregunta.height/8),RectPregunta.width/2 - Screen.width*131/1000,Screen.height*7/60),""))
					ValidarRespuesta(Pregunta.OPCIOND);
				GUI.skin = skinPreguntasC;
				GUI.Label(new Rect(RectPregunta.width/2 - Screen.width*439/10000,Screen.width*47/1000 + (RectPregunta.height/2) + (RectPregunta.height/8),RectPregunta.width/3 - Screen.width*146/10000,RectPregunta.height/8),DActual);
				GUI.skin = skinTemp;
			}
			else if(respuestaCorrecta){
				skinTemp = GUI.skin;
				GUI.skin = skinVacio;
				camaraAnimacion.enabled = true;
				camaraPrincipal.enabled = false;
				ventanaActiva = false;
				Control.activarCorrecto(true);
				StartCoroutine("esperar2");
				/*if(GUI.Button(new Rect(RectPregunta.width/3,RectPregunta.height/3,RectPregunta.width/3,RectPregunta.height/3), "¡CORRECTO!")){
					if(listaPreguntas.AvanzarPregunta()){
						respuestaCorrecta = false;
						MostrarPregunta();
					}
					else{
						respuestaCorrecta = false;
						termino = true;
					}
				}**/
			}
			else if(respuestaIncorrecta){
				skinTemp = GUI.skin;
				GUI.skin = skinVacio;
				ventanaActiva = false;
				camaraAnimacion.enabled = true;
				camaraPrincipal.enabled = false;
				Control.activarIncorrecto(true);
				StartCoroutine("esperar2");
				/*string respuesta = "La respuesta correcta es: " + listaPreguntas.DarRespuesta();
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
				}**/
			}
			else if(termino){

				skinTemp = GUI.skin;
				GUI.skin = skinVacio;
				ventanaActiva = false;
				camaraAnimacion.enabled = true;
				camaraPrincipal.enabled = false;
				Control.activarTerminaste(true);
				StartCoroutine("esperar3");

				if(!paso)
				{
					BulkActivo.AsignarTiempo(tiempoTotal);
					int id = (int.Parse(BulkActivo.DarID().Substring(4))) - 1;
					Control.CompletarBulk(id,BulkActivo.DarPuntuacion(),BulkActivo.DarTiempo(), BulkActivo);
					paso = true;
				}

			}
		}

		// Ventana de confirmacion
		if(WindowID == 1){
			GUI.Label(new Rect(RectConfirmacion.width/3 + 20,RectConfirmacion.height*1/5,(RectConfirmacion.width/2) - 10,RectConfirmacion.height/20), "Bienvenido A: " + Control.darNombre());
			GUI.Box(new Rect(RectConfirmacion.width*1/6 - 20 ,RectConfirmacion.height*1/3 - 20,RectConfirmacion.width*3/8,RectConfirmacion.height*2/5), Control.darImagen());
			GUI.Label(new Rect(RectConfirmacion.width*5/9 ,RectConfirmacion.height*1/3 - 20,RectConfirmacion.width*1/3,RectConfirmacion.height*2/5), "" + Control.darDescripcion());
			GUI.Label(new Rect(RectConfirmacion.width*1/6 - 20,RectConfirmacion.height*6/8,(RectConfirmacion.width/2) - 10,RectConfirmacion.height - 50),mensajeConfirmacion);
			if(repetido){
				if(GUI.Button(new Rect(RectConfirmacion.width*3/5,RectConfirmacion.height*6/8 - 20,RectConfirmacion.width/6,RectConfirmacion.height/9), "CONTINUAR")){
					confirmacion = false;
					repetido = false;
					personaje.GetComponent<Movimiento>().cambiarLlego(false);
				}
			}
			else{
				if(GUI.Button(new Rect(RectConfirmacion.width*3/5,RectConfirmacion.height*6/8 - 20,RectConfirmacion.width/10,RectConfirmacion.height/9), "SI")){
					Fader.Instance.FadeIn().StartCoroutine(this, "esperar");
					CargarLista(bulkActual);
					personaje.GetComponent<Movimiento>().cambiarLlego(false);
					Control.mostrarGUI(false);
				}
					
				if(GUI.Button(new Rect(RectConfirmacion.width*4/5,RectConfirmacion.height*6/8 - 20,RectConfirmacion.width/10,RectConfirmacion.height/9), "NO")){
					confirmacion = false;
					personaje.GetComponent<Movimiento>().cambiarLlego(false);
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
		preguntas = Control.preguntas();
		while(estaHecha(listaPreguntas.DarID())){
			bool avance = listaPreguntas.AvanzarPregunta();
			if(!avance){
				termino = true;
				break;
			}
		}
		MostrarPregunta();
	}

	/*
	 * Carga la informacion de la pregunta e inicia la visualizacion
	 * */
	void MostrarPregunta(){
		if(!termino){
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
	}

	/*
	 * Valida si la respuesta dada por el usuario es correcta y actua consecuentemente
	 * */
	void ValidarRespuesta(string Opcion){
		if(listaPreguntas.ValidarRespuesta(Opcion)){
			BulkActivo.SumarPunto();
			IDPreguntaActual = listaPreguntas.DarID();
			Control.guardarPregunta(IDPreguntaActual+";"+Opcion+";"+guiTime);
			preguntas += IDPreguntaActual+";";
			tiempoTotal += guiTime;
			preguntaActiva = false;
			respuestaCorrecta = true;
		}
		else{
			IDPreguntaActual = listaPreguntas.DarID();
			Control.guardarPregunta(IDPreguntaActual+";"+Opcion+";"+guiTime);
			preguntas += IDPreguntaActual+";";
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

		if(!(Control.usuarioActual.Equals("Administrador"))){
			if(Control.EstaCompleto((int.Parse(IDBulk.Substring(4)))- 1)){
				mensajeConfirmacion = "Ya has completado este edificio previamente";
				repetido = true;
			}
			else{
				mensajeConfirmacion = "¿Desea entrar a este edificio?";
			}
		}

		confirmacion = true;
	}

	private IEnumerator esperar()
	{
		yield return new WaitForSeconds (1);
		{
			confirmacion = false;
		}
		Fader.Instance.FadeOut(1);
	}

	private IEnumerator esperar2()
	{
		yield return new WaitForSeconds(3);
		{
			if(respuestaIncorrecta){
				Control.activarIncorrecto(false);
				if(listaPreguntas.AvanzarPregunta()){
					respuestaIncorrecta = false;
					ventanaActiva = true;
					camaraAnimacion.enabled = false;
					camaraPrincipal.enabled = true;
					MostrarPregunta();
				}
				else{
					respuestaIncorrecta = false;
					camaraAnimacion.enabled = false;
					camaraPrincipal.enabled = true;
					ventanaActiva = true;
					termino = true;
				}
			}
			else if(respuestaCorrecta){
				Control.activarCorrecto(false);
				if(listaPreguntas.AvanzarPregunta()){
					respuestaCorrecta = false;
					ventanaActiva = true;
					camaraAnimacion.enabled = false;
					camaraPrincipal.enabled = true;
					MostrarPregunta();
				}
				else{
					respuestaCorrecta = false;
					camaraAnimacion.enabled = false;
					camaraPrincipal.enabled = true;
					ventanaActiva = true;
					termino = true;
				}
			}
		}
	}

	private IEnumerator esperar3()
	{
		yield return new WaitForSeconds(2);
		{
			Control.activarTerminaste(false);
			termino = false;
			Control.mostrarGUI(true);
			ventanaActiva = false;
			camaraAnimacion.enabled = false;
			camaraPrincipal.enabled = true;
		}
	}

	private bool estaHecha(int IDPregunta)
	{
		string [] cadena = this.preguntas.Split(new char [] {';'});
		for(int i = 0; i < cadena.Length;i++)
		{
			if(cadena[i].Equals(""+IDPregunta))
			{
				Debug.Log("--------------Es verdadero----------------");
				return true;
			}
		}
		Debug.Log("--------------Es falso----------------");
		return false;
	}
}