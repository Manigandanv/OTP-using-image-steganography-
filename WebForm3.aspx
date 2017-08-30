<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm3.aspx.cs" Inherits="WebApplication4.WebForm3" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Login Form</title>
</head>
<body>
    <div>
<form id="form1" runat="server">
<div>
<table>
<tr>
<td>
Username:
</td>
<td>
<asp:TextBox ID="txtUserName" runat="server"/>
</td>
</tr>
<tr>
<td>
Password:
</td>
<td>
<asp:TextBox ID="txtPWD" runat="server" TextMode="Password"/>
</td>
</tr>
<tr>
<td>
</td>
<td>
<asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnsubmit" />
</td>
</tr>
</table>
</div>
</form></div>
</body>
</html>
