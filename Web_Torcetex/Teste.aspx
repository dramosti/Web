<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Teste.aspx.cs" Inherits="Teste" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="GridView1" runat="server">
            <Columns>
                <asp:TemplateField HeaderText="TESTE">
                    <ItemTemplate>
                        <asp:TextBox ID="txt1" runat="server" />
                    </ItemTemplate>                    
                </asp:TemplateField>
                
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
