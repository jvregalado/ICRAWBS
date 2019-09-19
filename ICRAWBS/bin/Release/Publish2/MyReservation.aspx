<%@ Page Title="" Language="C#" MasterPageFile="~/CDI.Master" AutoEventWireup="true" CodeBehind="MyReservation.aspx.cs" Inherits="ICRAWBS.MyReservation" %>
<%@ MasterType VirtualPath="~/CDI.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
				<div class="col-lg-12">
                    <section class="panel">
						<header class="panel-heading">
							Filter
						</header>
						<div class="panel-body">
							<div class="form-inline" role="form">
								<div class="form-group">
									<label class="sr-only" for="txtFrom">From</label>
									<asp:TextBox ID="txtFrom" runat="server" TextMode="Date" class="form-control" OnTextChanged="lbFilter_Click"></asp:TextBox>
								</div>
                                <div class="form-group">
									<asp:DropDownList ID="LocationSelection" runat="server" class="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="LocationSelection_SelectedIndexChanged"></asp:DropDownList>
								</div>
                                <div class="form-group">
									<asp:DropDownList ID="EmailAddSelection" runat="server" class="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="EmailAddSelection_SelectedIndexChanged"></asp:DropDownList>
								</div>
								<asp:LinkButton ID="lbFilter" runat="server" class="btn btn-primary" OnClick="lbFilter_Click">Filter</asp:LinkButton>
							</div>
						</div>
                        </section>
                      
	
					
                    <div class="btn-row">
				<div class="btn-group">
					<asp:LinkButton ID="lbBook" runat="server" class="btn btn-primary btn-lg" OnClick="lbBook_Click">Create a Reservation</asp:LinkButton>
				</div>
			</div>
			<div class="btn-row">
					<section class="panel">
						<div class="table-responsive">
							<asp:GridView ID="gvReservation" runat="server" CssClass="table table-striped table-bordered" GridLines="None" ShowHeaderWhenEmpty="False" EmptyDataText="No data found..." OnRowCreated="gvReservation_RowCreated">
								<Columns>
									<asp:TemplateField ItemStyle-Width="0%" >
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbOccupyUpdate" CssClass="btn btn-primary" runat="server" OnClick="lbOccupyCancelUpdate_Click">Occupy</asp:LinkButton>
                                            
                                                </ItemTemplate>
									</asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="0%">
                                        <ItemTemplate>
									<asp:LinkButton ID="lbCancelUpdate" CssClass="btn btn-info" runat="server" OnClick="lbOccupyCancelUpdate_Click">Cancel</asp:LinkButton>
                                                </ItemTemplate>
									</asp:TemplateField>
								</Columns>
							</asp:GridView>
						</div>
					</section>
				</div>
			</div>
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
											<asp:DropDownList ID="ddlLocation" runat="server" class="form-control"></asp:DropDownList>
										</div>
                                        <div class="form-group">
											<asp:Label ID="lblBookDate" runat="server" Text="Date *"></asp:Label>
											<asp:TextBox ID="txtBookDate" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
										</div>
										<div class="form-group">
											<asp:Label ID="lblBookDateTimeFrom" runat="server" Text="From *"></asp:Label>
											<asp:TextBox ID="txtBookDateTimeFrom" runat="server" class="form-control" TextMode="Time" step="1800"></asp:TextBox>
										</div>
										<div class="form-group">
											<asp:Label ID="lblBookDateTo" runat="server" Text="To *"></asp:Label>
											<asp:TextBox ID="txtBookDateTimeTo" runat="server" class="form-control" TextMode="Time" step="900"></asp:TextBox>
										</div>
										<div class="form-group">
											<asp:Label ID="lblBookReason" runat="server" Text="Reason *"></asp:Label>
											<asp:TextBox ID="txtBookReason" runat="server" class="form-control"></asp:TextBox>
										</div>
										<div class="form-group">
											<asp:Label ID="Label1" runat="server" Text="Remarks *"></asp:Label>
											<asp:TextBox ID="txtBookRemarks" runat="server" class="form-control"></asp:TextBox>
										</div>
									</div>
								</div>
								<div class="modal-footer">
									<asp:LinkButton ID="lblSaveEmp" runat="server" CssClass="btn btn-primary" OnClick="lblSaveEmp_Click">Reserve</asp:LinkButton>
                                    <%-- <asp:LinkButton ID="lblOccupiedConference" runat="server" CssClass="btn btn-primary" OnClick="lbOccupiedConference_Click">Occupy</asp:LinkButton>
									<asp:LinkButton ID="lbCancelConference" runat="server" CssClass="btn btn-danger" OnClick="lbCancelConference_Click">Cancel</asp:LinkButton>--%>
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