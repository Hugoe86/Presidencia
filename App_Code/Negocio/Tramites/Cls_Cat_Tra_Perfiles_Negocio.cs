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
using Presidencia.Catalogo_Perfiles.Datos;

/// <summary>
/// Summary description for Cls_Cat_Tra_Perfiles_Negocio
/// </summary>

namespace Presidencia.Catalogo_Perfiles.Negocio{

    public class Cls_Cat_Tra_Perfiles_Negocio{

        #region Variables Internas

            private String Perfil_ID;
            private String Nombre;
            private String Descripcion;
            private DataTable Detalles_Subproceso;
            private String Usuario;
            private String Tipo_DataTable;
            private String Tramite_id; 
            private GridView Gv_Actividades_Perfil;

        #endregion

        #region Variables Publicas

            public String P_Perfil_ID
            {
                get { return Perfil_ID; }
                set { Perfil_ID = value; }
            }

            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }

            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }

            public DataTable P_Detalles_Subproceso
            {
                get { return Detalles_Subproceso; }
                set { Detalles_Subproceso = value; }
            }

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public String P_Tipo_DataTable {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }
            public String P_Tramite_id
            {
                get { return Tramite_id; }
                set { Tramite_id = value; }
            }
            public GridView P_Gv_Actividades_Perfil
            {
                get { return Gv_Actividades_Perfil; }
                set { Gv_Actividades_Perfil = value; }
            }
        
        
        #endregion

        #region Metodos

            public void Alta_Perfil() {
                Cls_Cat_Tra_Perfiles_Datos.Alta_Perfil(this);
            }

            public void Modificar_Perfil() {
                Cls_Cat_Tra_Perfiles_Datos.Modificar_Perfil(this);
            }

            public void Elimnar_Perfil() {
                Cls_Cat_Tra_Perfiles_Datos.Eliminar_Perfil(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Tra_Perfiles_Datos.Consultar_DataTable(this);
            }

            public Cls_Cat_Tra_Perfiles_Negocio Consultar_Datos_Perfil() {
                return Cls_Cat_Tra_Perfiles_Datos.Consultar_Datos_Perfil(this);
            }
            
            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Tramites
            ///DESCRIPCIÓN          : Metodo para consultar los datos de las tramites
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 15/Junio/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Tramites()
            {
                return Cls_Cat_Tra_Perfiles_Datos.Consultar_Tramites(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Actividades_Tramites
            ///DESCRIPCIÓN          : Metodo para consultar los datos de las actividades
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 15/Junio/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Actividades_Tramites()
            {
                return Cls_Cat_Tra_Perfiles_Datos.Consultar_Actividades_Tramites(this);
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Actividades_Perfil
            ///DESCRIPCIÓN          : Metodo para consultar los datos de las actividades
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 15/Junio/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Actividades_Perfil()
            {
                return Cls_Cat_Tra_Perfiles_Datos.Consultar_Actividades_Perfil(this);
            } 
            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Tramites_Dependencia
            ///DESCRIPCIÓN          : Metodo para consultar los datos de los tramties ordenados por dependencia
            ///PROPIEDADES          :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 06/Diciembre/2012 
            ///*********************************************************************************************************
            public DataTable Consultar_Tramites_Dependencia()
            {
                return Cls_Cat_Tra_Perfiles_Datos.Consultar_Tramites_Dependencia(this);
            }
        #endregion

    }

}