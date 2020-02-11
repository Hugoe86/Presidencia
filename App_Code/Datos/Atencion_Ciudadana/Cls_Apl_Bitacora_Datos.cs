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
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora.Negocios;


/// <summary>
/// Summary description for Cls_Apl_Bitacora_Datos
/// </summary>

namespace Presidencia.Bitacora.Datos
{
    public class Cls_Apl_Bitacora_Datos
    {
        public Cls_Apl_Bitacora_Datos()
        {

        }
        
        #region Metodos
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Execute_Query
        ///DESCRIPCIÓN: Manda llamar la clase de OracleHelper y genera los query para 
        ///             generar los reportes
        ///PARAMETROS:  1.- Cls_Apl_Bitacora_Negocio Bitacora
        ///             2.- int Opcion valores (0,1,2) 
        ///                 0= Es para cuando se seleccionen paginas en el grid en la pestaña 0
        ///                 1= Es cuando se seleccionen las acciones de la pestaña 1
        ///                 2= Es para cuando se seleccionan todas las paginas en la pestaña 0
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 02/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public DataSet Consultar_Bitacora(Cls_Apl_Bitacora_Negocio Bitacora, int Opcion)
        {

            String Mi_SQL = "";
            DataSet Data_Set = new DataSet();

            Mi_SQL = "SELECT USUARIO." + Cat_Empleados.Campo_Nombre + 
                     " || ' ' || USUARIO." + Cat_Empleados.Campo_Apellido_Paterno + 
                     " || ' ' || USUARIO." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE_USUARIO" +
                     ", BITACORA." + Apl_Bitacora.Campo_Fecha_Hora + 
                     ", BITACORA." + Apl_Bitacora.Campo_Accion + 
                     ", MENU." + Apl_Cat_Menus.Campo_Menu_Descripcion + 
                     ", BITACORA." + Apl_Bitacora.Campo_Recurso_ID + 
                     ", BITACORA." + Apl_Bitacora.Campo_Descripcion + 
                     " FROM " + Cat_Empleados.Tabla_Cat_Empleados + 
                     " USUARIO JOIN " + Apl_Bitacora.Tabla_Apl_Bitacora + " BITACORA ON USUARIO." +
                    Cat_Empleados.Campo_Empleado_ID + " = BITACORA." + Apl_Bitacora.Campo_Empleado_ID +
                    " JOIN " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus +
                    " MENU ON BITACORA." + Apl_Bitacora.Campo_Recurso + " = MENU." + Apl_Cat_Menus.Campo_Pagina;
            
            switch (Opcion)
            {
                case 0:
                    Mi_SQL = Mi_SQL + " WHERE " + " BITACORA." + Apl_Bitacora.Campo_Recurso + " IN (" + Bitacora.P_Catalogos + ") " +
                    Bitacora.P_Accion + " " + Bitacora.P_Fecha + " " + Bitacora.P_Usuario + " ORDER BY " + Bitacora.P_Orden;
                    break;

                case 1:
                    Mi_SQL = "SELECT USUARIO." + Cat_Empleados.Campo_Nombre + 
                        " || ' ' || USUARIO." + Cat_Empleados.Campo_Apellido_Paterno + 
                        " || ' ' || USUARIO." + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE_USUARIO" +
                        ", BITACORA." + Apl_Bitacora.Campo_Fecha_Hora + ", BITACORA." +
                        Apl_Bitacora.Campo_Accion + ", BITACORA." +
                        Apl_Bitacora.Campo_Recurso_ID + ", BITACORA." + 
                        Apl_Bitacora.Campo_Descripcion + 
                        " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " USUARIO"+
                        " JOIN " + Apl_Bitacora.Tabla_Apl_Bitacora + " BITACORA ON USUARIO." +
                        Cat_Empleados.Campo_Empleado_ID + " = BITACORA." + Apl_Bitacora.Campo_Empleado_ID +
                        " WHERE BITACORA." + Apl_Bitacora.Campo_Accion + " IN (" + Bitacora.P_Accion + ") " +
                        Bitacora.P_Fecha + " " + Bitacora.P_Usuario + " ORDER BY " + Bitacora.P_Orden;

                    break;
                case 2:
                    Mi_SQL = Mi_SQL + " WHERE " + Bitacora.P_Accion + " " + Bitacora.P_Fecha + " " + Bitacora.P_Usuario + " ORDER BY " + Bitacora.P_Orden;
                    break;

            }//fin del Switch

            Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pagina
        ///DESCRIPCIÓN: Metodo que obtiene el nombre de las paginas y su clave  
        ///PARAMETROS:  
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consultar_Pagina()
        {
            String Mi_SQL = "";
            DataSet Data_Set = new DataSet();
            Mi_SQL = "SELECT " + Apl_Cat_Menus.Campo_Menu_Descripcion + "," + Apl_Cat_Menus.Campo_Clasificacion + "," + Apl_Cat_Menus.Campo_Pagina +
                " FROM " + Apl_Cat_Menus.Tabla_Apl_Cat_Menus + " WHERE " + Apl_Cat_Menus.Campo_Parent_ID + " NOT IN ('0') ORDER BY " + Apl_Cat_Menus.Campo_Menu_Descripcion;
            Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;

        }//fin de llenar grid
        #endregion
    }
}