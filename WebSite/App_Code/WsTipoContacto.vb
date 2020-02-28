﻿Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Clases

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class WsTipoContacto
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function TraerTodos() As Transfer
        Dim ws As New Transfer
        Try
            Dim result As New List(Of DTO.DTO_TipoContacto)
            Entidad.TipoContacto.Refresh()
            Dim lista As List(Of Entidad.TipoContacto) = Entidad.TipoContacto.TraerTodos()
            If Not lista Is Nothing Then
                For Each item As Entidad.TipoContacto In lista
                    result.Add(item.ToDTO)
                Next
            End If
            ws.data = result
            ws.todoOk = True
            ws.mensaje = ""
        Catch ex As Exception
            ws.todoOk = False
            ws.mensaje = ex.Message
            ws.data = Nothing
        End Try
        Return ws
    End Function
    <WebMethod()>
    Public Function Alta(entidad As Entidad.TipoContacto) As Transfer
        Dim ws As New Transfer
        Try
            entidad.IdUsuarioAlta = 1
            entidad.Alta()
            ws.data = entidad.IdEntidad
            ws.todoOk = True
            ws.mensaje = ""
        Catch ex As Exception
            ws.todoOk = False
            ws.mensaje = ex.Message
            ws.data = Nothing
        End Try
        Return ws
    End Function
    <WebMethod()>
    Public Function Modifica(entidad As Entidad.TipoContacto) As Transfer
        Dim ws As New Transfer
        Try
            entidad.IdUsuarioModifica = 1
            entidad.Modifica()
            ws.data = entidad.IdEntidad
            ws.todoOk = True
            ws.mensaje = ""
        Catch ex As Exception
            ws.todoOk = False
            ws.mensaje = ex.Message
            ws.data = Nothing
        End Try
        Return ws
    End Function
    <WebMethod()>
    Public Function Baja(entidad As Entidad.TipoContacto) As Transfer
        Dim ws As New Transfer
        Try
            entidad.IdUsuarioBaja = 1
            entidad.Baja()
            ws.data = entidad.IdEntidad
            ws.todoOk = True
            ws.mensaje = ""
        Catch ex As Exception
            ws.todoOk = False
            ws.mensaje = ex.Message
            ws.data = Nothing
        End Try
        Return ws
    End Function
End Class