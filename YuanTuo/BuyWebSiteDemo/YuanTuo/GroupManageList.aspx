﻿<%@ Page Language="C#" MasterPageFile="~/Layout/BackManagerListLayout.master" AutoEventWireup="true" CodeFile="GroupManageList.aspx.cs" Inherits="YuanTuo_GroupManageList" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
 <fieldset>
    <legend>查询条件</legend>
     
    请输入分组名：<asp:TextBox ID="txtGroupName" runat="server"></asp:TextBox>&nbsp;&nbsp;
    <asp:Button ID="btnSearch" runat="server" Text="查询" onclick="btnSearch_Click1" />&nbsp;
<asp:Button ID="btnAdd" runat="server" onclick="btnAdd_Click1" Text="添加" />
  </fieldset>
  <br />
</div>
<div> <wc:procedurepager ID="ProcedurePager1" runat="server" AllowBinded="True" 
        BindControlString="dgLists" FieldString="*" PageSize="10" 
        SortString="id asc" TableName="yuantuo_terminal_group">
    </wc:procedurepager></div>
<div>
 <asp:DataGrid ID="dgLists" runat="server" AutoGenerateColumns="false" 
            BorderWidth="0px" CellPadding="1" CellSpacing="1" CssClass="table-border" 
            Width="100%" onitemcommand="dgLists_ItemCommand" 
           >
         <Columns>
            
            <asp:BoundColumn DataField="name" HeaderText="分组名"  ItemStyle-Width="120"></asp:BoundColumn>
             <asp:BoundColumn DataField="adurl" HeaderText="分组流媒体地址"  ItemStyle-Width="500"></asp:BoundColumn>
            <asp:BoundColumn DataField="adcontent" HeaderText="促销内容" ></asp:BoundColumn>
             <asp:BoundColumn DataField="des" HeaderText="备注" ></asp:BoundColumn>
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center"  HeaderStyle-HorizontalAlign="Center" HeaderText="详细">
                            <ItemTemplate>
                            
                            <asp:ImageButton   runat="server" AlternateText="详细" CommandArgument='<%#Eval("id") %>' ToolTip="详细" ID="btnDetail" CommandName="Detail" ImageUrl="~/images/modify.gif" />
                            </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="删除">
                            <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="删除" CommandArgument='<%#Eval("id") %>' ToolTip="删除" OnClientClick="javascript:return confirm('确定删除吗？');" ID="btnDelete" CommandName="Delete" ImageUrl="~/images/delete.gif" />
                            </ItemTemplate>
                            </asp:TemplateColumn>
         </Columns>
         
         <HeaderStyle CssClass="table-title-grid" />
          <ItemStyle CssClass="table-content-grid" />
          <AlternatingItemStyle  CssClass="table-content-grid-alter"/>
         
       </asp:DataGrid>
    
    </div>
   

</div>
</asp:Content>

