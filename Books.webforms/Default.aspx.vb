Imports Books.Models
Imports Books.Models.Dao
Imports Books.Models.ViewModels

Public Class _Default
    'I changed Page to AppPage
    Inherits AppPage

    Public IsEditMode As Boolean = False
    Public EditedBook As Book = New Book()
    Public IsDeliteMode As Boolean = False
    Public IsSaveMode As Boolean = False


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    'I modified
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Dim author_findValue As String = ""
        Dim title_findValue As String = ""
        Dim booksDao As BooksDao = New BooksDao(Me.DbConnectionHolder)
        Dim booksVMs As List(Of BookViewModel)
        Dim books As List(Of Book)

        If Me.IsPostBack And IsDeliteMode = False And IsSaveMode = False Then

            Dim firstName As String = ""
            Dim lastName As String = ""
            Dim rlastNamerSec As String = ""

            author_findValue = Request.Form("ctl00$MainContent$AuthorsRepeater$ctl00$FindBox_Author")
            title_findValue = Request.Form("ctl00$MainContent$AuthorsRepeater$ctl00$FindBox_Title")


            If (author_findValue <> "" Or title_findValue <> "") Then



                Dim allValues As String() = author_findValue.Split(" ")


                firstName = allValues(0)
                If (allValues.Length > 1) Then
                    lastName = allValues(1)
                Else
                    rlastNamerSec = firstName
                End If
                books = booksDao.GetBooks(title_findValue, lastName, firstName, rlastNamerSec)
                IsEditMode = False
            Else
                Return
            End If


        Else
            ' if http method is GET
            books = booksDao.GetBooks()
        End If



        booksVMs = GetViewModel(books)

        AuthorsRepeater.DataSource = books
        AuthorsRepeater.DataBind()
    End Sub

    Private Function GetViewModel(ByVal books As List(Of Book)) As List(Of BookViewModel)
        Dim vm As New List(Of BookViewModel)


        Return vm
    End Function

    Protected Sub AddNewBook(sender As Object, e As CommandEventArgs)
        IsEditMode = True
    End Sub
    Protected Sub EditBook(sender As Object, e As CommandEventArgs)
        IsEditMode = True
        Dim editedBookId As Integer = Integer.Parse(e.CommandArgument)

        Dim booksDao As New BooksDao(Me.DbConnectionHolder)
        EditedBook = booksDao.GetBook(editedBookId)
    End Sub

    Protected Sub DeleteBook(sender As Object, e As CommandEventArgs)
        Dim deletedBookId As Integer = Integer.Parse(e.CommandArgument)

        Dim booksDao As New BooksDao(Me.DbConnectionHolder)
        EditedBook = booksDao.DeleteBook(deletedBookId)

        IsDeliteMode = True
    End Sub

    Protected Sub SaveBook(sender As Object, e As CommandEventArgs)
        Dim title As String = Request.Form("Editbox_title")
        Dim lastName As String = Request.Form("Editbox_lastName")
        Dim firstName As String = Request.Form("Editbox_firstName")
        Dim releaseDate As String = Request.Form("Editbox_date")
        Dim editedBookId As Integer = Integer.Parse(Request.Form("Editbox_id"))

        IsSaveMode = True

        Dim authorsDao As New AuthorsDAO(Me.DbConnectionHolder)
        Dim booksDao As New BooksDao(Me.DbConnectionHolder)

        If editedBookId = 0 Then
            ' INSERT new 
            authorsDao.InsertAuthor(firstName, lastName)
            booksDao.InsertBook(firstName, lastName, title, releaseDate)

        Else
            ' UPDATE
            Dim newBook As New Book
            authorsDao.UpdateAuthorByBookId(firstName, lastName, editedBookId)
            booksDao.UpdateBook(firstName, lastName, title, releaseDate, editedBookId)
        End If

    End Sub
End Class