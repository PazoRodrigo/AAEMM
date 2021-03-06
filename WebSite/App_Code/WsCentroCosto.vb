﻿Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Clases

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class WsCentroCosto
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function TraerTodos() As Transfer
        Dim ws As New Transfer
        Try
            Dim result As New List(Of DTO.DTO_CentroCosto)
            Entidad.CentroCosto.Refresh()
            Dim lista As List(Of Entidad.CentroCosto) = Entidad.CentroCosto.TraerTodos()
            If Not lista Is Nothing Then
                For Each item As Entidad.CentroCosto In lista
                    result.Add(item.ToDTO)
                Next
            End If
            ws.Data = result
            ws.TodoOk = True
            ws.Mensaje = ""
        Catch ex As Exception
            ws.TodoOk = False
            ws.Mensaje = ex.Message
            ws.Data = Nothing
        End Try
        Return ws
    End Function
    <WebMethod()>
    Public Function Alta(entidad As Entidad.CentroCosto) As Transfer
        Dim ws As New Transfer
        Try
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
    Public Function Modifica(entidad As Entidad.CentroCosto) As Transfer
        Dim ws As New Transfer
        Try
            entidad.Modifica()
            ws.Data = entidad.IdEntidad
            ws.TodoOk = True
            ws.Mensaje = ""
        Catch ex As Exception
            ws.TodoOk = False
            ws.Mensaje = ex.Message
            ws.Data = Nothing
        End Try
        Return ws
    End Function
    <WebMethod()>
    Public Function Baja(entidad As Entidad.CentroCosto) As Transfer
        Dim ws As New Transfer
        Try
            entidad.Baja()
            ws.Data = entidad.IdEntidad
            ws.TodoOk = True
            ws.Mensaje = ""
        Catch ex As Exception
            ws.TodoOk = False
            ws.Mensaje = ex.Message
            ws.Data = Nothing
        End Try
        Return ws
    End Function
End Class