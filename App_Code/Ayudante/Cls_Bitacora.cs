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
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;

/// <summary>
/// Summary description for Cls_Bitacora
/// </summary>
/// 

namespace Presidencia.Bitacora_Eventos
{

    public class Cls_Bitacora
    {
        public Cls_Bitacora()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        #region Metodos 
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Revisar_Actualizaciones
        ///DESCRIPCIÓN: Realiza la comparacion de dos data set para 
        ///verificar las diferencias de cada uno de sus campos
        ///PARAMETROS: 1.-Ds_Anterior, es el primer data a comparar
        ///            2.-Ds_Actual, segundo data set a comparar
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 17/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************


        public static String Revisar_Actualizaciones(DataSet Ds_Anterior, DataSet Ds_Actual)
        {

            String Actualizaciones = "Se actualizaron los siguientes datos: \n";
            String Dato1 = "";
            String Dato2 = "";
            int Num_Campos_Actualizados = 0;
            for (int i = 0; i < Ds_Actual.Tables[0].Columns.Count; i++)
            {
                Dato1 = Ds_Anterior.Tables[0].Rows[0].ItemArray[i].ToString();
                Dato2 = Ds_Actual.Tables[0].Rows[0].ItemArray[i].ToString();
                if (Dato1 != Dato2)
                {
                    Actualizaciones = Actualizaciones + Ds_Actual.Tables[0].Columns[i].ToString() +
                        ": " + Dato1 + " cambio a " + Dato2 + "\n";
                    Num_Campos_Actualizados = Num_Campos_Actualizados + 1;
                }
            }

            if (Num_Campos_Actualizados == 0)
            {
                Actualizaciones = "No se modificaron datos";
            }
            return Actualizaciones;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Bitacora
        ///DESCRIPCIÓN: Metodo que registra un evento en la tabla de Apl_Bitacora
        ///PARAMETROS:   1.- String Usuario: Usuario que se logueo
        ///              2.- String Accion: Accion que ejecuta el usuario (Alta, Baja, Consulta, Imprimir, Modificar, Acceso)
        ///              3.- String Recurso: Nombre del catalogo en el que se realiza la accion ejem. "Frm_Cat_Colonias"
        ///              4.- String Nombre_Recurso: Nombre del recurso del que se realizo una accion 
        ///              5.- String Descripcion: descripcion de la accion que se realizo 
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 01/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************


        public static void Alta_Bitacora(String Usuario, String Accion, String Pagina, String Nombre_Recurso, String Descripcion)
        {
            //Obtiene la cadena de inserción hacía la base de datos
            String Mi_SQL = "";         
            // Aplicamos un substring al parametro de Descripcion,para validar que no sobrepase los 4000 de longitud validos 
            if(Descripcion.Length >3999)
               Descripcion = Descripcion.Substring(0,3999);
            //se le asigna un caracter para diferenciar hasta donde termina la descripcion 
            Descripcion = Descripcion + "_";
            //Obtiene el ID con la cual se guardo los datos en la base de datos
            int Bitacora_ID = Obtener_Consecutivo(Apl_Bitacora.Campo_Bitacora_ID, Apl_Bitacora.Tabla_Apl_Bitacora);

            // Realizamos la sentencia para insertar en la tabla APL_BITACORA 
            Mi_SQL = "INSERT INTO " + Apl_Bitacora.Tabla_Apl_Bitacora + 
                " (" + Apl_Bitacora.Campo_Bitacora_ID + 
                ", " + Apl_Bitacora.Campo_Empleado_ID + 
                ", " + Apl_Bitacora.Campo_Fecha_Hora + 
                ", " + Apl_Bitacora.Campo_Accion +
                ", " + Apl_Bitacora.Campo_Recurso + 
                ", " + Apl_Bitacora.Campo_Recurso_ID + 
                ", " + Apl_Bitacora.Campo_Descripcion + ") VALUES " +
                "('" + Bitacora_ID.ToString() +
                "','" + Usuario + 
                "',SYSDATE,'" + Accion + 
                "','" + Pagina + 
                "','" + Nombre_Recurso + 
                "','" + Descripcion + "')";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        
        }//fin de Alta_Bitacora

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-Nombre de la tabla
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Obtener_Consecutivo(String Campo_ID, String Tabla)
        {
            int Consecutivo = 0;
            String Mi_Sql;
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),'00000') FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            return Consecutivo;
        }



        #endregion
    }
    
}
