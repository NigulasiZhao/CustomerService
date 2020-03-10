(function () {
    $(function () {
        var _goodsInfoService = abp.services.app.goodsInfo;
        var _goodsInfoidForCreateOrUpdate = "";
        var clickLoading = false;

        $tabs('#TreeTableList').treegrid
            ({
                idField: 'id',
                treeField: 'goodsName',
                animate: true,
                collapsible: true,
                fitColumns: true,
                width: '100%',
                fit: true,
                showFooter: false,
                //是否显示斑马线效果;同一行中显示数据;显示一个行号列;只允许选择一行
                striped: false, nowrap: true, rownumbers: true, singleSelect: true,
                queryParams: {
                    //fatherId: "00000000-0000-0000-0000-000000000000"
                },
                url: "/api/services/app/Catalog/GetAllCatalogNoPage",
                //分页相关属性
                pagination: true, pageSize: 20, pageList: [20, 50, 100, 150, 200], pageNumber: 1, sortName: 'GoodsName', sortOrder: 'asc',
                onClickCell: function (field, row) {
                    DG.selectClickCell(field);
                    if (row.children == undefined) {
                        if (field == "goodsName" && !clickLoading) {
                            clickLoading = true;
                            _goodsInfoService.getAllGoodsInfoList({
                                page: 1,
                                rows: 20,
                                sort: 'GoodsName',
                                order: "asc",
                                fatherId: row.id
                            }).done(function (data) {
                                $tabs('#TreeTableList').treegrid('append', {
                                    parent: row.id,
                                    data: data.rows
                                });
                            }).always(function () {
                                clickLoading = false;
                            }).fail(function (data) {
                            });
                        }
                    }
                    else {
                        if (row.state == "open") {
                            $tabs('#TreeTableList').treegrid('collapseAll', row.id);
                        } else {
                            $tabs('#TreeTableList').treegrid('expandAll', row.id);
                        }
                    }
                },
                rowStyler: function (index, row) { },
                onLoadSuccess: function (data) {
                    //$(".tree-icon,.tree-file").removeClass("tree-file");
                    //$(".tree-icon,.tree-folder").removeClass("tree-folder-open tree-folder");
                    $tabs("#TreeTableList").treegrid("keyCtr");
                    tableShow("#TreeTableList");
                },
                onBeginEdit: function (index, row) { },
                onAfterEdit: function (index, row, changes) { },
                columns: [[
                    {
                        field: 'Id', title: '操作', sortable: true, width: 150, align: 'center',
                        formatter: function (value, rowData, rowIndex) {
                            return renderGridButton([
                                addGridButton("编辑", EditGoodsInfo, true, ""),
                                addGridButton("详情", DetailGoodsInfo, true, "")
                            ], 2);
                        }
                    },
                    {
                        field: 'id', title: 'id', sortable: true, width: 150, hidden: true,
                    },
                    {
                        field: 'goodsName', title: '分类名称', sortable: true, width: 150, align: 'left'
                    },
                    {
                        field: 'goodsCode', title: '分类编码', sortable: true, width: 150, align: 'left'
                    },
                    {
                        field: 'classification', title: '分类类型', sortable: true, width: 150, align: 'left'
                    },
                    {
                        field: 'goodsArticleNumber', title: '分类货号', sortable: true, width: 150, align: 'left'
                    },
                    {
                        field: 'goodsSpecification', title: '分类规格', sortable: true, width: 150, align: 'left'
                    },
                    {
                        field: 'goodsModel', title: '分类型号', sortable: true, width: 150, align: 'left'
                    },
                    {
                        field: 'productionPlace', title: '分类产地', sortable: true, width: 150, align: 'left'
                    },
                    {
                        field: 'goodsUnit', title: '单位', sortable: true, width: 150, align: 'left'
                    },
                    {
                        field: 'auxiliaryUnit', title: '辅助单位', sortable: true, width: 150, align: 'left'
                    },
                    {
                        field: 'goodsBarcode', title: '分类条码', sortable: true, width: 150, align: 'left'
                    },
                    {
                        field: 'intoTaxRate', title: '进项税', sortable: true, width: 150, align: 'right'
                    },
                    {
                        field: 'saleTaxRate', title: '销项税', sortable: true, width: 150, align: 'right'
                    },
                    {
                        field: 'goodsRemark', title: '备注', sortable: true, width: 150, align: 'left'
                    },
                ]]
                ,

            });
        //function EditGoodsInfo(value, rowData, rowIndex) {
        //    _goodsInfoidForCreateOrUpdate = rowData.id;
        //    clickLeftNav("addGoodsInfoDiv", "分类信息", "/Goods/CreateOrUpdateGoodsIndex", { GoodsId: _goodsInfoidForCreateOrUpdate, hidden: false }, true);
        //}
        //function DetailGoodsInfo(value, rowData, rowIndex) {
        //    _goodsInfoidForCreateOrUpdate = rowData.id;
        //    clickLeftNav("addGoodsInfoDiv", "分类信息", "/Goods/CreateOrUpdateGoodsIndex", { GoodsId: _goodsInfoidForCreateOrUpdate, hidden: true }, true);
        //}
    });
})();