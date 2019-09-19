<%@ Page Title="" Language="C#" MasterPageFile="~/CDI.Master" AutoEventWireup="true" CodeBehind="Locations.aspx.cs" Inherits="ICRAWBS.Locations" %>
<%@ MasterType VirtualPath="~/CDI.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<div class="row">
				<div class="col-lg-4">
					<div class="form-group" id="Div1" runat="server" style="font-weight: bolder">
						<asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
							<asp:TextBox ID="txtSearch" runat="server" placeholder="Search by name..." CssClass="form-control"></asp:TextBox>
						</asp:Panel>
						<asp:Button ID="btnSearch" runat="server" CssClass="HideMe" OnClick="btnSearch_Click"></asp:Button>
					</div>
				</div>
			</div>
			<div class="btn-row">
				<div class="btn-group">
					<asp:LinkButton ID="lbAddNewLocation" runat="server" class="btn btn-primary" OnClick="lbAddNewLocation_Click">Add New Location</asp:LinkButton>
				</div>
			</div>
			<div class="row">
				<div class="col-lg-12">
					<section class="panel">
						<%--<header class="panel-heading">
                    ccc
                </header>--%>
						<div class="table-responsive">
							<asp:GridView ID="gvLocList" runat="server" CssClass="table table-striped table-bordered" GridLines="None" ShowHeaderWhenEmpty="False" EmptyDataText="No data found..." OnRowCreated="gvLocList_RowCreated">
								<Columns>
									<asp:TemplateField ItemStyle-Width="5%">
										<ItemTemplate>
											<asp:LinkButton ID="lbUpdateLocList" CssClass="btn ripple btn-3d btn-primary" runat="server" OnClick="lbUpdateLocList_Click">
										            <span class="fa fa-pencil-square-o"></span>
											</asp:LinkButton>
										</ItemTemplate>
									</asp:TemplateField>
								</Columns>
							</asp:GridView>
						</div>
					</section>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<div aria-hidden="true" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="AddNewLocationModal" class="modal fade">
		<div class="modal-dialog">
			<div class="modal-content">
				<div class="modal-header">
					<button aria-hidden="true" data-dismiss="modal" class="close" type="button">×</button>
					<h4 class="modal-title">Add New Location</h4>
				</div>
				<div class="modal-body">
					<asp:UpdatePanel ID="UpdatePanel2" runat="server">
						<ContentTemplate>
							<div role="form">
								<div class="row">
									<div class="col-lg-12">
										<div class="form-group">
											<asp:Label ID="lblLocationName" runat="server" Text="Location Name *"></asp:Label>
											<asp:TextBox ID="txtLocationName" runat="server" class="form-control"></asp:TextBox>
										</div>
										<div class="form-group">
											<asp:Label ID="lblDescription" runat="server" Text="Description *"></asp:Label>
											<asp:TextBox ID="txtDescription" runat="server" class="form-control"></asp:TextBox>
										</div>
										<div class="form-group">
											<asp:Label ID="lblType" runat="server" Text="Type *"></asp:Label>
											<asp:DropDownList ID="ddlType" runat="server" class="form-control">
												<asp:ListItem>Conference Room</asp:ListItem>
												<asp:ListItem>Workstation</asp:ListItem>
												<asp:ListItem>Parking</asp:ListItem>
											</asp:DropDownList>
										</div>
										<div class="form-group">
											<asp:Label ID="lblStatus" runat="server" Text="Status *"></asp:Label>
											<asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
												<asp:ListItem>Available</asp:ListItem>
												<asp:ListItem>Under Maintenance</asp:ListItem>
											</asp:DropDownList>
										</div>
									</div>
								</div>
								<div class="modal-footer">
									<asp:LinkButton ID="lblSaveEmp" runat="server" CssClass="btn btn-primary" OnClick="lblSaveEmp_Click">Save</asp:LinkButton>
									<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
								</div>
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
			</div>
		</div>
	</div>
	<div aria-hidden="true" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="ConfirmModal" class="modal fade">
		<div class="modal-dialog">
			<div class="modal-content">
				<div class="modal-header">
					<button aria-hidden="true" data-dismiss="modal" class="close" type="button">×</button>
					<h4 class="modal-title">System Message</h4>
				</div>
				<div class="modal-body">
					<div role="form">
						<div class="form-group">
							<asp:Label ID="lblMessage" runat="server" Text="Are you sure you want to save this record?"></asp:Label>
						</div>
						<div class="modal-footer">
							<asp:UpdatePanel ID="UpdatePanel3" runat="server">
								<ContentTemplate>
									<asp:LinkButton ID="lblYes" runat="server" CssClass="btn btn-primary" OnClick="lblYes_Click">Yes</asp:LinkButton>
									<button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<div aria-hidden="true" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="WarningModal" class="modal fade">
		<div class="modal-dialog">
			<div class="modal-content">
				<div class="modal-header">
					<button aria-hidden="true" data-dismiss="modal" class="close" type="button">×</button>
					<h4 class="modal-title">System Message</h4>
				</div>
				<div class="modal-body">
					<div role="form">
						<div class="form-group">
							<asp:UpdatePanel ID="UpdatePanel5" runat="server">
								<ContentTemplate>
									<asp:Label ID="lblWarningMessage" runat="server" Text="Please complete required fields."></asp:Label>
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
						<div class="modal-footer">
							<asp:UpdatePanel ID="UpdatePanel4" runat="server">
								<ContentTemplate>
									<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
								</ContentTemplate>
							</asp:UpdatePanel>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>