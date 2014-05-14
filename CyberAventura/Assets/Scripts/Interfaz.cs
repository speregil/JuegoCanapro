using UnityEngine;
using System.Collections;

public class Interfaz : MonoBehaviour {

	public 	GUISkin		skinLogin;
	public 	GUISkin		skinMenu;
	public	GUISkin		skinExtras;
	public 	GUISkin		skinEstadisticas;
	public	GUISkin		skinBotonVolver;
	public	GUISkin		skinPreguntas;
	public 	Texture2D	imgEstadisticas;
	public 	Texture2D	imgJugar;
	public 	Texture2D	imgExtras;
	public 	Texture2D	imgTutorial;
	public 	Texture2D	imgSalir;
	public 	Texture2D	imgNuevo;
	public 	Texture2D	imgCargar;
	public 	Texture2D	imgVolver;
	public	Texture2D	imgVolverExtras;
	public	Texture2D	imgVolverExtrasOver;
	public	Texture2D	contenidoBoxImagen;

	private GUIContent contenidoBoxJugar;
	private GUIContent contenidoBoxExtras;
	private GUIContent contenidoBoxEstadisticas;
	private GUIContent contenidoBoxTutorial;
	private GUIContent contenidoBoxSalir;
	private GUIContent contenidoBoxNuevo;
	private GUIContent contenidoBoxCargar;
	private GUIContent contenidoBoxVolver;


	private const int 	LOGIN = 1;
	private const int 	TUTO = 2;
	private const int 	PRINCIPAL = 3;
	private const int 	MAPA = 4;
	private const int 	ESTADISTICAS = 5;
	private const int 	MODO = 6;
	private const int	EXTRAS = 7;

	private string		login;
	private string		pass;
	private string		info;
	private int 		estado;
	private AdminSQL 	SQL;
	private int numeroImagenes = 82;
	private Vector2 scrollPosition = Vector2.zero;
	private bool modal = false;

	// Use this for initialization
	void Start () {
		login = "";
		pass = "";
		info = "BIENVENIDO A LA CYBERAVENTURA CANAPRO 2014, UN JUEGO QUE TE PERMITIRA GANAR, SI CONOCES DE COOPERATIVISMO. ANIMO!!! INICIA";
		contenidoBoxJugar = new GUIContent();
		contenidoBoxJugar.image = (Texture2D)imgJugar;
		contenidoBoxExtras = new GUIContent();
		contenidoBoxExtras.image = (Texture2D)imgExtras;
		contenidoBoxEstadisticas = new GUIContent();
		contenidoBoxEstadisticas.image = (Texture2D)imgEstadisticas;
		contenidoBoxTutorial = new GUIContent();
		contenidoBoxTutorial.image = (Texture2D)imgTutorial;
		contenidoBoxSalir = new GUIContent();
		contenidoBoxSalir.image = (Texture2D)imgSalir;
		contenidoBoxNuevo = new GUIContent();
		contenidoBoxNuevo.image = (Texture2D)imgNuevo;
		contenidoBoxCargar = new GUIContent();
		contenidoBoxCargar.image = (Texture2D)imgCargar;
		contenidoBoxVolver = new GUIContent();
		contenidoBoxVolver.image = (Texture2D)imgVolver;

	
		GameObject adSQL = GameObject.Find("AdminSQL");
		SQL = (AdminSQL)adSQL.GetComponent(typeof(AdminSQL));

		if(SQL.DarUsuarioActual().Equals(AdminSQL.LOGOUT))
			estado = LOGIN;
		else
			estado = PRINCIPAL;
	}
	
