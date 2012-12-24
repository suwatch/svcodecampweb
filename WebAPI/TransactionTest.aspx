<%@ Page Language="C#" %>
<%@ Import Namespace="CodeCampSV"%>
<%@ Import Namespace="System.Transactions"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        using (var scope = new TransactionScope())
        {
            CodeCampSV.PicturesManager.I.Insert(new PicturesResult() {FileName = "abcd"});
            
            Label1.Text = "Transaction Success";
            scope.Complete();
        }

    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
