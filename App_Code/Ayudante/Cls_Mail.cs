using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net.Mail;
using System.Web.Util;
using System.Net;
using Presidencia.Registro_Peticion.Datos;
//using System.Web.Mail;

/// <summary>
/// Summary description for Mail
/// </summary>
public class Cls_Mail{
    //datos del mail
    private String Envia;
    private String Recibe;
    private String Subject;
    private String Texto;
    //datos de configuracion
    private String Servidor;
    private String Password;
    private String Adjunto;

    public String P_Adjunto
    {
        get { return Adjunto; }
        set { Adjunto = value; }
    }
    
	public Cls_Mail()
	{
        Servidor = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Servidor_Correo].ToString();
        Envia = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Correo_Saliente].ToString();
        Password = Cls_Cat_Ate_Peticiones_Datos.Consulta_Parametros().Rows[0][Presidencia.Constantes.Apl_Parametros.Campo_Password_Correo].ToString();
        
	}
    public string P_Envia
    {
        get { return Envia; }
        set { Envia = value; }
    }
    public string P_Recibe {
        get { return Recibe; }
        set { Recibe = value; }
    }
    public string P_Subject {
        get { return Subject; }
        set { Subject = value; }
    }
    public string P_Texto {
        get { return Texto; }
        set { Texto = value; }
    }
    public string P_Servidor {
        get { return Servidor; }
        set { Servidor = value; }
    }
    public string P_Password
    {
        get { return Password; }
        set { Password = value; }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: enviar_correo
    ///DESCRIPCIÓN: Realiza los envios del mail
    ///PROPIEDADES: 
    ///METODOS    : 
    ///CREO:
    ///FECHA_CREO: 27/Agosto/2009 
    ///MODIFICO:Jesus Toledo
    ///FECHA_MODIFICO: 03/Sep/09
    ///CAUSA_MODIFICACIÓN:Se utilizó System.net.mail
    ///*******************************************************************************
    public void Enviar_Correo() {
        MailMessage email = new MailMessage(new MailAddress(Envia), new MailAddress(Recibe));
        Attachment Archivo_Mail = null;
        
        email.Subject = Subject;
        email.Body = Texto;
        email.IsBodyHtml = true;
        if (Adjunto != null)
        {
            Archivo_Mail = new Attachment(Adjunto);
            email.Attachments.Add(Archivo_Mail);
        }
        System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
        smtp.Host = Servidor;
        smtp.Port = 25;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(Envia, Password);
        smtp.Send(email);        
        email = null;        
        
    }//fin de enviar correo

}
