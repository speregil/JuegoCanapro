using UnityEngine;
using System.Collections;

public class AdminSQL : MonoBehaviour {

	public 	static string 	LOGOUT = "LogOut";
	private CRUD			ConexionBD;
	private string 			UsuarioActual;
	private string			LlaveUsuario;
	private Run 			RunActual;

	//variable ultra provicional
	public bool primerRun = false;

	void Awake(){
		DontDestroyOnLoad(transform.gameObject);
	}
	
	void Start () {
		UsuarioActual = LOGOUT;
		ConexionBD = (CRUD)GetComponent(typeof(CRUD));
	}

	public string DarUsuarioActual(){
		return UsuarioActual;
	}

	public Run DarRunActual(){
		return RunActual;
	}

	public string DarLlaveActual(){
		return LlaveUsuario;
	}

	public string LogIn(string User, string Password){
		ConexionBD.hacerLogin(User, Password);
		LlaveUsuario = ConexionBD.login;// Hay que parcear esta informacion
		// Prueba dummy para la presentacion
		if(User.Equals("123")){
			UsuarioActual = "Alexis";
			return "exito";
		}
		else if(User.Equals("456")){
			UsuarioActual = "Sonia";
			return "mora";
		}

		return "fallo";
	}

	public void LogOut(){
		UsuarioActual = LOGOUT;
	}

	public void NuevoRun(){
		ConexionBD.crearNuevoRun(int.Parse(LlaveUsuario));
		RunActual = new Run();
		ConexionBD.pedirAvance(LlaveUsuario);
	}

	public void CargarRun(){
		// Cargar los datos del run de la bd
		if(!ConexionBD.nuevoRun.Equals("Fracaso"))
		{
			string[] cadena = ConexionBD.avance.Split(new char [] {';'});
			if(cadena.Length > 7)
			{
				int[] bulks = new int[(cadena.Length-7)/3];
				int[] puntuaciones = new int[(cadena.Length-7)/3];
				float[] tiempos = new float[(cadena.Length-7)/3];
				bool[] flag = new bool[12];
				UsuarioActual =	cadena[0];
				int indice = 0;
				int sumaPuntuacion = 0;
				float sumaTiempo = 0;
				for(int i = 7; i < cadena.Length;)
				{
					bulks[indice] = int.Parse(cadena[i]);
					puntuaciones[indice] = int.Parse(cadena[i+1]);
					tiempos[indice] = float.Parse(cadena[i+2]);
					indice++;
					i = i+3;
				}
				int f = 0;
				while(f<indice)
				{
					if(puntuaciones[f]!=0 )
					{
						flag[f]=true;
						sumaPuntuacion += puntuaciones[f];
						sumaTiempo += tiempos[f];
					}
					f++;
				}

				RunActual = new Run(flag,sumaPuntuacion,sumaTiempo);
			}
			else
			{
				RunActual = new Run();
			}
		}
	}

	public void GuardarBulk(int IDBulk, int IDCuenta, int puntuacion, float tiempo, Bulk bulkActivo){
		string respuestas = bulkActivo.darRespuestas();
		int i = 0;
		int indice = 0;
		char[] caracteres = respuestas.ToCharArray();
		char[] respuestass = new char[caracteres.Length/2+1];
		int[] IDPreguntas = new int[caracteres.Length/2+1];
		float[] tiempos = new float[caracteres.Length/2+1];
		while(i+1 < caracteres.Length )
		{
			IDPreguntas[indice] = caracteres[i];
			respuestass[indice] = caracteres[i+1];
			tiempos[indice] = 0;
			indice++;
			i += 2;
		}
		ConexionBD.guardarCuestionario(IDBulk,tiempos, IDCuenta, respuestass, puntuacion, 0, IDPreguntas);
	}

	public string top20()
	{
		return ConexionBD.top20;
	}
	public void pedirTop20()
	{
		ConexionBD.pedirTop20 ();
	}
	public void verificarPrimerBulk()
	{
		ConexionBD.primerBulk(LlaveUsuario);
	}
	public bool esPrimerBulk()
	{
		return ConexionBD.esPrimerBulk;
	}
	public bool esPrimerRun()
	{
		return ConexionBD.esPrimerRun;
	}
	public void pedirAvance()
	{
		ConexionBD.pedirAvance(LlaveUsuario);
	}

	/**public bool verificarUsuario(string IDCuenta){
		//Verificar que no halla hecho un run previo

	}**/
}