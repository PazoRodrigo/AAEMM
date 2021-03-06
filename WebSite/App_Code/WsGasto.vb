﻿Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Clases

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class WsGasto
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function TraerTodos() As Transfer
        Dim ws As New Transfer
        Try
            Dim result As New List(Of DTO.DTO_Gasto)
            Dim lista As List(Of Entidad.Gasto) = Entidad.Gasto.TraerTodos()
            If lista.Count > 0 Then
                For Each item As Entidad.Gasto In lista
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
    Public Function TraerTodosUltimos5() As Transfer
        Dim ws As New Transfer
        Try
            Dim result As New List(Of DTO.DTO_Gasto)
            Dim lista As List(Of Entidad.Gasto) = Entidad.Gasto.TraerTodosUltimos5()
            If Not lista Is Nothing Then
                For Each item As Entidad.Gasto In lista
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
    Public Function TraerGastosAbiertos() As Transfer
        Dim ws As New Transfer
        Try
            Dim result As New List(Of DTO.DTO_Gasto)
            Dim lista As List(Of Entidad.Gasto) = Entidad.Gasto.TraerGastosAbiertos()
            If lista.Count > 0 Then
                For Each item As Entidad.Gasto In lista
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
    Public Function Alta(entidad As DTO.DTO_Gasto) As Transfer
        Dim ws As New Transfer
        Try
            Dim objGuardar As New Entidad.Gasto(entidad)
            objGuardar.Alta()
            ws.data = objGuardar.IdEntidad
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
    Public Function Baja(entidad As DTO.DTO_Gasto) As Transfer
        Dim ws As New Transfer
        Try
            Dim objGuardar As New Entidad.Gasto(entidad)
            objGuardar.Baja()
            ws.data = objGuardar.IdEntidad
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
    Public Function Cerrar(entidad As DTO.DTO_Gasto) As Transfer
        Dim ws As New Transfer
        Try
            Dim objGuardar As New Entidad.Gasto(entidad)
            objGuardar.Cerrar()
            ws.data = objGuardar.IdEntidad
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
    Public Function TraerTodosXBusqueda(Busqueda As Entidad.Gasto.StrBusquedaGasto) As Transfer
        Dim ws As New Transfer
        Try
            Dim result As New List(Of DTO.DTO_Gasto)
            Dim lista As List(Of Entidad.Gasto) = Entidad.Gasto.TraerTodosXBusqueda(Busqueda)
            If Not lista Is Nothing Then
                For Each item As Entidad.Gasto In lista
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
End Class