	// Update is called once per frame
	void OnGUI () {

		if(LOGIN == estado)
		{
			GUI.skin = skinLogin;
			GUI.Box(new Rect(0,0,Screen.width,Screen.height), "");
			login = GUI.TextField(new Rect((Screen.width/4),(Screen.height*3/8),(Screen.width*1/3),(Screen.height*1/10)),login);
			pass = GUI.PasswordField(new Rect((Screen.width/4),(Screen.height/2),(Screen.width*1/3),(Screen.height*1/10)),pass, "*"[0]);
			if(GUI.Button(new Rect((Screen.width/2-40),(Screen.height/2+Screen.height*2/16),(Screen.width*1/10),(Screen.height*1/16)), "INGRESAR"))
			{
				info = "BIENVENIDO A LA CYBERAVENTURA CANAPRO 2014, UN JUEGO QUE TE PERMITIRA GANAR, SI CONOCES DE COOPERATIVISMO. ANIMO!!! INICIA";
				string result = SQL.LogIn(login,pass);
				if(result.Equals("exito")){

					SQL.verificarPrimerBulk();
					SQL.pedirAvance();
					Fader.Instance.FadeIn(0.25f).StartCoroutine(this, "esperarInterno");
				}
				else if(result.Equals("mora")){
					info = "AL PARECER NO ESTÁS AL DÍA CON TUS OBLIGACIONES,\n NO PUEDES PARTICIPAR";
				}
				else{
					info = "USUARIO O CONTRASEÑA INCORRECTOS.\nSI NO ERES MIEMBRO DE CANAPRO ¡PUEDES INSCRIBIRTE YA!";
				}
			}
			GUI.Label(new Rect((Screen.width*1/4),(Screen.height*1/12),(Screen.width*1/2),(Screen.height*1/3)), info);
			GUI.Label(new Rect((Screen.width/4),(Screen.height*3/8)-(Screen.height*1/32),(Screen.width*1/4),(Screen.height*1/16)), "    Número de cédula");
			GUI.Label(new Rect((Screen.width/4),(Screen.height/2)-(Screen.height*1/32),(Screen.width*1/4),(Screen.height*1/16)), "    Contraseña");


		}
		else if(PRINCIPAL == estado)
		{
			GUI.skin = skinMenu;
			GUI.Box(new Rect(0,0,Screen.width,Screen.height), "");
			GUI.Label(new Rect((Screen.width*1/4),(Screen.height*1/12),(Screen.width*1/2),(Screen.height*1/3)), info);
			if(GUI.Button(new Rect((Screen.width/12),(Screen.height*8/48),(Screen.width*1/4),(Screen.height*1/8)), "Jugar"))
			{
				estado = MODO;
				info = "INICIA UN NUEVO JUEGO O CARGA TU AVANCE PREVIO";

			}
			GUI.Label(new Rect((Screen.width*3/12),(Screen.height*12/60),Screen.width/12,Screen.height/12), contenidoBoxJugar);
			if(GUI.Button(new Rect((Screen.width/12),(Screen.height*14/48),(Screen.width*1/4),(Screen.height*1/8)), "Extras"))
			{
				//Fader.Instance.FadeIn().StartCoroutine(this, "esperarInterno");
				estado = EXTRAS;
			}
			GUI.Label(new Rect((Screen.width*3/12),(Screen.height*18/60),Screen.width/10,Screen.height/10), contenidoBoxExtras);
			if(GUI.Button(new Rect((Screen.width/12),(Screen.height*20/48),(Screen.width*1/4),(Screen.height*1/8)), "Estadisticas"))
			{
				SQL.pedirTop20();
				//Fader.Instance.FadeIn().StartCoroutine(this, "esperarInterno");
				estado= ESTADISTICAS;
			}
			GUI.Label(new Rect((Screen.width*3/12),(Screen.height*24/60),Screen.width/7,Screen.height/7), contenidoBoxEstadisticas);
			if(GUI.Button(new Rect((Screen.width/12),(Screen.height*26/48),(Screen.width*1/4),(Screen.height*1/8)), "Tutorial"))
			{
				//Fader.Instance.FadeIn().StartCoroutine(this, "esperarInterno");
				estado= TUTO;
			}
			GUI.Label(new Rect((Screen.width*3/12),(Screen.height*33/60),Screen.width/10,Screen.height/10), contenidoBoxTutorial);
			if(GUI.Button(new Rect((Screen.width/12),(Screen.height*32/48),(Screen.width*1/4),(Screen.height*1/8)), "Salir"))
			{
				SQL.LogOut();
				//Fader.Instance.FadeIn().StartCoroutine(this, "esperarInterno");
				estado= LOGIN;
			}
			GUI.Label(new Rect((Screen.width*3/12),(Screen.height*41/60),Screen.width/10,Screen.height/10), contenidoBoxSalir);
			GUI.Label(new Rect((Screen.width*9/24),(Screen.height*11/48),Screen.width*97/180,Screen.height*97/180), contenidoBoxImagen);
		}
		else if(ESTADISTICAS == estado)
		{
			GUI.skin = skinEstadisticas;
			GUI.Label(new Rect((0),(0),(Screen.width),(Screen.height)), "");
			string formato = SQL.top20();
			string respuesta = "";
			string[] cadena = formato.Split(new char [] {';'});
			//GUI.Box(new Rect((Screen.width/12),(Screen.height*1/12),(Screen.width*10/12),(Screen.height*10/12)), "Panel principal");
			scrollPosition = GUI.BeginScrollView(new Rect((Screen.width/128),(Screen.height*1/128),(Screen.width*82/96),(Screen.height)), scrollPosition, 
			                                     new Rect(0,0,Screen.width*10/12,numeroImagenes/5*Screen.height/6+Screen.height/6), false, true);
			GUI.skin = skinPreguntas;
			int indice = 1;
			for(int i = 1; i < cadena.Length;)
			{
				GUI.Button(new Rect(0, Screen.height/12*(indice-1), (Screen.width*82/96), (Screen.height/12)), ""+indice+". *Nombre: "+cadena[i+2]+" *Puntuacion: "+cadena[i+3]);
				if(SQL.DarUsuarioActual().Equals(cadena[i+2]))
				{
					respuesta = "Estas de posicion "+indice;
				}
				indice++;
				i += 4;
			}
			if(respuesta.Equals(""))
			{
				respuesta = "No has participado aun";
			}
			GUI.skin = skinEstadisticas;
			//for(int i = 0; i < numeroImagenes; i++)
			//{
			//	if(!((Screen.width/6)+(Screen.width/6*(ancho)) >= Screen.width*11/12))
			//	{
			//		GUI.Button(new Rect((Screen.width/6*(ancho)), Screen.height/6*alto, (Screen.width/6), (Screen.height*1/6)), "Imagen");
			//		ancho++;
			//	}
			//	else
			//	{
			//		alto++;
			//		ancho = 0;
			//	}
			//}
			//GUI.Label(
			GUI.EndScrollView();
			//GUI.Label(new Rect(Screen.width*7/12,Screen.height/2,500,100), respuesta);

			GUI.Box(new Rect((0),(0),(Screen.width),(Screen.height)), "");
			GUI.skin = skinBotonVolver;
			Rect rect = new Rect((Screen.width*113/128), (Screen.height*57/64)+(Screen.height*1/128), (Screen.width*7/64), (Screen.height*1/13));
			if(!modal)
				GUI.ModalWindow(0, new Rect(Screen.width/2-300, Screen.height/2-150, 600, 300), doMyWindow, "\n\n\n"+respuesta);
			if(GUI.Button( rect, "Volver"))
			{
				//Fader.Instance.FadeIn().StartCoroutine(this, "esperarInterno");
				estado = PRINCIPAL;
				modal = false;
			}
				//GUI.Label(new Rect(300,300,500,500), SQL.top20());
		}
		else if(MODO == estado)
		{
			GUI.skin = skinMenu;
			GUI.Box(new Rect(0,0,Screen.width,Screen.height),"");
			GUI.Label(new Rect((Screen.width/3)-50,0,(Screen.width*1/2),(Screen.height*1/3)), info);
			if(GUI.Button(new Rect((Screen.width/3),(Screen.height/4),(Screen.width*1/4),(Screen.height*1/8)), "Jugar"))
			{

				if(SQL.esPrimerRun()){
					SQL.NuevoRun();
				}
				else if(!SQL.esPrimerBulk())
				{
					SQL.CargarRun();
					//info = "YA HAS COMPLETADO EL JUEGO, REVISA LA SECCION DE ESTADISTICAS PARA VER TUS RESULTADOS";
				}
				else
				{
					SQL.CargarRun();
				}
				Fader.Instance.FadeIn(0.25f).StartCoroutine(this,"esperar");
				//Application.LoadLevel("Mapa");
				//esperar();  

			}
			GUI.Label(new Rect((Screen.width/2),(Screen.height*1/4+15),Screen.width/12,Screen.height/12), contenidoBoxNuevo);
			//if(GUI.Button(new Rect((Screen.width/3),(Screen.height*4/9),(Screen.width*1/4),(Screen.height*1/6)), "Cargar Partida"))
			//{
			//	SQL.CargarRun("Usuario");
			//	Application.LoadLevel("Mapa");
			//}
			//GUI.Label(new Rect((Screen.width/2),(Screen.height*4/9),Screen.width/12,Screen.height/12), contenidoBoxCargar);
			if(GUI.Button(new Rect((Screen.width/3),(Screen.height*1/4+180),(Screen.width*1/4),(Screen.height*1/8)), "Volver"))
			{
				estado= PRINCIPAL;
			}
			GUI.Label(new Rect((Screen.width/2),(Screen.height*1/4+190),Screen.width/12,Screen.height/12), contenidoBoxVolver);
		}
		else if(EXTRAS == estado)
		{
			GUI.skin = skinExtras;
			GUI.Label(new Rect((0),(0),(Screen.width),(Screen.height)), "");
			scrollPosition = GUI.BeginScrollView(new Rect((Screen.width/128),(Screen.height*1/128),(Screen.width*82/96),(Screen.height)), scrollPosition, 
			                                     new Rect(0,0,Screen.width*10/12,numeroImagenes/5*Screen.height/6+Screen.height/6), false, true);
			int alto = 0;
			int ancho = 0;
			for(int i = 0; i < numeroImagenes;)
			{
				if(!((Screen.width/6)+(Screen.width/6*(ancho)) >= Screen.width*21/24))
				{
					GUI.Button(new Rect((Screen.width/6*(ancho)), Screen.height/6*alto, (Screen.width/6), (Screen.height*1/6)), "");
					ancho++;
					i++;
				}
				else
				{
					alto++;
					ancho = 0;
				}
			}
			GUI.EndScrollView();
			GUI.Box(new Rect((0),(0),(Screen.width),(Screen.height)), "");
			GUI.skin = skinBotonVolver;
			Rect rect = new Rect((Screen.width*113/128), (Screen.height*57/64)+(Screen.height*1/128), (Screen.width*7/64), (Screen.height*1/13));
			if(GUI.Button( rect, "Volver"))
				estado = PRINCIPAL;

		}
	}
	void doMyWindow(int windowID)
	{
		if (GUI.Button(new Rect(200, 100, 200, 100), "Aceptar"))
		{
			modal = true;
		}
	}
	IEnumerator esperar()
	{
		yield return new WaitForSeconds (0.25f);
		{        Application.LoadLevel("Mapa");  
		}
	}
	IEnumerator esperarInterno()
	{
		yield return new WaitForSeconds (0.25f);
		{        
			estado = PRINCIPAL;
			Fader.Instance.FadeOut(0.25f);
		}
	}

}