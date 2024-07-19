<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="Books.webforms._Default" %>
 
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Biblioteka</h1>
        <p class="lead">Prosta aplikacja WWW zbierająca dane o książkach i ich autorach</p>
    </div>

    <div class="row">
        <div class="col-md-8 col-md-offset-2">
            <h2>Książki</h2>
			 
			<asp:Button runat="server" ID="btnAddNew" CssClass="btn btn-default" OnCommand="AddNewBook" Text="Dodaj nową książkę"/>

			<asp:Repeater runat="server" Id="AuthorsRepeater">
			<HeaderTemplate>
				    <section>
        <div class="table-responsive text-nowrap">
            <!--Table-->
            <table class="table table-striped">
				<tr>
					<th style="width:100px">Autor</th>
					<th style="width:100px">Tytuł</th>
					<th style="width:100px">Data wydania</th>
					<th style="width:30px"></th>
					<th style="width:30px"></th>
				</tr>
                
				<tr>
					 
					<td style="width:100px"><asp:TextBox runat="server" Id="FindBox_Author"></asp:TextBox></td>
					<td style="width:100px"><asp:TextBox runat="server" Id="FindBox_Title"></asp:TextBox></td>
					<td style="width:100px"></td>
					<td style="width:100px"></td>
				</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<!--Table body-->
				<tr>
					<td><%# Eval("FirstName") + " " + Eval("LastName") %></td>
					<td><%# Eval("Title") %></td>
					<td><%# Eval("ReleaseDate", "{0:yyyy}") %></td>
					<td><asp:LinkButton ID="lkbEdit" runat="server" OnCommand="EditBook" CommandArgument='<%# Eval("BookId") %>'> E </asp:LinkButton></td>
					<td><asp:LinkButton ID="lkbDelete" runat="server" OnCommand="DeleteBook" CommandArgument='<%# Eval("BookId") %>' OnClientClick="return confirm('Are You sure to delete?')"> X </asp:LinkButton></td>
				</tr>
				<!--Table body-->
			</ItemTemplate>
			<FooterTemplate>
			            </table>
				</div>
				</section>
			</FooterTemplate>
			</asp:Repeater>
			<% if IsEditMode then %>
				<hr />
					<div class="form-group">
						<label for="Editbox_firstName">Imię</label>
						<input type="text" class="form-control" id="Editbox_firstName" name="Editbox_firstName" placeholder="Imię" value="<%=EditedBook.FirstName%>">
					</div>
					<div class="form-group">
						<label for="Editbox_lastName">Nazwisko</label>
						<input type="text" class="form-control" id="Editbox_lastName" name="Editbox_lastName" placeholder="Nazwisko"value="<%=EditedBook.LastName%>">
					</div>
					<div class="form-group">
						<label for="Editbox_title">Tytuł</label>
						<input type="text" class="form-control" id="Editbox_title" name="Editbox_title" placeholder="Tytuł"value="<%=EditedBook.Title%>">
					</div>
					<div class="form-group">
						<label for="Editbox_date">Data</label>
						<input MinLength="4" type="text" class="form-control" id="Editbox_date" name="Editbox_date" placeholder="Data"value="<%=EditedBook.ReleaseDate.Year%>">         			
					</div>
					<input type="hidden" id="Editbox_id" name="Editbox_id" placeholder="Nazwisko"value="<%=EditedBook.BookId%>">
		 
					<asp:Button runat="server" ID="btnEditSave" CssClass="btn btn-default" OnCommand="SaveBook" Text="Zapisz" OnClientClick="return validateAuthors();"/>
			<%       End if%>
			 
        </div>
    </div>

</asp:Content>
