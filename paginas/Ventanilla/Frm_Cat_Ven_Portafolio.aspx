<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage_Ventanilla.master"
 AutoEventWireup="true" CodeFile="Frm_Cat_Ven_Portafolio.aspx.cs" Inherits="paginas_Ventanilla_Frm_Cat_Ven_Portafolio" 
 Title="Mi Portafolio"%>

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
                            <td  colspan="2" class="label_titulo">Mi Portafolio
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
                    
                    <div id="Div_Cargar_Archivo" runat="server" style="color: #5D7B9D; display:none">
                        <asp:Panel ID="Panel1" runat="server" GroupingText="Ruta del archivo" Width="98%" BackColor="white"> 
                            
                             <table width="100%">   
                                <tr>
                                    <td style="width:30%">
                                       Formato del archivo pdf, jpg, jpeg
                                    </td>
                                    <td style="width:55%">
                                       <cc1:AsyncFileUpload ID="AFU_Subir_Archivo" runat="server" size="550px" 
                                            UploadingBackColor="Yellow" ErrorBackColor="Red" CompleteBackColor="LightGreen"
                                            ThrobberID="Throbber" onuploadedcomplete="AFU_Subir_Archivo_UploadedComplete"
                                            />    
                                    </td>  
                                    <td style="width:15%" align="center"> 
                                   <%--  <asp:Button ID="Btn_Realizar_Operacion" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                                             OnClick="Btn_Realizar_Operacion_Click"/> --%>
                                        
                                       <asp:Button ID="Btn_Operacion" runat="server" Text="" OnClick="Btn_Operacion_Click" />
                                       
                                      
                                    </td>
                                    <td style="width:5%" align="center"> 
                                        <asp:ImageButton ID="Btn_Cerrar_Div" CausesValidation="false" runat="server" style="cursor:pointer;" ToolTip="Cerrar Ventana"
                                            ImageUrl="~/paginas/imagenes/paginas/Sias_Close.png" OnClick="Btn_Cerrar_Div_Click"/>
                                    </td>
                                </tr>                                                    
                            </table>
                        </asp:Panel>
                    </div>
                    
                    <div id="Div_Campos_ocultos" runat="server" style="color: #5D7B9D; display:none">
                        <table width="100%">
                            <tr>
                                <td align="right" style="width:100%;" align="right">
                                    <asp:HiddenField ID="Hdf_Tramite_ID" runat="server" />
                                    <asp:HiddenField ID="Hdf_Nombre_Tramite" runat="server" />
                                   <%-- <asp:HiddenField ID="Hdf_Accion_Realizada" runat="server" />--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <table width="100%">
                         <tr>
                            <td>
                                <center>
                                    <div id="Div_Grid_Documentos_Ciudadano" runat="server" 
                                        style="overflow:auto;height:450px;width:99%;vertical-align:top;border-style:solid;border-color:Silver;display:block">
                                        <asp:GridView ID="Grid_Documentos_Ciudadano" runat="server" 
                                            CssClass="GridView_1" HeaderStyle-CssClass="tblHead" 
                                            GridLines="None"   AllowPaging="false"
                                            AutoGenerateColumns="False" Width="98%"
                                            onrowdatabound="Grid_Documentos_Ciudadano_RowDataBound" RowStyle-Height="30px" EmptyDataText="Sin registros" >
                                            <RowStyle CssClass="GridItem" />
                                            <Columns>
                                                <%-- 0 --%>
                                                <asp:BoundField DataField="DOCUMENTO_ID" HeaderText="Documento_ID" SortExpression="DOCUMENTO_ID" 
                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Width="0%" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-Font-Size="11px" ItemStyle-Width="0%" ItemStyle-HorizontalAlign="Left"/>
                                                <%-- 1 --%> 
                                                <asp:BoundField DataField="NOMBRE" HeaderText="DOCUMENTO" SortExpression="NOMBRE" 
                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Width="35%" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-Font-Size="11px" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Left"/>
                                                <%-- 2 --%>    
                                                <asp:BoundField DataField="DESCRIPCION" HeaderText="DESCRIPCIÓN" SortExpression="DESCRIPCION" 
                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Width="35%" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-Font-Size="11px" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Left"/>
                                                <%-- 3 --%>    
                                                <asp:TemplateField HeaderText="SUBIR"
                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Font-Size="11px" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:ImageButton ID="Btn_Subir_Documento" runat="server" AlternateText="Ver" 
                                                                ImageUrl="~/paginas/imagenes/paginas/subir.png" Width="24px" Height="24px"
                                                                onclick="Btn_Subir_Documento_Click" />
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 
                                                <%-- 4 --%>
                                                <asp:TemplateField HeaderText="ACTUALIZAR"
                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Font-Size="11px" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:ImageButton ID="Btn_Acutalizar_Documento" runat="server" AlternateText="Ver" 
                                                                ImageUrl="~/paginas/imagenes/paginas/actualizar_detalle.png" Width="24px" Height="24px"
                                                                onclick="Btn_Actualizar_Documento_Click" />
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <%-- 5 --%>
                                                <asp:TemplateField HeaderText="VER"
                                                    HeaderStyle-Font-Size="12px" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-Font-Size="11px" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:ImageButton ID="Btn_Ver_Documento" runat="server" AlternateText="Ver" 
                                                                ImageUrl="~/paginas/imagenes/paginas/busqueda.png" Width="24px" Height="24px" 
                                                                onclick="Btn_Ver_Documento_Click" />
                                                        </center>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="GridHeader" />
                                            <SelectedRowStyle CssClass="GridSelected" />
                                            <HeaderStyle CssClass="GridHeader" />
                                            <AlternatingRowStyle CssClass="GridAltItem" />
                                        </asp:GridView>
                                    </div>
                                </center>
                            </td>
                        </tr>
                    </table >
                    
                </div>
            </ContentTemplate>
      </asp:UpdatePanel>      
</asp:Content>
