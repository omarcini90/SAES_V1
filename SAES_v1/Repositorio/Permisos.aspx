<%@ Page Title="" Language="C#" MasterPageFile="~/Repositorio/Site1.Master" AutoEventWireup="true" CodeBehind="Permisos.aspx.cs" Inherits="SAES_v1.Repositorio.Permisos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Template/Sitemaster/vendors/jquery/dist/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.js" integrity="sha512-n/4gHW3atM3QqRcbCn6ewmpxcLAHGaDjpEBu4xZd47N0W2oQ+6q7oc3PXstrJYXcbNU1OHdQ1T7pAP+gi5Yu8g==" crossorigin="anonymous"></script>
    <style>
        #ContentPlaceHolder1_gvTipoDocumento_info {
            float: left;
        }

        #ContentPlaceHolder1_gvTipoDocumento_length {
            float: left;
        }

        .bottom {
            text-align: center;
            padding-top: 5px;
        }

        @media screen and (max-width: 540px) {
            .bottom {
                margin-top: 50px;
                text-align: center;
                display: grid;
            }
        }

        .ocultar {
            display: none;
        }

        table.dataTable tbody > tr.selected, table.dataTable tbody > tr > .selected {
            background-color: #88e5cf !important;
        }

        .ddlist {
            left: 725px;
            text-align: center;
        }

        .center_column {
            text-align: center;
        }

        .pills {
            /* overflow: visible; */
            background: #f5f7fa;
            height: 20px;
            /* margin: 21px 0 14px; */
            padding-left: 14px;
            position: relative;
            z-index: 1;
            width: 100%;
            border-bottom: 1px solid #e6e9ed;
        }

        .nav-item {
            margin-left: 8px;
            border: 1px solid #e6e9ed !important;
            margin-top: -17px !important;
            background: #f5f7fa;
        }

            .nav-item:hover {
                margin-left: 8px;
                border: 1px solid #e6e9ed !important;
                margin-top: -17px !important;
                background: #fff;
            }

            .nav-link.active,
            .nav-item.show .nav-link {
                border-right: 5px solid #169f85 !important;
            }

        .buttons-excel {
            margin-right: 5px !important;
            border-top-right-radius: 3px !important;
            border-bottom-right-radius: 3px !important;
        }

        .buttons-pdf {
            border-top-left-radius: 3px !important;
            border-bottom-left-radius: 3px !important;
        }

        .dataTables_scroll {
            height: 270px !important;
        }

        .focus {
            border: 1px solid rgb(227, 65, 0);
        }

        .right_col {
            min-height: 700px !important;
        }

        .textoError {
            color: red;
            /*position: absolute;*/
            font-size: .9em;
            font-weight: 700;
            margin-bottom: 1px;
            /*top: 13px;*/
        }

        .listbox {
            margin: auto;
            border-radius: 5px;
        }

        .ul_submenu {
            list-style-type: disc;
            margin-block-start: 1em;
            margin-block-end: 1em;
            margin-inline-start: 0px;
            margin-inline-end: 0px;
            padding-inline-start: 40px;
            margin-top: 0;
            margin-bottom: 0rem;
        }

        .ul_permiso {
            list-style-type: disc;
            margin-block-start: 1em;
            margin-block-end: 1em;
            margin-inline-start: 0px;
            margin-inline-end: 0px;
            padding-inline-start: 80px;
            margin-top: 0;
            margin-bottom: 0rem;
        }

        .table-mod_2 {
            color: #fff;
            width: 10px;
            border-right-style: hidden;
        }

        .ex3 {
            width: 100%;
            height: 300px;
            overflow: auto;
        }

        /* The switch - the box around the slider */
        .switch {
            position: relative;
            display: inline-block;
            width: 35px;
            height: 18px;
        }

            /* Hide default HTML checkbox */
            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        /* The slider */
        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 15px;
                width: 15px;
                left: 1px;
                bottom: 2px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #169f85;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #169f85;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(16px);
            -ms-transform: translateX(16px);
            transform: translateX(16px);
        }
        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }

        .ColumnaOculta {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_3 {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_4 {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_7 {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_8 {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_9 {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_11 {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_12 {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_13 {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_14 {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_16 {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_17 {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_18 {
            display: none;
        }

        #ContentPlaceHolder1_Permisos_App_MenuP_19 {
            display: none;
        }

    </style>
    <script>
        function save() {
            swal({
                allowEscapeKey: false,
                allowOutsideClick: false,
                type: 'success',
                html: '<h2 class="swal2-title" id="swal2-title">Se guardaron los datos exitosamente</h2>Favor de validar'
            })
                .then(willDelete => {
                    if (willDelete) {
                        loader_push();
                        window.location.reload();
                    }
                });
        }
        function guardar_permisos(rol) {
            swal("Permisos modificados exitosamente", "Los permisos del perifl han sido modificados exitosamente.", "success")
                .then(willDelete => {
                    if (willDelete) {
                        window.location = "Permisos.aspx?rol="+rol;
                        //window.location.reload();
                        //swal("Deleted!", "Your imaginary file has been deleted!", "success");
                    }
                });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="x_title">
        <h2>
            <img src="../Images/Repositorio/settings.png" style="width: 45px;" /><small>Configuración de Permisos</small></h2>
        <div class="clearfix"></div>
    </div>
    <div class="x_content">
        <div class="row g-3 justify-content-center" style="margin-top: 15px;">
            <div class="col-md-4">
                <label for="ContentPlaceHolder1_ddlRol" class="form-label">Rol</label>
                <asp:DropDownList ID="ddlRol" runat="server" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlRol_SelectedIndexChanged" CssClass="form-control">
                </asp:DropDownList>
            </div>
        </div>
        <div id="acordion" runat="server">
            <!-- start accordion -->
            <div class="accordion" id="accordion" role="tablist" aria-multiselectable="true" style="margin-top: 15px;">
                <div id="p_campus" runat="server">
                    <div class="panel" id="campus">
                        <a class="panel-heading" role="tab" id="headingOne" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                            <h4 class="panel-title">Campus</h4>
                        </a>
                        <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                            <div class="panel-body">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="row g-3 justify-content-center" style="margin-top: 15px; width: 70%; margin: auto;">
                                            <div class="col-md-3 ju">
                                                <label for="ContentPlaceHolder1_lstCampus" class="form-label">Listado de Campus</label>
                                                <asp:ListBox ID="lstCampus" runat="server" Height="150px" Width="200px" SelectionMode="Multiple" CssClass="form-control listbox" OnSelectedIndexChanged="lstCampus_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
                                            </div>
                                            <div class="col-md-1" style="text-align: center; margin-top: 80px;">
                                                <asp:Image ID="doble" runat="server" ImageUrl="~/Images/Repositorio/Doble.png" Width="40px" Height="40px" />
                                            </div>
                                            <div class="col-md-3">
                                                <label for="ContentPlaceHolder1_lstUsuariosCampus" class="form-label">Usuarios asignados al campus</label>
                                                <asp:ListBox ID="lstUsuariosCampus" runat="server" Height="150px" Width="200px" SelectionMode="Multiple" CssClass="form-control listbox"></asp:ListBox>
                                            </div>
                                            <div class="col-md-1" style="text-align: center; margin-top: 80px;">
                                                <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="~/Images/Repositorio/Izq.png"
                                                    Width="25px" OnClick="cmdAgregar_Click" /><br />
                                                <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="~/Images/Repositorio/Derecha.png"
                                                    Width="25px" OnClick="cmdBorrar_Click" />
                                            </div>
                                            <div class="col-md-3">
                                                <label for="ContentPlaceHolder1_lstUsuarios" class="form-label">Listado de Usuarios</label>
                                                <asp:ListBox ID="lstUsuarios" runat="server" Height="150px" Width="200px" SelectionMode="Multiple" CssClass="form-control listbox"></asp:ListBox>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="p_nivel" runat="server">
                    <div class="panel" id="nivel">
                        <a class="panel-heading collapsed" role="tab" id="headingTwo" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                            <h4 class="panel-title">Niveles</h4>
                        </a>
                        <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                            <div class="panel-body">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="row g-3 justify-content-center" style="margin-top: 15px; width: 70%; margin: auto;">
                                            <div class="col-md-3 ju">
                                                <label for="ContentPlaceHolder1_ListNiveles" class="form-label">Listado de Niveles</label>
                                                <asp:ListBox ID="ListNiveles" runat="server" Height="150px" Width="200px" SelectionMode="Multiple" CssClass="form-control listbox" OnSelectedIndexChanged="ListNiveles_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
                                            </div>
                                            <div class="col-md-1" style="text-align: center; margin-top: 80px;">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Repositorio/Doble.png" Width="40px" Height="40px" />
                                            </div>
                                            <div class="col-md-3">
                                                <label for="ContentPlaceHolder1_ListUsuariosNivel" class="form-label">Usuarios asignados al nivel</label>
                                                <asp:ListBox ID="ListUsuariosNivel" runat="server" Height="150px" Width="200px" SelectionMode="Multiple" CssClass="form-control listbox"></asp:ListBox>
                                            </div>
                                            <div class="col-md-1" style="text-align: center; margin-top: 80px;">
                                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/Repositorio/Izq.png"
                                                    Width="25px" OnClick="cmdAgregar_Click_n" /><br />
                                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/Repositorio/Derecha.png"
                                                    Width="25px" OnClick="cmdBorrar_Click_n" />
                                            </div>
                                            <div class="col-md-3">
                                                <label for="ContentPlaceHolder1_ListUsuarios" class="form-label">Listado de Usuarios</label>
                                                <asp:ListBox ID="ListUsuarios" runat="server" Height="150px" Width="200px" SelectionMode="Multiple" CssClass="form-control listbox"></asp:ListBox>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="p_documentos" runat="server">
                    <div class="panel" id="documentos">
                        <a class="panel-heading collapsed" role="tab" id="headingThree" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
                            <h4 class="panel-title">Tipos de Documentos</h4>
                        </a>
                        <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                            <div class="panel-body">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="row g-3 justify-content-center" style="margin-top: 15px; width: 70%; margin: auto;">
                                            <div class="col-md-3 ju">
                                                <label for="ContentPlaceHolder1_ListDocumentos" class="form-label">Listado de Documentos</label>
                                                <asp:ListBox ID="ListDocumentos" runat="server" Height="150px" Width="200px" SelectionMode="Multiple" CssClass="form-control listbox" OnSelectedIndexChanged="ListDocumentos_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
                                            </div>
                                            <div class="col-md-1" style="text-align: center; margin-top: 80px;">
                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Repositorio/Doble.png" Width="40px" Height="40px" />
                                            </div>
                                            <div class="col-md-3">
                                                <label for="ContentPlaceHolder1_ListUsuariosDocumentos" class="form-label">Usuarios asignados a documentos</label>
                                                <asp:ListBox ID="ListUsuariosDocumentos" runat="server" Height="150px" Width="200px" SelectionMode="Multiple" CssClass="form-control listbox"></asp:ListBox>
                                            </div>
                                            <div class="col-md-1" style="text-align: center; margin-top: 80px;">
                                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/Repositorio/Izq.png"
                                                    Width="25px" OnClick="cmdAgregar_Click_d" /><br />
                                                <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Images/Repositorio/Derecha.png"
                                                    Width="25px" OnClick="cmdBorrar_Click_d" />
                                            </div>
                                            <div class="col-md-3">
                                                <label for="ContentPlaceHolder1_ListUsuarios_1" class="form-label">Listado de Usuarios</label>
                                                <asp:ListBox ID="ListUsuarios_1" runat="server" Height="150px" Width="200px" SelectionMode="Multiple" CssClass="form-control listbox"></asp:ListBox>
                                            </div>
                                        </div>
                                        <div class="row g-3 justify-content-center" style="margin-top: 15px; width: 70%; margin: auto;">
                                            <div class="col-md-3 ju">
                                                <label for="ContentPlaceHolder1_ListDocs" class="form-label">Listado de Documentos</label>
                                                <asp:ListBox ID="ListDocs" runat="server" Height="150px" Width="200px" SelectionMode="Multiple" CssClass="form-control listbox" OnSelectedIndexChanged="ListDocs_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
                                            </div>
                                            <div class="col-md-1" style="text-align: center; margin-top: 40px;">
                                                <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/Repositorio/Doble.png" Width="40px" Height="40px" />
                                            </div>
                                            <div class="col-md-3">
                                                <label for="ContentPlaceHolder1_ListDocE" class="form-label">Estatus asignados a documentos</label>
                                                <asp:ListBox ID="ListDocE" runat="server" Height="150px" Width="200px" SelectionMode="Multiple" CssClass="form-control listbox"></asp:ListBox>
                                            </div>
                                            <div class="col-md-1" style="text-align: center; margin-top: 40px;">
                                                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/Images/Repositorio/Izq.png"
                                                    Width="25px" OnClick="cmAgregarEstatus_Click" /><br />
                                                <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Images/Repositorio/Derecha.png"
                                                    Width="25px" OnClick="cmBorrarEstatus_Click" />
                                            </div>
                                            <div class="col-md-3">
                                                <label for="ContentPlaceHolder1_ListEstatus" class="form-label">Listado de Estatus</label>
                                                <asp:ListBox ID="ListEstatus" runat="server" Height="150px" Width="200px" SelectionMode="Multiple" CssClass="form-control listbox"></asp:ListBox>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="p_permisos_app" runat="server">
                    <div class="panel" id="permisos_app">
                        <a class="panel-heading collapsed" role="tab" id="headingfour" data-toggle="collapse" data-parent="#accordion" href="#collapsefour" aria-expanded="false" aria-controls="collapsefour">
                            <h4 class="panel-title">Permisos de repositorio</h4>
                        </a>
                        <div id="collapsefour" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                            <div class="panel-body">
                                <asp:Panel ID="GridView" runat="server">
                                    <div id="privilegios_table">

                                        <div class="ex3">
                                            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:GridView ID="Permisos_App" runat="server" AutoGenerateColumns="False" RowHeaderColumn="IDPrivilegio" Width="100%" HorizontalAlign="Center" CssClass="table-hover" OnRowCommand="Permisos_App_RowCommand">
                                                        <Columns>
                                                            <asp:BoundField DataField="IDPrivilegio" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="ColumnaOculta" HeaderStyle-CssClass="ColumnaOculta">

                                                                <HeaderStyle CssClass="ColumnaOculta" />

                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton CommandName="Expand" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ID="MenuP" runat="server">
                                                                        <asp:Image ID="MenuP_" runat="server" ImageUrl="~/Images/Repositorio/flecha_rigth.png" />
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton CommandName="Collapse" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ID="MenuP2" runat="server" Visible="False">
                                                                        <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/Repositorio/Flecha_down.png" />
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Permiso" HeaderText="Permiso" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30%">
                                                                <ItemStyle Font-Size="Medium" HorizontalAlign="Left" Wrap="False" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50%">
                                                                <ItemStyle HorizontalAlign="Justify" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Activar/Desactivar">

                                                                <ItemTemplate>
                                                                    <label class="switch">
                                                                        <asp:CheckBox ID="Checkbox_P" runat="server" OnCheckedChanged="Checkbox_P_CheckedChanged" AutoPostBack="true" />
                                                                        <%--<asp:CheckBox ID="Checkbox_P" runat="server" OnCheckedChanged="Checkbox_P_CheckedChanged" AutoPostBack="true" xmlns:asp="#unknown"/>--%>

                                                                        <span class="slider round"></span>
                                                                    </label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="table-dark_1" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <%--<asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />--%>
                                                    <asp:AsyncPostBackTrigger ControlID="lstCampus" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ListNiveles" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ListDocumentos" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <div id="save_button" style="margin-top:15px;">
                                                <asp:Panel ID="Button" runat="server" HorizontalAlign="Center">
                                                    <asp:Button ID="Button1" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick="Button1_Click" />
                                                </asp:Panel>
                                            </div>
                                        </div>

                                    </div>
                                </asp:Panel>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <!-- end of accordion -->
        </div>
    </div>
    <!--Funciones pestaña permisos-->
    <script>
        function mostrar_permisos() {
            $("#accordion").css({ display: "initial" });
            $("#accordion").slideDown("slow");
        }
        function ocultar_permisos() {
            $("#accordion").slideUp("slow");
        }
        function ocultar_campus() {
            $("#campus").css({ display: "none" });
            $("#campus").fadeOut("fast");
        }
        function ocultar_niveles() {
            $("#nivel").fadeOut("fast");
        }
        function ocultar_documentos() {
            $("#documentos").fadeOut("fast");
        }
        function ocultar_permisos_app() {
            $("#permisos_app").fadeOut("fast");
        }
        function mostrar_campus() {
            $("#campus").fadeIn("fast");
        }
        function mostrar_niveles() {
            $("#nivel").fadeIn("fast");
        }
        function mostrar_documentos() {
            $("#documentos").fadeIn("fast");
        }
        function mostrar_permisos_app() {
            $("#permisos_app").fadeIn("fast");
        }
    </script>
    <!--/Funciones pestaña permisos-->
</asp:Content>
