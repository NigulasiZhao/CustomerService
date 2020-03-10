using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace AfarsoftResourcePlan.Authorization
{
    public class AfarsoftResourcePlanAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var basePermission = context.CreatePermission("Permission_base", L("nav_base"));
            SetPermissionsErp(context, basePermission);
            SetPermissionsRecovery(context, basePermission);
        }

        private void SetPermissionsErp(IPermissionDefinitionContext context, Permission basePermission)
        {
            //基础资料
            var baseErpPermission = basePermission.CreateChildPermission("Permission_ErpBasicInfo", L("nav_base_ErpBasicInfo"));
            var baseErpAreaInfoPermission = baseErpPermission.CreateChildPermission("Permission_Erp_AreaInfo", L("nav_base_Erp_AreaInfo"));
            var baseErpCustomerPermission = baseErpPermission.CreateChildPermission("Permission_Erp_Customer", L("nav_base_Erp_Customer"));
            var baseErpOrganizationPermission = baseErpPermission.CreateChildPermission("Permission_Erp_Organization", L("nav_base_Erp_Organization"));
            var baseErpUserInfoPermission = baseErpPermission.CreateChildPermission("Permission_Erp_UserInfo", L("nav_base_Erp_UserInfo"));
            var baseErpWarehousePermission = baseErpPermission.CreateChildPermission("Permission_Erp_Warehouse", L("nav_base_Erp_Warehouse"));
            var baseErpSupplierPermission = baseErpPermission.CreateChildPermission("Permission_Erp_Supplier", L("nav_base_Erp_Supplier"));
            var baseErpRolesPermission = baseErpPermission.CreateChildPermission("Permission_Erp_Roles", L("nav_base_Erp_Roles"));
            var baseErpEnterprisePermission = baseErpPermission.CreateChildPermission("Permission_Erp_Enterprise", L("nav_base_Erp_Enterprise"));

            var baseErpGoodsInfoPermission = basePermission.CreateChildPermission("Permission_ErpGoodsInfo", L("nav_base_ErpGoodsInfo"));
            var baseErpGoodsInfoDetailPermission = baseErpGoodsInfoPermission.CreateChildPermission("Permission_Erp_GoodsInfo", L("nav_base_Erp_GoodsInfo"));
            var baseErpGoodsPricePermission = baseErpGoodsInfoPermission.CreateChildPermission("Permission_Erp_GoodsPrice", L("nav_base_Erp_GoodsPrice"));

            //销售
            var SalePermission = context.CreatePermission("Permission_sale", L("nav_sale"));
            var SaleOrderPermission = SalePermission.CreateChildPermission("Permission_Sale_SaleOrder", L("nav_sale_saleOrder"));
            var NewSaleOrderPermission = SaleOrderPermission.CreateChildPermission("Permission_Sale_NewSaleOrder", L("nav_sale_saleOrder_newsaleOrder"));
            var SaleOrderListPermission = SaleOrderPermission.CreateChildPermission("Permission_Sale_SaleOrderList", L("nav_sale_saleOrder_saleOrderList"));

            var SaleOutOrderPermission = SalePermission.CreateChildPermission("Permission_Sale_SaleOutOrder", L("nav_sale_saleOutOrder"));
            var NewSaleOutOrderPermission = SaleOutOrderPermission.CreateChildPermission("Permission_SaleOut_NewSaleOutOrder", L("nav_sale_saleOutOrder_newsaleOutOrder"));
            var SaleOutOrderListPermission = SaleOutOrderPermission.CreateChildPermission("Permission_SaleOut_SaleOutOrderList", L("nav_sale_saleOutOrder_saleOutOrderList"));
            var SaleOutOrderTallyPermission = SaleOutOrderPermission.CreateChildPermission("Permission_SaleOut_SaleOutOrderTally", L("nav_sale_saleOutOrder_saleOutOrderTally"));

            //财务
            var FinancialPermission = context.CreatePermission("Permission_Financial", L("nav_financial"));
            var CollectionPermission = FinancialPermission.CreateChildPermission("Permission_Financial_Collection", L("nav_financial_collection"));
            var NewReceivedOrderPermission = CollectionPermission.CreateChildPermission("Permission_Financial_NewReceivedOrder", L("nav_financial_collection_newreceivedorder"));
            var ReceivedOrderListPermission = CollectionPermission.CreateChildPermission("Permission_Financial_ReceivedOrderList", L("nav_financial_collection_receivedorderList"));

            var ReceivablesPermission = FinancialPermission.CreateChildPermission("Permission_Financial_Receivables", L("nav_financial_receivables"));
            var ReceivablesListPermission = ReceivablesPermission.CreateChildPermission("Permission_Financial_ReceivablesList", L("nav_financial_receivables_receivablesList"));
            //报表
            var ReportPermission = context.CreatePermission("Permission_report", L("nav_financial"));
            var InventoryReportPermission = ReportPermission.CreateChildPermission("Permission_Report_Inventory", L("nav_report_inventory"));
            var WarehouseInventoryPermission = InventoryReportPermission.CreateChildPermission("Permission_Report_Warehouseinventory", L("nav_report_inventory_warehouseinventory"));
        }
        private void SetPermissionsRecovery(IPermissionDefinitionContext context, Permission basePermission)
        {
            var baseRecoveryPermission = basePermission.CreateChildPermission("Permission_base_recovery", L("nav_base_recovery"));
            var baseRecoveryCarPermission = baseRecoveryPermission.CreateChildPermission("Permission_recovery_car", L("nav_base_recovery_car"));
            var baseRecoveryServiceAreaPermission = baseRecoveryPermission.CreateChildPermission("Permission_recovery_serviceArea", L("nav_base_recovery_serviceArea"));

            var recoveryPermission = context.CreatePermission("Permission_recovery", L("nav_recovery"));
            var recoveryMemberPermission = recoveryPermission.CreateChildPermission("Permission_recovery_memberList", L("nav_recovery_memberList"));
            var recoveryRecoveryPermission = recoveryPermission.CreateChildPermission("Permission_recovery_recoveryLog", L("nav_recovery_recoveryLog"));
            var recoveryGoodsPackPermission = recoveryPermission.CreateChildPermission("Permission_recovery_goodpackLog", L("nav_recovery_goodspackLog"));
            var recoveryEntruckPermission = recoveryPermission.CreateChildPermission("Permission_recovery_entruckLog", L("nav_recovery_entruckLog"));
            var recoveryDetruckPermission = recoveryPermission.CreateChildPermission("Permission_recovery_detruckLog", L("nav_recovery_detruckLog"));
            var recoveryScorePermission = recoveryPermission.CreateChildPermission("Permission_recovery_scoreLog", L("nav_recovery_scoreLog"));
            var recoveryScoreConsumePermission = recoveryPermission.CreateChildPermission("Permission_recovery_scoreConsume", L("nav_recovery_scoreConsume"));

            var appPermission = context.CreatePermission("Permission_app", L("nav_app"));
            appPermission.CreateChildPermission("Permission_app_recycling", L("nav_app_recycling"));
            appPermission.CreateChildPermission("Permission_app_entruck", L("nav_app_entruck"));
            appPermission.CreateChildPermission("Permission_app_detruck", L("nav_app_detruck"));
            appPermission.CreateChildPermission("Permission_app_goodsshow", L("nav_app_goodsshow"));
            appPermission.CreateChildPermission("Permission_app_scoreoption", L("nav_app_scoreoption"));
            appPermission.CreateChildPermission("Permission_app_scoresearch", L("nav_app_scoresearch"));
            appPermission.CreateChildPermission("Permission_app_qrcode", L("nav_app_qrcode"));
            appPermission.CreateChildPermission("Permission_app_pack", L("nav_app_pack"));
            appPermission.CreateChildPermission("Permission_app_packsearch", L("nav_app_packsearch"));
            appPermission.CreateChildPermission("Permission_app_packmanagement", L("nav_app_packmanagement"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AfarsoftResourcePlanConsts.LocalizationSourceName);
        }
    }
}
