<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Frm_Menu_Pre_Propietarios.aspx.cs"
    Inherits="paginas_Predial_Ventanas_Emergentes_Orden_Variacion_Frm_Menu_Pre_Propietarios"
    Culture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contribuyentes</title>

    <script src="../../../../easyui/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../../../estilos/estilo_paginas.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .GridView_1
        {
            font-family: Verdana, Geneva, MS Sans Serif;
            font-size: 8px;
            color: #2F4E7D;
            font-weight: normal;
            padding: 3px 6px 3px 6px;
            vertical-align: middle;
            white-space: nowrap;
            background-color: White;
            border-style: none;
            border-width: 5px;
            width: 98%;
            text-align: left;
            margin-left: 0px;
        }
        *
        {
            font-family: Arial;
            font-size: small;
            text-align: left;
        }
        .GridHeader
        {
            font-weight: bold;
            background-color: #2F4E7D;
            color: #ffffff;
            text-align: left;
            position: relative;
            height: 23px;
        }
        .GridItem
        {
            background-color: white;
            color: #25406D;
        }
        .GridAltItem
        {
            background-color: #E6E6E6; /*#E6E6E6 #E0F8F7*/
            color: #25406D;
        }
        .GridSelected
        {
            background-color: #A4A4A4; /*#A9F5F2;*/
            color: #25406D;
        }
        .renglon_botones
        {
            vertical-align: middle;
            height: 40px;
        }
        .style2
        {
            width: 4px;
        }
    </style>

    <script type="text/javascript" language="javascript">   

            function Generar_Rfc() {
                var Ap = "";
                var Am = "X";
                var Nom = "";
                var Fecha="00/00/0000";               
                var Dia = "00";
                var Mes = "00";
                var Anio = "0000";
                var split;
    
                if($('#Txt_Fecha_Nacimiento').val().length > 0)
                {
	                if($('#Txt_Fecha_Nacimiento').val() != "<dd/MM/aaaa>")
	                {
		                Fecha = $('#Txt_Fecha_Nacimiento').val(); 
	                }
	                else
	                {
		                Fecha ="00/00/0000";                    
	                }
		            if (Fecha.indexOf("-") != -1)
                    {
	                    split=Fecha.split('-');
                    }
                    else
                    {
                        split=Fecha.split('/');
                    }

	                if (split.length==1)
	                {
		                Dia = "00" + split[0];
		                Dia = Dia.substring(Dia.length - 2);
	                }
	                if (split.length==2)
	                {
		                Dia = "00" + split[0];
		                Dia = Dia.substring(Dia.length - 2);
		                if (split[1].length <= 2)
		                {
		                    Mes = "00" + split[1];
		                    Mes = Mes.substring(Mes.length - 2);
		                }
		                else
		                {
		                    Mes = split[1];
		                }
	                }
	                if (split.length==3)
	                {
		                Dia = "00" + split[0];
		                Dia = Dia.substring(Dia.length - 2);
		                if (split[1].length <= 2)
		                {
		                    Mes = "00" + split[1];
		                    Mes = Mes.substring(Mes.length - 2);
		                }
		                else
		                {
		                    Mes = split[1];
		                }
		                Anio = split[2];
	                }
		            Fecha = Dia + "/" + Mes + "/" + Anio;
                }

                if($('#Txt_Apeido_Paterno').val() != null)
                     Ap = $('#Txt_Apeido_Paterno').val();                     
                
                 if($('#Txt_Apeido_Materno').val().length>1)
                     Am = $('#Txt_Apeido_Materno').val();                                         
                else
                    Am="X";
                 if($('#Txt_Nombre_Contribuyente').val() != null)
                     Nom = $('#Txt_Nombre_Contribuyente').val();

                 //Validar nombre si es Jose o Maria
                 var Comparar_Nombre = Nom.split(" ");
                 var i = 0;
                 for (i = 0; i < Comparar_Nombre.length; i++)
                 {											 
                    if (!Validar_Articulos(Comparar_Nombre[i].toUpperCase()))
		         { 
			     var Nom_Sub = Comparar_Nombre[Comparar_Nombre.length-1].substring(0, 1);
		         }
		            else
		            {
                        var Nom_Sub = Comparar_Nombre[i].substring(0, 1);
			            break;
		            }
                 }
                                 
                var validar_Apeido="false";
                var Apeido=""
                var Ap_Sub1 = Ap.substring(0, 1);                
                var Ap_Sub2=Ap.substring(1,2);
                var Ap_Sub3=Ap.substring(2,3);
                var Ap_Sub4=Ap.substring(3,4);
                var Ap_Sub5=Ap.substring(4,5);
                var Vocales=['a','e','i','o','u','A','E','I','O','U'];
                for(var i=0;i<Vocales .length ;i++){
                    if(Ap_Sub2 == Vocales[i]){
                        Apeido=Ap_Sub1 +Ap_Sub2;
                        validar_Apeido ="true";
                    }
                }
                if(validar_Apeido =="false"){
                for(var i=0;i<Vocales .length ;i++){
                    if(Ap_Sub3 == Vocales[i]){
                        Apeido=Ap_Sub1 +Ap_Sub3;
                        validar_Apeido ="true";
                    }
                }
                }
                if(validar_Apeido =="false"){
                for(var i=0;i<Vocales .length ;i++){
                    if(Ap_Sub4 == Vocales[i]){
                        Apeido=Ap_Sub1 +Ap_Sub4;
                        validar_Apeido ="true";
                    }
                }
                }                
                if(validar_Apeido =="false"){                
                    Apeido=Ap_Sub1 +Ap_Sub5 ;
                }
                   
                var Am_Sub = Am.substring(0, 1);

                if (Fecha.indexOf("-") != -1) {
                    split = Fecha.split('-');
                }
                else {
                    split = Fecha.split('/');
                }
                var Anio_Sub=split[2].substring(split[2].length-2);
                var Anio_Mes=split[1];
                var Anio_Dia=split[0];               
                //var Rfc=Apeido.toString().replace('ñ','X').replace('Ñ','X') + Am_Sub.toString().replace('Ñ','X').replace('ñ','X') + Nom_Sub.toString().replace('ñ','X').replace('Ñ','X') +Anio_Sub.toString()+Anio_Mes.toString()+Anio_Dia.toString();
                var Rfc = Validar_Lenguaje_Gamberro( Apeido.toString() + Am_Sub.toString() + Nom_Sub.toString() ) + Anio_Sub.toString() + Anio_Mes.toString() + Anio_Dia.toString();
                $('#Txt_RFC_Contribuyente').val(Rfc);

            }

            function Validar_Articulos(Cad) 
            {
            var Res = true;
            if (Cad.toUpperCase() == "JOSE")
                Res = false;
            if (Cad.toUpperCase() == "MARIA")
                Res = false;
            if (Cad.toUpperCase() == "MA")
                Res = false;
            if (Cad.toUpperCase() == "MA.")
                Res = false;
            if (Cad.toUpperCase() == "DE")
                Res = false;
            if (Cad.toUpperCase() == "DEL")
                Res = false;
            if (Cad.toUpperCase() == "LOS")
                Res = false;
            if (Cad.toUpperCase() == "EL")
                Res = false;
            if (Cad.toUpperCase() == "LA")
                Res = false;
            if (Cad.toUpperCase() == "LAS")
                Res = false;
                return Res;
            }
            function Validar_Lenguaje_Gamberro(Cad) 
            {
                var Res = Cad;
                if (Cad.toUpperCase() == "CACA")
                    Res = 'CACX';
                if (Cad.toUpperCase() == "COLA")
                    Res = 'COLX';
                if (Cad.toUpperCase() == "PUTA")
                    Res = 'PUTX';
                if (Cad.toUpperCase() == "ANOS")
                    Res = 'ANOX';
                if (Cad.toUpperCase() == "ANAL")
                    Res = 'ANAX';
                if (Cad.toUpperCase() == "ASNO")
                    Res = 'ASNX';
                if (Cad.toUpperCase() == "PEDO")
                    Res = 'PEDX';
                if (Cad.toUpperCase() == "PEDA")
                    Res = 'PEDX';
                if (Cad.toUpperCase() == "PUTO")
                    Res = 'PUTX';
                if (Cad.toUpperCase() == "PUPU")
                    Res = 'PUPX';
                if (Cad.toUpperCase() == "POPO")
                    Res = 'POPX';
                if (Cad.toUpperCase() == "PIPI")
                    Res = 'PIPX';
                if (Cad.toUpperCase() == "PENE")
                    Res = 'PENX';
                if (Cad.toUpperCase() == "PITO")
                    Res = 'PITX';
                if (Cad.toUpperCase() == "CULO")
                    Res = 'CULX';
                return Res;
            }

    </script>

    <script type="text/javascript" language="javascript">
    window.onerror = new Function("return true");
    //Abrir una ventana modal
		function Abrir_Ventana_Modal(Url, Propiedades)
		{
			window.showModalDialog(Url, null, Propiedades);
		}
    </script>

