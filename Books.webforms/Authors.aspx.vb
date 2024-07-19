Imports Books.Models
Imports Books.Models.Dao
Imports Books.Models.ViewModels

Public Class Authors
    Inherits AppPage

    Public IsEditMode As Boolean = False
    Public IsDeliteMode As Boolean = False
    Public IsSaveMode As Boolean = False
    Public EditedAuthor As Author = New Author()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Dim firstname_findValue As String = ""
        Dim lastname_findValue As String = ""
        Dim authorsDao As AuthorsDAO = New AuthorsDAO(Me.DbConnectionHolder)
        Dim authors As List(Of Author)

        If Me.IsPostBack And IsDeliteMode = False And IsSaveMode = False Then
            lastname_findValue = Request.Form("ctl00$MainContent$AuthorsRepeater$ctl00$FindBox_LastName")
            firstname_findValue = Request.Form("ctl00$MainContent$AuthorsRepeater$ctl00$FindBox_FirstName")

            If (lastname_findValue <> "" Or firstname_findValue <> "") Then
                authors = authorsDao.GetAuthors(firstname_findValue, lastname_findValue)
                IsEditMode = False
            Else
                Return
            End If
        Else
            IsDeliteMode = False
            authors = authorsDao.GetAuthors()
        End If

        Dim authorsVMs As List(Of AuthorViewModel) = GetViewModel(authors)

        AuthorsRepeater.DataSource = authorsVMs ' Use authorsVMs instead of authors
        AuthorsRepeater.DataBind()
    End Sub

    Protected Sub AddNewAuthor(sender As Object, e As CommandEventArgs)
        IsEditMode = True
    End Sub

    Protected Sub EditAuthor(sender As Object, e As CommandEventArgs)
        IsEditMode = True
        Dim editedAuthorId As Integer = Integer.Parse(e.CommandArgument)

        Dim authorsDao As New AuthorsDAO(Me.DbConnectionHolder)
        EditedAuthor = authorsDao.GetAuthor(editedAuthorId)
    End Sub

    Protected Sub DeleteAuthor(sender As Object, e As CommandEventArgs)
        Dim deletedAuthorId As Integer = Integer.Parse(e.CommandArgument)

        Dim authorsDao As New AuthorsDAO(Me.DbConnectionHolder)
        authorsDao.DeleteAuthor(deletedAuthorId)

        IsDeliteMode = True
    End Sub

    Protected Sub SaveAuthor(sender As Object, e As CommandEventArgs)
        Dim firstName As String = Request.Form("Editbox_firstName")
        Dim lastName As String = Request.Form("Editbox_lastName")
        Dim releaseDateStr As String = Request.Form("Editbox_releaseDate")
        Dim editedAuthorId As Integer = Integer.Parse(Request.Form("Editbox_id"))


        Dim authorsDao As New AuthorsDAO(Me.DbConnectionHolder)
        Dim booksDao As New BooksDao(Me.DbConnectionHolder)
        IsSaveMode = True

        If editedAuthorId = 0 Then
            authorsDao.InsertAuthor(firstName, lastName)
            ' Handle book insert logic if necessary
        Else
            authorsDao.UpdateAuthor(firstName, lastName, editedAuthorId)
            If Not String.IsNullOrEmpty(releaseDateStr) Then
                Dim releaseDate As DateTime
                If DateTime.TryParse(releaseDateStr, releaseDate) Then
                    authorsDao.UpdateFirstBookReleaseDate(editedAuthorId, releaseDate)
                End If
            End If
        End If
    End Sub



    Private Function GetViewModel(ByVal authors As List(Of Author)) As List(Of AuthorViewModel)
        Dim vm As New List(Of AuthorViewModel)

        For Each author As Author In authors
            Dim authorVM As New AuthorViewModel()
            authorVM.AuthorId = author.AuthorId
            authorVM.FirstName = author.FirstName
            authorVM.LastName = author.LastName

            If author.Books IsNot Nothing AndAlso author.Books.Any() Then
                authorVM.BookCount = author.Books.Count
                authorVM.ReleaseDateOfFirstBook = author.Books.Min(Function(b) b.ReleaseDate)
            Else
                authorVM.BookCount = 0
                authorVM.ReleaseDateOfFirstBook = Nothing ' Explicitly set to Nothing
            End If

            vm.Add(authorVM)
        Next

        Return vm
    End Function
End Class

Namespace Books.Models.ViewModels
    Public Class AuthorViewModel
        Public Property AuthorId As Integer
        Public Property FirstName As String
        Public Property LastName As String
        Public Property BookCount As Integer
        Public Property ReleaseDateOfFirstBook As Nullable(Of DateTime) ' Nullable DateTime
    End Class
End Namespace
