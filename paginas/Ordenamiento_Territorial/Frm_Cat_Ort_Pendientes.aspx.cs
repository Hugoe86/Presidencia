using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Ordenamiento_Territorial_Ficha_Revision.Negocio;
using Presidencia.Ordenamiento_Territorial_Zonas.Negocio;
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;

public partial class paginas_Ordenamiento_Territorial_Frm_Cat_Ort_Pendientes : System.Web.UI.Page
{
    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN         : Metodo que se carga cada que ocurre un PostBack de la Página
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {

                Tr_Proceso.Visible = false;
                Consultar_Fichas_Revision();
                Consultar_Pendientes();
            }
        }

    #endregion

    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Fichas_Revision
        ///DESCRIPCIÓN         : Metodo que se consulta las fichas de un usuario
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        public void Consultar_Fichas_Revision()
        {
            Cls_Ope_Ort_Ficha_Revision_Negocio Ficha_Revision_Negocio = new Cls_Ope_Ort_Ficha_Revision_Negocio();
            Cls_Cat_Ort_Zona_Negocio Zona_Negocio = new Cls_Cat_Ort_Zona_Negocio();
            Int16 Contador_Fichas = 0;

            DataTable Dt_Fichas_Revision = Ficha_Revision_Negocio.Consultar_Tabla_Ficha_Revision();
            DataTable Dt_Zonas = Zona_Negocio.Consultar_Zonas();

            try
            {
                foreach (DataRow Dr_Zona in Dt_Zonas.Rows)
                {
                    if (Dr_Zona[Cat_Ort_Zona.Campo_Empleado_ID].ToString() == Cls_Sessiones.Empleado_ID)
                    {
                        foreach (DataRow Dr_Ficha in Dt_Fichas_Revision.Rows)
                        {
                            if (Dr_Ficha[Ope_Ort_Ficha_Revision.Campo_Zona_ID].ToString() == Dr_Zona[Cat_Ort_Zona.Campo_Zona_ID].ToString())
                            {
                                if (String.IsNullOrEmpty(Dr_Ficha[Ope_Ort_Ficha_Revision.Campo_Respuesta].ToString()))
                                    Contador_Fichas++;
                            }
                        }
                    }
                }
                Lbl_Fichas_Pendientes.Text = "( " + Contador_Fichas.ToString() + " )";
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pendientes
        ///DESCRIPCIÓN         : Metodo que se consulta los pendientes de un usuario
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        public void Consultar_Pendientes()
        {
            Session["Dt_Pendientes"] = null;
            try
            {
                Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
                String Dependencia_ID_Ordenamiento = "";
                String Dependencia_ID_Ambiental = "";
                String Dependencia_ID_Urbanistico = "";
                String Dependencia_ID_Inmobiliario = "";
                String Dependencia_ID_Catastro = "";
                String Inspector_Ordenamiento = "";
                String Rol_Director_Ordenamiento = "";

                // consultar parámetros
                Obj_Parametros.Consultar_Parametros();

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                    Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                    Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                    Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                    Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Catastro))
                    Dependencia_ID_Catastro = Obj_Parametros.P_Dependencia_ID_Catastro;


                if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Inspector_Ordenamiento))
                    Inspector_Ordenamiento = Obj_Parametros.P_Rol_Inspector_Ordenamiento;


                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                {
                    Rol_Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;
                    if (Cls_Sessiones.Rol_ID == Obj_Parametros.P_Rol_Director_Ambiental ||
                        Cls_Sessiones.Rol_ID == Obj_Parametros.P_Rol_Director_Fraccionamientos ||
                        Cls_Sessiones.Rol_ID == Obj_Parametros.P_Rol_Director_Urbana)
                        Consultar_Pendientes_Dependencia();
                }

                Cls_Ope_Bandeja_Tramites_Negocio Tramites = new Cls_Ope_Bandeja_Tramites_Negocio();

                if (Cls_Sessiones.Empleado_ID != "")
                {
                    //  filtro para el rol del director de ordenamiento
                    if (Rol_Director_Ordenamiento == Cls_Sessiones.Rol_ID)
                    {
                        Tramites.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                        Tramites.P_Rol_ID = Cls_Sessiones.Rol_ID;
                    }
                    else
                    {
                        Tramites.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                        Tramites.P_Dependencia_ID = String.Empty;
                    }
                }

                if (Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Ordenamiento
                    || Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Ambiental
                    || Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Inmobiliario
                    || Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Urbanistico)
                {
                    Tramites.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado;

                    if (Cls_Sessiones.Rol_ID == Inspector_Ordenamiento)
                    {
                        Tramites.P_Estatus_Persona_Inspecciona = Cls_Sessiones.Empleado_ID;
                    }

                }
                Tramites.P_Tipo_DataTable = "BANDEJA_TRAMITES";
                Tramites.P_Estatus = "PENDIENTE_PROCESO";
                DataTable Tabla = Tramites.Consultar_DataTable();

                Session["Dt_Pendientes"] = Tabla; //guardamos los datos de la tabla en una session

                if (Tabla != null)
                {
                    Lbl_Solicitudes_Pendientes.Text = "( " + Tabla.Rows.Count.ToString() + " )";
                }
                else 
                {
                    Lbl_Solicitudes_Pendientes.Text = "( 0 )";
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Pendientes_Dependencia
        ///DESCRIPCIÓN         : Metodo que se consulta los pendientes por dependencia
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        public void Consultar_Pendientes_Dependencia()
        {
            Session["GRID_BANDEJA_TRAMITES"] = null;
            try
            {
                Cls_Ope_Bandeja_Tramites_Negocio Solicitudes = new Cls_Ope_Bandeja_Tramites_Negocio();

                Solicitudes.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado;
                Solicitudes.P_Estatus = "PROCESO";

                DataTable Dt_Solicitudes = Solicitudes.Consultar_Solicitudes_Dependencia();
                Session["GRID_BANDEJA_TRAMITES"] = Dt_Solicitudes; //guardamos los datos de las solicitudes en proceso en una session

                if (Dt_Solicitudes != null)
                {
                    Lbl_No_Solicitudes_Proceso.Text = "( " + Dt_Solicitudes.Rows.Count.ToString() + " )";
                }
                else 
                {
                    Lbl_No_Solicitudes_Proceso.Text = "( 0 )";
                }
                Tr_Proceso.Visible = true;
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Fichas_Pendientes_Click
        ///DESCRIPCIÓN         : Evento que se produce al dar click en el boton
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Btn_Fichas_Pendientes_Click(object sender, EventArgs e)
        {
            Response.Redirect("./Frm_Ope_Ort_Ficha_Revision.aspx");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Solicitudes_Proceso_Click
        ///DESCRIPCIÓN         : Evento que se produce al dar click en el boton
        ///PARÁMETROS          :
        ///CREO                : Leslie González Vázquez
        ///FECHA_CREO          : 10/Octubre/2012 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Btn_Solicitudes_Proceso_Click(object sender, EventArgs e)
        {
            DataTable Dt_Sol_Proceso = new DataTable();
            Dt_Sol_Proceso = (DataTable)Session["GRID_BANDEJA_TRAMITES"];
            Session["Dt_Proceso"] = null;

            if (Dt_Sol_Proceso != null)
            {
                if (Dt_Sol_Proceso.Rows.Count > 0)
                {
                    Response.Redirect("../Tramites/Frm_Bandeja_Tramites.aspx?Estatus=Proceso");
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('No existen datos para mostrar en la pantalla');", true);
                }
            }
            else 
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('No existen datos para mostrar en la pantalla');", true);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Solicitudes_Pendientes_Click
        ///DESCRIPCIÓN         : Evento que se produce al dar click en el boton
        ///PARÁMETROS          :
        ///CREO                : Salvador Vázquez Camacho
        ///FECHA_CREO          : 30/Julio/2010 
        ///MODIFICO            :
        ///FECHA_MODIFICO      :
        ///CAUSA_MODIFICACIÓN  :
        ///*******************************************************************************
        protected void Btn_Solicitudes_Pendientes_Click(object sender, EventArgs e)
        {
            DataTable Dt_Sol_Pendientes = new DataTable();
            Dt_Sol_Pendientes = (DataTable)Session["Dt_Pendientes"];
            Session["Dt_Pendientes"] = null;
            Session["GRID_BANDEJA_TRAMITES"] = null;

            if (Dt_Sol_Pendientes != null)
            {
                if (Dt_Sol_Pendientes.Rows.Count > 0)
                {
                    Response.Redirect("../Tramites/Frm_Bandeja_Tramites.aspx?Estatus=Pendiente_Proceso");
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('No existen datos para mostrar en la pantalla');", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('No existen datos para mostrar en la pantalla');", true);
            }
        }

    #endregion
}
