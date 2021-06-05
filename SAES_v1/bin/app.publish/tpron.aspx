﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="tpron.aspx.cs" Inherits="SAES_v1.tpron" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/gijgo@1.9.6/js/gijgo.min.js" type="text/javascript"></script>
    <script src="https://cdn.jsdelivr.net/npm/gijgo@1.9.6/js/messages/messages.es-es.js" type="text/javascript"></script>
    <link href="https://unpkg.com/gijgo@1.9.13/css/gijgo.min.css" rel="stylesheet" type="text/css" />
    
    <style>
        span button {
            margin-bottom: 0px !important;
        }
    </style>
   <script>
       function save() {
           swal({
               allowEscapeKey: false,
               allowOutsideClick: false,
               type: 'success',
               html: '<h2 class="swal2-title" id="swal2-title">Se guardaron los datos exitosamente</h2>Favor de validar en el listado.'
           })
       }

       function update() {
           swal({
               allowEscapeKey: false,
               allowOutsideClick: false,
               type: 'success',
               html: '<h2 class="swal2-title" id="swal2-title">Se guardaron los datos exitosamente</h2>Favor de validar en el listado.'
           })
       }

       function unsave() {
           swal({
               allowEscapeKey: false,
               allowOutsideClick: false,
               type: 'success',
               html: '<h2 class="swal2-title" id="swal2-title">NO SE PUEDE REALIZAR GENERACIÓN !!! EXISTEN ALUMNOS INSCRITOS'
           })
       }

       function valor_numerico() {
           swal({
               allowEscapeKey: false,
               allowOutsideClick: false,
               type: 'success',
               html: '<h2 class="swal2-title" id="swal2-title">ERROR !!!</h2>Valor de Pronostico debe ser numerico'
           })
       }

       function NoProcesar() {
           swal({
               allowEscapeKey: false,
               allowOutsideClick: false,
               type: 'success',
               html: '<h2 class="swal2-title" id="swal2-title">ERROR !!!</h2>No hay datos para procesar.'
           })
       }

       //-----  Función de agregar error  ------//
       function errorForm(idElementForm, textoAlerta) {
           let elemento = idElementForm;
           let textoInterno = textoAlerta;
           let elementoId = document.getElementById(elemento);
           elementoId.classList.add('error');
           elementoId.classList.remove('validado');
           elementoId.classList.remove('sinvalidar');
           errorId = 'error-' + String(elemento);
           let tieneError = document.getElementById(errorId);
           if (!tieneError) {
               const parrafo = document.createElement("p");
               const contParrafo = document.createTextNode(textoInterno);
               parrafo.appendChild(contParrafo);
               parrafo.classList.add('textoError');
               parrafo.id = 'error-' + String(elemento)
               //Depende de estructura de HTML
               elementoId.parentNode.appendChild(parrafo);
           }
       }

       //----- Función de remover error ------//
       function validadoForm(idElementForm) {
           let elemento = idElementForm;
           let elementoId = document.getElementById(elemento);
           elementoId.classList.remove('error');
           elementoId.classList.add('validado');
           elementoId.classList.remove('sinvalidar');
           errorId = 'error-' + String(elemento);
           let tieneError = document.getElementById(errorId);
           if (tieneError) {
               tieneError.remove();
           }
       }

   </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="x_title">
        <h2>
            <img src="Images/Operaciones/tpron.png" style="width: 30px;" /><small>Pronóstico Nuevo Ingreso</small></h2>
        <div class="clearfix"></div>
    </div>
    <div class="x_content">
        <asp:UpdatePanel ID="upd_tpron" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="form_tpron" runat="server">
                    <div class="row g-3 justify-content-center" style="margin-top: 15px;">
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_ddl_periodo" class="form-label">Periodo</label>
                            <asp:DropDownList ID="ddl_periodo" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_ddl_turno" class="form-label">Turno</label>
                            <asp:DropDownList ID="ddl_turno" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label for="ContentPlaceHolder1_ddl_campus" class="form-label">Campus</label>
                            <asp:DropDownList ID="ddl_campus" runat="server" CssClass="form-control" ></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row justify-content-center" style="text-align: center; margin: auto;" id="btn_tpron" runat="server">
                    <div class="col-md-4" style="text-align: center; margin-top: 15px;">
                        <asp:Button ID="btn_search" runat="server" CssClass="btn btn-round btn-secondary" Text="Buscar" OnClick="grid_tpron_bind" />
                        <asp:Button ID="btn_save" runat="server" CssClass="btn btn-round btn-success" Text="Agregar" OnClick="btn_save_Click" Visible="false"/>
                        <asp:Button ID="btn_update" runat="server" CssClass="btn btn-round btn-success" Text="Generar Grupos" Visible="false" OnClick="btn_update_Click" />
                    </div>
                </div>
                <div id="table_tpron">
                    <asp:GridView ID="Gridtpron" runat="server" CssClass="table table-striped table-bordered" Width="100%" AutoGenerateColumns="false" RowStyle-Font-Size="small" >
                        <Columns>
                            
                            <asp:BoundField DataField="NIVEL" HeaderText="Nivel" />
                            <asp:BoundField DataField="CLAVE" HeaderText="Clave" />
                            <asp:BoundField DataField="PROGRAMA" HeaderText="Programa" />
                            <ASP:TEMPLATEFIELD HEADERTEXT="Pronostico"  ItemStyle-HorizontalAlign="Center">
                                    <ITEMTEMPLATE >
                                        <ASP:LABEL ID="Lbl" RUNAT="server" ItemStyle-Width="10px" Text="-->" 
                                            BACKCOLOR="WhiteSmoke"  />
                                        <ASP:TEXTBOX ID="pronostico" RUNAT="server" width="80px" 
                                            BACKCOLOR="SteelBlue"  BorderStyle="None" 
                                            style="text-align:right"/>    
                                    </ITEMTEMPLATE>
                            </ASP:TEMPLATEFIELD>
                        </Columns>
                        <SelectedRowStyle CssClass="selected_table" />
                        <HeaderStyle BackColor="#2a3f54" ForeColor="white" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
 <script>
     function load_datatable() {
         let table_periodo = $("#ContentPlaceHolder1_Gridtpron").DataTable({
             language: {
                 bProcessing: 'Procesando...',
                 sLengthMenu: 'Mostrar _MENU_ registros',
                 sZeroRecords: 'No se encontraron resultados',
                 sEmptyTable: 'Ningún dato disponible en esta tabla',
                 sInfo: 'Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros',
                 sInfoEmpty: 'Mostrando registros del 0 al 0 de un total de 0 registros',
                 sInfoFiltered: '(filtrado de un total de _MAX_ registros)',
                 sInfoPostFix: '',
                 sSearch: 'Buscar:',
                 sUrl: '',
                 sInfoThousands: '',
                 sLoadingRecords: 'Cargando...',
                 oPaginate: {
                     sFirst: 'Primero',
                     sLast: 'Último',
                     sNext: 'Siguiente',
                     sPrevious: 'Anterior'
                 }
             },
             scrollResize: true,
             scrollY: '500px',
             scrollCollapse: true,
             order: [
                 [0, "asc"]
             ],
             lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
             "autoWidth": true,
             dom: '<"top"if>rt<"bottom"lBp><"clear">',
             buttons: [
                 {
                     title: 'SAES_Pronóstico Nuevo Ingreso',
                     className: 'btn-dark',
                     extend: 'excel',
                     text: 'Exportar Excel',
                     exportOptions: {
                         columns: [0, 1, 2, 3]
                     }
                 },
                 {
                     title: 'SAES_Pronóstico Nuevo Ingreso',
                     className: 'btn-dark',
                     extend: 'pdfHtml5',
                     text: 'Exportar PDF',
                     orientation: 'landscape',
                     pageSize: 'LEGAL',
                     exportOptions: {
                         columns: [0, 1, 2, 3]
                     }
                 }
             ],
             stateSave: true
         });
     }

     function destroy_table() {
         $("#ContentPlaceHolder1_Gridtpron").DataTable().destroy();
     }
     function remove_class() {
         $('.selected_table').removeClass("selected_table")
     }
 </script>
</asp:Content>







