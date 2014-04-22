using UnityEngine;
using System.Collections;

public class Interfaz : MonoBehaviour {

	private GUIStyle estiloBotonModoJuego;
	private GUIStyle estiloBotonMapa;
	private GUIStyle estiloBotonEstadisticas;
	private GUIStyle estiloBotonLogin;
	private GUIStyle estiloTextLogin;
	private GUIStyle estiloTextPassword;

	private GUIContent contenidoBoxLogin;
	private GUIContent contenidoBotonLogin;

	public 	GUISkin		skinLogin;
	private const int 	LOGIN = 1;
	private const int 	TUTO = 2;
	private const int 	PRINCIPAL = 3;
	private const int 	MAPA = 4;
	private const int 	ESTADISTICAS = 5;
	private const int 	MODO = 6;

	private string		login;
	private string		pass;
	private string		info;
	private int 		estado;
	private AdminSQL 	SQL;

	// Use this for initialization
	void Start () {
		login = "";
		pass = "";
		info = "BIENVENIDO A LA CYBER AVENTURA CANAPRO";
		estiloBotonModoJuego = new GUIStyle();
		estiloBotonMapa = new GUIStyle();
		estiloBotonEstadisticas = new GUIStyle();
		estiloBotonLogin = new GUIStyle();

		contenidoBoxLogin = new GUIContent();
		contenidoBotonLogin = new GUIContent();

		GameObject adSQL = GameObject.Find("AdminSQL");
		SQL = (AdminSQL)adSQL.GetComponent(typeof(AdminSQL));

		if(SQL.DarUsuarioActual().Equals(AdminSQL.LOGOUT))
			estado = LOGIN;
		else
			estado = PRINCIPAL;
	}
	
	// Update is called once per frame
	void OnGUI () {
		GUI.skin = skinLogin;
		if(LOGIN == estado)
		{
			GUI.Box(new Rect(0,0,Screen.width,Screen.height), contenidoBoxLogin);
			login = GUI.TextField(new Rect((Screen.width/4),(Screen.height*3/8),(Screen.width*1/4),(Screen.height*1/8)),login);
			pass = GUI.PasswordField(new Rect((Screen.width/4),(Screen.height/2),(Screen.width*1/4),(Screen.height*1/8)),pass, "*"[0]);
			if(GUI.Button(new Rect((Screen.width*3/8),(Screen.height/2+Screen.height*2/16),(Screen.width*1/10),(Screen.height*1/32)), "Entrar"))
			{
				string result = SQL.LogIn(login,pass);
				if(result.Equals("exito")){
					estado = PRINCIPAL;
				}
				else if(result.Equals("mora")){
					info = "AL PARECER NO ESTÁS AL DÍA CON TUS OBLIGACIONES,\n NO PUEDES PARTICIPAR";
				}
				else{
					info = "USUARIO O CONTRASEÑA INCORRECTOS.\nSI NO ERES MIEMBRO DE CANAPRO ¡PUEDES INSCRIBIRTE YA!";
				}
			}
			GUI.Label(new Rect((Screen.width/3),(Screen.height*1/12),(Screen.width*1/2),(Screen.height*1/3)), info);
			GUI.Label(new Rect((Screen.width/4),(Screen.height*3/8)-(Screen.height*1/32),(Screen.width*1/4),(Screen.height*1/16)), "Número de cédula");
			GUI.Label(new Rect((Screen.width/4),(Screen.height/2)-(Screen.height*1/32),(Screen.width*1/4),(Screen.height*1/16)), "Contraseña");


		}
		else if(PRINCIPAL == estado)
		{
			GUI.Box(new Rect(0,0,Screen.width,Screen.height), contenidoBoxLogin);
			if(GUI.Button(new Rect((Screen.width/8),(Screen.height/12),(Screen.width*1/4),(Screen.height*1/16)), "Jugar"))
			{
				estado = MODO;
			}
			if(GUI.Button(new Rect((Screen.width/8),(Screen.height*1/3),(Screen.width*1/4),(Screen.height*1/16)), "Estadisticas"))
			{
				estado = ESTADISTICAS;
			}
			if(GUI.Button(new Rect((Screen.width/8),(Screen.height*7/12),(Screen.width*1/4),(Screen.height*1/16)), "Tutorial"))
			{
				estado= TUTO;
			}
			if(GUI.Button(new Rect((Screen.width/8),(Screen.height*5/6),(Screen.width*1/4),(Screen.height*1/16)), "Salir"))
			{
				SQL.LogOut();
				estado= LOGIN;
			}
		}
		else if(ESTADISTICAS == estado)
		{
			// Cargar estaditicas
		}
		else if(MODO == estado)
		{
			GUI.Box(new Rect(0,0,Screen.width,Screen.height), contenidoBoxLogin);
			if(GUI.Button(new Rect((Screen.width/3),(Screen.height/9),(Screen.width*1/4),(Screen.height*1/16)), "Nuevo Juego"))
			{
				SQL.NuevoRun();
				Application.LoadLevel("Mapa");
			}
			if(GUI.Button(new Rect((Screen.width/3),(Screen.height*4/9),(Screen.width*1/4),(Screen.height*1/16)), "Cargar Partida"))
			{
				SQL.CargarRun("Usuario");
				Application.LoadLevel("Mapa");
			}
			if(GUI.Button(new Rect((Screen.width/3),(Screen.height*7/9),(Screen.width*1/4),(Screen.height*1/16)), "Volver"))
			{
				estado= PRINCIPAL;
			}
		}
	}
}