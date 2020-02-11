using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Turnos.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Turnos : System.Web.UI.Page
{
    
    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 28/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack) {
                Configuracion_Formulario(true);
                Llenar_Grid_Turnos(0);
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion
    
    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. Estatus.    Estatus en el que se cargara la configuración de los
        ///                            controles.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 28/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario(Boolean Estatus){
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Eliminar.Visible = Estatus;
            Txt_Comentarios.Enabled = !Estatus;
            Txt_Nombre.Enabled = !Estatus;
            Txt_Hora_Inicio.Enabled = !Estatus;
            Txt_Hora_Fin.Enabled = !Estatus;
            Grid_Turnos.Enabled = Estatus;
            Grid_Turnos.SelectedIndex = (-1);
            Btn_Buscar_Turno.Enabled = Estatus;
            Txt_Busqueda_Turno.Enabled = Estatus;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo
        ///FECHA_CREO: 28/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo()
        {
            Txt_Comentarios.Text = "";
            Txt_ID_Turno.Text = "";
            Hdf_Turno_ID.Value = "";
            Txt_Nombre.Text = "";
            Txt_Hora_Inicio.Text = "";
            Txt_Hora_Fin.Text = "";
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Turnos
        ///DESCRIPCIÓN: Llena la tabla de Turnos
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_View
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 28/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Turnos(int Pagina)
        {
            try
            {
                Cls_Cat_Pre_Turnos_Negocio Turno = new Cls_Cat_Pre_Turnos_Negocio();
                Grid_Turnos.DataSource = Turno.Consultar_Turno();
                Grid_Turnos.PageIndex = Pagina;
                Grid_Turnos.DataBind();
            }
            catch(Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }
    
        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
            ///DESCRIPCIÓN: Hace una validacion para que los campos de la hora sean correctos
            ///PROPIEDADES:     
            ///CREO: José Alfredo García Pichardo.
            ///FECHA_CREO: 30/Junio/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes()
            {
                
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Validar_Hora(Txt_Hora_Inicio.Text.ToString()))
                {
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Hora de Inicio correctamente.";
                    Validacion = false;
                }
                if (Validar_Hora(Txt_Hora_Fin.Text.ToString()))
                {
                    Validacion = false;
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Hora de Fin correctamente.";
                }
                if (!Validacion)
                {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Hora
            ///DESCRIPCIÓN: Se hace una validacion para que la hora ingresada en los campos de
            ///             hora sea viable.
            ///PROPIEDADES:    
            ///             1. Hora.    Compo de la hora que sera validada ya sea de inicio o de fin.
            ///CREO: José Alfredo García Pichardo.
            ///FECHA_CREO: 30/Junio/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private Boolean Validar_Hora(String Hora)
            {
                int Hor = int.Parse("" + Hora[0] + Hora[1]);
                int Min = int.Parse("" + Hora[3]);
                int Min2 = int.Parse("" + Hora[4]);
                if (Hor <= 12 && Min < 6)
                {   
                    return false;
                }
                else
                {
                    return true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Comprobar_Hora
            ///DESCRIPCIÓN: Se hace una comparacion entre la hora de inicio y la hora de fin
            ///             para despues hacer una validacion.
            ///PROPIEDADES:     
            ///CREO: José Alfredo García Pichardo.
            ///FECHA_CREO: 30/Junio/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Comprobar_Hora()
            {

                Lbl_Ecabezado_Mensaje.Text = "Verificar que.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Verificar_Hora(Txt_Hora_Fin.Text.ToString()) < Verificar_Hora(Txt_Hora_Inicio.Text.ToString()))
                {
                    Mensaje_Error = Mensaje_Error + "+ La Fecha de Fin sea despues que la de Inicio.";
                    Validacion = false;
                }
                if (!Validacion)
                {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Verificar_Hora
            ///DESCRIPCIÓN: Hace una conversion de la hora seleccionada a los minutos correspondientes.
            ///PROPIEDADES:   
            ///             1. Hora.    Hora del campo seleccionado ya sea Hora_Inicio o Hora_Fin
            ///CREO: José Alfredo García Pichardo.
            ///FECHA_CREO: 30/Junio/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private int Verificar_Hora(String Hora)
            {
                int Hor = int.Parse("" + Hora[0] + Hora[1]);
                int Min = int.Parse("" + Hora[3] + Hora[4]);
                char H = Hora[6];
                if(H=='a' && Hor == 12 )
                {
                    Hor = 0;
                }
                else if (H == 'p' && Hor >= 1 && Hor <= 11 )
                {
                    Hor += 12;
                }
                return (Hor * 60) + Min;
            }        

        #endregion

    #endregion

    #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Turnos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Turnos
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 28/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Turnos_PageIndexChanging(object sender, GridViewPageEventArgs e){
            Grid_Turnos.SelectedIndex = (-1);
            Llenar_Grid_Turnos(e.NewPageIndex);
            Limpiar_Catalogo();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Turnos_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de un Turno Seleccionado para mostrarlos en el 
        ///             formulario.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 28/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Turnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Grid_Turnos.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                Hdf_Turno_ID.Value = Grid_Turnos.SelectedRow.Cells[1].Text.Trim();
                Txt_ID_Turno.Text = Grid_Turnos.SelectedRow.Cells[1].Text.Trim();
                Txt_Nombre.Text = Grid_Turnos.SelectedRow.Cells[2].Text.Trim();
                Txt_Hora_Inicio.Text = Grid_Turnos.SelectedRow.Cells[3].Text.Trim();
                Txt_Hora_Fin.Text = Grid_Turnos.SelectedRow.Cells[4].Text.Trim();
                Txt_Comentarios.Text = Grid_Turnos.SelectedRow.Cells[5].Text.Trim();
                System.Threading.Thread.Sleep(500);
            }
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nuevo Turno
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 29/Junio/2011 
        ///MODIFICO:            José Alfredo García Pichardo
        ///FECHA_MODIFICO       08/Julio/2011
        ///CAUSA_MODIFICACIÓN   Convertir los campos de nombre y comentarios a mayusculas.
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                {
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                }
                else 
                {
                    if (Validar_Componentes() && Comprobar_Hora())
                    {
                        Cls_Cat_Pre_Turnos_Negocio Turno = new Cls_Cat_Pre_Turnos_Negocio();
                        Turno.P_Comentarios = Txt_Comentarios.Text.ToUpper().Trim();
                        Turno.P_Nombre = Txt_Nombre.Text.ToUpper().Trim();
                        Turno.P_Hora_Inicio = Txt_Hora_Inicio.Text.Trim();
                        Turno.P_Hora_Fin = Txt_Hora_Fin.Text.Trim();
                        Turno.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Turno.Alta_Turno();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Turnos(Grid_Turnos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Turnos", "alert('Alta de Turno Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                }
            }
            catch(Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Turno.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 29/Junio/2011 
        ///MODIFICO:            José Alfredo García Pichardo
        ///FECHA_MODIFICO       08/Julio/2011
        ///CAUSA_MODIFICACIÓN   Convertir los campos de nombre y comentarios a mayusculas.
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    if (Grid_Turnos.Rows.Count > 0 && Grid_Turnos.SelectedIndex > (-1))
                    {
                        Configuracion_Formulario(false);
                        Txt_Nombre.Enabled = false;
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } 
                else 
                {
                    if (Validar_Componentes())
                    {
                        Cls_Cat_Pre_Turnos_Negocio Turno = new Cls_Cat_Pre_Turnos_Negocio();
                        Turno.P_Turno_ID = Hdf_Turno_ID.Value;
                        Turno.P_Comentarios = Txt_Comentarios.Text.ToUpper().Trim();
                        Turno.P_Nombre = Txt_Nombre.Text.ToUpper().Trim();
                        Turno.P_Hora_Inicio = Txt_Hora_Inicio.Text.Trim();
                        Turno.P_Hora_Fin = Txt_Hora_Fin.Text.Trim();
                        Turno.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Turno.Modificar_Turno();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Turnos(Grid_Turnos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Turnos", "alert('Actualización del Turno Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                }
            }
            catch(Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Turno_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 02/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Turno_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Limpiar_Catalogo();
                Grid_Turnos.SelectedIndex = (-1);
                Grid_Turnos.SelectedIndex = (-1);
                Llenar_Grid_Turnos(0);
                if (Grid_Turnos.Rows.Count != 0 && Txt_Busqueda_Turno.Text.Trim().Length > 0)
                {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con la descripción \"" + Txt_Busqueda_Turno.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron todos los turnos almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Turno.Text = "";
                }
                else 
                {
                    Cls_Cat_Pre_Turnos_Negocio Turno = new Cls_Cat_Pre_Turnos_Negocio();
                    Turno.P_Turno_ID = Txt_Busqueda_Turno.Text.Trim();
                    Grid_Turnos.DataSource = Turno.Consultar_Busqueda();
                    Grid_Turnos.DataBind();
                }
            }
            catch(Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un Turno de la Base de Datos.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 29/Junio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Turnos.Rows.Count > 0 && Grid_Turnos.SelectedIndex > (-1))
                {
                    Cls_Cat_Pre_Turnos_Negocio Turno = new Cls_Cat_Pre_Turnos_Negocio();
                    Turno.P_Turno_ID = Grid_Turnos.SelectedRow.Cells[1].Text;
                    Turno.Eliminar_Turno();
                    Grid_Turnos.SelectedIndex = (-1);
                    Llenar_Grid_Turnos(Grid_Turnos.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Turnos", "alert('El Turno fue eliminado exitosamente');", true);
                    Limpiar_Catalogo();
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            catch(Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 28/Junio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e)
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else 
            {
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            }
        }

    #endregion
    
}