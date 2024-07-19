<%@ Page Title="Autorzy" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Authors.aspx.vb" Inherits="Books.webforms.Authors" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Autorzy</h2>

    <asp:Button runat="server" ID="btnAddNew" CssClass="btn btn-default" OnCommand="AddNewAuthor" Text="Dodaj nowego autora" />

    <asp:Repeater runat="server" ID="AuthorsRepeater">
        <HeaderTemplate>
            <section>
                <div class="table-responsive text-nowrap">
                    <!--Table-->
                    <table class="table table-striped">
                        <!-- Style this table using bootstrap CSS classes http://getbootstrap.com/docs/3.3/components/  -->
                        <!--Table head-->

                        <tr>
                            <th style="width:100px">Nazwisko</th>
                            <th style="width:100px">Imię</th>
                            <th style="width:100px">Liczba książek</th>
                            <th style="width:150px">Data wydania pierwszej książki</th>
                            <th style="width:30px"></th>
                            <th style="width:30px"></th>
                        </tr>

                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="FindBox_LastName" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="FindBox_FirstName" ></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <!--Table head-->
        </HeaderTemplate>

        <ItemTemplate>
            <!--Table body-->
            <tr>
                <td><%# Eval("LastName") %></td>
                <td><%# Eval("FirstName") %></td>
                <td><%# Eval("BookCount") %></td>
                <td><%# Eval("ReleaseDateOfFirstBook", "{0:yyyy-MM-dd}") %></td>
                <td>
                    <asp:LinkButton ID="lkbEdit" runat="server" OnCommand="EditAuthor" CommandArgument='<%# Eval("AuthorId") %>'> E </asp:LinkButton>
                </td>
                <td>
                    <asp:LinkButton ID="lkbDelete" runat="server" OnCommand="DeleteAuthor" CommandArgument='<%# Eval("AuthorId") %>' OnClientClick="return confirm('Are You sure to delete?')"> X </asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>

        <FooterTemplate>
            <!--Table body-->
            </table>
            <!--Table-->
            </div>
</section>

        </FooterTemplate>
    </asp:Repeater>

    <% If IsEditMode Then %>
    <hr />
    <div class="form-group">
        <label for="Editbox_firstName">Imię</label>
        <input type="text" class="form-control" id="Editbox_firstName" name="Editbox_firstName" placeholder="Imię" value="<%=EditedAuthor.FirstName%>">
    </div>
    <div class="form-group">
        <label for="Editbox_lastName">Nazwisko</label>
        <input type="text" class="form-control" id="Editbox_lastName" name="Editbox_lastName" placeholder="Nazwisko" value="<%=EditedAuthor.LastName%>">
    </div>
    
    <div class="form-group">
        <label for="Editbox_releaseDate">Data wydania pierwszej książki</label>
  
        <input type="date" class="form-control" id="Editbox_releaseDate" name="Editbox_releaseDate" placeholder="Data wydania pierwszej książki" value="<%=ReleaseDateOfFirstBookValue%>">

    </div>
    <input type="hidden" id="Editbox_id" name="Editbox_id" value="<%=EditedAuthor.AuthorId%>">
    <asp:Button runat="server" ID="btnEditSave" CssClass="btn btn-default" OnCommand="SaveAuthor" Text="Zapisz" OnClientClick="return validateAuthors();" />
    <% End If %>
</asp:Content>
