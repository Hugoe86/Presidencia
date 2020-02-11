<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Nom_Generar_Nomina.aspx.cs" Inherits="paginas_Nomina_Frm_Ope_Nom_Generar_Nomina" Title="Generación de Nómina" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server"> 
    <script src="../../javascript/Js_Generar_Nomina.js" type="text/javascript"></script> 

   <!--SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
   <script language="javascript" type="text/javascript">
    <!--
        //El nombre del controlador que mantiene la sesión
        var CONTROLADOR = "../../Mantenedor_Session.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);
        
    //-->
    function pageLoad(){
        inicializarEventos_Generacion_Nomina();     
    }
    
    function printme(div_imprimir){
        var winPrint = window.open();

        winPrint.document.write($(div_imprimir).html());
        winPrint.document.close();
        winPrint.focus();
    }    
   </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="36000"/>

    <asp:UpdatePanel ID="UPnl_Generar_Nomina" runat="server" >
        <ContentTemplate>
          
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="UPnl_Generar_Nomina" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress"><img alt="" src="../imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>               
            </asp:UpdateProgress>  
            
        <div style="width:99%">            
            <table width="98%"  border="0" cellspacing="0" class="estilo_fuente">
                <tr>
                    <td>
                        <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false"  />&nbsp;
                        <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"/>
                    </td>
                </tr>
            </table> 

            <table class="button_autorizar" style="width:100%;">
                <tr>
                    <td style="width:100%;font-size:14px;font-weight:bold;" colspan="4" align="center">
                        <table style="width:100%;">
                            <tr>
                                <td style="width:100%;" align="center">
                                    <div id="Contenedor_Titulo" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
                                        <table width="100%">
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td width="100%">
                                                    <font style="color: Black; font-weight: bold;">Generaci&oacute;n de N&oacute;mina</font>
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
                    </td>
                </tr>
                <tr>
                    <td style="width:100%;" colspan="4" align="center">
                        <hr />              
                    </td>
                </tr>     
                <tr>
                    <td style="width:100%;" colspan="4" align="center">
                        <br />              
                    </td>
                </tr>              
                <tr>
                    <td style="width:20%;font-size:12px;">
                        <asp:Label ID="Lbl_Tipo_Nomina" runat="server" Text="Tipo Nómina" Width="100%"/>
                    </td>
                    <td style="width:30%;font-size:12px;">
                        <asp:DropDownList ID="Cmb_Tipo_Nomina" runat="server" Width="100%" />
                    </td>            
                    <td style="width:20%;font-size:12px;">
                        
                    </td>
                    <td style="width:30%;font-size:12px;">
                        
                    </td>                        
                </tr>
                <tr>
                    <td style="width:20%;font-size:12px;">
                        <asp:Label ID="Lbl_Calendario_Nomina" runat="server" Text="Nomina" Width="100%"/>
                    </td>
                    <td style="width:30%;font-size:12px;">
                        <asp:DropDownList ID="Cmb_Calendario_Nomina" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Calendario_Nomina_SelectedIndexChanged"/>
                    </td>            
                    <td style="width:20%;font-size:12px;">
                        <asp:Literal ID="Ltr_Espacios" runat="server" Text="&nbsp;&nbsp;"/>
                        <asp:Label ID="Lbl_Periodo" runat="server" Text="Periodo" Width="100%" />
                    </td>
                    <td style="width:30%;font-size:12px;">
                        <asp:DropDownList ID="Cmb_Periodo" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="Cmb_Periodo_SelectedIndexChanged"/>
                    </td>                        
                </tr>
                <tr>
                    <td style="width:20%;font-size:12px;">
                        <asp:Label ID="Lbl_Inicia_Catorcena" runat="server" Text="Inicia Catorcena" Width="100%"/>
                    </td>
                    <td style="width:30%;font-size:12px;">
                        <asp:TextBox ID="Txt_Inicia_Catorcena" runat="server" Width="98%" Enabled="false"/>
                    </td>            
                    <td style="width:20%;font-size:12px;">
                        <asp:Literal ID="Literal1" runat="server" Text="&nbsp;&nbsp;"/>
                        <asp:Label ID="Lbl_Fin_Catorcena" runat="server" Text="Fin Catorcena" Width="100%"/>
                    </td>
                    <td style="width:30%;font-size:12px;">
                        <asp:TextBox ID="Txt_Fin_Catorcena" runat="server" Width="98%" Enabled="false"/>
                    </td>                        
                </tr>
                <tr>
                    <td style="width:100%;" colspan="4" align="center">
                        <hr />              
                    </td>
                </tr>          
                <tr>
                    <td style="width:100%;" colspan="4" align="right">
                        <table style="width:100%;background-color:White;">
                            <tr>
                                <td style="width:70%;">
                                    <table style="width:100%;">
                                        <tr>
                                            <td align="center">
                                                <div style="width:500px; height:200px;vertical-align:middle;">
                                                    <img src="../imagenes/paginas/escudo.jpg" alt="" width="180px" height="200px" id="Img_Irapuato"/>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>                        
                                </td>
                                <td style="width:30%;">
                                    <table style="width:100%;">
                                        <tr>
                                            <td style="width:100%;" colspan="4" align="center">
                                                <hr />              
                                            </td>
                                        </tr>                
                                        <tr>
                                            <td style="width:100%;font-size:13px;color:Blue;font-weight:bold;" align="center">
                                                 <div id="Div1" style="background-color:Silver;color:White;font-size:12;font-weight:bold;border-style:outset;">
                                                   <table width="100%">
                                                        <tr>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="100%">
                                                                <font style="color: Black; font-weight: bold;">N&oacute;mina a Generar</font>
                                                            </td>    
                                                        </tr>  
                                                        <tr>
                                                            <td></td>
                                                        </tr>                                      
                                                    </table>  
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:100%;" colspan="4" align="center">
                                                <hr />              
                                            </td>
                                        </tr>                    
                                        <tr>
                                            <td align="left" class="button_autorizar" style="width:100%; cursor:default;">
                                                <asp:RadioButtonList ID="RBL_Tipos_Nominas" runat="server" AutoPostBack="true" onclick='javascript:return OnSelectedIndexChanged();'
                                                    OnSelectedIndexChanged="RBL_Tipos_Nominas_SelectedIndexChanged" style="cursor:hand; font-size:10px;" class="tbl">
                                                    <asp:ListItem Value="CATORCENAL">Catorcenal</asp:ListItem>
                                                    <asp:ListItem Value="PVI">Prima Vacacional I</asp:ListItem>
                                                    <asp:ListItem Value="PVII_AGUINALDO">Prima Vacacional II Y Aguinaldo</asp:ListItem>    
                                                </asp:RadioButtonList>                         
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:100%;" colspan="4" align="center">
                                                <asp:Button ID="Btn_Cerrar_Nomina" runat="server" Text="Cerrar Nómina" Visible="false"
                                                     CssClass="button_autorizar" ToolTip="Cerrar Nómina" OnClick="Btn_Cerrar_Nomina_Click"/>                                                              
                                            </td>
                                        </tr>                                           
                                    </table>                         
                                </td>
                            </tr>                    
                        </table>      
                    </td>
                </tr>         
                <tr>
                    <td style="width:100%;" colspan="4" align="center">
                        <br />              
                    </td>
                </tr>        
                <tr>
                    <td style="width:100%;" colspan="4" align="center">
                        <hr />              
                    </td>
                </tr>
                <tr>
                    <td style="width:100%;" colspan="4" align="right">
                        <asp:Button ID="Btn_Generar_Nomina" runat="server" Text="Generar Nómina" 
                            onclick="Btn_Generar_Nomina_Click" CssClass="button_autorizar" ToolTip="Generar Nómina"/>                              
                    </td>
                </tr>
                <tr>
                    <td style="width:100%;" colspan="4" align="center">
                        <hr />              
                    </td>
                </tr>                         
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

