(function () {
    $(function () {
        var _Service = abp.services.app.goodsPack;
        var tableId = "#WarehouseInventoryTableList";
        $tabs(".querySearch").click(function () {
            var Search = $(this).parent().serializeObject();

            DG.setQueryParamsRequest($tabs(tableId), Search)
        })
        InitDatePicker($tabs('.datePicker'), 2, $tabs('.querySearch'));

        $tabs(tableId).datagrid
            ({
                queryParams: {
                    StartDate: $tabs(".StartDate").datebox('getValue'),
                    EndDate: $tabs(".EndDate").datebox('getValue')
                },
                url: "/FrontEndTest/GetIndex",
                //分页相关属性
                sortName: 'OrganizationName', sortOrder: 'asc',
                onClickCell: function (index, field) {
                    DG.onClickCell(index, field, this);
                    editRow = index;
                    $tabs(tableId).datagrid('beginEdit', index);
                    DG.selectClickCell(field);
                },
                onLoadSuccess: function (data) {
                    $tabs(tableId).datagrid("keyCtr");
                    tableShow(tableId);
                    DG.countColumns(tableId, "inventoryQuantity", "inventoryCost");
                },
                columns: [[
                    {
                        field: 'id', title: '操作', sortable: true, width: 150, align: 'center',
                        formatter: function (value, rowData, rowIndex) {
                            return renderGridButton([
                                addGridButton("详情", WarehouseInventoryDetail, typeof rowData.goodsName != "undefined", "")
                            ], 2);
                        }
                    },
                    {
                        field: 'organizationName', title: '机构名称', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'warehouseName', title: '仓库名称', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'goodsName', title: '分类名称', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'goodsCode', title: '分类编码', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'classification', title: '分类类别', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'goodsUnit', title: '分类单位', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'auxiliaryUnit', title: '辅助单位', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'goodsBarcode', title: '分类条码', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'inventoryQuantity', title: '库存数量', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'inventoryCost', title: '库存成本', sortable: true, width: 150, align: 'center'
                    }
                ]],
            });
        //跳转库存详情
        function WarehouseInventoryDetail(value, rowData, rowIndex) {
            //clickLeftNav("warehouseInventoryDetailDiv", "库存明细报表", "/WarehouseInventory/WarehouseinventoryDetailIndex", {
            //    GoodsInfoId: rowData.goodsInfoId,
            //    WarehouseId: rowData.warehouseId,
            //    OrganizationId: rowData.organizationId
            //}, true);
        }
    });
})();