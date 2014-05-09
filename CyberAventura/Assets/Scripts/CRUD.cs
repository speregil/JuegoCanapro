using System.Collections;
using UnityEngine;

public class CRUD : MonoBehaviour{
	//String con el formato de respuesta del login
	public string login;
	//String con el formato de respuesta de guardarCuestionario
	public string cuestionario;
	//String con el formato de respuesta de pedirAvance
	public string avance;
	//String de respuesta de pedir top 20
	public string top20;
	//String con el formato de respuesta de pedirpromdesvypor
	public string desvy;
	//String con el formato de respuesta de crear nuevo run
	public string nuevoRun;
	//String con el formato de repsuesta de buscarParticipante
	public string participante;
	//Booleano con confirmacion de si es primer Bulk
	public bool esPrimerBulk;
	//Booleano con confirmacion de si es primer Run
	public bool esPrimerRun;

	//Clase para hacer consultas en la base de datos
	public CRUD()
	{
		login = "";
		cuestionario = "";
		avance = "";
		top20 = "";
		desvy = "";
		nuevoRun = "";
		participante = "";
		esPrimerBulk = false;
		esPrimerRun = false;
	}
	//Permite hacer login en la aplicacion
	public void hacerLogin(string user, string password)
	{
		Application.ExternalCall("login", user, password);
		login = "1";
	}
	//Se deben pasar igual numero de tiempos, respuestas, y de IDPreguntas o pueden crear errores
	/*
	 * Este metodo puede retornar strings: "Fracaso" o "Exito"
	 * */
	public void guardarCuestionario(int IDBulk, float[] tiempos, int IDCuenta, char[] respuestas, int puntuacion, float tiempo, int[] IDPreguntas)
	{
		string formato = "";
		int indice = tiempos.Length;
		int i = 0;
		while(indice > i)
		{
			formato += IDPreguntas[i]+";"+respuestas[i]+";"+tiempos[i]+";";
			i++;
		}
		formato = formato.Substring(0, formato.Length-1);
		Application.ExternalCall("guardarcuestionario", IDBulk, IDCuenta, formato, puntuacion, tiempo);
	}
	/*
	 *Se pide el avance de un jugador en el juego y se entrega en el siguiente formato:
	 *nombre;puntuación;promedioTiempo;Run_actual;tiempoRun;puntuacionRunActual;bulks;idbulk;tiempobulk;puntuacionbulk
	 *Siendo "bulks" una palabra literal en el string para marcar el comienzo de la lista de bulks hechos por el jugador en ese run
	 *
	 */
	public void pedirAvance(string IDCuenta)
	{
		Application.ExternalCall("pediravance", IDCuenta);
	}
	/*
	 * Formato: top20;ID;login;nombre;puntuacion...
	 * siendo "top20" una palabra literal dentro del string
	 * */
	public void pedirTop20()
	{
		Application.ExternalCall("pedirtop20");
	}
	public void primerBulk(string IDCuenta)
	{
		Application.ExternalCall("esprimerbulk");
	}
	/*
	 * Formato: pedirpromdesvypor;promediopuntuaciones;desviacionestandarpuntuaciones;
	 * promediotiempos;desviacionestandarTiempos;porcentajeDeRunS1Terminados;cantidadDeRunsTotales
	 * siendo "pedirpromdesvypor" una palabra literal dentro del string
	 * */
	public void pedirPromDesvYPor()
	{
		Application.ExternalCall("pedirpromdesvypor");
	}
	/*
	 * Este metodo puede retornar strings: "Fracaso" o "Exito"
	 * */
	public void crearNuevoRun(int IDCuenta)
	{
		Application.ExternalCall("crearnuevorun", IDCuenta);
	}
	/*
	 * Formato: buscarparticipante;ID;NOMBRE;LOGIN;PUNTUACION;ROL
	 * */
	public void buscarParticipante(string login)
	{
		Application.ExternalCall("buscarparticipante", login);
	}
	/*
	 * Guarda el resultado del login en su variable
	 * */
	public void recibirLogin(string formato)
	{
		login = formato;
	}
	/*
	 * Guarda el resultado del avance en su variable
	 * */
	public void recibirAvance(string formato)
	{
		avance = formato;
	}
	/*
	 * Guarda el resultado del top20 en su variable
	 * */
	public void recibirtop20(string formato)
	{
		//Debug.Log(formato);
		top20 = formato;
	}
	/*
	 * Guarda el resultado del desvy en su variable
	 * */
	public void recibirDesvy(string formato)
	{
		desvy = formato;
	}
	/*
	 * Guarda el resultado del run en su variable
	 * */
	public void recibirRun(string formato)
	{
		nuevoRun = formato;
	}
	/*
	 * Guarda el resultado del participante en su variable
	 * */
	public void recibirParticipante(string formato)
	{
		participante = formato;
	}
	/*
	 * Guarda el resultado del participante en su variable
	 * */
	public void recibirCuestionario(string formato)
	{
		cuestionario = formato;
	}
	public void recibirPrimerBulk(string formato)
	{
		if(formato.Equals("si"))
		{
			esPrimerBulk = true;
			esPrimerRun = true;
		}
		else if(formato.Equals("sisi"))
		{
			esPrimerBulk = true;
			esPrimerRun = false;
		}
		else
		{
			esPrimerBulk = false;
		}
	}
}