﻿Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Clases

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class WsAsientoLinea
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function Alta(entidad As DTO_Entidad.DTO_AsientoLinea) As Transfer
        Dim ws As New Transfer
        Try
            Dim Obj As New Entidad_Asiento.AsientoLinea(entidad)
            Obj.Alta()
            ws.data = Obj.IdEntidad
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