﻿<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Pat_Listado_Bienes.aspx.cs" Inherits="paginas_Compras_Frm_Rpt_Pat_Listado_Bienes" Title="Reporte de Listado de Bienes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"  EnableScriptGlobalization ="true" EnableScriptLocalization = "True">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress"> <img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                    </ProgressTemplate>                     
            </asp:UpdateProgress>
            <div id="Div_Requisitos" style="background-color:#ffffff; width:100%; height:100%;">
                <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                     <tr align="center">
                        <td class="label_titulo" colspan="2">&nbsp;</td>
                    </tr>                  
                    <tr align="center">
                        <td class="label_titulo" colspan="2">Reporte de Listado de Bienes</td>
                    </tr>
                    <tr>
                        <td>
                          <div id="Div_Contenedor_Msj_Error" style="width:98%;" runat="server" visible="false">
                            <table style="width:100%;">
                              <tr>
                                <td colspan="2" align="left">
                                  <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="../imagenes/paginas/sias_warning.png" 
                                    Width="24px" Height="24px"/>
                                    <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>            
                              </tr>
                              <tr>
                                <td style="width:10%;">              
                                </td>          
                                <td style="width:90%;text-align:left;" valign="top">
                                  <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </td>
                              </tr>          
                            </table>                   
                          </div>                
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>                        
                    </tr>
                    <tr class="barra_busqueda" align="right">
                        <td align="left">&nbsp;</td>
                        <td>&nbsp;</td>                        
                    </tr>
                </table>   
                <br />
                <center>
                    <table width="98%">
                        <tr>
                            <td style="width:20%; text-align:left; ">
                                <asp:Label ID="Lbl_Tipo_Bien" runat="server" Text="Tipo" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:30%; text-align:left; ">
                                <asp:DropDownList ID="Cmb_Tipo_Bien" runat="server" Width="99%">
                                    <asp:ListItem Value="TODOS">&lt;-- TODOS --&gt;</asp:ListItem>
                                    <asp:ListItem Value="BIEN_MUEBLE">BIEN MUEBLE</asp:ListItem>
                                    <asp:ListItem Value="VEHICULO">VEHÍCULO</asp:ListItem>
                                    <asp:ListItem Value="CEMOVIENTE">ANIMAL</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>    
                        <tr>
                            <td style="width:20%; text-align:left; ">
                                <asp:Label ID="Lbl_Fecha_Adquisicion_Inicial" runat="server" Text="Fecha Inicial" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:30%; text-align:left; ">
                                <asp:TextBox ID="Txt_Fecha_Adquisicion_Inicial" runat="server" Width="85%" MaxLength="20" Enabled="false" ></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Adquisicion_Inicial" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt_Fecha_Adquisicion_Inicial" runat="server" TargetControlID="Txt_Fecha_Adquisicion_Inicial" PopupButtonID="Btn_Fecha_Adquisicion_Inicial" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                            <td style="width:20%; text-align:left; ">
                                <asp:Label ID="Lbl_Fecha_Adquisicion_Final" runat="server" Text="Fecha Final" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="width:30%; text-align:left; ">
                                <asp:TextBox ID="Txt_Fecha_Adquisicion_Final" runat="server" Width="85%" MaxLength="20" Enabled="false" ></asp:TextBox>
                                <asp:ImageButton ID="Btn_Fecha_Adquisicion_Final" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Txt__Fecha_Adquisicion_Final" runat="server" TargetControlID="Txt_Fecha_Adquisicion_Final" PopupButtonID="Btn_Fecha_Adquisicion_Final" Format="dd/MMM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>                   
                        <tr>
                            <td style="width:20%; text-align:left; ">
                                <asp:Label ID="Lbl_Busqueda_Resguardantes_Dependencias" runat="server" Text="Unidad Responsable" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left;" colspan="3">
                                <asp:DropDownList ID="Cmb_Busqueda_Resguardantes_Dependencias" runat="server" 
                                    Width="98%" AutoPostBack="True" OnSelectedIndexChanged="Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged">
                                    <asp:ListItem Text="&lt; TODAS &gt;" Value="TODAS"></asp:ListItem>   
                                </asp:DropDownList>                                   
                            </td>
                        </tr>                          
                        <tr>
                            <td style="width:20%; text-align:left; ">
                                <asp:Label ID="Lbl_Busqueda_Nombre_Resguardante" runat="server" Text="Nombre Resguardante" CssClass="estilo_fuente"></asp:Label>
                            </td>
                            <td style="text-align:left;" colspan="3">
                                <asp:DropDownList ID="Cmb_Busqueda_Nombre_Resguardante" runat="server" Width="98%" >
                                    <asp:ListItem Text="&lt;-- TODOS --&gt;" Value="TODOS"></asp:ListItem>
                                </asp:DropDownList>     
                            </td>
                        </tr>     
                        <tr>
                            <td style="text-align:center;" colspan="4">&nbsp;</td>
                        </tr>  
                        <tr>
                            <td style="text-align:right;" colspan="4">
                                <asp:ImageButton ID="Btn_Generar_Reporte_PDF" runat="server"  OnClick="Btn_Generar_Reporte_PDF_Click"
                                 ToolTip="Generar Reporte (Pdf)" 
                                    ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png"  />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="Btn_Generar_Reporte_Excel" runat="server" 
                                 ToolTip="Generar Reporte (Excel)" 
                                    ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                                    onclick="Btn_Generar_Reporte_Excel_Click"  />&nbsp;
                            </td>
                        </tr>                  
                    </table>
                </center>
                
                <asp:GridView ID="GridView1" runat="server">
                </asp:GridView>
                
                <br />                           
            </div>
            <br />
            <br />
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

