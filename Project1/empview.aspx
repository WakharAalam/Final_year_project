<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="empview.aspx.cs" Inherits="Project1.empview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vertical Side Menu</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            display: flex;
        }

        #sidebar {
            background-color: #333;
            color: #fff;
            width: 250px;
            height: 100vh;
            padding-top: 20px;
        }

        #sidebar a {
            display: block;
            padding: 10px 20px;
            color: #fff;
            text-decoration: none;
        }

        #sidebar a:hover {
            background-color: #555;
        }

        #sidebar a.active {
            background-color: #04AA6D; /* Add a green color to the "active/current" link */
            color: white;
        }

        #content {
            flex: 1;
            padding: 20px;
        }

        .container {

        }

        .outer-container {
            width:100%;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }


        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            display: block;
            font-weight: bold;
            margin-bottom: 5px;
        }
        .form-group input[type="text"],
        .form-group input[type="submit"] {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
            font-size: 14px;
        }
        .form-group input[type="submit"] {
            background-color: #007bff;
            color: #fff;
            cursor: pointer;
        }
        .form-group input[type="submit"]:hover {
            background-color: #0056b3;
        }
        /* Other styles or adjustments as needed */
    </style>
</head>
<body>
    <div id="sidebar">
       <h3><asp:Label ID="label2" runat="server"></asp:Label></h3>
        <a href="https://localhost:44308/empview.aspx" ID="view" class="active">All courses</a>
        <a href="https://localhost:44308/completed.aspx" ID="update">Completed courses</a>
        <a href="https://localhost:44308/pending.aspx" ID="pending">Pending courses</a>
        <a href="https://localhost:44308/notify.aspx" ID="notify">Update completed course</a>
        <a href="#" runat="server" id="linkServerClick" onserverclick="linkServerClick_ServerClick">Logout</a>
    </div>
    <div class="outer-container">
<div class="container">
<h2 style="text-align: center;">All Courses</h2>
<form id="form1" runat="server">
    <asp:GridView ID="GridView1" runat="server"></asp:GridView>

</form>

</div>
</div>
</body>
</html>
