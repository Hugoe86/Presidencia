<%@ Page Language="C#" AutoEventWireup="true" 
MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ventanilla.master" 
CodeFile="Frm_Ope_Ven_Lista_Tramites.aspx.cs" Inherits="paginas_Ventanilla_Frm_Ope_Ven_Lista_Tramites" 
Title="Lista Tramites"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">
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
   </script>

        
        <script type="text/javascript" language="javascript">
          
            //Abrir una ventana modal
            function Abrir_Ventana_Modal(Url, Propiedades)
            {
                window.showModalDialog(Url, null, Propiedades);
            }
            </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Cph_Area_Trabajo1" Runat="Server">
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Nomina" runat="server"  AsyncPostBackTimeout="3600"/>
       <asp:UpdatePanel ID="Upd_Panel" runat="server" UpdateMode="Always">
             <ContentTemplate>
             
               <asp:UpdateProgress ID="Upgrade" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                    <ProgressTemplate>
                       <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                        <div  class="processMessage" id="div_progress">
                            <img alt="" src="../Imagenes/paginas/Updating.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            
                <div id="Div_General" runat="server"  style="background-color:#ffffff; width:98%; height:100%;"> <%--Fin del div General--%>
                    <table width="98%" border="0" cellspacing="0" class="estilo_fuente" frame="border" >
                        <tr align="center">
                            <td  colspan="2" class="label_titulo">Lista de trámites
                            </td>
                       </tr>
                        <tr> <!--Bloque del mensaje de error-->
                            <td colspan="2" >
                                <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                                <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                            </td>      
                        </tr>
                        
                        <tr class="barra_busqueda" align="right" >
                            <td  align="left">
                            
                                <asp:ImageButton ID="Btn_Salir" runat="server" 
                                    CssClass="Img_Button" 
                                    ToolTip="Salir"
                                    ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                    onclick="Btn_Salir_Click"/>
                             </td> 
                             
                             <td>
                             <td>                                                   
                        </tr>
                    </table>
                    
                  
                    
                 
                     <div id="Div_Consultar_Tramite" runat="server" style="color: #5D7B9D;display:block" > 
                         <asp:Panel ID="Pnl_Consultar_Tramite" runat="server" GroupingText="Consultar" Width="98%" BackColor="white"> 
                         
                            <table width="99%">   
                                 <tr>
                                    <td style="width:15%">
                                        <asp:Label ID="Lbl_Unidad_Responsable_Filtro" runat="server"  Text="U. Responsable" Width="100%"></asp:Label> 
                                    </td>
                                    <td style="width:80%">
                                        <asp:DropDownList  ID="Cmb_Unidad_Responsable_Filtro" runat="server" Width="98%"  
                                            DropDownStyle="DropDownList" 
                                            AutoCompleteMode="SuggestAppend" 
                                            CaseSensitive="False" 
                                            CssClass="WindowsStyle" 
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged = "Cmb_Unidad_Responsable_Filtro_SelectedIndexChanged"
                                            ItemInsertLocation="Append"/>
                                       
                                    </td>
                                    <td style="width:5%" align="center"> 
                                            <asp:ImageButton ID="Btn_Buscar_Dependencia" runat="server" ToolTip="Seleccionar Unidad responsable"
                                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" Height="22px" Width="22px"
                                                OnClick="Btn_Buscar_Dependencia_Click" /> 
                                    </td>
                                </tr>  
                                <tr>
                                    <td style="width:15%">
                                        <asp:Label ID="Lbl_Clave_Tramite" runat="server"  Text="Clave del trámite" Width="100%"></asp:Label> 
                                    </td>
                                    <td style="width:80%">
                                        <asp:TextBox ID="Txt_Clave_Tramite" runat="server" Width="98%" MaxLength="20" 
                                            onkeyup='this.value = this.value.toUpperCase();'> </asp:TextBox>
                                            
                                    </td>
                                     <td style="width:5%" align="center">
                                       <asp:ImageButton ID="Btn_Busqueda_Clave" runat="server" 
                                                CssClass="Img_Button" 
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                OnClick="Btn_Buscar_Tramite_Click" Width="24px" Height="24px"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:15%">
                                        <asp:Label ID="Lbl_Nombre_Tramite" runat="server"  Text="Nombre del trámite" Width="100%"></asp:Label> 
                                    </td>
                                    <td style="width:80%">
                                        <asp:TextBox ID="Txt_Nombre_Tramite" runat="server" Width="98%"></asp:TextBox>
                                            
                                    </td> 
                                    <td style="width:5%" align="center">
                                       <asp:ImageButton ID="Btn_Buscar_Tramite" runat="server" 
                                                CssClass="Img_Button" 
                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png"
                                                OnClick="Btn_Buscar_Tramite_Click" Width="24px" Height="24px"/>
                                    </td>
                                </tr> 
                              
                               
                            </table>
                        </asp:Panel>
                    </div> 
                    
                    <table width="100%">
                        <tr>
                            <td rowspan="5"></td>
                        </tr>
                    </table>
                    
                    <asp:Panel ID="Pnl_Tramites" runat="server" GroupingText="Trámites más solicitados" Width="98%" BackColor="white"> 
                        <table class="estilo_fuente" width="100%">      
                            <tr>
                                <td style="width:100%;text-align:center;vertical-align:top;">
                                    <center>
                                        <div id="Div_Presentacion" runat="server" 
                                            style="overflow: auto; height:350px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">
                                            <asp:GridView ID="Grid_Lista_Tramites" runat="server" Width="97%" 
                                                CssClass="GridView_1" HeaderStyle-CssClass="tblHead" 
                                                GridLines="None"   AllowPaging="false"
                                                AutoGenerateColumns="False" 
                                                OnRowDataBound="Grid_Lista_Tramites_OnRowDataBound" 
                                                EmptyDataText="No se encuentra ningun tramite"  >
                                                <Columns>
                                                    <%-- 0 --%>
                                                    <asp:BoundField DataField="tramite_id" HeaderText="tramite_id"
                                                         HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="11px">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField> 
                                                    
                                                    <%-- 1 --%>
                                                     <asp:BoundField DataField="CLAVE_TRAMITE" HeaderText="Clave"
                                                         HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="11px">
                                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:BoundField>
                                                    
                                                    <%-- 2 --%>
                                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre"
                                                         HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="11px">
                                                        <HeaderStyle HorizontalAlign="Left" Width="40%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                    </asp:BoundField>
                                                    
                                                    <%-- 3 --%>
                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción"
                                                         HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="11px">
                                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="0%" />
                                                    </asp:BoundField>
                                                    
                                                    <%-- 4 --%>
                                                    <asp:BoundField DataField="Tiempo_estimado" HeaderText="Duración (Días)" DataFormatString="{0:#,###}"
                                                         HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="11px">
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                    </asp:BoundField>
                                                    
                                                    <%-- 5 --%>
                                                    <asp:BoundField DataField="Costo" HeaderText="Costo" DataFormatString="{0:n}"
                                                         HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="11px">
                                                        <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                    </asp:BoundField>
                                                    
                                                    <%-- 6 --%>
                                                     <asp:TemplateField  HeaderText= "Requisitos"  HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="11px">
                                                        <ItemTemplate >
                                                            <center>
                                                                <asp:ImageButton ID="Btn_Ver_Requisitos" runat="server" 
                                                                    ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" Height="24px" 
                                                                    OnClick="Btn_Ver_Requisitos_Click" 
                                                                    CausesValidation="false"/> 
                                                            </center>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="center" Width="5%" />
                                                        <ItemStyle HorizontalAlign="center" Width="5%" />
                                                    </asp:TemplateField>
                                                    
                                                     <%-- 7 --%>
                                                     <asp:TemplateField  HeaderText= "Formato"  HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="11px">
                                                        <ItemTemplate >
                                                            <center>
                                                                <asp:ImageButton ID="Btn_Ver_Formato" runat="server" 
                                                                    ImageUrl="~/paginas/imagenes/paginas/icono_rep_pdf.png" Width="24px" Height="24px" 
                                                                    OnClick="Btn_Ver_Formato_Click" 
                                                                    CausesValidation="false"/> 
                                                            </center>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="center" Width="5%" />
                                                        <ItemStyle HorizontalAlign="center" Width="5%" />
                                                    </asp:TemplateField>
                                                    
                                                    <%-- 8 --%>
                                                    <asp:TemplateField  HeaderText= "Solicitar"  HeaderStyle-Font-Size="12px"  ItemStyle-Font-Size="11px">
                                                        <ItemTemplate >
                                                            <center>
                                                                <asp:ImageButton ID="Btn_Autorizar_Solicitud" runat="server" 
                                                                    ImageUrl="~/paginas/imagenes/paginas/icono_modificar.png" Width="24px" Height="24px" 
                                                                    OnClick="Btn_Autorizar_Solicitud_Click" 
                                                                    CausesValidation="false"/> 
                                                            </center>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="center" Width="5%" />
                                                        <ItemStyle HorizontalAlign="center" Width="5%" />
                                                    </asp:TemplateField>
                                                    
                                                </Columns>
                                                <SelectedRowStyle CssClass="GridSelected" />
                                                <PagerStyle CssClass="GridHeader" />
                                                <HeaderStyle CssClass="GridHeader" />
                                                <AlternatingRowStyle CssClass="GridAltItem" />
                                            </asp:GridView>
                                        </div>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
             </ContentTemplate>
      </asp:UpdatePanel>
</asp:Content>