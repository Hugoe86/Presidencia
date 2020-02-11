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
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Empleados.Negocios;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using Presidencia.Vacaciones_Empleado.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.DateDiff;
using Presidencia.Calculo_Percepciones.Negocio;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Sindicatos.Negocios;
using System.Text;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;

namespace Presidencia.Empleados.Datos
{
    public class Cls_Cat_Empleados_Datos
    {
        #region (Metodos)

        #region (Operaciones [Alta - Actualizar - Eliminar])
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Empleado
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Empleado en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Empleado(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL;                              //Obtiene la cadena de inserción hacía la base de datos
            Object Empleado_ID;                         //Obtiene el ID con la cual se guardo los datos en la base de datos
            Object No_Movimiento = null;                //Obtiene el ID con la cual se guardo los datos en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                //Consulta para la obtención del último ID dado de alta en el  catálogo de empleados
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Empleados.Campo_Empleado_ID + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                Comando_SQL.CommandText = Mi_SQL; //Realiza la ejecuón de la obtención del ID del empleado
                Empleado_ID = Comando_SQL.ExecuteScalar();

                //Valida si el ID es nulo para asignarle automaticamente el primer registro
                if (Convert.IsDBNull(Empleado_ID))
                {
                    Datos.P_Empleado_ID = "0000000001";
                }
                //Si no esta vacio el registro entonces al registro que se obtenga se le suma 1 para poder obtener el último registro
                else
                {
                    Datos.P_Empleado_ID = String.Format("{0:0000000000}", Convert.ToInt32(Empleado_ID) + 1);
                }
                //Consulta para la inserción del Empleado con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_Empleados.Tabla_Cat_Empleados + " (";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Empleado_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Area_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Contrato_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Puesto_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Escolaridad_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Sindicato_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Turno_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Zona_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Trabajador_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Rol_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Empleado + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Password + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Paterno + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Materno + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Calle + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Colonia + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Codigo_Postal + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Ciudad + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Estado + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Telefono_Casa + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Telefono_Oficina + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Extension + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fax + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Celular + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Nextel + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Correo_Electronico + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Sexo + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Nacimiento + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_RFC + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_CURP + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Ruta_Foto + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Nombre_Foto + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_IMSS + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Forma_Pago + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Cuenta_Bancaria + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Inicio + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Finiquito + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Termino_Contrato + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Baja + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Salario_Diario + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Salario_Diario_Integrado + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Lunes + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Martes + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Miercoles + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Jueves + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Viernes + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Sabado + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Domingo + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Nomina_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Terceros_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Licencia_Manejo + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Banco_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Reloj_Checador + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Tarjeta + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Seguro + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Beneficiario + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Programa_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Area_Responsable_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Partida_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Empleado + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Confronto + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Aplica_ISSEG + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Cuenta_Contable_ID + ") VALUES (";

                Mi_SQL = Mi_SQL + "'" + Datos.P_Empleado_ID + "', ";

                if (Datos.P_Area_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Area_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Dependencia_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Dependencia_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (Datos.P_Tipo_Contrato_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Tipo_Contrato_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Puesto_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Puesto_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Escolaridad_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Escolaridad_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                ;
                if (Datos.P_Sindicado_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Sindicado_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Turno_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Turno_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Zona_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Zona_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Tipo_Trabajador_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Tipo_Trabajador_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Rol_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Rol_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_No_Empleado != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_No_Empleado + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Password != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Password + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Apellido_Paterno != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Apellido_Paterno + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Apelldo_Materno != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Apelldo_Materno + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Nombre != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Calle != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Calle + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Colonia != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Colonia + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Codigo_Postal > 0)
                {
                    Mi_SQL = Mi_SQL + Datos.P_Codigo_Postal + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Ciudad != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Ciudad + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Estado != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Estado + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Telefono_Casa != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Telefono_Casa + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Telefono_Oficina != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Telefono_Oficina + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Extension != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Extension + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Fax != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Fax + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Celular != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Celular + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Nextel != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Nextel + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Correo_Electronico != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Correo_Electronico + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Sexo != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Sexo + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Nacimiento) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Nacimiento) + "','DD/MM/YY'), ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_RFC != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_RFC + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_CURP != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_CURP + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Estatus != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Estatus + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Ruta_Foto != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Ruta_Foto + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Nombre_Foto != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre_Foto + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_No_IMSS != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_No_IMSS + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Forma_Pago != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Forma_Pago + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_No_Cuenta_Bancaria != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_No_Cuenta_Bancaria + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Inicio) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Inicio) + "','DD/MM/YY'), ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Tipo_Finiquito != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Tipo_Finiquito + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Termino_Contrato) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Termino_Contrato) + "','DD/MM/YY'), ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Baja) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Baja) + "','DD/MM/YY'), ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Salario_Diario > 0)
                {
                    Mi_SQL = Mi_SQL + Datos.P_Salario_Diario + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Salario_Diario_Integrado > 0)
                {
                    Mi_SQL = Mi_SQL + Datos.P_Salario_Diario_Integrado + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Lunes != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Lunes + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Martes != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Martes + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Miercoles != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Miercoles + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Jueves != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Jueves + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Viernes != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Viernes + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Sabado != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Sabado + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                if (Datos.P_Domingo != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Domingo + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                Mi_SQL = Mi_SQL + "'" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre_Usuario + "', SYSDATE, ";

                if (Datos.P_Tipo_Nomina_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Tipo_Nomina_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (Datos.P_Terceros_ID != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Terceros_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (Datos.P_No_Licencia != null)
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_No_Licencia + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Vigencia_Licencia) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Vigencia_Licencia) + "','DD/MM/YY'), ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_Banco_ID))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Banco_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_Reloj_Checador))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Reloj_Checador + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_No_Tarjeta))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_No_Tarjeta + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_No_Seguro_Poliza))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_No_Seguro_Poliza + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_Beneficiario_Seguro))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Beneficiario_Seguro + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                //-----------------  SAP Código Programático  ------------------------
                if (!String.IsNullOrEmpty(Datos.P_SAP_Fuente_Financiamiento))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_SAP_Fuente_Financiamiento + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_SAP_Programa_ID))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_SAP_Programa_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_SAP_Area_Responsable_ID))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_SAP_Area_Responsable_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_SAP_Partida_ID))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_SAP_Partida_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_SAP_Codigo_Programatico))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.P_SAP_Codigo_Programatico + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }
                //--------------------------------------------------------------------

                Mi_SQL = Mi_SQL + "'EMPLEADO', '" + Datos.P_Confronto + "', '" + Datos.P_Aplica_ISSEG + "', 'A', '" + Datos.P_Cuenta_Contable_ID + "')";

                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos  
                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos

                //Generamos el registro en la bitacora de movimientos del empleado.
                //Cls_Bitacora.Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Cat_Empleados.aspx", Datos.P_No_Empleado, Mi_SQL);

                //Guarda la ruta de los documentos anexados en la BD. 
                Guardar_Documentos_Empleado(Datos.P_Documentos_Anexos_Empleado, Datos.P_Empleado_ID);

                //Bitacora de Movimientos de los empleados
                Registro_Movimiento_Empleado(Datos);

                //Generamos los registros de alta de las percepciones y deducciones que tendrá el empleado.
                Registro_Percepciones_Deducciones_Tipo_Nomina_Sindicato(Datos.P_Dt_Sindicato_Lista_Percepciones, Datos.P_Empleado_ID, "SINDICATO");
                Registro_Percepciones_Deducciones_Tipo_Nomina_Sindicato(Datos.P_Dt_Sindicato_Lista_Deducciones, Datos.P_Empleado_ID, "SINDICATO");
                Registro_Percepciones_Deducciones_Tipo_Nomina_Sindicato(Datos.P_Dt_Tipo_Nomina_Lista_Percepciones, Datos.P_Empleado_ID, "TIPO_NOMINA");
                Registro_Percepciones_Deducciones_Tipo_Nomina_Sindicato(Datos.P_Dt_Tipo_Nomina_Lista_Deducciones, Datos.P_Empleado_ID, "TIPO_NOMINA");

                Insertar_Actualizar_Detalle_Periodo_Vacacional(Datos.P_Empleado_ID);

                Actualizar_Estatus_Puesto_Dependencia(Datos.P_Puesto_ID, Datos.P_Dependencia_ID, "OCUPADO", Datos.P_Empleado_ID, 0, Datos.P_Clave);
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Empleado
        /// DESCRIPCION : Modifica los datos del Empleado con lo que fueron introducidos por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán modificados en la base de datos
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Modificar_Empleado(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos
            String Puesto_ID = String.Empty;
            String Dependencia_ID = String.Empty;
            Boolean Change_Tipo_Nomina = false;
            Boolean Change_Sindicato_ID = false;
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

            try
            {
                Puesto_ID = Obtener_Puesto_ID(Datos.P_Empleado_ID);
                Dependencia_ID = Obtener_Dependencia_ID(Datos.P_Empleado_ID);

                INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Datos.P_Empleado_ID);

                //Consulta para la modificación del Día Festivo con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_Empleados.Tabla_Cat_Empleados + " SET ";

                if (!String.IsNullOrEmpty(Datos.P_Confronto))
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Confronto + " = '" + Datos.P_Confronto + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Confronto + " = NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_Cuenta_Contable_ID))
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Cuenta_Contable_ID + " = '" + Datos.P_Cuenta_Contable_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Cuenta_Contable_ID + " = NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_Aplica_ISSEG))
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Aplica_ISSEG + " = '" + Datos.P_Aplica_ISSEG + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Aplica_ISSEG + " = NULL, ";
                }

                if (Datos.P_Area_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Area_ID + " = '" + Datos.P_Area_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Area_ID + " = NULL, ";
                }

                if (Datos.P_Dependencia_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Dependencia_ID + " = NULL, ";
                }
                if (Datos.P_Tipo_Contrato_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Contrato_ID + " = '" + Datos.P_Tipo_Contrato_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Contrato_ID + " = NULL, ";
                }
                if (Datos.P_Puesto_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Puesto_ID + " = '" + Datos.P_Puesto_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Puesto_ID + " = NULL, ";
                }
                if (Datos.P_Escolaridad_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Escolaridad_ID + " = '" + Datos.P_Escolaridad_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Escolaridad_ID + " = NULL, ";
                }

                if (Datos.P_Sindicado_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Sindicato_ID + " = '" + Datos.P_Sindicado_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Sindicato_ID + " = NULL, ";
                }

                if (Datos.P_Turno_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Turno_ID + " = '" + Datos.P_Turno_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Turno_ID + " = NULL, ";
                }
                if (Datos.P_Zona_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Zona_ID + " = '" + Datos.P_Zona_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Zona_ID + " = NULL, ";
                }

                if (Datos.P_Tipo_Trabajador_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Trabajador_ID + " = '" + Datos.P_Tipo_Trabajador_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Trabajador_ID + " = NULL, ";
                }

                if (Datos.P_Rol_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Rol_ID + " = '" + Datos.P_Rol_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Rol_ID + " = NULL, ";
                }

                if (Datos.P_Tipo_Nomina_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Nomina_ID + " = '" + Datos.P_Tipo_Nomina_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Nomina_ID + " = NULL, ";
                }

                if (Datos.P_Terceros_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Terceros_ID + " = '" + Datos.P_Terceros_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Terceros_ID + " = NULL, ";
                }

                if (Datos.P_Banco_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Banco_ID + " = '" + Datos.P_Banco_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Banco_ID + " = NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_No_Seguro_Poliza))
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Seguro + " = '" + Datos.P_No_Seguro_Poliza + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Seguro + " = NULL, ";
                }

                if (!String.IsNullOrEmpty(Datos.P_Beneficiario_Seguro))
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Beneficiario + " = '" + Datos.P_Beneficiario_Seguro + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Beneficiario + " = NULL, ";
                }
                //--------------------  SAP Código Programático  -----------------------------
                if (Datos.P_SAP_Fuente_Financiamiento != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID + " = '" + Datos.P_SAP_Fuente_Financiamiento + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID + " = NULL, ";
                }

                if (Datos.P_SAP_Programa_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Programa_ID + " = '" + Datos.P_SAP_Programa_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Programa_ID + " = NULL, ";
                }

                if (Datos.P_SAP_Area_Responsable_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Area_Responsable_ID + " = '" + Datos.P_SAP_Area_Responsable_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Area_Responsable_ID + " = NULL, ";
                }

                if (Datos.P_SAP_Partida_ID != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Partida_ID + " = '" + Datos.P_SAP_Partida_ID + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Partida_ID + " = NULL, ";
                }

                if (Datos.P_SAP_Codigo_Programatico != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Codigo_Programatico + " = '" + Datos.P_SAP_Codigo_Programatico + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_SAP_Codigo_Programatico + " = NULL, ";
                }
                //----------------------------------------------------------------------------

                if (Datos.P_Reloj_Checador != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Reloj_Checador + " = '" + Datos.P_Reloj_Checador + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Reloj_Checador + " = NULL, ";
                }

                if (Datos.P_No_Empleado != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Empleado + " = '" + Datos.P_No_Empleado + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Empleado + " = NULL, ";
                }

                if (Datos.P_Password != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Password + " = '" + Datos.P_Password + "', ";
                }
                if (Datos.P_Apellido_Paterno != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Paterno + " = '" + Datos.P_Apellido_Paterno + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Paterno + " = NULL, ";
                }

                if (Datos.P_Apelldo_Materno != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Materno + " = '" + Datos.P_Apelldo_Materno + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Materno + " = NULL, ";
                }

                if (Datos.P_Nombre != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Nombre + " = '" + Datos.P_Nombre + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Nombre + " = NULL, ";
                }

                if (Datos.P_Calle != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Calle + " = '" + Datos.P_Calle + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Calle + " = NULL, ";
                }

                if (Datos.P_Colonia != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Colonia + " = '" + Datos.P_Colonia + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Colonia + " = NULL, ";
                }

                if (Datos.P_Codigo_Postal > 0)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Codigo_Postal + " = " + Datos.P_Codigo_Postal + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Codigo_Postal + " = NULL, ";
                }

                if (Datos.P_Ciudad != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Ciudad + " = '" + Datos.P_Ciudad + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Ciudad + " = NULL, ";
                }

                if (Datos.P_Estado != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Estado + " = '" + Datos.P_Estado + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Estado + " = NULL, ";
                }

                if (Datos.P_Telefono_Casa != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Telefono_Casa + " = '" + Datos.P_Telefono_Casa + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Telefono_Casa + " = NULL, ";
                }

                if (Datos.P_Telefono_Oficina != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Telefono_Oficina + " = '" + Datos.P_Telefono_Oficina + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Telefono_Oficina + " = NULL, ";
                }

                if (Datos.P_Extension != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Extension + " = '" + Datos.P_Extension + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Extension + " = NULL, ";
                }

                if (Datos.P_Fax != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fax + " = '" + Datos.P_Fax + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fax + " = NULL, ";
                }

                if (Datos.P_Celular != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Celular + " = '" + Datos.P_Celular + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Celular + " = NULL, ";
                }

                if (Datos.P_Nextel != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Nextel + " = '" + Datos.P_Nextel + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Nextel + " = NULL, ";
                }

                if (Datos.P_Correo_Electronico != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Correo_Electronico + " = '" + Datos.P_Correo_Electronico + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Correo_Electronico + " = NULL, ";
                }

                if (Datos.P_Sexo != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Sexo + " = '" + Datos.P_Sexo + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Sexo + " = NULL, ";
                }

                if (String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Nacimiento) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Nacimiento + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Nacimiento) + "','DD/MM/YY'), ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Nacimiento + " = NULL, ";
                }

                if (Datos.P_RFC != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_RFC + " = '" + Datos.P_RFC + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_RFC + " = NULL, ";
                }

                if (Datos.P_CURP != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_CURP + " = '" + Datos.P_CURP + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_CURP + " = NULL, ";
                }

                if (Datos.P_Estatus != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Estatus + " = 'ACTIVO', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Estatus + " = NULL, ";
                }

                if (Datos.P_Ruta_Foto != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Ruta_Foto + " = '" + Datos.P_Ruta_Foto + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Ruta_Foto + " = NULL, ";
                }

                if (Datos.P_Nombre_Foto != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Nombre_Foto + " = '" + Datos.P_Nombre_Foto + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Nombre_Foto + " = NULL, ";
                }

                if (Datos.P_No_IMSS != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_IMSS + " = '" + Datos.P_No_IMSS + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_IMSS + " = NULL, ";
                }

                if (Datos.P_Forma_Pago != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Forma_Pago + " = '" + Datos.P_Forma_Pago + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Forma_Pago + " = NULL, ";
                }

                if (Datos.P_No_Cuenta_Bancaria != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Cuenta_Bancaria + " = '" + Datos.P_No_Cuenta_Bancaria + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Cuenta_Bancaria + " = NULL, ";
                }

                if (Datos.P_No_Tarjeta != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Tarjeta + " = '" + Datos.P_No_Tarjeta + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Tarjeta + " = NULL, ";
                }

                if (String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Inicio) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Inicio + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Inicio) + "','DD/MM/YY'), ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Inicio + " = NULL, ";
                }

                if (Datos.P_Tipo_Finiquito != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Finiquito + " = '" + Datos.P_Tipo_Finiquito + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Finiquito + " = NULL, ";
                }

                if (String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Termino_Contrato) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Termino_Contrato + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Termino_Contrato) + "','DD/MM/YY'), ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Termino_Contrato + " = NULL, ";
                }

                if (String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Baja) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Baja + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Baja) + "','DD/MM/YY'), ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Baja + " = NULL, ";
                }

                if (Datos.P_Salario_Diario > 0)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Salario_Diario + " = " + Datos.P_Salario_Diario + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Salario_Diario + " = NULL, ";
                }

                if (Datos.P_Salario_Diario_Integrado > 0)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Salario_Diario_Integrado + " = " + Datos.P_Salario_Diario_Integrado + ", ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Salario_Diario_Integrado + " = NULL, ";
                }

                if (Datos.P_Lunes != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Lunes + " = '" + Datos.P_Lunes + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Lunes + " = NULL, ";
                }

                if (Datos.P_Martes != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Martes + " = '" + Datos.P_Martes + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Martes + " = NULL, ";
                }

                if (Datos.P_Miercoles != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Miercoles + " = '" + Datos.P_Miercoles + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Miercoles + " = NULL, ";
                }

                if (Datos.P_Jueves != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Jueves + " = '" + Datos.P_Jueves + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Jueves + " = NULL, ";
                }

                if (Datos.P_Viernes != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Viernes + " = '" + Datos.P_Viernes + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Viernes + " = NULL, ";
                }

                if (Datos.P_Sabado != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Sabado + " = '" + Datos.P_Sabado + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Sabado + " = NULL, ";
                }

                if (Datos.P_Domingo != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Domingo + " = '" + Datos.P_Domingo + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Domingo + " = NULL, ";
                }

                if (Datos.P_No_Licencia != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Licencia_Manejo+ " = '" + Datos.P_No_Licencia + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Licencia_Manejo + " = NULL, ";
                }

                if (String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Vigencia_Licencia) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Vigencia_Licencia) + "','DD/MM/YY'), ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia + " = NULL, ";
                }

                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Movimiento + " = 'C', ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";

                Change_Tipo_Nomina = Cambio_Tipo_Nomina(Datos.P_No_Empleado, Datos.P_Tipo_Nomina_ID);
                Change_Sindicato_ID = Cambio_Sindicato(Datos.P_No_Empleado, Datos.P_Sindicado_ID);

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Registramos el movimiento del empleado en la Bitacara.
                //Cls_Bitacora.Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Cat_Empleados.aspx", Datos.P_No_Empleado, Mi_SQL);

                //Guarda los documentos del empleado.
                Guardar_Documentos_Empleado(Datos.P_Documentos_Anexos_Empleado, Datos.P_Empleado_ID);



                //if (INF_EMPLEADO is Cls_Cat_Empleados_Negocios)
                //{
                //    //El moviemto del empleado solo se dará de alta cuando el empleado este con estatus de inactivo.
                //    if (INF_EMPLEADO.P_Estatus.Equals("INACTIVO"))
                //    {
                //        //obtenemos el numero de moviento consecutivo.
                //        Registro_Movimiento_Empleado(Datos);
                //    }
                //}

                if (Change_Tipo_Nomina)
                {
                    //Removemos las percepciones deducciones del empleado.
                    Mi_SQL = "DELETE FROM " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                             " WHERE " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    Mi_SQL = Mi_SQL + " AND ";
                    Mi_SQL = Mi_SQL + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + "='TIPO_NOMINA'";

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    Registro_Percepciones_Deducciones_Tipo_Nomina_Sindicato(Datos.P_Dt_Tipo_Nomina_Lista_Percepciones, Datos.P_Empleado_ID, "TIPO_NOMINA");
                    Registro_Percepciones_Deducciones_Tipo_Nomina_Sindicato(Datos.P_Dt_Tipo_Nomina_Lista_Deducciones, Datos.P_Empleado_ID, "TIPO_NOMINA");
                }

                if (Change_Sindicato_ID)
                {
                    //Removemos las percepciones deducciones del empleado.
                    Mi_SQL = "DELETE FROM " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                             " WHERE " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    Mi_SQL = Mi_SQL + " AND ";
                    Mi_SQL = Mi_SQL + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + "='SINDICATO'";

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    //Generamos los registros de alta de las percepciones y deducciones que tendrá el empleado.
                    Registro_Percepciones_Deducciones_Tipo_Nomina_Sindicato(Datos.P_Dt_Sindicato_Lista_Percepciones, Datos.P_Empleado_ID, "SINDICATO");
                    Registro_Percepciones_Deducciones_Tipo_Nomina_Sindicato(Datos.P_Dt_Sindicato_Lista_Deducciones, Datos.P_Empleado_ID, "SINDICATO");
                }

                if (!String.IsNullOrEmpty(Datos.P_Clave))
                {
                    Actualizar_Estatus_Puesto_Dependencia(Datos.P_Puesto_ID, Puesto_ID, Datos.P_Dependencia_ID, Dependencia_ID,
                        Datos.P_Empleado_ID, Datos.P_Clave);
                }

                //Insertar un movimiento si hubo una promoción.
                Isertar_Promocion(INF_EMPLEADO, Datos);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Empleado
        /// DESCRIPCION : Elimina el Empleado que fue seleccionado por el usuario de la BD
        /// PARAMETROS  : Datos: Obtiene que Empleado desea eliminar de la BD
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Eliminar_Empleado(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del Empleado
            try
            {
                Mi_SQL = "UPDATE " + Cat_Empleados.Tabla_Cat_Empleados + " SET ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Baja + "='" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Baja) + "', ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Finiquito + "='" + Datos.P_Tipo_Finiquito + "', ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "' WHERE ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Removemos las percepciones deducciones del empleado.
                Mi_SQL = "DELETE FROM " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                         " WHERE " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Se hace el registro de movimiento del empleado.
                Registro_Movimiento_Baja_Empleado(Datos);

                Actualizar_Estatus_Puesto_Dependencia(Datos.P_Puesto_ID, Datos.P_Dependencia_ID, "DISPONIBLE",
                    Datos.P_Empleado_ID, 1, Datos.P_Clave);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Registro_Percepciones_Deducciones_Tipo_Nomina_Sindicato
        /// DESCRIPCION : Registra las percepciones o deducciones que aplicaran para él 
        ///               empleado.
        /// 
        /// PARAMETROS  : Dt_Datos.- Percepciones on Deducciones a aplicar al empleado. 
        ///               Empleado_ID.- Empleado al que se le aplicaran las percepciones y/o
        ///                             Deducciones.
        ///               Concepto.- Concepto al que pertenece la percepción o deducción.                 
        /// 
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 05/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private static void Registro_Percepciones_Deducciones_Tipo_Nomina_Sindicato(DataTable Dt_Percepciones_Deducciones,
            String Empleado_ID, String Concepto)
        {
            StringBuilder Mi_SQL = new StringBuilder();           //Obtiene la cadena de inserción hacía la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;
            DataTable Dt_Periodos = null;
            String Nomina_ID = String.Empty;
            String No_Nomina = String.Empty;

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }

            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Dt_Periodos = Consultar_Periodo_Actual_Fecha();

                if (Dt_Periodos is DataTable)
                {
                    if (Dt_Periodos.Rows.Count > 0)
                    {
                        foreach (DataRow PERIODO in Dt_Periodos.Rows)
                        {
                            if (PERIODO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(PERIODO[Cat_Nom_Nominas_Detalles.Campo_Nomina_ID].ToString().Trim()))
                                    Nomina_ID = PERIODO[Cat_Nom_Nominas_Detalles.Campo_Nomina_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(PERIODO[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString().Trim()))
                                    No_Nomina = PERIODO[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString().Trim();
                            }
                        }
                    }
                }

                if (Dt_Percepciones_Deducciones is DataTable)
                {
                    if (Dt_Percepciones_Deducciones.Rows.Count > 0)
                    {
                        foreach (DataRow PERCEPCION_DEDUCCION in Dt_Percepciones_Deducciones.Rows)
                        {
                            if (PERCEPCION_DEDUCCION is DataRow)
                            {
                                Mi_SQL = new StringBuilder();
                                Mi_SQL.Append("INSERT INTO ");
                                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det);
                                Mi_SQL.Append(" (");
                                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + ", ");
                                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID + ", ");
                                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + ", ");
                                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad);

                                if (Concepto.Trim().ToUpper().Equals("SINDICATO"))
                                {
                                    Mi_SQL.Append(", " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Nomina_ID);
                                    Mi_SQL.Append(", " + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_No_Nomina);
                                }

                                Mi_SQL.Append(") VALUES(");
                                Mi_SQL.Append("'" + Empleado_ID + "', ");
                                Mi_SQL.Append("'" + PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString().Trim() + "', ");
                                Mi_SQL.Append("'" + Concepto + "', ");
                                Mi_SQL.Append(Convert.ToDouble((!String.IsNullOrEmpty(
                                    PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim())) ?
                                    PERCEPCION_DEDUCCION[Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Cantidad].ToString().Trim() : "0.00"));

                                if (Concepto.Trim().ToUpper().Equals("SINDICATO"))
                                {
                                    Mi_SQL.Append(", '" + Nomina_ID + "'");
                                    Mi_SQL.Append(", " + No_Nomina);
                                }

                                Mi_SQL.Append(")");

                                Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos            
                            }
                        }

                        Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
                    }
                }
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Registro_Movimiento_Empleado
        /// DESCRIPCION : Registra los movimientos que a tenido el empleado [Altas, Bajas o Reactivaciones]. 
        /// 
        /// PARAMETROS  : Datos: Obtiene que Empleado desea eliminar de la BD
        /// 
        /// CREO        : Juan Alberto Hernández Negrete.
        /// FECHA_CREO  : 05/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private static void Registro_Movimiento_Empleado(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL;           //Obtiene la cadena de inserción hacía la base de datos
            Object No_Movimiento = null;//Obtiene el ID con la cual se guardo los datos en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos


            try
            {

                //obtenemos el numero de moviento consecutivo.
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Emp_Movimientos_Det.Campo_No_Movimiento + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det;
                No_Movimiento = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(No_Movimiento))
                {
                    Datos.P_No_Movimiento = "00001";
                }
                else
                {
                    Datos.P_No_Movimiento = String.Format("{0:00000}", Convert.ToInt32(No_Movimiento) + 1);
                }

                Mi_SQL = "INSERT INTO " + Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + " (";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_No_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Empleado_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Puesto_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Tipo_Nomina_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Motivo_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Sueldo_Actual + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + ") VALUES(";
                Mi_SQL = Mi_SQL + "'" + Datos.P_No_Movimiento + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Empleado_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Dependencia_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Puesto_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Tipo_Nomina_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Tipo_Movimiento + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Motivo_Movimiento + "', ";
                Mi_SQL = Mi_SQL + "" + Datos.P_Sueldo_Actual + ", ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre_Usuario + "', SYSDATE)";

                //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos   
                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Registro_Movimiento_Baja_Empleado
        /// DESCRIPCION : Registra los movimientos que a tenido el empleado [Altas, Bajas o Reactivaciones]. 
        /// 
        /// PARAMETROS  : Datos: Obtiene que Empleado desea eliminar de la BD
        /// 
        /// CREO        : Juan Alberto Hernández Negrete.
        /// FECHA_CREO  : 05/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private static void Registro_Movimiento_Baja_Empleado(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL;           //Obtiene la cadena de inserción hacía la base de datos
            Object No_Movimiento = null;//Obtiene el ID con la cual se guardo los datos en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos


            try
            {

                //obtenemos el numero de moviento consecutivo.
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Emp_Movimientos_Det.Campo_No_Movimiento + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det;
                No_Movimiento = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(No_Movimiento))
                {
                    Datos.P_No_Movimiento = "00001";
                }
                else
                {
                    Datos.P_No_Movimiento = String.Format("{0:00000}", Convert.ToInt32(No_Movimiento) + 1);
                }

                Mi_SQL = "INSERT INTO " + Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + " (";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_No_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Empleado_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Puesto_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Tipo_Nomina_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Motivo_Movimiento + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Sueldo_Actual + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Aplica_Baja_Licencia + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Fecha_Inicio_Licencia + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Fecha_Termino_Licencia + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Fecha_Baja_IMSS + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Tipo_Baja + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + ") VALUES(";
                Mi_SQL = Mi_SQL + "'" + Datos.P_No_Movimiento + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Empleado_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Dependencia_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Puesto_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Tipo_Nomina_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Tipo_Movimiento + "', ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Motivo_Movimiento + "', ";
                Mi_SQL = Mi_SQL + "" + Datos.P_Sueldo_Actual + ", ";

                if (!String.IsNullOrEmpty(Datos.P_Aplica_Baja_Licencia))
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Aplica_Baja_Licencia + "', ";
                else
                    Mi_SQL = Mi_SQL + "NULL, ";

                if (!String.IsNullOrEmpty(Datos.P_Fecha_Inicio_Licencia))
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Fecha_Inicio_Licencia + "', ";
                else
                    Mi_SQL = Mi_SQL + "NULL, ";

                if (!String.IsNullOrEmpty(Datos.P_Fecha_Termino_Licencia))
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Fecha_Termino_Licencia + "', ";
                else
                    Mi_SQL = Mi_SQL + "NULL, ";

                if (!String.IsNullOrEmpty(Datos.P_Fecha_Baja_IMSS))
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Fecha_Baja_IMSS + "', ";
                else
                    Mi_SQL = Mi_SQL + "NULL, ";

                if (!String.IsNullOrEmpty(Datos.P_Tipo_Baja))
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Tipo_Baja + "', ";
                else
                    Mi_SQL = Mi_SQL + "NULL, ";

                Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre_Usuario + "', SYSDATE)";

                //OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos   
                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }

        private static void Isertar_Promocion(Cls_Cat_Empleados_Negocios Antes_Promocion, Cls_Cat_Empleados_Negocios Despues_Promocion)
        {
            try
            {
                //if (!Antes_Promocion.P_Puesto_ID.Equals(Despues_Promocion.P_Puesto_ID)) {

                if (Despues_Promocion.P_Tipo_Movimiento.Equals("PROMOCION") ||
                    Despues_Promocion.P_Tipo_Movimiento.Equals("ACTUALIZACION") ||
                    Despues_Promocion.P_Tipo_Movimiento.Equals("REINGRESO"))
                {
                    Antes_Promocion.P_Tipo_Movimiento = Despues_Promocion.P_Tipo_Movimiento;
                    Antes_Promocion.P_Sueldo_Actual = (Antes_Promocion.P_Salario_Diario * Presidencia.Utilidades_Nomina.Cls_Utlidades_Nomina.Dias_Mes_Fijo);
                    Antes_Promocion.P_Motivo_Movimiento = "NIVEL-ANTERIOR";
                    Antes_Promocion.P_Nombre_Usuario = Despues_Promocion.P_Nombre_Usuario;

                    Despues_Promocion.P_Tipo_Movimiento = Despues_Promocion.P_Tipo_Movimiento;
                    Despues_Promocion.P_Sueldo_Actual = (Despues_Promocion.P_Salario_Diario * Presidencia.Utilidades_Nomina.Cls_Utlidades_Nomina.Dias_Mes_Fijo);
                    Despues_Promocion.P_Motivo_Movimiento = "NIVEL-NUEVO";

                    Registro_Movimiento_Empleado(Antes_Promocion);//Para guardar el nivel anterior del empleado antes de ser promocionado.
                    Registro_Movimiento_Empleado(Despues_Promocion);//Para guardar el nivel actual ya con la promocion.
                }
                else if (Despues_Promocion.P_Tipo_Movimiento.Equals("ALTA"))
                {
                    Despues_Promocion.P_Tipo_Movimiento = Despues_Promocion.P_Tipo_Movimiento;
                    Despues_Promocion.P_Sueldo_Actual = (Despues_Promocion.P_Salario_Diario * Presidencia.Utilidades_Nomina.Cls_Utlidades_Nomina.Dias_Mes_Fijo);
                    Despues_Promocion.P_Motivo_Movimiento = "NIVEL-NUEVO";

                    Registro_Movimiento_Empleado(Despues_Promocion);//Para guardar el nivel actual ya con la promocion.
                }
                else {
                    Antes_Promocion.P_Tipo_Movimiento = Despues_Promocion.P_Tipo_Movimiento;
                    Antes_Promocion.P_Sueldo_Actual = (Antes_Promocion.P_Salario_Diario * Presidencia.Utilidades_Nomina.Cls_Utlidades_Nomina.Dias_Mes_Fijo);
                    Antes_Promocion.P_Motivo_Movimiento = "NIVEL-ANTERIOR";
                    Antes_Promocion.P_Nombre_Usuario = Despues_Promocion.P_Nombre_Usuario;

                    Registro_Movimiento_Empleado(Antes_Promocion);//Para guardar el nivel anterior del empleado antes de ser promocionado.
                }

                //}
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al guardar el movimiento de promocion. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

        #region (Consultas)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Empleados
        /// DESCRIPCION : Consulta todos los Empleados que estan dados de alta en la BD y que
        ///               tengan alguna similitud con lo proporcionado por el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 10-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Empleados(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL;    //Variable para la consulta para el Empleado
            Boolean Consulta; //Indica si ya fue realizado una consulta anterior
            try
            {
                Consulta = false;
                //Consulta todos los Empleados que coincidan con lo proporcionado por el usuario
                Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + ", " + Cat_Empleados.Campo_No_Empleado + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_RFC + ", " + Cat_Empleados.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + "(" + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Nombre + ") AS Empleado";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                if (!String.IsNullOrEmpty(Datos.P_Nombre))
                {
                    Mi_SQL = Mi_SQL + " WHERE (" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Materno + " LIKE UPPER ('%" + Datos.P_Nombre + "%'))";
                    Mi_SQL = Mi_SQL + " OR (" + Cat_Empleados.Campo_RFC + " LIKE UPPER ('%" + Datos.P_Nombre + "%'))";
                    Mi_SQL = Mi_SQL + " OR (" + Cat_Empleados.Campo_No_Empleado + " = '" + Datos.P_Nombre + "')";
                    Consulta = true;
                }

                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Consulta == false)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                    }
                    Consulta = true;
                }

                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Consulta == false)
                    {
                        Mi_SQL = Mi_SQL + " WHERE (" + Cat_Empleados.Campo_No_Empleado + " = '" + Datos.P_No_Empleado + "'";
                        Mi_SQL = Mi_SQL + " OR " + Cat_Empleados.Campo_No_Empleado + " = '" + String.Format("{0:0000000000}", Convert.ToInt32(Datos.P_No_Empleado)) + "'";
                        Mi_SQL = Mi_SQL + " OR " + Cat_Empleados.Campo_No_Empleado + " = '" + String.Format("{0:00000}", Convert.ToInt32(Datos.P_No_Empleado)) + "')";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " AND (" + Cat_Empleados.Campo_No_Empleado + " = '" + Datos.P_No_Empleado + "'";
                        Mi_SQL = Mi_SQL + " OR " + Cat_Empleados.Campo_No_Empleado + " = '" + String.Format("{0:00000}", Convert.ToInt32(Datos.P_No_Empleado)) + "'";
                        Mi_SQL = Mi_SQL + " OR " + Cat_Empleados.Campo_No_Empleado + " = '" + String.Format("{0:0000000000}", Convert.ToInt32(Datos.P_No_Empleado)) + "')";
                    }
                }

                if (Mi_SQL.Contains("WHERE"))
                {
                    Mi_SQL += " AND " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }
                else
                {
                    Mi_SQL += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Campo_Apellido_Paterno;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Empleados_Dependencia
        /// DESCRIPCION : Consulta a los empleados que se encuentran asignados a una 
        ///               dependencia y que estos esten activos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 01-Diciembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Empleados_Dependencia(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL; //Variable para la consulta para el Empleado

            try
            {
                //Consulta todos los Empleados que coincidan con lo proporcionado por el usuario
                Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + ", " + Cat_Empleados.Campo_No_Empleado + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_RFC + ", " + Cat_Empleados.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + "(" + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Nombre + ") AS Empleado";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "'";

                if (Datos.P_Estatus != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Campo_Estatus + " = '" + Datos.P_Estatus + "'";
                }

                if (Datos.P_Nombre != null)
                {
                    Mi_SQL = Mi_SQL + " AND (" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Materno + " LIKE '%" + Datos.P_Nombre + "%'";
                    Mi_SQL = Mi_SQL + " OR " + Cat_Empleados.Campo_No_Empleado + " = '" + Datos.P_Nombre + "')";
                }

                if (Datos.P_Empleado_ID != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                }

                if (Mi_SQL.Contains("WHERE"))
                {
                    Mi_SQL += " AND " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }
                else
                {
                    Mi_SQL += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Campo_Apellido_Paterno;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Empleado
        /// DESCRIPCION : Consulta todos los datos del Empleado que selecciono el usuario
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 11-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Empleado(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL; //Variable para la consulta para del Empleado

            try
            {
                //Consulta todos los datos datos del empleado que fue seleccionado por el usuario
                Mi_SQL = "SELECT * FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                if (Datos.P_Empleado_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID + " = '" + String.Format("{0:0000000000}", Convert.ToDouble(Datos.P_Empleado_ID)) + "'";
                }

                if (Datos.P_Nombre != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE (" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Apellido_Materno + ") LIKE '%" + Datos.P_Nombre + "%'";
                }

                if (Datos.P_RFC != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE (" + Cat_Empleados.Campo_RFC + ") LIKE '%" + Datos.P_RFC + "%'";
                }

                if (Datos.P_No_Empleado != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE (" + Cat_Empleados.Campo_No_Empleado + ") ='" + Datos.P_No_Empleado + "'";
                }

                if (Mi_SQL.Contains("WHERE"))
                {
                    Mi_SQL += " AND " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }
                else
                {
                    Mi_SQL += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Campo_Apellido_Paterno;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Usuario_Password
        /// DESCRIPCION : Consulta si el No. de Empelado y el Password estan dados de alta para
        ///               poder acceder al sistema y realizar las diferentes operaciones que tienen
        ///               asignado de acuerdo al ROL
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 11-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Usuario_Password(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL; //Variable para la consulta para del Empleado

            try
            {
                int No_Empleado = int.Parse(Datos.P_No_Empleado);
                Datos.P_No_Empleado = String.Format("{0:000000}", No_Empleado);
                //Consulta todos los datos datos del empleado que fue seleccionado por el empleado
                Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + ", " + Cat_Empleados.Campo_Rol_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_No_Empleado + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Area_ID + ", ";
                Mi_SQL = Mi_SQL + "(" + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + "||' '||" + Cat_Empleados.Campo_Nombre + ") AS Empleado";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado + "= '" + Datos.P_No_Empleado + "'";
                Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Campo_Password + "= '" + Datos.P_Password + "'";

                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Campo_Apellido_Paterno;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Id_Empleado
        /// DESCRIPCION : Consulta el ID consecutivo de la tabla de CAT_EMPLEADOS.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 26/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static String Consulta_Id_Empleado(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL;
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;
            Object Empleado_ID;

            Conexion_Base.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion_Base.Open();
            //Esta inserción se realiza sin el Ayudante de SQL y con el BeginTrans y Commit para proteger la información
            //el ayudante de SQL solo debe usarse cuando solo se afecte una tabla o para movimientos que NO son críticos
            Transaccion_SQL = Conexion_Base.BeginTransaction();
            Comando_SQL.Connection = Conexion_Base;
            Comando_SQL.Transaction = Transaccion_SQL;

            try
            {
                //COnsulta para el ID de la region
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Empleados.Campo_Empleado_ID + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                Comando_SQL.CommandText = Mi_SQL; //Realiza la ejecuón de la obtención del ID del empleado
                Empleado_ID = Comando_SQL.ExecuteScalar();
                Transaccion_SQL.Commit();

                //Verificar si no es nulo
                //Valida si el ID es nulo para asignarle automaticamente el primer registro
                if (Convert.IsDBNull(Empleado_ID))
                {
                    Datos.P_Empleado_ID = "0000000001";
                }
                //Si no esta vacio el registro entonces al registro que se obtenga se le suma 1 para poder obtener el último registro
                else
                {
                    Datos.P_Empleado_ID = String.Format("{0:0000000000}", Convert.ToInt32(Empleado_ID) + 1);
                }
                Conexion_Base.Close();
                //entregar resultado
                return Datos.P_Empleado_ID;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Guardar_Documentos_Empleado
        /// DESCRIPCION : Da de Alta los documentos del empleado.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 26/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Guardar_Documentos_Empleado(DataTable Documentos_Anexos_Empleado, String Empleado_ID)
        {
            String Mi_ORACLE;
            Object Requisitos_Empleado_ID;
            String _Requisitos_Empleado_ID;

            try
            {
                if (Documentos_Anexos_Empleado != null)
                {
                    foreach (DataRow Documento in Documentos_Anexos_Empleado.Rows)
                    {
                        if (Consultar_Requisitos_Empleados_Entregados_Por_ID(Empleado_ID, Documento[Ope_Nom_Requisitos_Empleado.Campo_Requisitos_ID].ToString()).Rows.Count > 0)
                        {
                            Mi_ORACLE = "UPDATE " + Ope_Nom_Requisitos_Empleado.Tabla_Ope_Nom_Requisitos_Empleado + " SET " +
                                        Ope_Nom_Requisitos_Empleado.Campo_Usuario_Modifico + "='" + Cls_Sessiones.Nombre_Empleado + "', " +
                                        Ope_Nom_Requisitos_Empleado.Campo_Fecha_Modifico + "= SYSDATE" +
                                        " WHERE " +
                                        Ope_Nom_Requisitos_Empleado.Campo_Requisitos_ID + "='" + Documento[Ope_Nom_Requisitos_Empleado.Campo_Requisitos_ID].ToString() + "'" +
                                        " AND " + Ope_Nom_Requisitos_Empleado.Campo_Empleado_ID + "='" + Empleado_ID + "'";

                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_ORACLE);
                        }
                        else
                        {
                            //Consulta para el ID de la region
                            Mi_ORACLE = "SELECT NVL(MAX(" + Ope_Nom_Requisitos_Empleado.Campo_Requisitos_Empleado_ID + "),'0000000000') ";
                            Mi_ORACLE = Mi_ORACLE + "FROM " + Ope_Nom_Requisitos_Empleado.Tabla_Ope_Nom_Requisitos_Empleado;
                            Requisitos_Empleado_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_ORACLE);

                            //Verificar si no es nulo
                            //Valida si el ID es nulo para asignarle automaticamente el primer registro
                            if (Convert.IsDBNull(Requisitos_Empleado_ID))
                            {
                                _Requisitos_Empleado_ID = "0000000001";
                            }
                            //Si no esta vacio el registro entonces al registro que se obtenga se le suma 1 para poder obtener el último registro
                            else
                            {
                                _Requisitos_Empleado_ID = String.Format("{0:0000000000}", Convert.ToInt32(Requisitos_Empleado_ID) + 1);
                            }

                            Mi_ORACLE = "INSERT INTO " + Ope_Nom_Requisitos_Empleado.Tabla_Ope_Nom_Requisitos_Empleado + " (" +
                                Ope_Nom_Requisitos_Empleado.Campo_Requisitos_Empleado_ID + ", " +
                                Ope_Nom_Requisitos_Empleado.Campo_Requisitos_ID + ", " +
                                Ope_Nom_Requisitos_Empleado.Campo_Empleado_ID + ", " +
                                Ope_Nom_Requisitos_Empleado.Campo_Nombre + ", " +
                                Ope_Nom_Requisitos_Empleado.Campo_Ruta_Documento + ", " +
                                Ope_Nom_Requisitos_Empleado.Campo_Entregado + ", " +
                                Ope_Nom_Requisitos_Empleado.Campo_Usuario_Creo + ", " +
                                Ope_Nom_Requisitos_Empleado.Campo_Fecha_Creo + ") " +
                                "VALUES('" +
                                 _Requisitos_Empleado_ID + "', '" +
                                 Documento[Ope_Nom_Requisitos_Empleado.Campo_Requisitos_ID].ToString() + "', '" +
                                 Empleado_ID + "', '" +
                                 Documento[Ope_Nom_Requisitos_Empleado.Campo_Nombre].ToString() + "', '" +
                                 Documento[Ope_Nom_Requisitos_Empleado.Campo_Ruta_Documento].ToString() + "', '" +
                                 Documento[Ope_Nom_Requisitos_Empleado.Campo_Entregado].ToString() + "', '" +
                                 Cls_Sessiones.Nombre_Empleado + "', SYSDATE )";

                            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_ORACLE);
                        }//Validacion si existe el requisito.
                    }//For que recorre el DataTable con los Documentos Anexos
                }//Validacion para revizar que el DataTable no se Nulo
            }//Fin del Try
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Requisitos_Empleados
        /// DESCRIPCION : Consulta de la base de Datos todos los requisitos que el empleado
        /// tiene que entregar, para poder ser dado de alta en nomina.
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 27/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Requisitos_Empleados()
        {
            DataTable tabla;
            try
            {
                String Mi_SQL = "SELECT " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados + ".* " +
                    " FROM " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados +
                    " WHERE " + Cat_Nom_Requisitos_Empleados.Campo_Estatus + "='ACTIVO'" +
                    " ORDER BY " + Cat_Nom_Requisitos_Empleados.Campo_Requisito_ID;

                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset == null)
                {
                    tabla = new DataTable();
                }
                else
                {
                    tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return tabla;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Requisitos_Empleados_Entregados
        /// DESCRIPCION : Consulta de la base de Datos todos los requisitos del empleados
        /// que ya fueron entregados.
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 19/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Requisitos_Empleados_Entregados(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL = "SELECT " + Ope_Nom_Requisitos_Empleado.Tabla_Ope_Nom_Requisitos_Empleado + ".* " +
                " FROM " + Ope_Nom_Requisitos_Empleado.Tabla_Ope_Nom_Requisitos_Empleado +
                " WHERE " + Ope_Nom_Requisitos_Empleado.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'" +
                " ORDER BY " + Ope_Nom_Requisitos_Empleado.Campo_Requisitos_ID;

            DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            DataTable tabla;
            if (dataset == null)
            {
                tabla = new DataTable();
            }
            else
            {
                tabla = dataset.Tables[0];
            }
            return tabla;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Requisitos_Empleados_Entregados_Por_ID
        /// DESCRIPCION : Consulta si el requisito con el Requisito_ID que corresponde al empleado con el ID 
        /// pasado ya existe. 
        /// PARAMETROS:
        ///             Empleado_ID: Empleado al cual pertenece dicho requisito.
        ///             Requisito_ID: ID del documento.
        ///             
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 27/Octubre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Requisitos_Empleados_Entregados_Por_ID(String Empleado_ID, String Requisito_ID)
        {
            String Mi_SQL = "SELECT " + Ope_Nom_Requisitos_Empleado.Tabla_Ope_Nom_Requisitos_Empleado + ".* " +
                " FROM " + Ope_Nom_Requisitos_Empleado.Tabla_Ope_Nom_Requisitos_Empleado +
                " WHERE " + Ope_Nom_Requisitos_Empleado.Campo_Empleado_ID + "='" + Empleado_ID + "'" +
                " AND " + Ope_Nom_Requisitos_Empleado.Campo_Requisitos_ID + "='" + Requisito_ID + "'" +
                " ORDER BY " + Ope_Nom_Requisitos_Empleado.Campo_Requisitos_ID;

            DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            DataTable tabla;
            if (dataset == null)
            {
                tabla = new DataTable();
            }
            else
            {
                tabla = dataset.Tables[0];
            }
            return tabla;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Requisitos_Empleados_Entregados_Por_ID
        /// DESCRIPCION : Consultar los tipos de Nomina que existen actualmente.          
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 5/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Tipos_Nomina()
        {
            String Mi_SQL = "SELECT " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + ".* " +
                " FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas;

            DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            DataTable tabla;
            if (dataset == null)
            {
                tabla = new DataTable();
            }
            else
            {
                tabla = dataset.Tables[0];
            }
            return tabla;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Terceros
        /// DESCRIPCION : Consultar las Retenciones a Terceros que existen actualmente.          
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 5/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Terceros()
        {
            String Mi_SQL = "SELECT " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + ".* " +
                " FROM " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros;

            DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            DataTable tabla;
            if (dataset == null)
            {
                tabla = new DataTable();
            }
            else
            {
                tabla = dataset.Tables[0];
            }
            return tabla;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Puestos_Empleados
        /// DESCRIPCION : Consultar los puestos disponibles ene le sistema.        
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 10/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Puestos_Empleados(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL = null; //Variable para la consulta de los Puestos
            DataTable Dt_Puestos = null; // Tabla que se va a retornar
            try
            {
                Mi_SQL = "SELECT " + Cat_Puestos.Tabla_Cat_Puestos + ".* " +
                    " FROM " + Cat_Puestos.Tabla_Cat_Puestos + " WHERE " + Cat_Puestos.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'" +
                     " ORDER BY " + Cat_Puestos.Campo_Nombre;

                Dt_Puestos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Puestos;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Empleados_General
        /// DESCRIPCION : Consulta los empelados del sistema.        
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 13/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Empleados_General(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_Oracle = "";//Variable que alamcenara la consulta.
            DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.
            try
            {
                Mi_Oracle = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + ".*, ";

                Mi_Oracle = Mi_Oracle + "(" + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_Oracle = Mi_Oracle + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_Oracle = Mi_Oracle + "||' '||" + Cat_Empleados.Campo_Nombre + ") AS Empleado, ";

                Mi_Oracle = Mi_Oracle + "('[' || " + Cat_Empleados.Campo_No_Empleado + " || '] -- ' || " + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_Oracle = Mi_Oracle + " ||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                Mi_Oracle = Mi_Oracle + " ||' '|| " + Cat_Empleados.Campo_Nombre + ") AS EMPLEADOS";

                Mi_Oracle = Mi_Oracle + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                if (!string.IsNullOrEmpty(Datos.P_SAP_Codigo_Programatico))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND REPLACE(" + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ' ', '') LIKE TRIM(UPPER('%" + Datos.P_SAP_Codigo_Programatico + "%'))";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE REPLACE(" + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ' ', '') LIKE TRIM(UPPER('%" + Datos.P_SAP_Codigo_Programatico + "%'))";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_RFC))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND (UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                        Mi_Oracle += " OR UPPER(" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + "||' '||" + Cat_Empleados.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%'))";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE (UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                        Mi_Oracle+=" OR UPPER("+ Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + "||' '||" + Cat_Empleados.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%'))";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Rol_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Programa_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Contrato_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Area_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Puesto_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Escolaridad_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Sindicado_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Turno_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Zona_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Trabajador_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Password))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Password + "='" + Datos.P_Password + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Password + "='" + Datos.P_Password + "'";
                    }
                }

                if (Mi_Oracle.Contains("WHERE"))
                {
                    Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }
                else
                {
                    Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }

                Mi_Oracle += " ORDER BY " + Cat_Empleados.Campo_Apellido_Paterno + ", " + Cat_Empleados.Campo_Apellido_Materno + ", " + Cat_Empleados.Campo_Nombre + " ASC";

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Empleados;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Percepciones_Deducciones_Tipo_Nomina
        /// DESCRIPCION : Consulta las percepciones o deducciones por su concepto [TIPO_NOMINA o SINDICATO].
        /// PARAMETROS  : Datos: Obtiene que Empleado desea eliminar de la BD
        /// CREO        : JUan Alberto Hernández Negrete
        /// FECHA_CREO  : 05/Enero/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Percepciones_Deducciones_Tipo_Nomina(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL = "";//Variable que almacenara la consulta.
            DataTable Dt_Lista_Percepciones_Deducciones = null;//Lista de Percepciones o deducciones.

            try
            {
                Mi_SQL = "SELECT " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + ".*" +
                         " FROM " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det +
                         " WHERE " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'" +
                         " AND " +
                         Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Concepto + "='TIPO_NOMINA'";

                Dt_Lista_Percepciones_Deducciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            return Dt_Lista_Percepciones_Deducciones;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Informacion_Empleado_Incidencias
        /// DESCRIPCION : Consulta la informacion necesaria que se mostrara en las tablas de empleadas
        ///               ocupadas para el control de incidencias.
        /// PARAMETROS  : Datos: Obtiene que Empleado desea eliminar de la BD
        /// CREO        : JUan Alberto Hernández Negrete
        /// FECHA_CREO  : 16/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Informacion_Empleado_Incidencias(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL = "";//Variable que almacenara la consulta.
            DataTable Dt_Empleados = null;//Variable que almacenara la informacion del empleado consultado.

            try
            {
                Mi_SQL = "SELECT " +
                        Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO, " +
                        "(" +
                        Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "||' '||" +
                        Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                        Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") AS NOMBRE_EMPLEADO, " +
                        "(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + ") AS PAGO_DIA_NORMAL, " +
                        "(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + " * 2) AS PAGO_DIA_DOBLE, " +
                         Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS PUESTO" +
                        " FROM " + Cat_Empleados.Tabla_Cat_Empleados + ", " + Cat_Puestos.Tabla_Cat_Puestos +
                        " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "' " +
                        " AND (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID + "=" +
                        Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID + ")";

                if (Mi_SQL.Contains("WHERE"))
                {
                    Mi_SQL += " AND " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }
                else
                {
                    Mi_SQL += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error generado al consultar la información que se mostrara en las incidencias de los empleados. Error: [" + Ex.Message + "]");
            }
            return Dt_Empleados;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Partidas
        /// DESCRIPCION : Consulta las partidas.
        /// 
        /// PARAMETROS  : Datos: Obtiene que Empleado desea eliminar de la BD
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 30/Abril/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consultar_Partidas(String Programa_ID)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Partidas = null;//Variable que almacena las partidas.

            try
            {
                Mi_SQL.Append("SELECT " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + ".*, ('[' || " + Cat_Sap_Partidas_Especificas.Campo_Clave + " || '] - ' || " + Cat_Sap_Partidas_Especificas.Campo_Descripcion + ") AS PARTIDA");
                Mi_SQL.Append(" FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                Mi_SQL.Append(" WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                Mi_SQL.Append(" IN ");
                Mi_SQL.Append(" (SELECT " + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID);
                Mi_SQL.Append(" FROM " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas);
                Mi_SQL.Append(" WHERE " + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + "='" + Programa_ID + "')");

                Dt_Partidas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las partidas especificas. Error: [" + Ex.Message + "]");
            }
            return Dt_Partidas;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Partidas
        /// DESCRIPCION : Consulta las partidas.
        /// 
        /// PARAMETROS  : Datos: Obtiene que Empleado desea eliminar de la BD
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 30/Abril/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consultar_Partida(String Partida_ID)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Partidas = null;//Variable que almacena las partidas.

            try
            {
                Mi_SQL.Append("SELECT " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + ".*");
                Mi_SQL.Append(" FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                Mi_SQL.Append(" WHERE " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                Mi_SQL.Append(" ='" + Partida_ID + "'");

                Dt_Partidas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las partidas especificas. Error: [" + Ex.Message + "]");
            }
            return Dt_Partidas;
        }
        /// ****************************************************************************************
        /// Nombre: Consultar_Periodo_Actual_Fecha
        /// 
        /// Descripción: Consulta el registro que corresponde al periodo actual.
        /// 
        /// Parámetros: No Aplica.
        /// 
        /// Usuario creó: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 08/Julio/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ****************************************************************************************
        internal static DataTable Consultar_Periodo_Actual_Fecha()
        {
            DataTable Dt_Periodo_Actual = null;
            StringBuilder Mi_SQL = new StringBuilder();

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + ".*");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append("TO_CHAR(SYSDATE, 'dd/MM/yy') >= " + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append("TO_CHAR(SYSDATE, 'dd/MM/yy') <= " + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin);

                Dt_Periodo_Actual = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el periodo actual por fecha. Error: [" + Ex.Message + "]");
            }
            return Dt_Periodo_Actual;
        }
        public static DataTable Consultar_Empleados_Resguardos(Cls_Cat_Empleados_Negocios Parametros)
        {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            try
            {
                Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                Mi_SQL = Mi_SQL + ", TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS NOMBRE";
                Mi_SQL = Mi_SQL + ", TRIM(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS DEPENDENCIA";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = Mi_SQL + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + " = 'ACTIVO'";
                if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID.Trim() + "'";
                }
                if (Parametros.P_RFC != null && Parametros.P_RFC.Trim().Length > 0)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + " = '" + Parametros.P_RFC.Trim() + "'";
                }
                if (Parametros.P_No_Empleado != null && Parametros.P_No_Empleado.Trim().Length > 0)
                {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Parametros.P_No_Empleado.Trim()), 6) + "'";
                }
                if (Parametros.P_Nombre != null && Parametros.P_Nombre.Trim().Length > 0)
                {
                    Mi_SQL = Mi_SQL + " AND (TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") LIKE '%" + Parametros.P_Nombre.Trim() + "%'";
                    Mi_SQL = Mi_SQL + " OR TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") LIKE '%" + Parametros.P_Nombre.Trim() + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY NOMBRE";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null)
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Datos;
        }

        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
        {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
            {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }
        #endregion

        #region (Periodos Vacacionales)
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Insertar_Actualizar_Detalle_Periodo_Vacacional
        ///
        ///DESCRIPCIÓN: Consulta el Año nóminal y el periodo vacacional en el que se encuentra actualmente. Consulta si ya existe previamente
        ///             el registro del periodo vacacional para el empleado en la tabla  de Ope_Nom_Vacaciones_Empl_Det. Si el registro ya existe
        ///             se procede a realizar la actualización de los dias disponibles de vacaciones descontando los dias de vacaciones que se 
        ///             tomaron en la nómina generada el el periodo nóminal actual. En caso contrario se realiza la Inserción del registro del
        ///             periodo vacacional del empleado y los dias disponibles serán en realación asu antiguedad dentro de la empresa.
        ///             
        /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
        ///             
        ///CREO: Juan Alberto Hernández Negrete
        ///FECHA_CREO: 04/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        private static void Insertar_Actualizar_Detalle_Periodo_Vacacional(String Empleado_ID)
        {
            Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones_Empleados =
                                new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Vacaciones_Empl_Det = null;                          //Variable que almacena la información del periodo vacacional actual.
            Int32 Anio_Calendario_Nomina = 0;                                 //Variable que almacenara el año del calendario de nómina actual.
            Int32 Periodo_Vacacional_Actual = 0;                              //Variable que almacenara el número periodo vacacional actual.
            Int32 Dias_Disponibles = 0;                                       //Variable que almacenara los dias disponibles del periodo vacacional actual.
            Int32 Dias_Tomados = 0;                                           //Variable que almacenara los dias tomados en el periodo vacacional actual.
            Int32 Dias_Vacaciones_Tipo_Nomina = 0;                            //Variable que almacenara los dias de vacaciones de acuardo al tipo de nómina.
            String Tipo_Nomina_ID = "";                                       //Variable que almacena el tipo de nomina del empleado.

            try
            {
                Anio_Calendario_Nomina = Obtener_Anio_Calendario_Nomina();//Obtenemos el año del calendario nominal actual.
                Periodo_Vacacional_Actual = Obtener_Periodo_Vacacional();//Obtenemos el periodo vacacional actual.

                Tipo_Nomina_ID = Obtener_Tipo_Nomina_Empleado(Empleado_ID);
                Dias_Vacaciones_Tipo_Nomina = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, Periodo_Vacacional_Actual);


                if (Antiguedad_Empleado_Es_Un_Anio(Empleado_ID))
                {
                    //Si no se encontro ningún registro. Se hará un Insert del periodo vacacional.
                    Alta_Periodo_Vacacional_Actual(Empleado_ID, Dias_Vacaciones_Tipo_Nomina, 0);
                }
                else
                {
                    //Si no se encontro ningún registro. Se hará un Insert del periodo vacacional.
                    Alta_Periodo_Vacacional_Actual(Empleado_ID, Dias_Vacaciones_Base_Formula(Empleado_ID), 0);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Insertar o Actualizar un Detalle de las Vacaciones del Empleado. Error: [" + Ex.Message + "]");
            }
        }
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Periodo_Vacacional
        ///
        ///DESCRIPCIÓN: Consulta y obtiene el periodo vacacional en el que se encuentra el empleado. PERIODO I [ENERO - JUNIO] Ó PERIODO II 
        ///             [JULIO - DICIEMBRE].
        ///             
        ///CREO: Juan Alberto Hernández Negrete
        ///FECHA_CREO: 04/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        private static Int32 Obtener_Periodo_Vacacional()
        {
            Int32 Periodo_Vacacional = 0;           //Variable que almacenara el periodo vacacional actual.    
            Int32 Anio_Calendario_Nomina = 0;       //Variable que almacena el año del calendario de la nomina.
            DateTime Fecha_Actual = DateTime.Now;   //Variable que almacena la fecha actual.

            try
            {
                //Consulta el año actual del periodo nóminal.
                Anio_Calendario_Nomina = Obtener_Anio_Calendario_Nomina();

                if ((DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 1, 1)) >= 0) &&
                    (DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 6, 30)) <= 0))
                {
                    //PERIODO VACACIONAL I
                    Periodo_Vacacional = 1;
                }
                else if ((DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 7, 1)) >= 0) &&
                    (DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 12, 31)) <= 0))
                {
                    //PERIODO VACACIONAL II
                    Periodo_Vacacional = 2;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener el periodo vacacional. Error: [" + Ex.Message + "]");
            }
            return Periodo_Vacacional;
        }
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Anio_Calendario_Nomina
        ///
        ///DESCRIPCIÓN: Consulta y obtiene el año de la nómina actual. Está búsqueda se realiza en los calendarios de nómina que se encuentran
        ///             registrados actualmente en el sistema.
        ///             
        ///CREO: Juan Alberto Hernández Negrete
        ///FECHA_CREO: 04/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        private static Int32 Obtener_Anio_Calendario_Nomina()
        {
            Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la clase de negocios.
            DataTable Dt_Calendario_Nomina = null;                                                                      //Variable que guardara la información del calendario de nómina consultado.
            Int32 Anio_Calendario_Nomina = 0;                                                                          //Variable que almacena el año del calendario de nomina.         

            try
            {
                //Consultamos la el calendario de nómina que esta activo actualmente. 
                Dt_Calendario_Nomina = Obj_Calendario_Nomina.Consultar_Calendario_Nomina_Fecha_Actual();

                if (Dt_Calendario_Nomina is DataTable)
                {
                    foreach (DataRow Informacion_Calendario_Nomina in Dt_Calendario_Nomina.Rows)
                    {
                        if (Informacion_Calendario_Nomina is DataRow)
                        {
                            if (!String.IsNullOrEmpty(Informacion_Calendario_Nomina[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString()))
                            {
                                Anio_Calendario_Nomina = Convert.ToInt32(Informacion_Calendario_Nomina[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el año del calendario de nómina del año actual. Error: [" + Ex.Message + "]");
            }
            return Anio_Calendario_Nomina;
        }
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Insertar_Actualizar_Detalle_Periodo_Vacacional
        ///
        ///DESCRIPCIÓN: Obtiene en base a formúla los dias de vacaciones que le corresponden al empelado según su antiguedad en la empresa.
        ///
        ///             Si Antiguedad Laboral Menor a 1 entoncés:
        ///             
        ///                 Dias_Año [365 ó 366] ---> 20 Dias de Vacaciones al año.
        ///                 N Dias Laborados     --->  X Dias de Vacaciones al año.
        ///             
        /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
        ///             
        ///CREO: Juan Alberto Hernández Negrete
        ///FECHA_CREO: 04/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        private static Int32 Dias_Vacaciones_Base_Formula(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
            DataTable Dt_Empleados = null;                                              //Variable que almacena la informacion del empleado consultado.     
            Int32 Cantidad_Dias = 0;                                                    //Cantidad de dias que le corresponden al empleado segun su fecha de ingreso a presidencia.
            Int32 DIAS_PERIODO = 0;                                                     //Variable que almacena los dias que tiene el año nominal.
            Int32 Dias = 0;                                                             //Cantidad de dias que lleva el empleado laborando en presidencia.
            Int32 Anio_Calendario_Nomina = 0;                                           //Variable que almacena el año del periodo nominal.
            DateTime? Fecha_Ingreso_Empleado = null;                                    //Variable que alamaceba la fecha de ingreso del empleado a presidencia.
            DateTime Fecha_Actual = DateTime.Now;                                       //Variable que almacena la fecha actual.
            String Tipo_Nomina_ID = "";                                                   //Variable que almacenara el tipo de nómina al que pertence el empleado.
            Int32 Dias_Vacaciones_Tipo_Nomina = 0;                                      //Variable que almacenara los dias de vacaciones que le corresponden al periodo por tipo de nomina consultado.
            Int32 Anio = 0;                                                             //Variable que almacenara el año de calendario de nómina vigente actualmente.         
            Int32 Periodo_Actual = 0;                                                   //Variable que almacenara el periodo vacacional actual.
            Int32 Auxiliar = 0;                                                         //Variable auxiliar que almacenara los dias entre la fecha de ingreso y el 30 de Junio del año actual.
            Int32 Dias_Totales_Primer_Periodo_Vacacional = 0;                           //Variable que almacenara los dias totales del primer periodo [Enero - Junio].
            Int32 Dias_Totales_Segundo_Periodo_Vacacional = 0;                          //Variable que almacenara los dias totales del segundo periodo [Julio - Diciembre].

            try
            {
                Anio = Obtener_Anio_Calendario_Nomina();//Obtenemos el anio del calendario de nomina vigente actualmente.
                Periodo_Actual = Obtener_Periodo_Vacacional();//Obtenemos el periodo vacacional en el que nos encontramos actualmente.

                //Obtenemos el total de dias del periodo de [Enero - Junio] y del periodo [Julio - Diciembre] del anio del calendario de nomina vigente.
                Dias_Totales_Primer_Periodo_Vacacional = (Int32)Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio, 1, 1), new DateTime(Anio, 6, 30)) + 1;
                Dias_Totales_Segundo_Periodo_Vacacional = (Int32)Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio, 7, 1), new DateTime(Anio, 12, 31)) + 1;

                //Consultamos el tipo de nomina al que pertence el empleado.
                Tipo_Nomina_ID = Obtener_Tipo_Nomina_Empleado(Empleado_ID);
                //Obtenemos los dias de vacaciones que le corresponden al periodo vacacional de acuerdo al tipo de nómina.
                Dias_Vacaciones_Tipo_Nomina = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, Periodo_Actual);

                //Consultamos la información general del empleado.
                Obj_Empleados.P_Empleado_ID = Empleado_ID;
                Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();

                //Validamos que la búsqueda.
                if (Dt_Empleados is DataTable)
                {
                    if (Dt_Empleados.Rows.Count > 0)
                    {
                        foreach (DataRow Empleado in Dt_Empleados.Rows)
                        {
                            if (Empleado is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Fecha_Inicio].ToString()))
                                {
                                    //Obtenemos la fecha de ingreso del empleado.
                                    Fecha_Ingreso_Empleado = Convert.ToDateTime(Empleado[Cat_Empleados.Campo_Fecha_Inicio].ToString());

                                    //Si el año de ingreso del empleado a presidencia es igual a l año actual entoncés:
                                    if (((DateTime)Fecha_Ingreso_Empleado).Year == Anio)
                                    {
                                        //Identificamos el periodo, si el periodo actual es el primero entonces:
                                        if (Periodo_Actual == 1)
                                        {
                                            //Obtenemos los dias que el empleado lleva laborando en presidencia.
                                            Dias = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, ((DateTime)Fecha_Ingreso_Empleado), new DateTime(Anio, 06, 30, 23, 59, 59)) + 1);
                                            //Obtenemos los dias totales del periodo de [Enero - Junio].
                                            DIAS_PERIODO = Dias_Totales_Primer_Periodo_Vacacional;

                                        }
                                        else if (Periodo_Actual == 2)
                                        {
                                            //Si el periodo actual es el segundo entoncés:

                                            //Consultamos los dias que lleva laborando el empleado en presidencia.
                                            Dias = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, ((DateTime)Fecha_Ingreso_Empleado), new DateTime(Anio, 12, 31, 23, 59, 59)) + 1);
                                            //Obtenemos los dias laborados del empleado desde su fecha de ingreso hasta el 30 de Junio del año actual.
                                            Auxiliar = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, ((DateTime)Fecha_Ingreso_Empleado), new DateTime(Anio, 6, 30)) + 1);
                                            //A los dias totales que lleva el empleado laborando el empleado en presidencia se le restan los dias que le corresponden 
                                            //al primer periodo, esto quiere decir que solo se consideraran los dias apartir del 1 de Julio del año actual, para el
                                            //calculo de los dias que le corresponden al empleado de vacaciones en base a su antiguedad en presidencia.
                                            Dias = Dias - Auxiliar;
                                            //Obtenemos los dias totales del periodo de [Julio - Dicciembre].
                                            DIAS_PERIODO = Dias_Totales_Segundo_Periodo_Vacacional;
                                        }

                                        //Cantidad de dias que le corresponden al empleado de vacaciones si es que el empleado no tiene un año completo en presidencia
                                        Cantidad_Dias = (Int32)((Dias * Dias_Vacaciones_Tipo_Nomina) / DIAS_PERIODO);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener el porcentaje [%] de los dias que le corresponden al" +
                                    "empleado en base a la fecha de ingreso que tiene. Error: [" + Ex.Message + "]");
            }
            return Cantidad_Dias;
        }
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Periodo_Vacacional_Actual
        ///
        ///DESCRIPCIÓN: Consulta y obtiene el Año de nómina actual y el periodo en el que nos encontramos actualmente a partir de ahí obtenemos
        ///             el Año [Anterior - Actual - Siguiente] y el Periodo [Anterior - Actual - Siguiente]. Se procede a realizar el alta de 
        ///             del siguiente periodo vacacional.
        ///             
        /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
        ///             Dias_Disponibles.- Son los dias que el empleado tendrá disponibles en ese periodo vacacional.
        ///             Dias_Tomados.- Son los días que el empleado a tomado de este periodo vacacional.
        ///             
        ///CREO: Juan Alberto Hernández Negrete 
        ///FECHA_CREO: 04/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        private static void Alta_Periodo_Vacacional_Actual(String Empleado_ID, Int32 Dias_Disponibles, Int32 Dias_Tomados)
        {
            Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones_Empleados = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexión con la capa de negocios.
            Int32 Anio_Periodo_Anterior = 0;    //Variable que almacena el año del periodo vacacional anterior al actual.        
            Int32 Anio_Periodo_Actual = 0;      //Variable que almacena el año del periodo vacacional actual.
            Int32 Anio_Periodo_Siguiente = 0;   //Variable que almacena el año del periodo vacacional siguiente al actual.
            Int32 Periodo_Anterior = 0;         //Variable que almacena el periodo anterior al actual.
            Int32 Periodo_Actual = 0;           //Variable que almacena el periodo actual.
            Int32 Periodo_Siguiente = 0;        //Variable que almacena el periodo siguiente al actual.
            DataTable Dt_Vacaciones_Empl_Det = null;
            Int32 Dias_Vacaciones_Tipo_Nomina = 0;                            //Variable que almacenara los dias de vacaciones de acuardo al tipo de nómina.
            String Tipo_Nomina_ID = "";                                       //Variable que almacena el tipo de nomina del empleado.

            try
            {
                //Consultamos el año del periodo vacacional del empleado.
                Anio_Periodo_Actual = Obtener_Anio_Calendario_Nomina();
                //Consultamos el periodo vacacional del empleado.
                Periodo_Actual = Obtener_Periodo_Vacacional();

                Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
                Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
                Obj_Vacaciones_Empleados.P_Anio = Anio_Periodo_Actual;
                Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo_Actual;
                Dt_Vacaciones_Empl_Det = Obj_Vacaciones_Empleados.Consultar_Vacaciones_Empl_Det();

                if (Dt_Vacaciones_Empl_Det is DataTable)
                {
                    if (Dt_Vacaciones_Empl_Det.Rows.Count <= 0)
                    {
                        //Establecemos los valores para realizar la operación de alta del periodo vacacional
                        Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
                        Obj_Vacaciones_Empleados.P_Anio = Anio_Periodo_Actual;
                        Obj_Vacaciones_Empleados.P_Dias_Disponibles = Dias_Disponibles;
                        Obj_Vacaciones_Empleados.P_Dias_Tomados = Dias_Tomados;
                        Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo_Actual;
                        Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
                        Obj_Vacaciones_Empleados.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                        //ejecuta el alta.
                        Obj_Vacaciones_Empleados.Alta_Detalle_Vacaciones_Empleados();
                    }
                }

                Tipo_Nomina_ID = Obtener_Tipo_Nomina_Empleado(Empleado_ID);
                Dias_Vacaciones_Tipo_Nomina = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, Periodo_Actual);

                //Identificamos el periodo vacacional que corresponde al siguiente periodo a partir del actual.
                if (Periodo_Actual == 1)
                {
                    //Si el periodo actual es el primero entoncés:
                    Anio_Periodo_Anterior = Anio_Periodo_Actual - 1;//El año del periodo anterior es el año actual menos 1.
                    Anio_Periodo_Siguiente = Anio_Periodo_Actual;//El año del periodo siguiente es el mismo.

                    Periodo_Anterior = 2;
                    Periodo_Siguiente = 2;
                    Dias_Vacaciones_Tipo_Nomina = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, Periodo_Siguiente);//Agregada

                    //Establecemos los valores requeridos para realizar la inserción del periodo siguiente despues del actual. 
                    Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
                    Obj_Vacaciones_Empleados.P_Anio = Anio_Periodo_Siguiente;
                    Obj_Vacaciones_Empleados.P_Dias_Disponibles = Dias_Vacaciones_Tipo_Nomina;
                    Obj_Vacaciones_Empleados.P_Dias_Tomados = 0;
                    Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo_Siguiente;
                    Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
                    Obj_Vacaciones_Empleados.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                    //Ejecutamos del alta del periodo siguiente al actual.
                    Obj_Vacaciones_Empleados.Alta_Detalle_Vacaciones_Empleados();
                }
                else if (Periodo_Actual == 2)
                {
                    //Si el periodo actual es el segundo entoncés:
                    Anio_Periodo_Anterior = Anio_Periodo_Actual;//El año del periodo anterior es el mismo.
                    Anio_Periodo_Siguiente = Anio_Periodo_Actual + 1;//El año del periodo siguiente es Año actual mas 1.

                    Periodo_Anterior = 1;
                    Periodo_Siguiente = 1;
                    Dias_Vacaciones_Tipo_Nomina = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, Periodo_Siguiente);

                    //Establecemos los valores requeridos para realizar la inserción del periodo siguiente despues del actual. 
                    Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
                    Obj_Vacaciones_Empleados.P_Anio = Anio_Periodo_Siguiente;
                    Obj_Vacaciones_Empleados.P_Dias_Disponibles = Dias_Vacaciones_Tipo_Nomina;
                    Obj_Vacaciones_Empleados.P_Dias_Tomados = 0;
                    Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo_Siguiente;
                    Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
                    Obj_Vacaciones_Empleados.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                    //Ejecutamos del alta del periodo siguiente al actual.
                    Obj_Vacaciones_Empleados.Alta_Detalle_Vacaciones_Empleados();
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al dar de alta el periodo vacacional actual. Error: [" + Ex.Message + "]");
            }
        }
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Antiguedad_Empleado_Es_Un_Anio
        ///
        ///DESCRIPCIÓN: Consulta la fecha de ingreso del empleado y en base a esta información se identifica si el empleado cuenta con mas de un 
        ///             año de antiguedad en la empresa. Si Cuenta con un año la función retorna un valor booleano iwual a true de lo contrario
        ///             retorna un valor false.
        ///             
        /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
        ///             
        ///CREO: Juan Alberto Hernández Negrete 
        ///FECHA_CREO: 04/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        private static Boolean Antiguedad_Empleado_Es_Un_Anio(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
            DataTable Dt_Informacion_Empelado = null;                                   //Variable que almacenara los datos del empleado.
            DateTime? Fecha_Ingreso_Empleado = null;                                    //Variable que alamaceba la fecha de ingreso del empleado a presidencia.
            DateTime Fecha_Actual = DateTime.Now;                                       //Variable que almacena la fecha actual.
            Boolean Estatus = false;

            try
            {
                Obj_Empleados.P_Empleado_ID = Empleado_ID;
                Dt_Informacion_Empelado = Obj_Empleados.Consulta_Empleados_General();

                if (Dt_Informacion_Empelado is DataTable)
                {
                    if (Dt_Informacion_Empelado.Rows.Count > 0)
                    {
                        foreach (DataRow Empleado in Dt_Informacion_Empelado.Rows)
                        {
                            if (Empleado is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Fecha_Inicio].ToString()))
                                {
                                    Fecha_Ingreso_Empleado = Convert.ToDateTime(Empleado[Cat_Empleados.Campo_Fecha_Inicio].ToString());
                                    //Validamos si la antiguedad del empelado es de 1 año.
                                    if (Cls_DateAndTime.DateDiff(DateInterval.Year, ((DateTime)Fecha_Ingreso_Empleado), Fecha_Actual) >= 1)
                                    {
                                        Estatus = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar si la antiguedad del empelado en presidencia es de unn año. Error: [" + Ex.Message + "]");
            }
            return Estatus;
        }
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dias_Vacaciones_Tipo_Nomina
        ///
        ///DESCRIPCIÓN: Consulta los dias de vacaciones que le corresponde al empleado por el tipo de nómina al que pertence. Y de acuerdo al 
        ///             periodo vacacional consultado.
        ///             
        /// PARÁMETROS: Tipo_Nomina_ID.- Identificador o clave única para identificar a los tipos de nomina que se encuentran dadas de 
        ///                              alta en el sistema.
        ///             
        ///CREO: Juan Alberto Hernández Negrete 
        ///FECHA_CREO: 15/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        private static DataTable Consultar_Dias_Vacaciones_Tipo_Nomina(String Tipo_Nomina_ID)
        {
            Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nomina = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Tipos_Nomina = null;                                                    //Variable que guarda la información del tipo de nomina.
            DataTable Dt_Tipo_Nomina_Dias_Vacaciones = new DataTable("DIAS_VACACIONES");         //Variable que almacenara los dias de vacaciones de los periodos vacacionales.
            DataRow Registro_Dias_Vacaciones = null;                                             //Variable que almacena un registro de las vacaciones por periodo vacacional.
            Int32 Dias_Vacaciones_PVI = 0;                                                       //Variable que almacena las dias de vacaciones del periodo primer periodo vacacional.
            Int32 Dias_Vacaciones_PVII = 0;                                                      //Variable que almacena las dias de vacaciones del periodo segundo periodo vacacional.

            try
            {
                Dt_Tipo_Nomina_Dias_Vacaciones.Columns.Add("PVI", typeof(Int32));
                Dt_Tipo_Nomina_Dias_Vacaciones.Columns.Add("PVII", typeof(Int32));

                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Dt_Tipos_Nomina = Obj_Tipos_Nomina.Consulta_Datos_Tipo_Nomina();

                if (Dt_Tipos_Nomina is DataTable)
                {
                    if (Dt_Tipos_Nomina.Rows.Count > 0)
                    {
                        foreach (DataRow Tipo_Nomina in Dt_Tipos_Nomina.Rows)
                        {
                            if (Tipo_Nomina is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString()))
                                {
                                    Dias_Vacaciones_PVI = Convert.ToInt32(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString());
                                    if (!String.IsNullOrEmpty(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString()))
                                    {
                                        Dias_Vacaciones_PVII = Convert.ToInt32(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString());

                                        Registro_Dias_Vacaciones = Dt_Tipo_Nomina_Dias_Vacaciones.NewRow();
                                        Registro_Dias_Vacaciones["PVI"] = Dias_Vacaciones_PVI;
                                        Registro_Dias_Vacaciones["PVII"] = Dias_Vacaciones_PVII;
                                        Dt_Tipo_Nomina_Dias_Vacaciones.Rows.Add(Registro_Dias_Vacaciones);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los dias de vacaciones del empleado de acuerdo a su tipo de nómina. Error: [" + Ex.Message + "]");
            }
            return Dt_Tipo_Nomina_Dias_Vacaciones;
        }
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dias_Vacaciones_Tipo_Nomina
        ///
        ///DESCRIPCIÓN: Consulta los dias de vacaciones que le corresponde al empleado por el tipo de nómina al que pertence. Y de acuerdo al 
        ///             periodo vacacional consultado.
        ///             
        /// PARÁMETROS: Tipo_Nomina_ID.- Identificador o clave única para identificar a los tipos de nomina que se encuentran dadas de 
        ///                              alta en el sistema.
        ///             Periodo.- Periodo Vacacional a consultar los dias de vacaciones del empleado.                  
        ///             
        ///CREO: Juan Alberto Hernández Negrete 
        ///FECHA_CREO: 15/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        private static Int32 Consultar_Dias_Vacaciones_Tipo_Nomina(String Tipo_Nomina_ID, Int32 Periodo)
        {
            Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nomina = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
            DataTable Dt_Tipos_Nomina = null;                                                    //Variable que guarda la información del tipo de nomina.
            Int32 Dias_Vacaciones_PVI = 0;                                                       //Variable que almacena las dias de vacaciones del primer periodo vacacional.
            Int32 Dias_Vacaciones_PVII = 0;                                                      //Variable que almacena las dias de vacaciones del segundo periodo vacacional.
            Int32 Dias = 0;

            try
            {
                Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Dt_Tipos_Nomina = Obj_Tipos_Nomina.Consulta_Datos_Tipo_Nomina();

                if (Dt_Tipos_Nomina is DataTable)
                {
                    if (Dt_Tipos_Nomina.Rows.Count > 0)
                    {
                        foreach (DataRow Tipo_Nomina in Dt_Tipos_Nomina.Rows)
                        {
                            if (Tipo_Nomina is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString()))
                                {
                                    Dias_Vacaciones_PVI = Convert.ToInt32(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString());
                                    if (!String.IsNullOrEmpty(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString()))
                                    {
                                        Dias_Vacaciones_PVII = Convert.ToInt32(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString());

                                        if (Periodo == 1) Dias = Dias_Vacaciones_PVI;
                                        else if (Periodo == 2) Dias = Dias_Vacaciones_PVII;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los dias de vacaciones del empleado de acuerdo a su tipo de nómina. Error: [" + Ex.Message + "]");
            }
            return Dias;
        }
        ///***************************************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Tipo_Nomina_Empleado
        ///
        ///DESCRIPCIÓN: Consulta y obtiene el tipo de nomina al que pertence el empleado.
        ///             
        /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los empleados que se encuentran dadas de 
        ///                           alta en el sistema.
        ///             
        ///CREO: Juan Alberto Hernández Negrete 
        ///FECHA_CREO: 15/Marzo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///***************************************************************************************************************************************
        private static String Obtener_Tipo_Nomina_Empleado(String Empleado_ID)
        {
            Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
            DataTable Dt_Informacion_Empleado = null;                                   //Variable que almacenara los datos del empleado consultado.
            String Tipo_Nomina_ID = "";                                                 //Variable que almacena el tipo de nomina del empleado.

            try
            {
                Obj_Empleados.P_Empleado_ID = Empleado_ID;
                Dt_Informacion_Empleado = Obj_Empleados.Consulta_Empleados_General();

                if (Dt_Informacion_Empleado is DataTable)
                {
                    if (Dt_Informacion_Empleado.Rows.Count > 0)
                    {
                        foreach (DataRow Empleado in Dt_Informacion_Empleado.Rows)
                        {
                            if (Empleado is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString()))
                                    Tipo_Nomina_ID = Empleado[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el tipo de nómina del empleado. Error: [" + Ex.Message + "]");
            }
            return Tipo_Nomina_ID;
        }
        #endregion

        #region (Reportes)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Rpt_Empleados
        /// DESCRIPCION : Consulta los empleados de acuerdo a los filtros establecidos.
        /// 
        /// PARAMETROS  : Datos: Obtiene que Empleado desea eliminar de la BD
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 4/Mayo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consultar_Rpt_Empleados(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Empelados = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "|| ' ' ||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "|| ' ' ||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UNIDAD_RESPONSABLE, ");
                Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS FUENTE_FINANCIAMIENTO, ");
                Mi_SQL.Append(Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Descripcion + " AS PROYECTO_PROGRAMA, ");
                Mi_SQL.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Descripcion + " AS AREA_FUNCIONAL, ");
                Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Descripcion + " AS PARTIDA, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS PUESTO, ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + " AS TIPO_NOMINA, ");
                Mi_SQL.Append(Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Nombre + " AS SINDICATO, ");
                Mi_SQL.Append(Cat_Nom_Tipos_Contratos.Tabla_Cat_Nom_Tipos_Contratos + "." + Cat_Nom_Tipos_Contratos.Campo_Descripcion + " AS TIPO_CONTRATO, ");
                Mi_SQL.Append(Cat_Nom_Escolaridad.Tabla_Cat_Nom_Escolaridad + "." + Cat_Nom_Escolaridad.Campo_Escolaridad + " AS ESCOLARIDAD, ");
                Mi_SQL.Append("(" + Cat_Turnos.Tabla_Cat_Turnos + "." + Cat_Turnos.Campo_Descripcion + " || ' DE ' || " + "TO_CHAR(" + Cat_Turnos.Tabla_Cat_Turnos + "." + Cat_Turnos.Campo_Hora_Entrada + ", 'HH24:MI') || ' A ' ||" + "TO_CHAR(" + Cat_Turnos.Tabla_Cat_Turnos + "." + Cat_Turnos.Campo_Hora_Salida + ", 'HH24:MI')) AS TURNO, ");
                Mi_SQL.Append(Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica + "." + Cat_Nom_Zona_Economica.Campo_Zona_Economica + " AS ZONA, ");
                Mi_SQL.Append(Cat_Nom_Tipo_Trabajador.Tabla_Cat_Nom_Tipo_Trabajador + "." + Cat_Nom_Tipo_Trabajador.Campo_Descripcion + " AS TIPO_TRABAJADOR, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Reloj_Checador + ", ");
                Mi_SQL.Append(Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Apl_Cat_Roles.Campo_Nombre + " AS ROLES, ");                                
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Calle + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Colonia + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Codigo_Postal + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Ciudad + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estado + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Telefono_Casa + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Telefono_Oficina + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Extension + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fax + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Celular + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nextel + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Correo_Electronico + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sexo + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Nacimiento + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_CURP + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Ruta_Foto + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre_Foto + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_IMSS + ", ");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre + " AS BANCO, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Forma_Pago + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Cuenta_Bancaria + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Tarjeta + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Finiquito + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Termino_Contrato + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Baja + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario_Integrado + ", ");               
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Lunes + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Martes + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Miercoles + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Jueves + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Viernes + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sabado + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Domingo + ", ");
                Mi_SQL.Append(Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + "." + Cat_Nom_Terceros.Campo_Nombre + " AS TERCERO, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Licencia_Manejo + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia);
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Usuario_Creo + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Creo + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Usuario_Modifico + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Modifico + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Comentarios + ", ");                
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre_Beneficiario + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Cantidad_Porcentaje + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Aplica_Orden_Judicial + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Desc_Orden_Judicial + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Desc_Orden_Judicial_Aguinaldo + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Cantidad_Porcentaje_Aguinaldo + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Desc_Orden_Judicial_Prima_Vacacional + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Cantidad_Porcentaje_Prima_Vacacional + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Mensual_Actual + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Orden_Judicial_Bruto_Neto_Sueldo_Normal + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Orden_Judicial_Bruto_Neto_Aguinaldo + ", ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Orden_Judicial_Bruto_Neto_Prima_Vacacional + ", ");
                //Mi_SQL.Append(Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre + " AS AREA, ");
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Password);  
                //Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ", ");

                Mi_SQL.Append(" FROM ");

                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Contratos.Tabla_Cat_Nom_Tipos_Contratos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Contrato_ID + "=" + Cat_Nom_Tipos_Contratos.Tabla_Cat_Nom_Tipos_Contratos + "." + Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Puestos.Tabla_Cat_Puestos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID + "=" + Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Escolaridad.Tabla_Cat_Nom_Escolaridad + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Escolaridad_ID + "=" + Cat_Nom_Escolaridad.Tabla_Cat_Nom_Escolaridad + "." + Cat_Nom_Escolaridad.Campo_Escolaridad_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sindicato_ID + "=" + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Sindicato_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Turnos.Tabla_Cat_Turnos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Turno_ID + "=" + Cat_Turnos.Tabla_Cat_Turnos + "." + Cat_Turnos.Campo_Turno_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Zona_ID + "=" + Cat_Nom_Zona_Economica.Tabla_Cat_Nom_Zona_Economica + "." + Cat_Nom_Zona_Economica.Campo_Zona_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipo_Trabajador.Tabla_Cat_Nom_Tipo_Trabajador + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Trabajador_ID + "=" + Cat_Nom_Tipo_Trabajador.Tabla_Cat_Nom_Tipo_Trabajador + "." + Cat_Nom_Tipo_Trabajador.Campo_Tipo_Trabajador_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Rol_ID + "=" + Apl_Cat_Roles.Tabla_Apl_Cat_Roles + "." + Apl_Cat_Roles.Campo_Rol_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + "=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Terceros_ID + "=" + Cat_Nom_Terceros.Tabla_Cat_Nom_Terceros + "." + Cat_Nom_Terceros.Campo_Tercero_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID + "=" + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID + "=" + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Programa_ID + "=" + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Area_Responsable_ID + "=" + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Partida_ID + "=" + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Areas.Tabla_Cat_Areas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Area_ID + "=" + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Area_ID);

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_RFC))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                            Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Rol_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Programa_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Contrato_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Area_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Puesto_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Escolaridad_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Sindicado_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Turno_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Zona_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Trabajador_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_CURP))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_CURP + "='" + Datos.P_CURP + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_CURP + "='" + Datos.P_CURP + "'");
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda))
                {
                    if (Mi_SQL.ToString().Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                    }
                }

                if (Mi_SQL.ToString().Contains("WHERE"))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                }
                else
                {
                    Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                }

                Dt_Empelados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            return Dt_Empelados;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Rpt_Incidencias_Empleados
        /// DESCRIPCION : Consulta las incidencias que el empleado a tenido en el periodo
        ///               nominal por concepto de Faltas y Retardos.
        ///               
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 13/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consulta_Rpt_Incidencias_Empleados(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_Oracle = "";//Variable que alamcenara la consulta.
            DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.
            StringBuilder Mi_SQL = new StringBuilder();

            try
            {
                Mi_SQL.Append("SELECT " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + ".*, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "|| ' ' ||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "|| ' ' ||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + " AS NOMINA");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=" + Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Empleado_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " ON ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Nomina_ID + "=" + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_No_Nomina + "=" + Datos.P_No_Nomina);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." +
                    Ope_Nom_Faltas_Empleado.Campo_Estatus + "='Autorizado'");

                if (!String.IsNullOrEmpty(Datos.P_Tipo_Incidencia))
                {
                    switch (Datos.P_Tipo_Incidencia)
                    {
                        case "Faltas":
                            Mi_SQL.Append(" AND ");
                            Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." +
                              Ope_Nom_Faltas_Empleado.Campo_Retardo + "='NO'");
                            break;
                        case "Retardos":
                            Mi_SQL.Append(" AND ");
                            Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." +
                              Ope_Nom_Faltas_Empleado.Campo_Retardo + "='SI'");
                            break;
                        default:
                            break;
                    }

                }

                if (!String.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda_Incidencia) && !String.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda_Incidencia))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." + Ope_Nom_Faltas_Empleado.Campo_Fecha + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda_Incidencia + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                       " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda_Incidencia + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                }

                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Faltas_Empleado.Tabla_Ope_Nom_Faltas_Empleado + "." +
                    Ope_Nom_Faltas_Empleado.Campo_Empleado_ID + " IN ");

                Mi_Oracle = Mi_Oracle + "(SELECT " + Cat_Empleados.Campo_Empleado_ID;
                Mi_Oracle = Mi_Oracle + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                if (Mi_Oracle.Contains("WHERE"))
                {
                    Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }
                else
                {
                    Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_RFC))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Rol_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Programa_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Contrato_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Area_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Puesto_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Escolaridad_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Sindicado_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Turno_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Zona_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Trabajador_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }

                Mi_Oracle = Mi_Oracle + ") ORDER BY NO_FALTA ASC";

                Mi_SQL = new StringBuilder(Mi_SQL.ToString() + Mi_Oracle);

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Empleados;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Empleados_General
        /// DESCRIPCION : Consulta las vacaciones que el empleado ha tomado en algun determinado 
        ///               periodo nominal.
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 9/Mayo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consulta_Rpt_Vacaciones_Empleados(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_Oracle = "";//Variable que alamcenara la consulta.
            DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.
            StringBuilder Mi_SQL = new StringBuilder();

            try
            {
                Mi_SQL.Append("SELECT "); 
                Mi_SQL.Append("(cast(" + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Vacacion +
                        " as decimal(10,5) )) as Vacacion,");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + ".*, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "|| ' ' ||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "|| ' ' ||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + " AS NOMINA");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=" + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " ON ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID + "=" + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina + "=" + Datos.P_No_Nomina);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Estatus + "='Autorizado'");

                if (!String.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda_Incidencia) && !String.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda_Incidencia))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append("(" + OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda_Incidencia + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                       " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda_Incidencia + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                }

                if (!String.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda_Incidencia) && !String.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda_Incidencia))
                {
                    Mi_SQL.Append(" OR ");
                    Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Termino + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda_Incidencia + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                       " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda_Incidencia + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS'))");
                }

                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." +
                    OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + " IN ");

                Mi_Oracle = Mi_Oracle + "(SELECT " + Cat_Empleados.Campo_Empleado_ID;
                Mi_Oracle = Mi_Oracle + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                if (Mi_Oracle.Contains("WHERE"))
                {
                    Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }
                else
                {
                    Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_RFC))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Rol_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Programa_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Contrato_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Area_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Puesto_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Escolaridad_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Sindicado_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Turno_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Zona_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Trabajador_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }

                Mi_Oracle = Mi_Oracle + ") ORDER BY NO_VACACION ASC";

                Mi_SQL = new StringBuilder(Mi_SQL.ToString() + Mi_Oracle);

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Empleados;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Rpt_Tiempo_Extra_Empleados
        /// DESCRIPCION : Consulta los empelados del sistema.        
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 9/Mayo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consulta_Rpt_Tiempo_Extra_Empleados(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_Oracle = "";//Variable que alamcenara la consulta.
            DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.
            StringBuilder Mi_SQL = new StringBuilder();

            try
            {
                //-----------------  [ CAMPOS A OBTENER ]  ------------------
                Mi_SQL.Append("SELECT " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + ".*, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "|| ' ' ||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "|| ' ' ||");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + " AS NOMINA, ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Estatus + " AS ESTATUS_EMPLEADO");

                //-----------------  [ TABLAS CONSULTAR ]  ------------------
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=" + Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID);

                Mi_SQL.Append(" INNER JOIN " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + " ON ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra_Emp_Det.Campo_No_Tiempo_Extra + "=" + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " ON ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Nomina_ID + "=" + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);

                //-----------------  [ CONDICIONES QUE SE DEBEN CUMPLIR ]  ------------------
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Nomina + "=" + Datos.P_No_Nomina);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Estatus + "='Aceptado'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + ".");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Estatus + "='Autorizado'");

                if (!String.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda_Incidencia) && !String.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda_Incidencia))
                {
                    Mi_SQL.Append(" AND ");
                    Mi_SQL.Append("(" + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_Fecha + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda_Incidencia + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                       " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda_Incidencia + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                }

                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Tiempo_Extra_Emp_Det.Tabla_Ope_Nom_Tiempo_Extra + "." +
                    Ope_Nom_Tiempo_Extra_Emp_Det.Campo_Empleado_ID + " IN ");

                //-----------------  [ SUBCONSULTA ]  ------------------
                Mi_Oracle = Mi_Oracle + "(SELECT " + Cat_Empleados.Campo_Empleado_ID;
                Mi_Oracle = Mi_Oracle + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                if (Mi_Oracle.Contains("WHERE"))
                {
                    Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }
                else
                {
                    Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_RFC))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Rol_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Programa_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Contrato_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Area_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Puesto_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Escolaridad_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Sindicado_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Turno_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Zona_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Trabajador_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda))
                {
                    if (Mi_Oracle.Contains("WHERE"))
                    {
                        Mi_Oracle += " AND " + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }

                Mi_Oracle = Mi_Oracle + ") ORDER BY " + Ope_Nom_Tiempo_Extra.Tabla_Ope_Nom_Tiempo_Extra + "." + Ope_Nom_Tiempo_Extra.Campo_No_Tiempo_Extra + " ASC";

                Mi_SQL = new StringBuilder(Mi_SQL.ToString() + Mi_Oracle);

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Empleados;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Rpt_Conceptos_Empleados
        /// DESCRIPCION : Consulta el los conceptos tanto percepciones y/o deducciones 
        ///               y los montos.  
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 9/Mayo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consulta_Rpt_Conceptos_Empleados(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            String Mi_SQL_Aux = String.Empty;
            DataTable DT_Conceptos_Empleados = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") As EMPLEADO, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Mensual_Actual + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, ");
                Mi_SQL.Append(Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Nombre + " AS SINDICATO, ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                    Cat_Nom_Percepcion_Deduccion.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo + " AS TIPO, ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                    Cat_Nom_Percepcion_Deduccion.Campo_Nombre + " AS CONCEPTO, ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                    Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + " AS TIPO_NOMINA, ");
                Mi_SQL.Append(Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre + " AS AREA, ");
                Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS FUENTE_FINANCIAMIENTO, ");
                Mi_SQL.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Descripcion + " AS  AREA_FUNCIONAL, ");
                Mi_SQL.Append(Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Nombre + " AS PROYECTO_PROGRAMA, ");
                Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS PARTIDA, ");
                Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " AS CLAVE_FUENTE_FINANCIAMIENTO, ");
                Mi_SQL.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Clave + " AS  CLAVE_AREA_FUNCIONAL, ");
                Mi_SQL.Append(Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Clave + " AS CLAVE_PROGRAMA, ");
                Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " AS CLAVE_PARTIDA, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " AS CLAVE_UNIDAD_RESPONSABLE, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Deducciones + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Gravado + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Exento + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dias_Trabajados + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Areas.Tabla_Cat_Areas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Area_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Area_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sindicato_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Sindicato_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Area_Responsable_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Programa_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Partida_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);

                Mi_SQL.Append(" RIGHT OUTER JOIN " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID);

                Mi_SQL.Append(" RIGHT OUTER JOIN " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);

                Mi_SQL.Append(" RIGHT OUTER JOIN " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " ON ");
                Mi_SQL.Append("(");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(")");
                Mi_SQL.Append(" WHERE ");

                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                    Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                    Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Datos.P_No_Nomina);

                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " IN ");

                Mi_SQL_Aux = Mi_SQL_Aux + "(SELECT " + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL_Aux = Mi_SQL_Aux + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                if (Mi_SQL_Aux.Contains("WHERE"))
                {
                    Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }
                else
                {
                    Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_RFC))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Rol_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Programa_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Contrato_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Area_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Puesto_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Escolaridad_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Sindicado_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Turno_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Zona_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Trabajador_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }
                Mi_SQL = new StringBuilder(Mi_SQL.ToString() + Mi_SQL_Aux);
                Mi_SQL.Append(") ORDER BY " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo);

                DT_Conceptos_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return DT_Conceptos_Empleados;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Rpt_Exentos_Gravados
        /// DESCRIPCION : Consulta el los conceptos tanto percepciones y/o deducciones 
        ///               y los montos.  
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 9/Mayo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consulta_Rpt_Exentos_Gravados(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            String Mi_SQL_Aux = String.Empty;
            DataTable DT_Conceptos_Empleados = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") As EMPLEADO, ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Deducciones + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Gravado + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Exento + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dias_Trabajados + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + " AS NOMINA, ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + ", ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + ", ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=" +
                    Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "=" +
                    Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" +
                    Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "=" +
                    Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);

                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                    Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                    Ope_Nom_Recibos_Empleados.Campo_No_Nomina + ">=" + (Datos.P_No_Nomina.Split(new Char[] { ',' })[0]));
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                    Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "<=" + (Datos.P_No_Nomina.Split(new Char[] { ',' })[1]));

                if (Mi_SQL.ToString().Contains("WHERE"))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                }
                else
                {
                    Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                }

                DT_Conceptos_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return DT_Conceptos_Empleados;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Rpt_Totales_Nomina
        /// 
        /// DESCRIPCION : Consulta los totales de cada concepto por nomina generada.
        ///               
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : Diciembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consultar_Rpt_Totales_Nomina(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Totales_Nomina = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + ".*, ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " ON ");
                Mi_SQL.Append(Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + "." + Ope_Nom_Totales_Nomina.Campo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_SQL.Append(Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + "." + Ope_Nom_Totales_Nomina.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + "." + Ope_Nom_Totales_Nomina.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("='" + Datos.P_Tipo_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + "." + Ope_Nom_Totales_Nomina.Campo_Nomina_ID);
                Mi_SQL.Append("='" + Datos.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + "." + Ope_Nom_Totales_Nomina.Campo_No_Nomina);
                Mi_SQL.Append(">=" + (Datos.P_No_Nomina.Trim().Split(new Char[] { ',' })[0]));
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + "." + Ope_Nom_Totales_Nomina.Campo_No_Nomina);
                Mi_SQL.Append(">=" + (Datos.P_No_Nomina.Trim().Split(new Char[] { ',' })[1]));

                Dt_Totales_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los totales de nomina. Error: [" + Ex.Message + "]");
            }
            return Dt_Totales_Nomina;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Columnas_Table_Totales
        /// 
        /// DESCRIPCION : Consultamos los metadatos de la tabla de OPE_NOM_TOTALES_NOMINA.
        ///               
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : Diciembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consultar_Columnas_Table_Totales()
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Columnas_Tabla_Totales = null;

            try
            {
                Mi_SQL.Append("SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS WHERE TABLE_NAME='" + Ope_Nom_Totales_Nomina.Tabla_Ope_Nom_Totales_Nomina + "'");
                Mi_SQL.Append(" ORDER BY COLUMN_NAME");

                Dt_Columnas_Tabla_Totales = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las columnas de la tabla de totales de nómina. Error: [" + Ex.Message + "]");
            }
            return Dt_Columnas_Tabla_Totales;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Movimientos_Empleados
        /// 
        /// DESCRIPCION : Consultamos los movimientos que ha tenido el empleado Alta, Baja y
        ///               Reactivaciones.
        ///               
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : Diciembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consultar_Movimientos_Empleados(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            StringBuilder Mi_SQL_Aux = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Mov_Empl = null;//Variable que almacenara los movimientos de los empleados.

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + ".*, ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") As EMPLEADO, ");
                Mi_SQL.Append(Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + "." + Cat_Emp_Movimientos_Det.Campo_No_Movimiento + ", ");
                Mi_SQL.Append(Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + "." + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento + ", ");
                Mi_SQL.Append(Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + "." + Cat_Emp_Movimientos_Det.Campo_Motivo_Movimiento + ", ");
                Mi_SQL.Append(Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + "." + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " AS FECHA, ");
                Mi_SQL.Append(Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + "." + Cat_Emp_Movimientos_Det.Campo_Sueldo_Actual + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS PUESTO, ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + " AS TIPO_NOMINA");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " ON ");
                Mi_SQL.Append(Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + "." + Cat_Emp_Movimientos_Det.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + "." + Cat_Emp_Movimientos_Det.Campo_Dependencia_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Puestos.Tabla_Cat_Puestos + " ON ");
                Mi_SQL.Append(Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + "." + Cat_Emp_Movimientos_Det.Campo_Puesto_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_SQL.Append(Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + "." + Cat_Emp_Movimientos_Det.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);


                Mi_SQL.Append(" WHERE ");

                Mi_SQL.Append(Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + "." + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento
                    + "='" + Datos.P_Tipo_Movimiento + "'");

                if (Mi_SQL.ToString().Contains("WHERE"))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                }
                else
                {
                    Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                }

                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " IN ");
                Mi_SQL_Aux.Append("(SELECT " + Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + "." +
                    Cat_Emp_Movimientos_Det.Campo_Empleado_ID);
                Mi_SQL_Aux.Append(" FROM " + Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det);

                if (!String.IsNullOrEmpty(Datos.P_Tipo_Movimiento))
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" AND " + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento + "='" + Datos.P_Tipo_Movimiento + "'");
                    else
                        Mi_SQL_Aux.Append(" WHERE " + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento + "='" + Datos.P_Tipo_Movimiento + "'");

                }

                if (!String.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                        Mi_SQL_Aux.Append(" AND " + Cat_Emp_Movimientos_Det.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                    else
                        Mi_SQL_Aux.Append(" WHERE " + Cat_Emp_Movimientos_Det.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                }

                if (!String.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda) && !String.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda))
                {
                    if (Mi_SQL_Aux.ToString().Contains("WHERE"))
                    {
                        Mi_SQL_Aux.Append(" AND " + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                        Mi_SQL_Aux.Append(" AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                    }
                    else
                    {
                        Mi_SQL_Aux.Append(" WHERE " + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                        Mi_SQL_Aux.Append(" AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                    }
                }

                Mi_SQL_Aux.Append(")");
                Mi_SQL = new StringBuilder(Mi_SQL.ToString() + Mi_SQL_Aux.ToString());

                Dt_Mov_Empl = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los movimientos de los empleados. Error: [" + Ex.Message + "]");
            }
            return Dt_Mov_Empl;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Total_Pago_Bancos_Tipo_Nomina
        /// 
        /// DESCRIPCION : Consulta el total que se pago por cada tipo de nomina.
        ///               
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : Diciembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consultar_Total_Pago_Bancos_Tipo_Nomina(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.
            DataTable Dt_Totales_Tipo_Nomina_Banco = null;//Variable que almacena los totales por tipo de nómina de cada banco.

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append("SUM(" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + ") AS TOTAL");


                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID);


                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID + "='" + Datos.P_Banco_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Datos.P_No_Nomina + "");

                if (Mi_SQL.ToString().Contains("WHERE"))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                }
                else
                {
                    Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                }

                Dt_Totales_Tipo_Nomina_Banco = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: [" + Ex.Message + "]");
            }
            return Dt_Totales_Tipo_Nomina_Banco;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Nomina_Personal
        /// 
        /// DESCRIPCION : Consulta del recibo de nomina por empleado.
        ///               
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : Diciembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consultar_Nomina_Personal(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            String Mi_SQL_Aux = String.Empty;
            DataTable DT_Conceptos_Empleados = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") As EMPLEADO, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Mensual_Actual + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Cuenta_Bancaria + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, ");
                Mi_SQL.Append(Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Nombre + " AS SINDICATO, ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                    Cat_Nom_Percepcion_Deduccion.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo + " AS TIPO, ");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                    Cat_Nom_Percepcion_Deduccion.Campo_Nombre + " AS CONCEPTO, ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." +
                    Ope_Nom_Recibos_Empleados_Det.Campo_Monto + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + " AS TIPO_NOMINA, ");
                Mi_SQL.Append(Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre + " AS AREA, ");
                Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS FUENTE_FINANCIAMIENTO, ");
                Mi_SQL.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Descripcion + " AS  AREA_FUNCIONAL, ");
                Mi_SQL.Append(Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Nombre + " AS PROYECTO_PROGRAMA, ");
                Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS PARTIDA, ");
                Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " AS CLAVE_FUENTE_FINANCIAMIENTO, ");
                Mi_SQL.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Clave + " AS  CLAVE_AREA_FUNCIONAL, ");
                Mi_SQL.Append(Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Clave + " AS CLAVE_PROGRAMA, ");
                Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " AS CLAVE_PARTIDA, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " AS CLAVE_UNIDAD_RESPONSABLE, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Deducciones + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Gravado + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Exento + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Dias_Trabajados + ", ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS CATEGORIA");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Puestos.Tabla_Cat_Puestos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Areas.Tabla_Cat_Areas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Area_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Area_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sindicato_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Sindicato_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Fuente_Financiamiento_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Area_Responsable_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_SAP_Area_Funcional.Tabla_Cat_SAP_Area_Funcional + "." + Cat_SAP_Area_Funcional.Campo_Area_Funcional_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Programa_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + "." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Partida_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);

                Mi_SQL.Append(" RIGHT OUTER JOIN " + Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Empleado_ID);

                Mi_SQL.Append(" RIGHT OUTER JOIN " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + " ON ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_No_Recibo);

                Mi_SQL.Append(" RIGHT OUTER JOIN " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + " ON ");
                Mi_SQL.Append("(");
                Mi_SQL.Append(Cat_Nom_Emp_Perc_Dedu_Deta.Tabla_Cat_Nom_Emp_Perc_Dedu_Det + "." + Cat_Nom_Emp_Perc_Dedu_Deta.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados_Det.Tabla_Ope_Nom_Recibos_Empleados_Det + "." + Ope_Nom_Recibos_Empleados_Det.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." + Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID);
                Mi_SQL.Append(")");

                Mi_SQL.Append(" WHERE ");

                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                    Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_ID + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." +
                    Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Datos.P_No_Nomina);

                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID + " IN ");

                Mi_SQL_Aux = Mi_SQL_Aux + "(SELECT " + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL_Aux = Mi_SQL_Aux + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                if (Mi_SQL_Aux.Contains("WHERE"))
                {
                    Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }
                else
                {
                    Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Empleado_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_RFC))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_RFC + "='" + Datos.P_RFC + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Estatus))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                             Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Rol_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Rol_ID + "='" + Datos.P_Rol_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Programa_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Programa_ID + "='" + Datos.P_Programa_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Contrato_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Tipo_Contrato_ID + "='" + Datos.P_Tipo_Contrato_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Area_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Area_ID + "='" + Datos.P_Area_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Puesto_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Escolaridad_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Escolaridad_ID + "='" + Datos.P_Escolaridad_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Sindicado_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Sindicato_ID + "='" + Datos.P_Sindicado_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Turno_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Turno_ID + "='" + Datos.P_Turno_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Zona_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Trabajador_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Tipo_Trabajador_ID + "='" + Datos.P_Tipo_Trabajador_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'";
                    }
                }

                if (!string.IsNullOrEmpty(Datos.P_Fecha_Inicio_Busqueda) && !string.IsNullOrEmpty(Datos.P_Fecha_Fin_Busqueda))
                {
                    if (Mi_SQL_Aux.Contains("WHERE"))
                    {
                        Mi_SQL_Aux += " AND " + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                    else
                    {
                        Mi_SQL_Aux += " WHERE " + Cat_Empleados.Campo_Fecha_Inicio + " BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')" +
                            " AND TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    }
                }
                Mi_SQL = new StringBuilder(Mi_SQL.ToString() + Mi_SQL_Aux);
                Mi_SQL.Append(") ORDER BY " + Cat_Nom_Percepcion_Deduccion.Tabla_Cat_Nom_Percepcion_Deduccion + "." +
                    Cat_Nom_Percepcion_Deduccion.Campo_Tipo);
                DT_Conceptos_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los totales de nomina. Error: [" + Ex.Message + "]");
            }
            return DT_Conceptos_Empleados;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Puestos_Dependencia
        /// 
        /// DESCRIPCION : Consulta los puestos que se encuentran disponibles u ocupados en 
        ///               las unidades responsables.
        ///               
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : Diciembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consultar_Puestos_Dependencia(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Puestos = null;
            Boolean Comprobacion_And = false;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " AS CLAVE_DEPENDENCIA, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS PUESTO, ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Clave + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Estatus + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Salario_Mensual + ", ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza + ", ");
                

                Mi_SQL.Append("(SELECT COUNT(" + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + ") FROM " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + " WHERE " + Cat_Nom_Dep_Puestos_Det.Campo_Estatus + "='" + Datos.P_Estatus + "'" + " AND " + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " AND " + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "=" + Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID + ") NO_PLAZAS, ");
                Mi_SQL.Append("(SELECT COUNT(" + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + ") FROM " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + " WHERE " + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + " AND " + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "=" + Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID + ") TOTAL_PLAZAS");
                
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Puestos.Tabla_Cat_Puestos + " ON ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " ON ");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);


                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    //if (Mi_SQL.ToString().Trim().Contains("WHERE"))
                    if (Comprobacion_And == true)
                    {
                        Mi_SQL.Append(" AND ");
                        Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");
                        Comprobacion_And = true;
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE ");
                        Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");
                        Comprobacion_And = true;
                    }
                }

                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    //if (Mi_SQL.ToString().Trim().Contains("WHERE"))
                    if (Comprobacion_And == true)
                    {
                        Mi_SQL.Append(" AND ");
                        Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                        Comprobacion_And = true;
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE ");
                        Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det + "." + Cat_Nom_Dep_Puestos_Det.Campo_Estatus + "='" + Datos.P_Estatus + "'");
                        Comprobacion_And = true;
                    }
                }

                Dt_Puestos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los puestos que le pertencen a la dependencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Puestos;
        }
        #endregion

        #region (Actualizar Puesto Dependencia)
        private static void Actualizar_Estatus_Puesto_Dependencia(String Puesto_ID, String Dependencia_ID, String Estatus,
            String Empleado_ID, Int32 Operacion, String Clave)
        {
            String Mi_SQL = String.Empty;

            try
            {
                Mi_SQL = "UPDATE " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det +
                         " SET " +
                         Cat_Nom_Dep_Puestos_Det.Campo_Estatus + "='" + Estatus + "'";

                if (Operacion == 0)
                    Mi_SQL += ", " + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "='" + Empleado_ID + "' ";
                else if (Operacion == 1)
                    Mi_SQL += ", " + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "=NULL ";

                Mi_SQL += " WHERE " + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Dependencia_ID + "'" +
                  " AND " + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "='" + Puesto_ID + "'";

                if (!String.IsNullOrEmpty(Clave))
                {
                    Mi_SQL += " AND " + Cat_Nom_Dep_Puestos_Det.Campo_Clave + "=" + Clave;
                }

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cambiar el estatus del puesto de la dependencia. Error: [" + Ex.Message + "]");
            }
        }

        private static void Actualizar_Estatus_Puesto_Dependencia(String Puesto_ID, String Puesto_ID_Anterior,
            String Dependencia_ID, String Dependencia_ID_Anterior, String Empleado_ID, String Clave)
        {
            String Mi_SQL = String.Empty;

            try
            {
                Mi_SQL = "UPDATE " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det +
                         " SET " +
                         Cat_Nom_Dep_Puestos_Det.Campo_Estatus + "='DISPONIBLE', " +
                         Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "=NULL " +
                         " WHERE " + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Dependencia_ID_Anterior + "'" +
                         " AND " + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "='" + Puesto_ID_Anterior + "'" +
                         " AND " + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "='" + Empleado_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);



                Mi_SQL = "UPDATE " + Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det +
                         " SET " +
                         Cat_Nom_Dep_Puestos_Det.Campo_Estatus + "='OCUPADO', " +
                         Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "='" + Empleado_ID + "' " +
                         " WHERE " + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Dependencia_ID + "'" +
                         " AND " + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "='" + Puesto_ID + "'" +
                         " AND " + Cat_Nom_Dep_Puestos_Det.Campo_Clave + "=" + Clave;

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cambiar el estatus del puesto de la dependencia. Error: [" + Ex.Message + "]");
            }
        }

        public static String Obtener_Puesto_ID(String Empleado_ID)
        {
            String Mi_SQL = String.Empty;
            DataTable Dt_Empleado = null;
            String Puesto_ID = String.Empty;

            try
            {
                Mi_SQL = "SELECT " + Cat_Empleados.Campo_Puesto_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Empleado_ID + "'";
                Dt_Empleado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (Dt_Empleado is DataTable)
                {
                    if (Dt_Empleado.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleado.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Puesto_ID].ToString().Trim()))
                                {
                                    Puesto_ID = EMPLEADO[Cat_Empleados.Campo_Puesto_ID].ToString().Trim();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el puesto que tiene el empleado. Error: [" + Ex.Message + "]");
            }
            return Puesto_ID;
        }

        private static String Obtener_Dependencia_ID(String Empleado_ID)
        {
            String Mi_SQL = String.Empty;
            DataTable Dt_Empleado = null;
            String Dependencia_ID = String.Empty;

            try
            {
                Mi_SQL = "SELECT " + Cat_Empleados.Campo_Dependencia_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Empleado_ID + "'";
                Dt_Empleado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                if (Dt_Empleado is DataTable)
                {
                    if (Dt_Empleado.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleado.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString().Trim()))
                                {
                                    Dependencia_ID = EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el puesto que tiene el empleado. Error: [" + Ex.Message + "]");
            }
            return Dependencia_ID;
        }

        #endregion

        #region (Consulta Finiquitos)
        internal static DataTable Consultar_Informacion_Mostrar_Finiquitos(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Empleados = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + " * 30.42) AS SALARIO_MENSUAL, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario_Integrado + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + "/8) AS COSTO_HORA, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Baja + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Ruta_Foto + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIAS, ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS PUESTO, ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion + "." + Cat_Nom_Indemnizacion.Campo_Nombre + " AS INDEMNIZACION, ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + " AS TIPO_NOMINA, ");
                Mi_SQL.Append(Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Nombre + " AS SINDICATO");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + " LEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "=");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID + "=");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Finiquito + "=");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion + "." + Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + "=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Nom_Sindicatos.Campo_Sindicato_ID + "=");
                Mi_SQL.Append(Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Sindicato_ID);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'");

                if (Mi_SQL.ToString().Contains("WHERE"))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                }
                else
                {
                    Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                }

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en la informacion mostrada en la generacion del finiquito. Error: [" + Ex.Message + "]");
            }
            return Dt_Empleados;
        }

        internal static DataTable Consultar_Informacion_Rpt_Finiquitos(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Empleados = null;

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append("UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS EMPLEADO, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + " * 30.42) AS SALARIO_MENSUAL, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario_Integrado + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + "/8) AS COSTO_HORA, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Baja + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Ruta_Foto + ", ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIAS, ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS PUESTO, ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion + "." + Cat_Nom_Indemnizacion.Campo_Nombre + " AS INDEMNIZACION, ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + " AS TIPO_NOMINA, ");
                Mi_SQL.Append(Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Nombre + " AS SINDICATO, ");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre + " AS BANCO");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + " LEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "=");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID + "=");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Finiquito + "=");
                Mi_SQL.Append(Cat_Nom_Indemnizacion.Tabla_Cat_Nom_Indemnizacion + "." + Cat_Nom_Indemnizacion.Campo_Indemnizacion_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + "=");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sindicato_ID + "=");
                Mi_SQL.Append(Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + "." + Cat_Nom_Sindicatos.Campo_Sindicato_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN ");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + " ON ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID + "=");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "'");

                if (Mi_SQL.ToString().Contains("WHERE"))
                {
                    Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                }
                else
                {
                    Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'");
                }

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error en la informacion mostrada en la generacion del finiquito. Error: [" + Ex.Message + "]");
            }
            return Dt_Empleados;
        }
        #endregion

        #region (Reloj Checador)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Reloj_Checador_Empleado
        /// DESCRIPCION : Consulta los datos generales del empleado con respecto a la 
        ///               información del reloj checador
        /// PARAMETROS  : Datos: Obtiene el ID del empleado a consultar su información
        /// CREO        : Yazmin Abigail Delgado Gómez
        /// FECHA_CREO  : 20-Septiembre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static DataTable Consulta_Datos_Reloj_Checador_Empleado(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la variable de consulta de los datos del empleado
            try
            {
                //Actualiza los datos del empleado con respecto a su reloj checador
                Mi_SQL.Append("SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + ", ");
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append("||' '||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append("||' '||" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS Empleado, ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Reloj_Checador + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Inicio_Reloj_Checador + ", ");
                Mi_SQL.Append("(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS Unidad_Responsable, ");
                Mi_SQL.Append(Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID + ", ");
                Mi_SQL.Append(Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Clave);
                Mi_SQL.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_SQL.Append(" ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador);
                Mi_SQL.Append(" ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Reloj_Checador_ID);
                Mi_SQL.Append(" = " + Cat_Nom_Reloj_Checador.Tabla_Cat_Nom_Reloj_Checador + "." + Cat_Nom_Reloj_Checador.Campo_Reloj_Checador_ID);
                Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        #endregion

        #region (Reloj Checador)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Modificacion_Reloj_Checador
        /// DESCRIPCION : Registra los datos del reloj checador del empleado en donde se
        ///               indicara la fecha en que inicia a checar el empleado y en que
        ///               reloj checador lo realizara
        /// PARAMETROS  : Datos: Obtiene los datos del reloj checador a registrar
        /// CREO        : Yazmin Abigail Delgado Gómez
        /// FECHA_CREO  : 20-Septiembre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Modificacion_Reloj_Checador(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Obtiene la variable a realizar la modificación de los datos del empleado
            try
            {
                //Actualiza los datos del empleado con respecto a su reloj checador
                Mi_SQL.Append("UPDATE " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" SET " + Cat_Empleados.Campo_Reloj_Checador + " = '" + Datos.P_Reloj_Checador + "', ");
                //Si los datos del reloj checador no vienen vacios entonces son asignados estos datos al empleado
                if (!String.IsNullOrEmpty(Datos.P_Reloj_Checator_ID))
                {
                    Mi_SQL.Append(Cat_Empleados.Campo_Reloj_Checador_ID + " = '" + Datos.P_Reloj_Checator_ID + "', ");
                    Mi_SQL.Append(Cat_Empleados.Campo_Fecha_Inicio_Reloj_Checador + " = TO_DATE('" + String.Format("{0:MM/dd/yyyy}", Datos.P_Fecha_Inicio_Reloj_Checador) + "','MM/DD/YYYY'), ");
                }
                //Si los datos vienen vacios entonces limpia los datos del reloj checador del empleado
                else
                {
                    Mi_SQL.Append(Cat_Empleados.Campo_Reloj_Checador_ID + " = NULL, ");
                    Mi_SQL.Append(Cat_Empleados.Campo_Fecha_Inicio_Reloj_Checador + " = NULL, ");
                }
                Mi_SQL.Append(Cat_Empleados.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                Mi_SQL.Append(Cat_Empleados.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        #endregion

        #region "ESPECIAL?"
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Empleados_Especial
        /// DESCRIPCION : Consulta los empleados que cumplen con los requerimientos.
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 7/Octubre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Empleados_Especial(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL;    //Variable para la consulta para el Empleado
            try
            {
                //Consulta todos los Empleados que coincidan con lo proporcionado por el usuario
                Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + ", " + Cat_Empleados.Campo_No_Empleado + ", ";
                Mi_SQL += Cat_Empleados.Campo_RFC + ", " + Cat_Empleados.Campo_Estatus + ", ";
                Mi_SQL += "" + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL += "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL += "||' '||" + Cat_Empleados.Campo_Nombre + " AS EMPLEADO";
                Mi_SQL += " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL += " WHERE (" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||";
                Mi_SQL += Cat_Empleados.Campo_Apellido_Materno + " LIKE UPPER ('%" + Datos.P_Nombre + "%'))";
                Mi_SQL += " OR (" + Cat_Empleados.Campo_No_Empleado + " = '" + String.Format("{0:000000}", Convert.ToInt16(Datos.P_Nombre)) + "')";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        #endregion

        #endregion



        private static Boolean Cambio_Tipo_Nomina(String No_Empleado, String Tipo_Nomina_ID)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;
            Cls_Cat_Tipos_Nominas_Negocio INF_TIPO_NOMINA = null;
            Boolean Cambio = true;

            try
            {
                INF_EMPLEADO = Consultar_Informacion_Empleado(No_Empleado);
                INF_TIPO_NOMINA = Consultar_Tipo_Nomina(INF_EMPLEADO.P_Tipo_Nomina_ID);

                if (INF_TIPO_NOMINA is Cls_Cat_Tipos_Nominas_Negocio)
                {
                    if (INF_TIPO_NOMINA.P_Tipo_Nomina_ID.Trim().Equals(Tipo_Nomina_ID))
                    {
                        Cambio = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al validar si el empleado cambio de tipo de nomina. Error: [" + Ex.Message + "]");
            }
            return Cambio;
        }
        private static Boolean Cambio_Sindicato(String No_Empleado, String Sindicato_ID)
        {
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;
            Cls_Cat_Nom_Sindicatos_Negocio INF_SINDICATO = null;
            Boolean Cambio = true;

            try
            {
                if (!String.IsNullOrEmpty(Sindicato_ID))
                {
                    INF_EMPLEADO = Consultar_Informacion_Empleado(No_Empleado);
                    INF_SINDICATO = Consultar_Sindicato(INF_EMPLEADO.P_Sindicado_ID);

                    if (INF_SINDICATO is Cls_Cat_Nom_Sindicatos_Negocio)
                    {
                        if (INF_SINDICATO.P_Sindicato_ID.Trim().Equals(Sindicato_ID))
                        {
                            Cambio = false;
                        }
                    }
                }
                else Cambio = false;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al validar si el empleado cambio de sindicato. Error: [" + Ex.Message + "]");
            }
            return Cambio;
        }
        /// ***********************************************************************************
        /// Nombre: Consultar_Tipo_Nomina
        /// 
        /// Descripción: Consulta la información del tipo de nómina a la que pertence el empleado.
        /// 
        /// Parámetros: Tipo_Nomina_ID.- identificador del tipo de nómina.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 16/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        private static Cls_Cat_Tipos_Nominas_Negocio Consultar_Tipo_Nomina(String Tipo_Nomina_ID)
        {
            Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión a la capa de negocios.
            Cls_Cat_Tipos_Nominas_Negocio INF_TIPO_NOMINA = new Cls_Cat_Tipos_Nominas_Negocio();//Variable que almacena la información del tipo de nómina.
            DataTable Dt_Tipo_Nomina = null;//Variable que almacena el registro del tipo de nómina búscado.

            try
            {
                Obj_Tipos_Nominas.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Dt_Tipo_Nomina = Obj_Tipos_Nominas.Consulta_Datos_Tipo_Nomina();//Consultamos la información del tipo de nómina.

                if (Dt_Tipo_Nomina is DataTable)
                {
                    if (Dt_Tipo_Nomina.Rows.Count > 0)
                    {
                        foreach (DataRow TIPO_NOMINA in Dt_Tipo_Nomina.Rows)
                        {
                            if (TIPO_NOMINA is DataRow)
                            {

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Prima_Vacacional].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Exenta_Prima_Vacacional = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Prima_Vacacional].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Aguinaldo].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Exenta_Aguinaldo = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Aguinaldo].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Aguinaldo].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Aguinaldo = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Aguinaldo].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Prima_Vacacional_1 = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Prima_Vacacional_2 = Convert.ToDouble(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString().Trim());

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Dias_Prima_Antiguedad = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad].ToString().Trim();

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Aplica_ISR = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR].ToString().Trim();

                                if (!String.IsNullOrEmpty(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString().Trim()))
                                    INF_TIPO_NOMINA.P_Tipo_Nomina_ID = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString().Trim();

                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Errro al consultar la información del empleado. Error: [" + Ex.Message + "]");
            }
            return INF_TIPO_NOMINA;
        }
        /// ***********************************************************************************
        /// Nombre: Consultar_Tipo_Nomina
        /// 
        /// Descripción: Consulta la información del tipo de nómina a la que pertence el empleado.
        /// 
        /// Parámetros: Sindicato_ID.- identificador del sindicato.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 16/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        private static Cls_Cat_Nom_Sindicatos_Negocio Consultar_Sindicato(String Sindicato_ID)
        {
            Cls_Cat_Nom_Sindicatos_Negocio Obj_Sindicato = new Cls_Cat_Nom_Sindicatos_Negocio();//Variable de conexión a la capa de negocios.
            Cls_Cat_Nom_Sindicatos_Negocio INF_SINDICATO = new Cls_Cat_Nom_Sindicatos_Negocio();//Variable que almacena la información del tipo de nómina.
            DataTable Dt_Sindicato = null;//Variable que almacena el registro del tipo de nómina búscado.

            try
            {
                Obj_Sindicato.P_Sindicato_ID = Sindicato_ID;
                Dt_Sindicato = Obj_Sindicato.Consulta_Datos_Sindicato();//Consultamos la información del tipo de nómina.

                if (Dt_Sindicato is DataTable)
                {
                    if (Dt_Sindicato.Rows.Count > 0)
                    {
                        foreach (DataRow SINDICATO in Dt_Sindicato.Rows)
                        {
                            if (SINDICATO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(SINDICATO[Cat_Nom_Sindicatos.Campo_Sindicato_ID].ToString().Trim()))
                                    INF_SINDICATO.P_Sindicato_ID = SINDICATO[Cat_Nom_Sindicatos.Campo_Sindicato_ID].ToString().Trim();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Errro al consultar la información del sindicato. Error: [" + Ex.Message + "]");
            }
            return INF_SINDICATO;
        }
        /// ***********************************************************************************
        /// Nombre: Consultar_Informacion_Empleado
        /// 
        /// Descripción: Consulta la información general del empleado.
        /// 
        /// Parámetros: No_Empleado.- Identificador interno del sistema para las operaciones que
        ///                           se realizan sobre los empelados.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 16/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// ***********************************************************************************
        private static Cls_Cat_Empleados_Negocios Consultar_Informacion_Empleado(String No_Empleado)
        {
            Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
            Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();//Variable que almacenara la información del empleado.
            DataTable Dt_Empleado = null;//Variable que almacena el registro búscado del empleado.

            try
            {
                Obj_Empleados.P_No_Empleado = No_Empleado;
                Dt_Empleado = Obj_Empleados.Consulta_Empleados_General();//Consultamos la información del empleado.

                if (Dt_Empleado is DataTable)
                {
                    if (Dt_Empleado.Rows.Count > 0)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleado.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Tipo_Nomina_ID = EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Zona_ID = EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim()))
                                    INF_EMPLEADO.P_Salario_Diario = Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Terceros_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Terceros_ID = EMPLEADO[Cat_Empleados.Campo_Terceros_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Sindicato_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Sindicado_ID = EMPLEADO[Cat_Empleados.Campo_Sindicato_ID].ToString().Trim();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las información del empleado. Error: [" + Ex.Message + "]");
            }
            return INF_EMPLEADO;
        }
        public static Boolean Cambiar_Password(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_SQL = String.Empty;
            Boolean Estatus = false;

            try
            {
                Mi_SQL = "UPDATE " + Cat_Empleados.Tabla_Cat_Empleados + " SET ";
                Mi_SQL += Cat_Empleados.Campo_Password + "='" + Datos.P_Password + "'";
                Mi_SQL += " WHERE ";
                Mi_SQL += Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Estatus = true;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Estatus;
        }




        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Empleados_General
        /// DESCRIPCION : Consulta los empelados del sistema.        
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 13/Diciembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable JSON_Consulta_Empleados_General(Cls_Cat_Empleados_Negocios Datos)
        {
            String Mi_Oracle = "";//Variable que alamcenara la consulta.
            DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.
            try
            {
                Mi_Oracle = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + ".*, ";

                Mi_Oracle = Mi_Oracle + "(" + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_Oracle = Mi_Oracle + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_Oracle = Mi_Oracle + "||' '||" + Cat_Empleados.Campo_Nombre + ") AS Empleado, ";

                Mi_Oracle = Mi_Oracle + "('[' || " + Cat_Empleados.Campo_No_Empleado + " || '] -- ' || " + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_Oracle = Mi_Oracle + " ||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                Mi_Oracle = Mi_Oracle + " ||' '|| " + Cat_Empleados.Campo_Nombre + ") AS EMPLEADOS, ";

                Mi_Oracle = Mi_Oracle + "(SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "=" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + ") AS UR";

                Mi_Oracle = Mi_Oracle + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";

                if (!string.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_Oracle += " AND " + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'";
                }

                if (!string.IsNullOrEmpty(Datos.P_Nombre))
                {
                    Mi_Oracle += " AND UPPER(" + Cat_Empleados.Campo_Nombre + "||' '||" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||" +
                         Cat_Empleados.Campo_Apellido_Materno + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                }

                Mi_Oracle += " ORDER BY " + Cat_Empleados.Campo_Apellido_Paterno + ", " + Cat_Empleados.Campo_Apellido_Materno + ", " + Cat_Empleados.Campo_Nombre + " ASC";

                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Dt_Empleados;
        }
        public static String Consultar_No_Empleado_Consecutivo()
        {
            String Mi_SQL;                              //Obtiene la cadena de inserción hacía la base de datos
            Object No_Empleado;                         //Obtiene el ID con la cual se guardo los datos en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            String Str_No_Empleado = String.Empty;

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                //Consulta para la obtención del último ID dado de alta en el  catálogo de empleados
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Empleados.Campo_No_Empleado + "),'000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Empleados.Tabla_Cat_Empleados;

                Comando_SQL.CommandText = Mi_SQL; //Realiza la ejecuón de la obtención del ID del empleado
                No_Empleado = Comando_SQL.ExecuteScalar();

                //Valida si el ID es nulo para asignarle automaticamente el primer registro
                if (Convert.IsDBNull(No_Empleado))
                {
                    Str_No_Empleado = "000001";
                }
                //Si no esta vacio el registro entonces al registro que se obtenga se le suma 1 para poder obtener el último registro
                else
                {
                    Str_No_Empleado = String.Format("{0:000000}", Convert.ToInt32(No_Empleado) + 1);
                }
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            }
            return Str_No_Empleado;
        }

        public static Boolean Actualizar_Datos_Bancarios_Empleado(Cls_Cat_Empleados_Negocios Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Boolean Operacion_Compleata = false;

            try
            {
                Mi_SQL.Append("UPDATE ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" SET ");
                Mi_SQL.Append(Cat_Empleados.Campo_No_Tarjeta + "='" + Datos.P_No_Tarjeta + "', ");
                Mi_SQL.Append(Cat_Empleados.Campo_Banco_ID + "='" + Datos.P_Banco_ID + "', ");
                Mi_SQL.Append(Cat_Empleados.Campo_Forma_Pago + "='" + Datos.P_Forma_Pago + "', ");
                Mi_SQL.Append(Cat_Empleados.Campo_No_Cuenta_Bancaria + "='" + Datos.P_No_Cuenta_Bancaria + "'");
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Compleata = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al actualizar los datos bancarios del empleado. Error: [" + Ex.Message + "]");
            }
            return Operacion_Compleata;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Documentos
        /// DESCRIPCION : Consulta los documentos de los empleados.        
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : Enero/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Documentos(Cls_Cat_Empleados_Negocios Datos) {

            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacena la consulta.
            DataTable Dt_Requisitos = null;//Variable que almacena el listado de requisitos.

            try
            {
                Mi_SQL.Append(" select ");

                Mi_SQL.Append("(select (" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") "); 
                Mi_SQL.Append(" from " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" where " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Ope_Nom_Requisitos_Empleado.Tabla_Ope_Nom_Requisitos_Empleado + "." + Ope_Nom_Requisitos_Empleado.Campo_Empleado_ID + ") as EMPLEADO, ");

                Mi_SQL.Append("(select " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados + "." + Cat_Nom_Requisitos_Empleados.Campo_Nombre);
                Mi_SQL.Append(" from " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados);
                Mi_SQL.Append(" where " + Cat_Nom_Requisitos_Empleados.Tabla_Cat_Nom_Requisitos_Empleados + "." + Cat_Nom_Requisitos_Empleados.Campo_Requisito_ID + "=");
                Mi_SQL.Append(Ope_Nom_Requisitos_Empleado.Tabla_Ope_Nom_Requisitos_Empleado + "." + Ope_Nom_Requisitos_Empleado.Campo_Requisitos_ID + ") as REQUISITO, ");

                Mi_SQL.Append(Ope_Nom_Requisitos_Empleado.Campo_Ruta_Documento);

                Mi_SQL.Append(" from ");
                Mi_SQL.Append(Ope_Nom_Requisitos_Empleado.Tabla_Ope_Nom_Requisitos_Empleado);

                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Ope_Nom_Requisitos_Empleado.Campo_Empleado_ID + "='" + Datos.P_Empleado_ID + "'");
                Mi_SQL.Append(" order by ");
                Mi_SQL.Append(Ope_Nom_Requisitos_Empleado.Campo_Nombre + " ASC ");

                Dt_Requisitos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cosnultar los requisitos del empleado. Error: [" + Ex.Message + "]");
            }
            return Dt_Requisitos;
        }
    }
}