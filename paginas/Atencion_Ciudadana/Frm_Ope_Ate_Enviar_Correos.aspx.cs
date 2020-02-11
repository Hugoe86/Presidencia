using System;
using System.Data;
using System.Web.UI;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Atencion_Ciudadana_Parametros_Correo.Negocio;
using Presidencia.Operacion_Atencion_Ciudadana_Registro_Correos_Enviados.Negocio;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Presidencia.Operacion_Atencion_Ciudadana_Envios_Correo.Negocio;

public partial class paginas_Atencion_Ciudadana_Frm_Ope_Ate_Enviar_Correos : System.Web.UI.Page
{
    private const string Directorio_Imagenes_Correo = "~/Archivos/Atencion_Ciudadana/Imagenes/";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // si la página se abre por primera vez, iniciar envío
            if (!Page.IsPostBack)
            {
                int Correos_Enviados = 0;
                Txt_Fecha_Inicio.Text = DateTime.Today.ToString("dd/MMM/yyyy");

                // envío de correos
                Correos_Enviados = Enviar_Correos_Cumpleanios(DateTime.Now);
                // si se enviaron correos mostrar mensaje indicando el número de correos enviados
                if (Correos_Enviados > 0)
                {
                    Mostrar_Mensaje("Se enviaron " + Correos_Enviados.ToString("#,##0") + " correos a contribuyentes.");
                }
                else
                {
                    Mostrar_Mensaje("No se encontraron contribuyentes para envío de correo.");
                }
            }
        }
        catch { };
    }
    #region METODOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Enviar_Correos_Cumpleanios
    ///DESCRIPCIÓN: Obtiene un listado de contribuyentes con email y fecha de nacimiento para enviar correos,
    ///         se obtiene un listado de correos enviados para no duplicar envíos y se hace el envío de correos
    ///         a las direcciones a las que no se haya enviado
    ///PARÁMETROS:
    ///         1. Fecha_Envio: especifica la fecha en la que se van a buscar cumpleaños para enviar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 24-oct-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected int Enviar_Correos_Cumpleanios(DateTime Fecha_Envio)
    {
        DataTable Dt_Contribuyentes_Cumpleanios;
        DataTable Dt_Correos_Enviados;
        Dictionary<string, int> Dicc_Cumpleanios;
        var Neg_Consultar_Cumpleanios = new Cls_Ope_Ate_Envios_Correo_Negocio();
        var Neg_Consultar_Correos_Enviados = new Cls_Ope_Ate_Registro_Correo_Enviados_Negocio();
        var Neg_Consulta_Parametros = new Cls_Cat_Ate_Parametros_Correo_Negocio();
        int Correos_Enviados = 0;

        try
        {
            // llamar al método que consulta los parámetros pasando como tipo de correo FELICITACION
            Neg_Consulta_Parametros.P_Tipo_Correo = "FELICITACION";
            Neg_Consulta_Parametros.Consultar_Parametros_Correo();


            // consultar los correos enviados
            Neg_Consultar_Correos_Enviados.P_Motivo = "FELICITACION";
            Neg_Consultar_Correos_Enviados.P_Fecha_Notificacion = Fecha_Envio;
            Dt_Correos_Enviados = Neg_Consultar_Correos_Enviados.Consultar_Registro_Correos_Enviados();

            // consultar los contribuyentes que cumplen años
            Neg_Consultar_Cumpleanios.P_Fecha = Fecha_Envio;
            Dt_Contribuyentes_Cumpleanios = Neg_Consultar_Cumpleanios.Consultar_Contribuyentes_Cumpleanios();

            // llamar método para obtener solo los contribuyentes a los que se va a enviar correo
            Dicc_Cumpleanios = Obtener_Contribuyentes_Enviar_Correo(Dt_Contribuyentes_Cumpleanios, Dt_Correos_Enviados);

            // validar que hay parámetros de correo para hacer el envío
            if (Neg_Consulta_Parametros.P_Correo_Servidor.Length > 0 && Neg_Consulta_Parametros.P_Correo_Remitente.Length > 0)
                // enviar correos a las direcciones que resulten en el diccionario que regresó el método
                foreach (KeyValuePair<string, int> Fila_Correo in Dicc_Cumpleanios)
                {
                    Enviar_Correo(
                        Neg_Consulta_Parametros
                        , Dt_Contribuyentes_Cumpleanios.Rows[Fila_Correo.Value][Cat_Pre_Contribuyentes.Campo_Email].ToString()
                        , Dt_Contribuyentes_Cumpleanios.Rows[Fila_Correo.Value][Cat_Pre_Contribuyentes.Campo_Nombre].ToString()
                        + " " + Dt_Contribuyentes_Cumpleanios.Rows[Fila_Correo.Value][Cat_Pre_Contribuyentes.Campo_Apellido_Paterno].ToString()
                        + " " + Dt_Contribuyentes_Cumpleanios.Rows[Fila_Correo.Value][Cat_Pre_Contribuyentes.Campo_Apellido_Materno].ToString()
                        , Dt_Contribuyentes_Cumpleanios.Rows[Fila_Correo.Value][Cat_Pre_Contribuyentes.Campo_Contribuyente_ID].ToString()
                        , Dt_Contribuyentes_Cumpleanios.Rows[Fila_Correo.Value][Cat_Pre_Contribuyentes.Campo_Calle_Ubicacion].ToString()
                        , Dt_Contribuyentes_Cumpleanios.Rows[Fila_Correo.Value][Cat_Pre_Contribuyentes.Campo_Colonia_Ubicacion].ToString()
                        , Fecha_Envio);
                    Correos_Enviados++;
                }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Correos_Enviados;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Mensaje
    ///DESCRIPCIÓN: Muestra el mensaje recibido como parámetro en la pagina y si es nulo o vacío, oculta los controles del mensaje
    ///PARÁMETROS:
    /// 		1. Mensaje: texto a mostrar en la página en el área de notificación
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 05-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Mostrar_Mensaje(string Mensaje)
    {
        // validar que Mensaje contenga texto
        if (!string.IsNullOrEmpty(Mensaje))
        {
            Lbl_Informacion.Text = Mensaje;
            Lbl_Informacion.Visible = true;
            Img_Advertencia.Visible = true;
        }
        else
        {
            Lbl_Informacion.Text = "";
            Lbl_Informacion.Visible = false;
            Img_Advertencia.Visible = false;
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Obtener_Contribuyentes_Enviar_Correo
    ///DESCRIPCIÓN: se excluyen las direcciones de correo electrónico en la tabla Dt_Correos_Enviados de la tabla 
    ///             Dt_Contribuyentes_Cumpleanios para obtener un listado de direcciones a las que no se ha enviado correo
    ///PARÁMETROS:
    ///         1. Dt_Contribuyentes_Cumpleanios: tabla con los registros a enviar
    ///         2. Dt_Correos_Enviados: tabla con los registros de correos enviados
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 24-oct-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected Dictionary<string, int> Obtener_Contribuyentes_Enviar_Correo(DataTable Dt_Contribuyentes_Cumpleanios, DataTable Dt_Correos_Enviados)
    {
        List<string> Lista_Correos_Enviados = new List<string>();
        Dictionary<string, int> Dicc_Cumpleanios = new Dictionary<string, int>();
        int Indice_Columna_Correo = 0;
        try
        {
            // validar que la tabla contiene valores y que tiene una columna con el campo correo (destinatario)
            if (Dt_Correos_Enviados != null && Dt_Correos_Enviados.Rows.Count > 0 && Dt_Correos_Enviados.Columns.Contains(Ope_Ate_Correos_Enviados.Campo_Destinatario))
            {
                Indice_Columna_Correo = Dt_Correos_Enviados.Columns.IndexOf(Ope_Ate_Correos_Enviados.Campo_Destinatario);
                // generar un listado de direcciones de correo a las que ya se les envió correo a partir de la tabla
                for (int Fila = 0; Fila < Dt_Correos_Enviados.Rows.Count; Fila++)
                {
                    // si la dirección de correo no está en la lista, agregarla
                    if (!Lista_Correos_Enviados.Contains(Dt_Correos_Enviados.Rows[Fila][Indice_Columna_Correo].ToString()))
                    {
                        Lista_Correos_Enviados.Add(Dt_Correos_Enviados.Rows[Fila][Indice_Columna_Correo].ToString());
                    }
                }
            }

            // validar que la tabla contiene valores y que tiene una columna con el campo correo (destinatario)
            if (Dt_Contribuyentes_Cumpleanios != null && Dt_Contribuyentes_Cumpleanios.Rows.Count > 0 && Dt_Contribuyentes_Cumpleanios.Columns.Contains(Cat_Pre_Contribuyentes.Campo_Email))
            {
                Indice_Columna_Correo = Dt_Contribuyentes_Cumpleanios.Columns.IndexOf(Cat_Pre_Contribuyentes.Campo_Email);
                // recorrer la tabla eliminando las filas con registro en la lista de correos enviados
                for (int Fila = Dt_Contribuyentes_Cumpleanios.Rows.Count - 1; Fila >= 0; Fila--)
                {
                    if (Lista_Correos_Enviados.Contains(Dt_Contribuyentes_Cumpleanios.Rows[Fila][Indice_Columna_Correo].ToString()))
                    {
                        Dt_Contribuyentes_Cumpleanios.Rows.RemoveAt(Fila);
                    }
                }
                Dt_Contribuyentes_Cumpleanios.AcceptChanges();

                // generar un diccionario con las direcciones de correo a enviar y el número de fila en la que se encuentra
                // el registro en la tabla Dt_Contribuyentes_Cumpleanios, de esta manera, se omiten correos duplicados en caso de haberlos
                for (int Fila = 0; Fila < Dt_Contribuyentes_Cumpleanios.Rows.Count; Fila++)
                {
                    if (Dicc_Cumpleanios.ContainsKey(Dt_Contribuyentes_Cumpleanios.Rows[Fila][Indice_Columna_Correo].ToString()) == false)
                    {
                        Dicc_Cumpleanios.Add(Dt_Contribuyentes_Cumpleanios.Rows[Fila][Indice_Columna_Correo].ToString(), Fila);
                    }
                }
            }

            return Dicc_Cumpleanios;
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Enviar_Correo
    ///DESCRIPCIÓN: Envia un correo al ciudadano con los parámetros recibidos
    ///PROPIEDADES: 
    ///             1. Neg_Parametros_Correo: parámetros para envío de correo
    ///             2. Email_Destinatario: direccción de correo electrónico a la que se envía la notificación
    /// 		    3. Nombre: nombre del ciudadano que solicita
    /// 		    4. Ciudadano_Id: id del ciudadano al que se envia el correo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 16-oct-2012
    ///MODIFICO:
    ///FECHA_MODIFICO: 
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Enviar_Correo(Cls_Cat_Ate_Parametros_Correo_Negocio Neg_Parametros_Correo, string Email_Destinatario, string Nombre, string Ciudadano_Id, string Calle, string Colonia, DateTime Fecha)
    {
        Cls_Ope_Ate_Registro_Correo_Enviados_Negocio Neg_Alta_Registro_Correo = new Cls_Ope_Ate_Registro_Correo_Enviados_Negocio();
        MatchCollection Coincidencias_Encontradas;
        // expresión para encontrar el texto entre comillas precedido por cid:
        string Expresion = "\"cid:(.*)\"";
        List<string> Listado_Archivos;
        string Ruta_Directorio = Server.MapPath(Directorio_Imagenes_Correo);

        try
        {
            // validar que haya un Destinatario
            if (!string.IsNullOrEmpty(Email_Destinatario))
            {
                MailMessage Manejador_Correo = new MailMessage();
                string Cuerpo_Correo = Neg_Parametros_Correo.P_Correo_Cuerpo.Replace("[NOMBRE]", Nombre).Replace("[PETICION]", "").Replace("[SOLUCION]", "").Replace("[DOMICILIO]", Calle + ", " + Colonia).Replace("[EMAIL]", Email_Destinatario).Replace("[FOLIO]", "").Replace("[FECHA_SOLUCION]", "").Replace("[FECHA]", Fecha.ToString("d 'de' MMMM 'de' yyyy")).Replace("[FECHA_NACIMIENTO]", Fecha.ToString("d 'de' MMMM 'de' yyyy")).Replace("[HOY]", DateTime.Today.ToString("d 'de' MMMM 'de' yyyy"));
                // agregar parámetros al correo
                Manejador_Correo.To.Add(Email_Destinatario);
                Manejador_Correo.From = new MailAddress(Neg_Parametros_Correo.P_Correo_Remitente, "Atención Ciudadana");
                Manejador_Correo.Subject = Neg_Parametros_Correo.P_Correo_Saludo;

                // crear vistas con el contenido del correo (una HTML y otra en texto plano para los clientes que visualizan sólo el texto)
                AlternateView Correo_HTML = AlternateView.CreateAlternateViewFromString(Cuerpo_Correo, null, "text/html");

                // obtener listado de archivos del directorio con imagenes para correo
                Listado_Archivos = Directory.GetFiles(Ruta_Directorio, "*.*", SearchOption.TopDirectoryOnly).ToList<string>();
                // obtener un listado de recursos dentro del correo
                Coincidencias_Encontradas = Regex.Matches(Cuerpo_Correo, Expresion);
                foreach (Match Coincidencia in Coincidencias_Encontradas)
                {
                    // validar el conteo del grupos en la coincidencia
                    if (Coincidencia.Groups.Count > 0)
                    {
                        // recorrer el listado de archivos en el directorio hasta encontrar el recurso
                        foreach (string Archivo in Listado_Archivos)
                        {
                            // si el nombre del archivo es igual al del recurso en el cuerpo del correo, agregar como LinkedResource
                            if(Coincidencia.Groups[1].Value == Path.GetFileNameWithoutExtension(Archivo))
                            {
                                // agregar LinkedResource para insertar imagenes en el correo
                                LinkedResource Recurso_Adjunto = new LinkedResource(Archivo);
                                Recurso_Adjunto.ContentId = Coincidencia.Groups[1].Value;
                                Correo_HTML.LinkedResources.Add(Recurso_Adjunto);
                                break;
                            }
                        }

                    }
                }

                // agregar vistas con el cuerpo de correo a la instancia de la clase 
                Manejador_Correo.AlternateViews.Add(Correo_HTML);
                // crear instancia de cliente SMTP
                SmtpClient sc = new SmtpClient(Neg_Parametros_Correo.P_Correo_Servidor);
                // usuario y contraseña para el envío de correo
                sc.Credentials = new System.Net.NetworkCredential(Neg_Parametros_Correo.P_Correo_Remitente, Neg_Parametros_Correo.P_Password_Usuario_Correo);
                // enviar el correo
                sc.Send(Manejador_Correo);

                Manejador_Correo.Dispose();

                // guardar registro de correo enviado
                Neg_Alta_Registro_Correo.P_Destinatario = Email_Destinatario;
                Neg_Alta_Registro_Correo.P_Nombre_Contribuyente = Nombre;
                Neg_Alta_Registro_Correo.P_Motivo = "FELICITACION";
                Neg_Alta_Registro_Correo.P_Contribuyente_ID = Ciudadano_Id;
                if (string.IsNullOrEmpty(Cls_Sessiones.Nombre_Empleado))
                {
                    Neg_Alta_Registro_Correo.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                }
                else
                {
                    Neg_Alta_Registro_Correo.P_Usuario_Creo = "ENVIO DE CORREOS AUTOMATICO";
                }
                Neg_Alta_Registro_Correo.Alta_Registro_Correo_Enviado();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al enviar correo: " + Ex.Message);
        }
    }

    #endregion METODOS
    #region EVENTOS

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Enviar_Correos_Click
    ///DESCRIPCIÓN: Manejo del evento click en el botón enviar correos que llama al 
    ///         método enviar correos con la fecha de la caja de texto como parámetro
    ///PROPIEDADES: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 05-nov-2012
    ///MODIFICO:
    ///FECHA_MODIFICO: 
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Enviar_Correos_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        DateTime Fecha_Envio;
        int Correos_Enviados = 0;

        Mostrar_Mensaje("");

        // validar que se haya ingresado texto
        if (Txt_Fecha_Inicio.Text.Trim().Length > 0)
        {
            DateTime.TryParse(Txt_Fecha_Inicio.Text, out Fecha_Envio);
            // validar que sea una fecha válida
            if (Fecha_Envio != DateTime.MinValue)
            {
                Txt_Fecha_Inicio.Text = Fecha_Envio.ToString("dd/MMM/yyyy");
                Correos_Enviados = Enviar_Correos_Cumpleanios(Fecha_Envio);
                Mostrar_Mensaje("Correos enviados a contribuyentes: " + Correos_Enviados.ToString("#,##0"));
            }
            else
            {
                Mostrar_Mensaje("La fecha ingresada no tiene el formato correcto.");
            }
        }
        else
        {
            Mostrar_Mensaje("Debe ingresar una fecha para el envío.");
        }
    }
    #endregion EVENTOS

}
