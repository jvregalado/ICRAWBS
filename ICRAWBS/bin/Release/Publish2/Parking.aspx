<%@ Page Title="" Language="C#" MasterPageFile="~/CDI.Master" AutoEventWireup="true" CodeBehind="Parking.aspx.cs" Inherits="ICRAWBS.Parking" %>
<%@ MasterType VirtualPath="~/CDI.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<div class="row">
				<div class="col-lg-12">
					<section class="panel">
						<header class="panel-heading">
							Date Filter
						</header>
						<div class="panel-body">
							<div class="form-inline" role="form">
								<div class="form-group">
									<label class="sr-only" for="txtFrom">From</label>
									<asp:TextBox ID="txtFrom" runat="server" TextMode="Date" class="form-control"></asp:TextBox>
								</div>
								<div class="form-group">
									<label class="sr-only" for="txtTo">To</label>
									<asp:TextBox ID="txtTo" runat="server" TextMode="Date" class="form-control"></asp:TextBox>
								</div>
								<asp:LinkButton ID="lbFilter" runat="server" class="btn btn-primary" OnClick="lbFilter_Click">Filter</asp:LinkButton>
							</div>
						</div>
					</section>
				</div>
			</div>
			<div class="btn-row">
				<div class="btn-group">
					<asp:LinkButton ID="lbBook" runat="server" class="btn btn-primary btn-lg" OnClick="lbBook_Click">Create a Reservation</asp:LinkButton>
				</div>
			</div>
			<div class="btn-row">
				<div class="btn-group">
					<asp:LinkButton ID="lbReserved" runat="server" CssClass="btn btn-info btn-lg" OnClick="lbReserved_Click">Reserved</asp:LinkButton>
					<asp:LinkButton ID="lbBooked" runat="server" CssClass="btn btn-info btn-lg" OnClick="lbBooked_Click">Booked</asp:LinkButton>
					<asp:LinkButton ID="lbCancelled" runat="server" CssClass="btn btn-info btn-lg" OnClick="lbCancelled_Click">Cancelled</asp:LinkButton>
				</div>
			</div>
			<div class="row">
				<div class="col-lg-12">
					<section class="panel">
						<div class="table-responsive">
							<asp:GridView ID="gvWorkstation" runat="server" CssClass="table table-striped table-bordered" GridLines="None" ShowHeaderWhenEmpty="False" EmptyDataText="No data found..." OnRowCreated="gvWorkstation_RowCreated">
								<Columns>
									<asp:TemplateField ItemStyle-Width="5%">
										<ItemTemplate>
											<asp:LinkButton ID="lbUpdateWorkstation" CssClass="btn ripple btn-3d btn-primary" runat="server" OnClick="lbUpdateWorkstation_Click">
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
	<div aria-hidden="true" aria-labelledby="myModalLabel" role="dialog" tabindex="-1" id="AddNewBookingModal" class="modal fade">
		<div class="modal-dialog">
			<div class="modal-content">
				<div class="modal-header">
					<button aria-hidden="true" data-dismiss="modal" class="close" type="button">×</button>
					<h4 class="modal-title">Reservation</h4>
				</div>
				<div class="modal-body">
					<asp:UpdatePanel ID="UpdatePanel2" runat="server">
						<ContentTemplate>
							<div role="form">
								<div class="row">
									<div class="col-lg-12">
										<div class="form-group">
											<asp:Label ID="lblLocationName" runat="server" Text="Location Name *"></asp:Label>
											<asp:DropDownList ID="ddlLocation" runat="server" class="form-control"></asp:DropDownList>
										</div>
										<div class="form-group">
											<asp:Label ID="lblBookDateTimeFrom" runat="server" Text="From *"></asp:Label>
											<asp:TextBox ID="txtBookDateTimeFrom" runat="server" class="form-control" TextMode="DateTimeLocal"></asp:TextBox>
										</div>
										<div class="form-group">
											<asp:Label ID="lblBookDateTo" runat="server" Text="To *"></asp:Label>
											<asp:TextBox ID="txtBookDateTimeTo" runat="server" class="form-control" TextMode="DateTimeLocal"></asp:TextBox>
										</div>
										<div class="form-group">
											<asp:Label ID="lblBookReason" runat="server" Text="Reason *"></asp:Label>
											<asp:TextBox ID="txtBookReason" runat="server" class="form-control"></asp:TextBox>
										</div>
										<div class="form-group">
											<asp:Label ID="Label1" runat="server" Text="Remarks *"></asp:Label>
											<asp:TextBox ID="txtBookRemarks" runat="server" class="form-control"></asp:TextBox>
										</div>
                                        <!--<div class="form-group">
											<asp:Label ID="Label2" runat="server" Text="Confirmation Image *"></asp:Label>
											<asp:FileUpload ID="ConfirmationImage" runat="server" accept=".png,.jpg,.jpeg,.gif" />
										</div>
                                        <asp:Image ID="Image1" runat="server" />-->
                                        
									</div>
								</div>
								<div class="modal-footer">
									<asp:LinkButton ID="lblSaveEmp" runat="server" CssClass="btn btn-primary" OnClick="lblSaveEmp_Click">Reserve</asp:LinkButton>
									<asp:LinkButton ID="lbBookWorkstation" runat="server" CssClass="btn btn-info" OnClick="lbBookWorkstation_Click">Book</asp:LinkButton>
									<asp:LinkButton ID="lbCancelWorkstation" runat="server" CssClass="btn btn-danger" OnClick="lbCancelWorkstation_Click">Cancel</asp:LinkButton>
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
