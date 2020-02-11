<%@ Page Language="C#" MasterPageFile="~/paginas/Paginas_Generales/MasterPage.master" AutoEventWireup="true" CodeFile="Frm_Ope_Pre_Generar_Archivo_Adeudos.aspx.cs" Inherits="paginas_Predial_Frm_Ope_Pre_Generar_Archivo_Adeudos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css" >
    .Cpe_Panel
    {
        overflow: visible;
        width:98%;
    }
    .Etiqueta_Campo
    {
        display:block;
        clear:both;
        float:left;
        width:185px;
        padding-left:3px;
    }
    .Caja_Texto_Campo
    {
        float:left;
        display:block;
        width:150px;
        text-align:right;
    }
    .Combos
    {
        float:left;
        display:block;
        width:150px;
        text-align:left;
    }
    .Etiqueta_Mensaje_Campo
    {
        float:left;
        display:block;
        width:56%;
        margin:2px 0 0 15px;
    }
    .Division_Entre_Controles
    {
        margin-bottom:7px;
        width:98.8%;
    }
    .Espaciado_Floats {clear:both;width:auto;height:1px;line-height:1px;}
    .Enlace_Archivo:link{color:#3F68B6; font-weight:normal;}
    .Enlace_Archivo:hover{color:#00388C; font-weight:bold;}
    .Enlace_Archivo:active{color:#00388C;}
    .Enlace_Archivo:visited{color:#3F68B6;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Cph_Area_Trabajo2" Runat="Server">

    <script type="text/javascript" >
    $(document).ready(function() {
    $("#Td_Tabla_Parametros").click(function() {
            colapsarPanel();
        });
    });
    
    function colapsarPanel() {
        var collPanel = $("#Cpe_Parametros");
        if (collPanel.get_Collapsed())
            collPanel.set_Collapsed(false);
        else
            collPanel.set_Collapsed(true);
    }
</script>

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
    <cc1:ToolkitScriptManager ID="Tsm_Generar_Archivo_Adeudos" runat="server"  AsyncPostBackTimeout="800000"/>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
        
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div  class="processMessage" id="div_progress"><img alt="" src="../Imagenes/paginas/Updating.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <div id="Div_Contenido" style="background-color:#ffffff; width:100%; height:100%;">
                    
                <table width="98%" class="estilo_fuente">
                    <tr align="center">
                        <td colspan="4" class="label_titulo">
                            Generar archivo de adeudos
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                            <asp:Image ID="Img_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png" Visible="false" />&nbsp;
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="Mensaje" Visible="false" CssClass="estilo_fuente_mensaje_error"></asp:Label>
                        </td>
                    </tr>
               </table>             
            
               <table width="98%"  border="0" cellspacing="0">
                <tr align="center">
                    <td colspan="2">                
                        <div style="width:100%; background-color: #2F4E7D; color: #FFFFFF; font-weight: bold; font-style: normal; font-variant: normal; font-family: fantasy; height:32px"  >
                            <table style="width:100%;height:28px;">
                                <tr>
                                    <td align="left" style="width:59%;">
                                        <asp:ImageButton ID="Btn_Archivo_Adeudos" runat="server" ToolTip="Generar archivo de adeudos" CssClass="Img_Button" TabIndex="2"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_xls.png" OnClick="Btn_Archivo_Adeudos_Click" />
                                        <asp:ImageButton ID="Btn_Imprimir" runat="server" ToolTip="Imprimir" CssClass="Img_Button" TabIndex="3"
                                            ImageUrl="~/paginas/imagenes/gridview/grid_print.png" Visible="true"
                                            OnClick="Btn_Imprimir_Click" />
                                        <asp:ImageButton ID="Btn_Salir" runat="server" ToolTip="Inicio" 
                                            CssClass="Img_Button" TabIndex="3"
                                            ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" 
                                            onclick="Btn_Salir_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                </table>
                <br />
                
                        <%------------------ Parametros ------------------%>
                <asp:Panel ID="Pnl_Parametros" runat="server" CssClass="Cpe_Panel" >
                    <div id="Encabezado_Parametros" class="Division_Entre_Controles" >
                        <asp:Label ID="Lbl_Parametros_Enlace" runat="server" style="font-size:15px;">Generar</asp:Label><br />
                    </div>
                    <asp:Panel id="Pnl_Contenedor_Parametros" runat="server" EnableViewState="true">
                        <asp:Label ID="Lbl_Txt_Anio_Generar" runat="server" Text="Anio a generar" CssClass="Etiqueta_Campo" ></asp:Label>
                        <asp:TextBox ID="Txt_Anio_Generar" runat="server" 
                            MaxLength="4" style="text-align:right;" ></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="Fte_Txt_Anio_Generar" runat="server"
                            TargetControlID="Txt_Anio_Generar" FilterType="Numbers" ></cc1:FilteredTextBoxExtender>
                        <br />
                        <asp:CheckBox ID="Chk_Urbano" runat="server" Text="Urbano" Checked="true" />
                        <asp:Label ID="Lbl_Enlace_Urbanos" runat="server" ></asp:Label>
                        <div class="Espaciado_Floats">&nbsp;</div>
                    
                        <asp:CheckBox ID="Chk_Rural" runat="server" Text="Rústico" Checked="true" />
                        <asp:Label ID="Lbl_Enlace_Rural" runat="server" ></asp:Label>
                        <div class="Espaciado_Floats">&nbsp;</div>
                        
                        <asp:CheckBox ID="Chk_Foraneos" runat="server" Text="Foráneos" Checked="true" />
                        <asp:Label ID="Lbl_Enlace_Foraneo" runat="server" ></asp:Label>
                    </asp:Panel>
                    <br /><br />
                        <asp:Label ID="Lbl_Anio" runat="server" Text="Utilizar tabulador del año" CssClass="Etiqueta_Campo" ></asp:Label>
                        <asp:DropDownList ID="Cmb_Anio_Tabulador" runat="server" CssClass="Caja_Texto_Campo"  TabIndex="10" ></asp:DropDownList>
                        <div class="Espaciado_Floats">&nbsp;</div>    
                        
                        <asp:Label ID="Lbl_Cmb_Tabulador_Enero" runat="server" Text="Tabulador para enero" CssClass="Etiqueta_Campo" ></asp:Label>
                        <asp:DropDownList ID="Cmb_Tabulador_Enero" runat="server" CssClass="Caja_Texto_Campo"  TabIndex="10" >
                        <asp:ListItem Text = "<SELECCIONE>" Value = "SELECCIONE"></asp:ListItem>
                        <asp:ListItem Text = "ENERO" Value = "ENERO"></asp:ListItem>
                        <asp:ListItem Text = "FEBRERO" Value = "FEBRERO"></asp:ListItem>
                        <asp:ListItem Text = "MARZO" Value = "MARZO"></asp:ListItem>
                        <asp:ListItem Text = "ABRIL" Value = "ABRIL"></asp:ListItem>
                        <asp:ListItem Text = "MAYO" Value = "MAYO"></asp:ListItem>
                        <asp:ListItem Text = "JUNIO" Value = "JUNIO"></asp:ListItem>
                        <asp:ListItem Text = "JULIO" Value = "JULIO"></asp:ListItem>
                        <asp:ListItem Text = "AGOSTO" Value = "AGOSTO"></asp:ListItem>
                        <asp:ListItem Text = "SEPTIEMBRE" Value = "SEPTIEMBRE"></asp:ListItem>
                        <asp:ListItem Text = "OCTUBRE" Value = "OCTUBRE"></asp:ListItem>
                        <asp:ListItem Text = "NOVIEMBRE" Value = "NOVIEMBRE"></asp:ListItem>
                        <asp:ListItem Text = "DICIEMBRE" Value = "DICIEMBRE"></asp:ListItem>
                        </asp:DropDownList>
                        <div class="Espaciado_Floats">&nbsp;</div>
                        
                        <asp:Label ID="Lbl_Cmb_Tabulador_Febrero" runat="server" Text="Tabulador para febrero" CssClass="Etiqueta_Campo" ></asp:Label>
                        <asp:DropDownList ID="Cmb_Tabulador_Febrero" runat="server" CssClass="Caja_Texto_Campo"  TabIndex="10" >
                        <asp:ListItem Text = "<SELECCIONE>" Value = "SELECCIONE"></asp:ListItem>
                        <asp:ListItem Text = "ENERO" Value = "ENERO"></asp:ListItem>
                        <asp:ListItem Text = "FEBRERO" Value = "FEBRERO"></asp:ListItem>
                        <asp:ListItem Text = "MARZO" Value = "MARZO"></asp:ListItem>
                        <asp:ListItem Text = "ABRIL" Value = "ABRIL"></asp:ListItem>
                        <asp:ListItem Text = "MAYO" Value = "MAYO"></asp:ListItem>
                        <asp:ListItem Text = "JUNIO" Value = "JUNIO"></asp:ListItem>
                        <asp:ListItem Text = "JULIO" Value = "JULIO"></asp:ListItem>
                        <asp:ListItem Text = "AGOSTO" Value = "AGOSTO"></asp:ListItem>
                        <asp:ListItem Text = "SEPTIEMBRE" Value = "SEPTIEMBRE"></asp:ListItem>
                        <asp:ListItem Text = "OCTUBRE" Value = "OCTUBRE"></asp:ListItem>
                        <asp:ListItem Text = "NOVIEMBRE" Value = "NOVIEMBRE"></asp:ListItem>
                        <asp:ListItem Text = "DICIEMBRE" Value = "DICIEMBRE"></asp:ListItem>
                        </asp:DropDownList>
                        <div class="Espaciado_Floats">&nbsp;</div>
                    
                <asp:Button Style="background-color:transparent; border-style:none;" ID="Btn_Comodin" runat="server" Text="" />
                </asp:Panel>
                <br />
                <asp:Panel ID="PnL_Filtros" runat="server" CssClass="Cpe_Panel" Visible="true" >
                    <%------------------ Filtros ------------------%>
                    <div id="Div_Filtros" class="Division_Entre_Controles" >
                        <asp:Label ID="Lbl_Titulo_Filtros" runat="server" style="font-size:15px;">Ordenar cuentas</asp:Label>
                    </div>
                    <asp:DropDownList ID="Cmb_Filtro_Uno" runat="server" 
                        CssClass="Combos" TabIndex="10" >
                        <asp:ListItem >SELECCIONE</asp:ListItem>
                        <asp:ListItem Value="SECTOR" >Sector</asp:ListItem>
                        <asp:ListItem Value="COLONIA" >Colonia</asp:ListItem>
                        <asp:ListItem Value="CALLE" >Calle</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="Cmb_Orden_Uno" runat="server" 
                        CssClass="Combos" TabIndex="11" >
                        <asp:ListItem Value="ASC" >Ascendente</asp:ListItem>
                        <asp:ListItem Value="DESC" >Descendente</asp:ListItem>
                    </asp:DropDownList>
                    <div class="Espaciado_Floats">&nbsp;</div>
                    
                    <asp:DropDownList ID="Cmb_Filtro_Dos" runat="server" 
                        CssClass="Combos" TabIndex="12" >
                        <asp:ListItem >SELECCIONE</asp:ListItem>
                        <asp:ListItem Value="SECTOR" >Sector</asp:ListItem>
                        <asp:ListItem Value="COLONIA" >Colonia</asp:ListItem>
                        <asp:ListItem Value="CALLE" >Calle</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="Cmb_Orden_Dos" runat="server" 
                        CssClass="Combos" TabIndex="13" >
                        <asp:ListItem Value="ASC" >Ascendente</asp:ListItem>
                        <asp:ListItem Value="DESC" >Descendente</asp:ListItem>
                    </asp:DropDownList>
                    <div class="Espaciado_Floats">&nbsp;</div>
                    
                    <asp:DropDownList ID="Cmb_Filtro_Tres" runat="server" 
                        CssClass="Combos" TabIndex="14" >
                        <asp:ListItem >SELECCIONE</asp:ListItem>
                        <asp:ListItem Value="SECTOR" >Sector</asp:ListItem>
                        <asp:ListItem Value="COLONIA" >Colonia</asp:ListItem>
                        <asp:ListItem Value="CALLE" >Calle</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="Cmb_Orden_Tres" runat="server" 
                        CssClass="Combos" TabIndex="15" >
                        <asp:ListItem Value="ASC" >Ascendente</asp:ListItem>
                        <asp:ListItem Value="DESC" >Descendente</asp:ListItem>
                    </asp:DropDownList>
                    <div class="Espaciado_Floats">&nbsp;</div>
                    
                </asp:Panel>
                
                <br />
                
                <asp:Panel ID="Pnl_Resultados" runat="server" CssClass="Cpe_Panel" Visible="false" >
                    <%------------------ Resultados ------------------%>
                    <div id="Encabezado_Resultados" class="Division_Entre_Controles" >
                        <asp:Label ID="Lbl_Encabezado_Resultados" runat="server" style="font-size:15px;">Adeudos generados</asp:Label>
                    </div>
                    
                    <asp:Label ID="Lbl_Resultado_Generacion" runat="server" Text="" ></asp:Label>
                    
                    <asp:GridView ID="Grid_Resultados_Generacion" runat="server" CssClass="GridView_1"
                        AutoGenerateColumns="False" Width="98%"
                        HeaderStyle-CssClass="tblHead" style="white-space:normal;"
                        PageSize="10" AllowPaging="false">
                        <Columns>
                            <asp:BoundField DataField="CUOTA_ANUAL" HeaderText="Cuota anual" DataFormatString="{0:c}" />
                            <asp:BoundField DataField="BIMESTRE_1" HeaderText="Bimestre 1" DataFormatString="{0:c}" />
                            <asp:BoundField DataField="BIMESTRE_2" HeaderText="Bimestre 2" DataFormatString="{0:c}" />
                            <asp:BoundField DataField="BIMESTRE_3" HeaderText="Bimestre 3" DataFormatString="{0:c}" />
                            <asp:BoundField DataField="BIMESTRE_4" HeaderText="Bimestre 4" DataFormatString="{0:c}" />
                            <asp:BoundField DataField="BIMESTRE_5" HeaderText="Bimestre 5" DataFormatString="{0:c}" />
                            <asp:BoundField DataField="BIMESTRE_6" HeaderText="Bimestre 6" DataFormatString="{0:c}" />
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                </asp:Panel>
                <br />
                <asp:Panel ID="Pnl_Errores_Generacion" runat="server" CssClass="Cpe_Panel" Visible="false" >
                    <%------------------ Listado errores ------------------%>
                    <div id="Encabezado_Errores" class="Division_Entre_Controles" >
                        <asp:Label ID="Lbl_Titulo_Errores" runat="server" style="font-size:15px;">Errores de generación de adeudos</asp:Label>
                    </div>
                    
                    <asp:Label ID="Lbl_Errores_Generacion" runat="server" Text="" ></asp:Label>
                    
                    <asp:GridView ID="Grid_Errores" runat="server" CssClass="GridView_1"
                        AutoGenerateColumns="False" Width="98%"
                        HeaderStyle-CssClass="tblHead" style="white-space:normal;"
                        PageSize="10" AllowPaging="false">
                        <Columns>
                            <asp:BoundField DataField="Key" HeaderText="Cuenta predial" HeaderStyle-Width="15%" />
                            <asp:BoundField DataField="Value" HeaderText=" Error" >
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="GridSelected" />
                        <PagerStyle CssClass="GridHeader" />
                        <AlternatingRowStyle CssClass="GridAltItem" />
                    </asp:GridView>
                </asp:Panel>
                <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
