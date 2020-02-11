<%@ Page Language="C#"  MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Rpt_Presupuesto_Egresos.aspx.cs" Inherits="paginas_Paginas_Presupuestos_Frm_Rpt_Presupuesto_Egresos" Title="Reporte Presupuestos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="Upd_Panel" runat="server"  >
        <ContentTemplate>  
            
            <asp:UpdateProgress ID="Uprg_Reloj_Checador" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Reporte_Dependencia" style="background-color:#ffffff; width:100%; height:100%;">    
                 <table width="100%" class="estilo_fuente">
                    <tr align="center">
                        <td class="label_titulo">Reporte Presupuesto</td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
                </table>
                
               
                <table width="98%"  border="0" cellspacing="0">
                    <tr align="center">
                        <td colspan="2">                
                            <div align="right" class="barra_busqueda">                        
                                <table style="width:100%;height:28px;">
                                    <tr>
                                        <td align="left" style="width:59%;">
                                           
                                             
                                            <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                                CssClass="Img_Button" TabIndex="2"
                                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                                onclick="Btn_Salir_Click"/>
                                        </td>
                                    
                                      <td align="right" style="width:41%;">&nbsp;</td>       
                                    </tr>         
                                </table>                      
                            </div>
                        </td>
                    </tr>
                </table>   
                
                <table style="width:99%;">
                    <tr>
                        <td style="width:100%;" align="center">
                            <div id="Contenedor_Titulo" style="color:White;font-size:12;font-weight:bold;border-style:outset;background:url(../imagenes/paginas/titleBackground.png) repeat-x top;background-color:Silver;">
                                <table width="100%">
                                    <tr>
                                        <td></td>
                                    </tr>            
                                    <tr>
                                        <td width="100%">
                                            <font style="color: Black; font-weight: bold;">Generar Reporte de Presupuesto</font>
                                        </td>    
                                    </tr>  
                                    <tr>
                                        <td></td>
                                    </tr>                                      
                                </table>    
                            </div>
                        </td>
                    </tr>
                </table>
            
                <table width="98%" class="estilo_fuente">
                    <tr>
                    <td style="width:50%;text-align:left;" class="button_autorizar"> 
                        Año
                    </td>
                        <td style="width:50%;text-align:left;" class="button_autorizar">
                       
                            <asp:DropDownList ID="Cmb_Anio" runat="server" Width="60%" TabIndex="3" AutoPostBack="true" BackColor=""
                            OnSelectedIndexChanged="Cmb_Anio_OnSelectedIndexChanged">
                                <asp:ListItem>&lt; -- Seleccione -- &gt;</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                       
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                </table>
                
                
                    <table  width="98%" class="estilo_fuente">
                        <tr>
                            <td style="width:98%;text-align:left;" class="button_autorizar">
                             Reportes de presupuesto a realizar por clasificación 
                            </td>
                        </tr>
                       
                        <tr>
                            <td style="width:50%;text-align:left;" class="button_autorizar">
                                Dependencia
                            </td>
                         </tr>
                       
                        <tr>  
                            <td style="width:50%;text-align:left;" class="button_autorizar">
                                Unidad responsable
                            </td>
                        </tr>
                        <tr>
                            <td style="width:50%;text-align:left;" class="button_autorizar">
                               Programa
                            </td>
                        </tr>
                        <tr>
                            <td style="width:50%;text-align:left;" class="button_autorizar">
                                 Partida
                            </td>
                        </tr>
                        <tr>
                            <td style="width:50%;text-align:left;" class="button_autorizar">
                                 Analitico
                            </td>
                            
                        </tr> 
                        <tr>
                            <td></td>
                        </tr>
                    </table>
               
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
   <%--- 
            <table width="98%" class="estilo_fuente">
                <tr>
                    <td  style="width:25%;text-align:center;" class="button_autorizar">
                    
                        <asp:ImageButton ID="Btn_Reporte_Grupo_Dependencia" runat="server" 
                            Width="15%" Height="15%" class="button_autorizar"
                            ToolTip="Generar Reporte (Excel)" CssClass="Img_Button" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png" 
                            onclick="Btn_Generar_Reporte_Excel_Click" />
                    </td> 
                </tr>  
            </table>  
        ---%>    
        
             <table width="98%" class="estilo_fuente">
               <tr>
             
                    <td style="width:50%;text-align:center;" class="button_autorizar">  
                        <asp:ImageButton ID="Btn_Reporte_Grupo_Dependencia" runat="server" onclick="Btn_Generar_Reporte_Excel_Click"  
                            ToolTip="Generar Reporte (Excel)" 
                            ImageUrl="~/paginas/imagenes/paginas/icono_rep_excel.png"/>
                    </td>
                    
               </tr>
               
            </table>
        
            
            
</asp:Content>
