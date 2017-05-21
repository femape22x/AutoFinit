<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/AutoFinit.Master" AutoEventWireup="true" CodeBehind="InicioPages.aspx.cs" Inherits="AutoFinit.Formulario_web11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container " role="main">
        <!-- Main jumbotron for a primary marketing message or call to action -->
        <div class="jumbotron">
            <h1>AutoFinit</h1>
            <br/>
            <p>
                Este es un proyecto informático educativo, sin ánimos de lucro, 
                desarrollado para la clase de Autómatas, Gramáticales y Lenguaje; 
                cuyo fin es llevar a cabo la impletación ó práctica de los temas 
                tratados en el salón de clases.
            </p>
            <p>
                <strong>AutoFinit</strong> permite convertir un Automata Finito No Determinado(AFN)
                a uno Determinado(AFD) a travez de datos de entrada de un archivo de texto con 
                notación XML, el cual requiere ser importado en esta página.
            </p>
            <br/>
            <div class="centrado">
                <label style="font-weight:bold;font-size:20px;">Importar aquí su archivo XML</label>
                <div class="row"> 
                    <div class="col-md-5"></div>
                    <div class="col-md-2">
                        <asp:FileUpload ID="fluArchivo" runat="server" />
                        <br/>
                        <asp:Button ID="BtnLeer" CssClass="btn btn-primary" Text="Leer Archivo" runat="server" OnClick="BtnLeer_Click"></asp:Button>
                    </div>
                    <div class="col-md-5"></div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
