<%@ Page Title="" Language="C#" MasterPageFile="~/CDI.Master" AutoEventWireup="true" CodeBehind="Reservations.aspx.cs" Inherits="ICRAWBS.Reservations" %>
<%@ MasterType VirtualPath="~/CDI.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<div class="row">
				<div class="col-lg-4">
					<asp:LinkButton ID="lbConferenceRoom" runat="server" OnClick="lbConferenceRoom_Click">
						<div class="info-box blue-bg">
							<i class="fa fa-users"></i>
							<div class="title">Conference Room</div>
						</div>
					</asp:LinkButton>
				</div>
				<div class="col-lg-4">
					<asp:LinkButton ID="lbWorkStations" runat="server" OnClick="lbWorkStations_Click">
						<div class="info-box blue-bg">
							<i class="fa fa-desktop"></i>
							<div class="title">Workstations</div>
						</div>
					</asp:LinkButton>
				</div>
				<div class="col-lg-4">
					<asp:LinkButton ID="lbParking" runat="server" OnClick="lbParking_Click">
						<div class="info-box blue-bg">
							<i class="fa fa-id-card"></i>
							<div class="title">Parking</div>
						</div>
					</asp:LinkButton>
				</div>
			</div>
			<div class="row">
				<div class="col-lg-12">
					<section class="panel">
						<header class="panel-heading">
							<asp:Label ID="lblGVHeader" runat="server" Text="Chester G. Palis"></asp:Label>
						</header>
						<div class="table-responsive">
							<asp:GridView ID="gvReservationList" runat="server" CssClass="table table-striped table-bordered" GridLines="None" ShowHeaderWhenEmpty="False" EmptyDataText="No data found..." OnRowCreated="gvReservationList_RowCreated">
								<Columns>
									<asp:TemplateField ItemStyle-Width="5%">
										<ItemTemplate>
											<asp:LinkButton ID="lbUpdateGV" CssClass="btn ripple btn-3d btn-primary" runat="server" OnClick="lbUpdateGV_Click">
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
</asp:Content>
