<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ICRAWBS.login" %>

<!DOCTYPE html>
<html lang="en">

<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0">
	<meta name="description" content="Chester G. Palis">
	<meta name="author" content="Chester G. Palis">
	<meta name="keyword" content="LOGISTIKUS INC., Chester G. Palis, Chester Palis">
	<link rel="shortcut icon" href="img/icons/cdi.ico">

	<title>LOGISTIKUS INC.</title>

	<!-- Bootstrap CSS -->
	<link href="css/bootstrap.min.css" rel="stylesheet">
	<!-- bootstrap theme -->
	<link href="css/bootstrap-theme.css" rel="stylesheet">
	<!--external css-->
	<!-- font icon -->
	<link href="css/elegant-icons-style.css" rel="stylesheet" />
	<link href="css/font-awesome.css" rel="stylesheet" />
	<!-- Custom styles -->
	<link href="css/style.css" rel="stylesheet">
	<link href="css/style-responsive.css" rel="stylesheet" />
	<script src="js/cModal.js"></script>
</head>

<body class="login-img3-body">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
		<asp:UpdatePanel ID="UpdatePanel1" runat="server">
			<ContentTemplate>
				<div class="container">
					<div class="login-form" action="#">
						<div class="login-wrap">
							<p class="login-img"><i class="icon_lock_alt"></i></p>
							<asp:Panel ID="Panel1" runat="server" DefaultButton="btnLogin">
								<div class="input-group">
									<span class="input-group-addon"><i class="icon_profile"></i></span>
									<asp:TextBox ID="txtUsername" runat="server" class="form-control" placeholder="Username" autofocus></asp:TextBox>
								</div>
								<div class="input-group">
									<span class="input-group-addon"><i class="icon_key_alt"></i></span>
									<asp:TextBox ID="txtPassword" runat="server" class="form-control" placeholder="Password" TextMode="Password"></asp:TextBox>
								</div>
							</asp:Panel>
							<asp:Button ID="btnLogin" runat="server" class="btn btn-primary btn-lg btn-block" Text="Login" OnClick="btnLogin_Click" />
						</div>
					</div>
					<div class="text-right">
						<div class="credits">
							Logistikus Inc.
						</div>
					</div>
				</div>


			</ContentTemplate>
		</asp:UpdatePanel>
		<div aria-hidden="true" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="loginModal" class="modal fade">
			<div class="modal-dialog">
				<div class="modal-content">
					<div class="modal-header">
						<button aria-hidden="true" data-dismiss="modal" class="close" type="button">×</button>
						<h4 class="modal-title">System Message</h4>
					</div>
					<div class="modal-body">
						<div role="form">
							<div class="form-group">
								<asp:Label ID="lblLoginModal" runat="server" Text="Invalid login credentials."></asp:Label>
							</div>
							<div class="modal-footer">
								<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<script src="js/jquery.js"></script>
		<script src="js/bootstrap.min.js"></script>

		<script src="js/jquery.scrollTo.min.js"></script>
		<script src="js/jquery.nicescroll.js" type="text/javascript"></script>

		<script src="js/scripts.js"></script>
	</form>
</body>
</html>
