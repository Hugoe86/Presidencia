<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master"
CodeFile="Frm_Rpt_Ort_Obras_Arquitecto.aspx.cs" Inherits="paginas_Ordenamiento_Territorial_Frm_Rpt_Ort_Obras_Arquitecto" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
<script src="../jquery/jquery-1.5.js" type="text/javascript"></script>
    
    <script type="text/javascript" language="javascript">
        
        //Abrir una ventana modal
        function Abrir_Ventana_Modal(Url, Propiedades)
        {
            window.showModalDialog(Url, null, Propiedades);
        }
        //  abre el grid anidado
        function Mostrar_Tabla(Renglon, Imagen) {
        object = document.getElementById(Renglon);
        if (object.style.display == "none") {
            object.style.display = "block";
            document.getElementById(Imagen).src = " ../../paginas/imagenes/paginas/stocks_indicator_down.png";
        } else {
            object.style.display = "none";
            document.getElementById(Imagen).src = "../../paginas/imagenes/paginas/add_up.png";
        }
    }    
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
<asp:ScriptManager ID="ScriptManager_Reportes" runat="server" EnableScriptGlobalization = "true" EnableScriptLocalization = "True"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>                     
            </asp:UpdateProgress>
           <%--Div de Contenido --%>
           <div id="Div_Contenido" runat="server" style="background-color: #ffffff; width: 100%; height: 100%;">
                <table width = "98%" border="0" cellspacing="0">                     
                    <tr>
                        <td colspan ="4" class="label_titulo">Reporte Obras por Arquitecto</td>
                    </tr>
                    <%--Fila de div de Mensaje de Error --%>
                    <tr>
                        <td colspan ="6">
                        <div id="Div_Contenedor_Msj_Error" style="width:95%;font-size:9px;display:block" runat="server" visible="true" >
                            <table style="width:100%;" class="estilo_fuente">
                                <tr>
                                    <td colspan="2" align="left" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;">
                                    <asp:ImageButton ID="Img_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text=""
                                        ForeColor="Red" style="font-size:12px;color:Red;font-family:Tahoma;text-align:left;" />
                                    </td>            
                                </tr>
                                <tr>
                                    <td style="width:10%;">              
                                    </td>            
                                    <td style="font-size:9px;width:90%;text-align:left;" valign="top">
                                    <%--<asp:Label ID="Lbl_Mensaje_Error" runat="server" ForeColor="Red" />--%>
                                    </td>
                                </tr>          
                            </table>                   
                        </div>
                        </td>
                    </tr>
                     <%--Manejo de la barra de busqueda--%>
                    <tr class="barra_busqueda">
                        <td colspan = "2" align = "left" >
                           <asp:ImageButton ID="Btn_Reporte_Pdf" runat="server" CssClass ="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" ToolTip="Generar Reporte" 
                                onclick="Btn_Reporte_Pdf_Click" />
                            <asp:ImageButton ID="Btn_Exportar_Excel" runat="server" CssClass ="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" ToolTip="Exportar a Excel" 
                                onclick="Btn_Exportar_Excel_Click" />
                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" CssClass="Img_Button" 
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png"
                                onclick="Btn_Salir_Click"/>
                        </td>
                        <td colspan="2" align = "right">
                               
                           
                        </td> 
                    </tr>
                </table>
                
            <div id="Div_Solicitud" runat="server">
                <table width="98%">
                    <tr>                          
                        <td style="width:15%" align="left">
                             Arquitecto o Perito                           
                        </td>
                        <td  style="width:85%" align="left">
                            <asp:DropDownList ID="Cmb_Perito" runat="server" Width="90%" ></asp:DropDownList>  
                            <asp:ImageButton ID="Btn_Buscar_Personal" runat="server" 
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                OnClick="Btn_Buscar_Personal_Click"   />          
                        </td>
                    </tr>
                </table>
            </div>   
         </div>         
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
