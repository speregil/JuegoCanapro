using UnityEngine;
using System.Collections;

public class AdminSQL : MonoBehaviour {

	public 	static string 	LOGOUT = "LogOut";
	private CRUD			ConexionBD;
	private string 			UsuarioActual;
	private string			LlaveUsuario;
	private Run 			RunActual;
	//variable con el ID de las preguntas respondidas
	private string			preguntass;
	private Interfaz		conexionInterfaz;

	//variable ultra provicional
	public bool primerRun = false;

	void Awake(){
		DontDestroyOnLoad(transform.gameObject);
	}
	
	void Start () {
		UsuarioActual = LOGOUT;
		LlaveUsuario = "Login vacio";
		ConexionBD = (CRUD)GetComponent(typeof(CRUD));
		GameObject adInterfaz = GameObject.Find("GUIPrincipal");
		conexionInterfaz= (Interfaz)adInterfaz.GetComponent(typeof(Interfaz));
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

	public void LogIn(string User, string Password){
		if(!User.Equals("1991")){
			ConexionBD.hacerLogin(User, Password);
			ConexionBD.buscarParticipante(User);
 			StartCoroutine("CargarLogin");
		}
		else{
			UsuarioActual = "Administrador";
			conexionInterfaz.CargarLogin("1991");
		}
	}

	private IEnumerator CargarLogin(){
		yield return new WaitForSeconds(8);
		{
			LlaveUsuario = ConexionBD.login;
			string[] user = ConexionBD.participante.Split(' ');
			UsuarioActual = user[0];
			conexionInterfaz.CargarLogin(LlaveUsuario);
		}
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
			string[] cadena = ConexionBD.avance.Split(new char [] {';'});
				int[] bulks = new int[(int)((cadena.Length-7)/3)];
				int[] puntuaciones = new int[(int)((cadena.Length-7)/3)];
		        float[] tiempos = new float[(int)((cadena.Length-7)/3)];
				bool[] flag = new bool[12];
				UsuarioActual =	cadena[0];
				int indice = 0;
				int sumaPuntuacion = 0;
				float sumaTiempo = 0;
				for(int i = 7; i < cadena.Length;)
				{
					bulks[indice] = int.Parse(cadena[i]);
					tiempos[indice] = float.Parse(cadena[i+1]);
					puntuaciones[indice] = int.Parse(cadena[i+2]);
					indice++;
					i = i+3;
				}
				int f = 0;
				while(f<indice)
				{
					flag[bulks[f]]=true;
					sumaPuntuacion += puntuaciones[f];
					sumaTiempo += tiempos[f];
					f++;
				}

				RunActual = new Run(flag,sumaPuntuacion,sumaTiempo);
	}

	public void GuardarBulk(int IDBulk, int IDCuenta, int puntuacion, float tiempo, Bulk bulkActivo){
		//string respuestas = bulkActivo.darRespuestas();
		//int i = 0;
		//int indice = 0;
		//respuestas = respuestas.Substring(0, respuestas.Length-1);
		//string[] caracteres = respuestas.Split(new char [] {';'});
		//string[] respuestass = new string[caracteres.Length/2];
		//string[] IDPreguntas = new string[caracteres.Length/2];
		//float[] tiempos = new float[caracteres.Length/2];
		//while(i+1 < caracteres.Length )
		//{
		//	IDPreguntas[indice] = caracteres[i];
		//	respuestass[indice] = caracteres[i+1];
		//	tiempos[indice] = 0;
		//	indice++;
		//	i += 2;
		//}
		ConexionBD.guardarCuestionario(IDBulk, IDCuenta, puntuacion, tiempo);
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
	public void asginarRunFalse()
	{
		ConexionBD.esPrimerRun = false;
	}
	private void asignarPreguntaAVacio()
	{
		ConexionBD.guardarPregunta = "";
	}
	public void guardarPregunta(string formato)
	{
		formato = LlaveUsuario+";"+formato;
		ConexionBD.guardaPregunta(formato);
	}
	public bool guardoPregunta()
	{
		if(ConexionBD.guardarPregunta.Equals( "Fracaso" ) || ConexionBD.guardarPregunta.Equals( "" ))
		{
			asignarPreguntaAVacio();
			return false;
		}
		else if(ConexionBD.guardarPregunta.Equals( "Exito" ))
		{
			asignarPreguntaAVacio();
			return true;
		}
		return false;
	}
	public void pedirPreguntas()
	{
		ConexionBD.pedirPreguntas(LlaveUsuario);
	}

	public string preguntas()
	{
		preguntass = ConexionBD.preguntas;
		return preguntass;
	}

	public void contarParticipantes(){
		ConexionBD.pedirConteo();
	}

	public string pedirConteo(){
		return ConexionBD.numParticipantes;
	}
	/**public bool verificarUsuario(string IDCuenta){
		//Verificar que no halla hecho un run previo

	}**/
}