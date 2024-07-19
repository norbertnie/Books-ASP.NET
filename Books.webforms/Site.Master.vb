Imports System.Web.Configuration
Imports Books.Models.Dao

Public Class SiteMaster
    Inherits MasterPage
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        Dim page as Page = me.Page
        Dim appPage As AppPage = TryCast(page, AppPage)
        If  appPage IsNot nothing Then
            appPage.DbConnectionHolder = new DbConnectionHolder(WebConfigurationManager.AppSettings("ConnectionString"))
        End If
    End Sub
End Class