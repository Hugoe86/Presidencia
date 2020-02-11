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
using Presidencia.Empleados_No_Nominales.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

namespace Presidencia.Empleados_No_Nominales.Datos
{
    public class Cls_Cat_Nom_Empleados_No_Nominales_Datos
    {
        #region (Métodos)

        #region (Operación)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Empleado
        /// DESCRIPCION : 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///               2. Da de Alta el Empleado en la BD con los datos proporcionados por el usuario
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 07-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static void Alta_Empleado(Cls_Cat_Nom_Empleados_No_Nominales_Negocio Datos)
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
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Confronto+ ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Tipo_Empleado + ") VALUES (";

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

                if (!String.IsNullOrEmpty(Datos.Confronto))
                {
                    Mi_SQL = Mi_SQL + "'" + Datos.Confronto + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + "NULL, ";
                }

                Mi_SQL = Mi_SQL + " 'EMPLEADO')";

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
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 07-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static void Modificar_Empleado(Cls_Cat_Nom_Empleados_No_Nominales_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos
            String Puesto_ID = String.Empty;
            String Dependencia_ID = String.Empty;

            try
            {
                //Consulta para la modificación del Día Festivo con los datos proporcionados por el usuario
                Mi_SQL = "UPDATE " + Cat_Empleados.Tabla_Cat_Empleados + " SET ";

                if (Datos.Confronto != null)
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Confronto + " = '" + Datos.Confronto + "', ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Area_ID + " = NULL, ";
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

                if (String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Vigencia_Licencia) != "01/01/0001")
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia + " = TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Vigencia_Licencia) + "','DD/MM/YY'), ";
                }
                else
                {
                    Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Vencimiento_Licencia + " = NULL, ";
                }

                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 13-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        internal static void Eliminar_Empleado(Cls_Cat_Nom_Empleados_No_Nominales_Negocio Datos)
        {
            String Mi_SQL; //Variable de Consulta para la eliminación del Empleado

            try
            {
                Mi_SQL = "UPDATE " + Cat_Empleados.Tabla_Cat_Empleados + " SET ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Estatus + "='" + Datos.P_Estatus + "' WHERE ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
        #endregion

        #region (Consultas)
        ///***************************************************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Empleados_General
        /// 
        /// DESCRIPCION : Consulta los empelados que no aplicaran nominalmente.
        /// 
        /// CREO        : Juan Alberto Hernández Negrete
        /// FECHA_CREO  : 8/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///***************************************************************************************************************
        public static Cls_Cat_Nom_Empleados_No_Nominales_Negocio Consulta_Empleados_General(Cls_Cat_Nom_Empleados_No_Nominales_Negocio Datos)
        {
            String Mi_Oracle = "";//Variable que alamcenara la consulta.
            DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados.
            Cls_Cat_Nom_Empleados_No_Nominales_Negocio INF_EMPLEADO = new Cls_Cat_Nom_Empleados_No_Nominales_Negocio();

            try
            {
                Mi_Oracle = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + ".*, ";

                Mi_Oracle = Mi_Oracle + "(" + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_Oracle = Mi_Oracle + "||' '||" + Cat_Empleados.Campo_Apellido_Materno;
                Mi_Oracle = Mi_Oracle + "||' '||" + Cat_Empleados.Campo_Nombre + ") AS Empleado";

                Mi_Oracle = Mi_Oracle + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;

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

                if (Mi_Oracle.Contains("WHERE"))
                {
                    Mi_Oracle += " AND " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }
                else
                {
                    Mi_Oracle += " WHERE " + Cat_Empleados.Campo_Tipo_Empleado + "='EMPLEADO'";
                }


                Dt_Empleados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
                INF_EMPLEADO.P_Dt_Empleados = Dt_Empleados;

                if (Dt_Empleados is DataTable)
                {
                    if (Dt_Empleados.Rows.Count == 1)
                    {
                        foreach (DataRow EMPLEADO in Dt_Empleados.Rows)
                        {
                            if (EMPLEADO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim()))
                                    INF_EMPLEADO.P_No_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Apellido_Paterno].ToString().Trim()))
                                    INF_EMPLEADO.P_Apellido_Paterno = EMPLEADO[Cat_Empleados.Campo_Apellido_Paterno].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Apellido_Materno].ToString().Trim()))
                                    INF_EMPLEADO.P_Apelldo_Materno = EMPLEADO[Cat_Empleados.Campo_Apellido_Materno].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Nombre].ToString().Trim()))
                                    INF_EMPLEADO.P_Nombre = EMPLEADO[Cat_Empleados.Campo_Nombre].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_RFC].ToString().Trim()))
                                    INF_EMPLEADO.P_RFC = EMPLEADO[Cat_Empleados.Campo_RFC].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_CURP].ToString().Trim()))
                                    INF_EMPLEADO.P_CURP = EMPLEADO[Cat_Empleados.Campo_CURP].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Calle].ToString().Trim()))
                                    INF_EMPLEADO.P_Calle = EMPLEADO[Cat_Empleados.Campo_Calle].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Colonia].ToString().Trim()))
                                    INF_EMPLEADO.P_Colonia = EMPLEADO[Cat_Empleados.Campo_Colonia].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Codigo_Postal].ToString().Trim()))
                                    INF_EMPLEADO.P_Codigo_Postal = Convert.ToInt32(EMPLEADO[Cat_Empleados.Campo_Codigo_Postal].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Ciudad].ToString().Trim()))
                                    INF_EMPLEADO.P_Ciudad = EMPLEADO[Cat_Empleados.Campo_Ciudad].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Estado].ToString().Trim()))
                                    INF_EMPLEADO.P_Estado = EMPLEADO[Cat_Empleados.Campo_Estado].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Sexo].ToString().Trim()))
                                    INF_EMPLEADO.P_Sexo = EMPLEADO[Cat_Empleados.Campo_Sexo].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Fecha_Nacimiento].ToString().Trim()))
                                    INF_EMPLEADO.P_Fecha_Nacimiento = Convert.ToDateTime(EMPLEADO[Cat_Empleados.Campo_Fecha_Nacimiento].ToString().Trim());

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Estatus].ToString().Trim()))
                                    INF_EMPLEADO.P_Estatus = EMPLEADO[Cat_Empleados.Campo_Estatus].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Dependencia_ID = EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Area_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Area_ID = EMPLEADO[Cat_Empleados.Campo_Area_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Rol_ID].ToString().Trim()))
                                    INF_EMPLEADO.P_Rol_ID = EMPLEADO[Cat_Empleados.Campo_Rol_ID].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Comentarios].ToString().Trim()))
                                    INF_EMPLEADO.P_Comentarios = EMPLEADO[Cat_Empleados.Campo_Comentarios].ToString().Trim();

                                if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Confronto].ToString().Trim()))
                                    INF_EMPLEADO.Confronto = EMPLEADO[Cat_Empleados.Campo_Confronto].ToString().Trim();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return INF_EMPLEADO;
        }
        #endregion

        #endregion
    }
}
