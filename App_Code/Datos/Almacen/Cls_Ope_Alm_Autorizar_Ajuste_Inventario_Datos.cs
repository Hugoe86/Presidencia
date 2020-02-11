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
using Presidencia.Autorizar_Ajuste.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;


/// <summary>
/// Summary description for Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Datos
/// </summary>
/// 
namespace Presidencia.Autorizar_Ajuste.Datos
{

    public class Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Datos
    {
        

        public static DataTable Consultar_Ajustes_Inventario(Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";

            Mi_SQL = "SELECT AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste;
            Mi_SQL = Mi_SQL + ", TO_CHAR(AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Hora + ",'DD/MON/YYYY HH:MM:SS') AS " + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Hora;
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Empleados.Campo_Apellido_Paterno;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
            Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID;
            Mi_SQL = Mi_SQL + "=AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Elaboro_ID + ") AS EMPLEADO_ELABORO";
            Mi_SQL = Mi_SQL + ", AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Estatus;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + " AJUSTE";
            Mi_SQL = Mi_SQL + " WHERE AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Estatus + " IN('GENERADO','AUTORIZADO','CANCELADO')";

            Mi_SQL = Mi_SQL + " ORDER BY AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste + " DESC";

            if (Clase_Negocio.P_No_Ajuste != null)
            {
                Mi_SQL = "SELECT AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste;
                Mi_SQL = Mi_SQL + ", TO_CHAR(AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Hora + ",'DD/MON/YYYY HH:MM:SS') AS " + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Hora;
                Mi_SQL = Mi_SQL + ", AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Motivo_Ajuste_Coor;
                Mi_SQL = Mi_SQL + ", AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Motivo_Ajuste_Dir;
                Mi_SQL = Mi_SQL + ", TO_CHAR(AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Elaboro + ",'DD/MON/YYYY') AS " + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Elaboro;
                Mi_SQL = Mi_SQL + ", TO_CHAR(AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Autorizo + ",'DD/MON/YYYY') AS " + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Autorizo;
                Mi_SQL = Mi_SQL + ", TO_CHAR(AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Rechazo + ",'DD/MON/YYYY') AS " + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Rechazo;
                Mi_SQL = Mi_SQL + ", AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + "=AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Elaboro_ID + ") AS " + Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Elaboro_ID;
                Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + "=AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Autorizo_ID + ") AS " + Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Autorizo_ID;
                Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + "=AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Rechazo_ID + ") AS " + Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Rechazo_ID;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock + " AJUSTE";
                Mi_SQL = Mi_SQL + " WHERE AJUSTE." + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste + "='" + Clase_Negocio.P_No_Ajuste.Trim()+ "'";
            }

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static DataTable Consultar_Detalle_Ajustes(Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            Mi_SQL = "SELECT DET." + Ope_Alm_Ajustes_Detalles.Campo_Producto_ID;
            Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Com_Productos.Campo_Clave;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Campo_Producto_ID;
            Mi_SQL = Mi_SQL + "=DET." + Ope_Alm_Ajustes_Detalles.Campo_Producto_ID;
            Mi_SQL = Mi_SQL + ") AS CLAVE";
            Mi_SQL = Mi_SQL + ", DET." + Ope_Alm_Ajustes_Detalles.Campo_Nombre_Descipcion;
            Mi_SQL = Mi_SQL + ", DET." + Ope_Alm_Ajustes_Detalles.Campo_Existencia_Sistema;
            Mi_SQL = Mi_SQL + ", DET." + Ope_Alm_Ajustes_Detalles.Campo_Conteo_Fisico;
            Mi_SQL = Mi_SQL + ", DET." + Ope_Alm_Ajustes_Detalles.Campo_Diferencia;
            Mi_SQL = Mi_SQL + ", DET." + Ope_Alm_Ajustes_Detalles.Campo_Tipo_Movimiento;
            Mi_SQL = Mi_SQL + ", DET." + Ope_Alm_Ajustes_Detalles.Campo_Importe_Diferencia;
            Mi_SQL = Mi_SQL + ", DET." + Ope_Alm_Ajustes_Detalles.Campo_Precio_Promedio;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen + " DET";
            Mi_SQL = Mi_SQL + " WHERE DET." + Ope_Alm_Ajustes_Detalles.Campo_No_Ajuste;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Ajuste + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        }


        public static bool Modificar_Ajuste_Inventario(Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio Clase_Negocio)
        {
            String Mi_SQL = "";
            bool Operacion_Realizada = false;
            try
            {
                Mi_SQL = "UPDATE " + Ope_Alm_Ajustes_Inv_Stock.Tabla_Ope_Alm_Ajustes_Inv_Stock;
                Mi_SQL = Mi_SQL + " SET " + Ope_Alm_Ajustes_Inv_Stock.Campo_Motivo_Ajuste_Dir;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Motivo_Ajuste_Dir + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Alm_Ajustes_Inv_Stock.Campo_Estatus;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Estatus.Trim() + "'";

                if (Clase_Negocio.P_Empleado_Autorizo_ID != null)
                {
                    Mi_SQL = Mi_SQL + ", " + Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Autorizo_ID;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Empleado_Autorizo_ID.Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Autorizo;
                    Mi_SQL = Mi_SQL + "=SYSDATE";

                }

                if (Clase_Negocio.P_Empleado_Rechazo_ID != null)
                {
                    Mi_SQL = Mi_SQL + ", " + Ope_Alm_Ajustes_Inv_Stock.Campo_Empleado_Rechazo_ID;
                    Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Empleado_Rechazo_ID.Trim() + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Alm_Ajustes_Inv_Stock.Campo_Fecha_Rechazo;
                    Mi_SQL = Mi_SQL + "=SYSDATE";


                }

                Mi_SQL = Mi_SQL + " WHERE " + Ope_Alm_Ajustes_Inv_Stock.Campo_No_Ajuste;
                Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_No_Ajuste.Trim() + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Operacion_Realizada = true;
            }
            catch
            {
                Operacion_Realizada = false;
            }

            return Operacion_Realizada;
        }
    }
}//Fin del Namespace