</head>
<body>
    <form id="Frm_Menu_Pre_Propietarios" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Upd_Panel" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="Uprg_Reporte" runat="server" AssociatedUpdatePanelID="Upd_Panel"
                DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter">
                    </div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../../../imagenes/paginas/Sias_Roler.gif" /></div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="Div_Contenedor_Msj_Error" style="width: 98%;" runat="server" visible="false">
                <table style="width: 100%;">
                    <tr>
                        <td colspan="2" align="left">
                            <asp:ImageButton ID="IBtn_Imagen_Error" runat="server" ImageUrl="~/paginas/imagenes/paginas/sias_warning.png"
                                Width="24px" Height="24px" />
                            <asp:Label ID="Lbl_Ecabezado_Mensaje" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%;">
                        </td>
                        <td style="width: 90%; text-align: left;" valign="top">
                            <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table style="width: 98%;" border="0" cellspacing="0" class="estilo_fuente" cellpadding="4">
                    <tr>
                        <td align="center" class="style2" style="width: 20%">
                            &nbsp;
                        </td>
                        <td align="center" style="width: 30%">
                            &nbsp;
                        </td>
                        <td align="center" class="style2" style="width: 20%">
                            &nbsp;
                        </td>
                        <td align="center" class="style2" style="width: 30%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr class="barra_busqueda">
                        <td colspan="3" align="center">
                            <asp:ImageButton ID="Btn_Regresar" runat="server" AlternateText="Regresar" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/icono_salir.png" OnClick="Btn_Regresar_Click"
                                Width="24px" />
                            <asp:ImageButton ID="Btn_Busqueda" runat="server" AlternateText="Regresar" CssClass="Img_Button"
                                ImageUrl="~/paginas/imagenes/paginas/Busqueda_00001.png" OnClick="Btn_Busqueda_Click"
                                Width="24px" />
                        </td>
                        <td style="text-align: right">
                            <asp:ImageButton ID="Btn_Agregar_Co_Propietarios" runat="server" ToolTip="Agregar"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/sias_add.png" Height="22px"
                                Width="22px" OnClick="Btn_Agregar_Co_Propietarios_Click" />
                            <asp:ImageButton ID="Btn_Guardar_Copropietario" runat="server" ToolTip="Guardar"
                                TabIndex="10" ImageUrl="~/paginas/imagenes/paginas/icono_guardar.png" Height="22px"
                                Width="22px" Visible="false" OnClick="Btn_Guardar_Copropietario_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="background-color: #6699FF" colspan="4">
                            <asp:Label ID="Lbl_Title" runat="server" Text="Label" Font-Bold="True" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <div id="Buscar" runat="server">
                        <tr>
                            <td>
                                Nombre
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="Txt_Nombre" runat="server" Width="98%" TabIndex="2" Style="text-transform: uppercase"
                                    AutoPostBack="true" OnTextChanged="Txt_Nombre_TextChanged"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="Txt_Nombre"
                                    WatermarkText="Nombre(s)" WatermarkCssClass="watermarked" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                RFC
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Rfc" runat="server" Width="96%" TabIndex="4" Style="text-transform: uppercase"
                                    AutoPostBack="true" OnTextChanged="Txt_Rfc_TextChanged"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="Txt_Rfc"
                                    WatermarkText="Registro Federal de Contribuyentes" WatermarkCssClass="watermarked" />
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView runat="server" AllowPaging="True" AutoGenerateColumns="False" GridLines="None"
                                    CssClass="GridView_1" Width="100%" ID="Grid_Contribuyentes" PageSize="5" OnPageIndexChanging="Grid_Contribuyentes_PageIndexChanging"
                                    OnSelectedIndexChanged="Grid_Contribuyentes_SelectedIndexChanged" Style="white-space: normal;">
                                    <AlternatingRowStyle CssClass="GridAltItem"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:ButtonField CommandName="Select" ImageUrl="~/paginas/imagenes/gridview/blue_button.png"
                                            ButtonType="Image">
                                            <ItemStyle Width="30px"></ItemStyle>
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="CONTRIBUYENTE_ID" HeaderText="Contribuyente_ID" SortExpression="CONTRIBUYENTE_ID">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" SortExpression="NOMBRE">
                                            <HeaderStyle Width="60%" />
                                            <ItemStyle Width="60%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RFC" HeaderText="RFC" SortExpression="RFC">
                                            <HeaderStyle Width="30%" />
                                            <ItemStyle Width="30%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                    <PagerStyle CssClass="GridHeader"></PagerStyle>
                                    <RowStyle CssClass="GridItem"></RowStyle>
                                    <SelectedRowStyle CssClass="GridSelected"></SelectedRowStyle>
                                </asp:GridView>
                            </td>
                        </tr>
                </table>
            </div>
            <div id="Registro" runat="server">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 30%">
                            Tipo Persona
                        </td>
                        <td style="width: 66%">
                            <asp:DropDownList ID="Cmb_Tipo_Persona" runat="server" Width="70%" AutoPostBack="True"
                                OnSelectedIndexChanged="Cmb_Tipo_Persona_SelectedIndexChanged">
                                <asp:ListItem Text="<SELECCIONE>" Value="SELECCIONE" />
                                <asp:ListItem Text="FISICA" Value="FISICA" />
                                <asp:ListItem Text="MORAL" Value="MORAL" />
                            </asp:DropDownList>
                            <%--<asp:TextBox ID ="Txt_Tipo_Persona" runat ="server" ></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            RFC
                        </td>
                        <td>
                            <asp:TextBox ID="Txt_RFC_Contribuyente" runat="server" Style="text-transform: uppercase"
                                Width="70%"></asp:TextBox>
                        </td>
                    </tr>
                    <div id="Moral" runat="server">
                        <tr>
                            <td>
                                Razón Social
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Razon_Social" runat="server" Width="70%" Style="text-transform: uppercase"></asp:TextBox>
                            </td>
                        </tr>
                    </div>
                    <div id="Fisica" runat="server">
                        <tr>
                            <td>
                                Fecha Nacimiento
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Fecha_Nacimiento" runat="server" Width="65%" MaxLength="20"
                                    onkeyup="Generar_Rfc();" onchange="Generar_Rfc();"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TWE_Txt_Fecha_Nacimiento" runat="server" WatermarkText="<dd/MM/aaaa>"
                                    TargetControlID="Txt_Fecha_Nacimiento" WatermarkCssClass="watermarked">
                                </cc1:TextBoxWatermarkExtender>
                                <asp:ImageButton ID="Btn_Fecha_Nacimiento" runat="server" ImageUrl="~/paginas/imagenes/paginas/SmallCalendar.gif" />
                                <cc1:CalendarExtender ID="CE_Fecha_Nacimiento" runat="server" TargetControlID="Txt_Fecha_Nacimiento"
                                    PopupButtonID="Btn_Fecha_Nacimiento" Format="dd/MM/yyyy">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Apellido Paterno
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Apeido_Paterno" runat="server" onkeyup="Generar_Rfc();" Style="text-transform: uppercase"
                                    Width="70%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Apellido Materno
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Apeido_Materno" runat="server" onkeyup="Generar_Rfc();" Style="text-transform: uppercase"
                                    Width="70%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nombre(s)
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_Nombre_Contribuyente" runat="server" onkeyup="Generar_Rfc();"
                                    Style="text-transform: uppercase" Width="70%"></asp:TextBox>
                            </td>
                        </tr>
                    </div>
            </div>
            </table>
            <%--<table style="width:100%;" border="0" cellspacing="0" class="estilo_fuente">
                        <tr style="background-color: #DADADA">
                            <td style="width:15%">Contribuyente ID</td>
                            <td>
                                <asp:Label ID="Lbl_Contribuyente_ID" runat="server"></asp:Label>
                                  </td>
                            <td style="width:15%">Nombre</td>
                            <td>
                                <asp:Label ID="Lbl_Nombre" runat="server"></asp:Label>
                                  </td>
                        </tr>
                        <tr>
                            <td style="width:15%">RFC</td>
                            <td>
                                <asp:Label ID="Lbl_RFC" runat="server"></asp:Label>
                                  </td>
                            <td style="width:15%">Domicilio</td>
                            <td>
                                <asp:Label ID="Lbl_Domicilio" runat="server"></asp:Label>
                                  </td>
                        </tr>
                        <tr style="background-color: #DADADA">
                            <td style="width:15%">Interior</td>
                            <td>
                                <asp:Label ID="Lbl_Interior" runat="server"></asp:Label>
                                  </td>
                            <td style="width:15%">Exterior</td>
                            <td>
                                <asp:Label ID="Lbl_Exterior" runat="server"></asp:Label>
                                  </td>
                        </tr>
                        <tr>
                            <td style="width:15%">Colonia</td>
                            <td>
                                <asp:Label ID="Lbl_Colonia" runat="server"></asp:Label>
                                  </td>
                            <td style="width:15%">Ciudad</td>
                            <td>
                                <asp:Label ID="Lbl_Ciudad" runat="server"></asp:Label>
                                  </td>
                        </tr>
                        <tr style="background-color: #DADADA">
                            <td style="width:15%">CP</td>
                            <td>
                                <asp:Label ID="Lbl_CP" runat="server"></asp:Label>
                                  </td>
                            <td style="width:15%">Estado</td>
                            <td>
                                <asp:Label ID="Lbl_Estado" runat="server"></asp:Label>
                                  </td>
                        </tr>
                    </table>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
