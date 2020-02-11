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
using System.Collections.Generic;
using Presidencia.Catalogo_Impuestos_Traslado_Dominio.Datos;

namespace Presidencia.Catalogo_Impuestos_Traslado_Dominio.Negocio
{

    public class Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio
    {

        #region Variables Internas

        private String Tasa_ID;  
        private String Anio;     
        private String Estatus;    
        private String Valor_Inicial;
        private String Valor_Final;
        private String Tasa;
        private String Deducible_Uno;
        private String Deducible_Dos;
        private String Deducible_Tres;
        private String Comentarios;
        private String Usuario;
        private DataTable Ciudades;

        #endregion

        #region Variables Publicas
        
        public String P_Tasa_ID
        {
            get { return Tasa_ID; }
            set { Tasa_ID = value; }
        }
        
        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Valor_Inicial
        {
            get { return Valor_Inicial; }
            set { Valor_Inicial = value; }
        }
        
        public String P_Valor_Final
        {
            get { return Valor_Final; }
            set { Valor_Final = value; }
        }

        public String P_Tasa
        {
            get { return Tasa; }
            set { Tasa = value; }
        }

        public String P_Deducible_Uno
        {
            get { return Deducible_Uno; }
            set { Deducible_Uno = value; }
        }
        
        public String P_Deducible_Dos
        {
            get { return Deducible_Dos; }
            set { Deducible_Dos = value; }
        }

        public String P_Deducible_Tres
        {
            get { return Deducible_Tres; }
            set { Deducible_Tres = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public DataTable P_Ciudades
        {
            get { return Ciudades; }
            set { Ciudades = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Impuesto_Traslado_Dominio()
        {
            Cls_Cat_Pre_Impuestos_Traslado_Dominio_Datos.Alta_Impuesto_Traslado_Dominio(this);
        }

        public void Modificar_Impuesto_Traslado_Dominio()
        {
            Cls_Cat_Pre_Impuestos_Traslado_Dominio_Datos.Modificar_Impuesto_Traslado_Dominio(this);
        }

        public void Eliminar_Impuesto_Traslado_Dominio()
        {
            Cls_Cat_Pre_Impuestos_Traslado_Dominio_Datos.Eliminar_Impuesto_Traslado_Dominio(this);
        }

        public DataTable Consultar_Anio() //Busqueda
        {
            return Cls_Cat_Pre_Impuestos_Traslado_Dominio_Datos.Consultar_Anio(this);
        }

        public DataTable Consultar_Tasas_ID() 
        {
            return Cls_Cat_Pre_Impuestos_Traslado_Dominio_Datos.Consultar_Tasas_ID(this);
        }

        public DataTable Consultar_Impuestos_Traslado_Dominio()
        {
            return Cls_Cat_Pre_Impuestos_Traslado_Dominio_Datos.Consultar_Impuestos_Traslado_Dominio();
        }

        #endregion

    }
}