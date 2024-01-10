<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin_login.aspx.cs" Inherits="Project1.Admin_login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
       <title>Login Page</title>
    <style>
            body {
                  font-family: Arial, sans-serif;
                  display: flex;
                  justify-content: center;
                  align-items: center;
                  height: 100vh;
                  margin: 0;
                  background-image: url('./bglogo.jpg');
                  background-position: center;
                  background-size: cover;

        }

        .login-box {
            width: 350px;
            padding: 40px 20px;
            background: rgba(0, 0, 0, 0.8);
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding-bottom: 100px;
            position: relative;
        }

                 .login-box h2 {
                      color: white;
                      text-align: center;
                      margin-bottom: 20px;

            }

                 .login-box input[type="email"],
                 .login-box input[type="password"] {
                      width: calc(100% - 20px);
                      padding: 15px 10px;
                      margin-bottom: 15px;
                      border: 1px solid #ccc;
                      border-radius: 10px;

            }

                 .login-box input[type="submit"] {
                      width: 100%;
                      padding: 10px;
                      border: none;
                      border-radius: 10px;
                      background: #007bff;
                      color: #fff;
                      cursor: pointer;

            }

                     .login-box input[type="submit"]:hover {
                          background: #0056b3;

                }

                 .login-box a.admin {
                      position: absolute;
                      bottom: 10px;
                      left: 10px;

            }

                 .login-box a.feedback {
                      position: absolute;
                      bottom: 10px;
                      right: 10px;

            }


    </style>
</head>
<body>
      <div class="login-box">
          <h2>Admin Login</h2>
    <form id="form1" runat="server">
         <asp:TextBox ID="TextBox1" runat="server" Type="email" Placeholder="Email" required="ture"></asp:TextBox>
         <asp:TextBox ID="TextBox2" runat="server" Type="password" Placeholder="Password" required="true"></asp:TextBox>
        <asp:Button href="#"  ID="Button1" runat ="server" Text="Login"  OnClick="Button1_Click" />
    </form>
          </div>
    <script>
        function validateEmail() {
            var emailInput = document.getElementById('TextBox1').value;
            var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Regular expression to validate email

            if (!emailRegex.test(emailInput)) {
                alert('Please enter a valid email address');
                return false;
            }

            return true;
        }

        function validateForm() {
            return validateEmail();
        }
    </script>
</body>
</html>
