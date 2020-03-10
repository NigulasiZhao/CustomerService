(function () {
    $(function () {
        var _unionInfoService = abp.services.app.unionInfo;

        var _$Modal = $tabs('.addCustomerModal');
        var _$form = $tabs('form[name=form1]');
        var tableId = "#CustomerTableList";

        _$form.closest('div.modal-content').find(".btn-primary").click(function (e) {
            e.preventDefault();
            CustomerSave();
        });


         $tabs(".querySearch").click(function () {
            var Search = $(this).parent().serializeObject();

             DG.setQueryParamsRequest($tabs(tableId), Search)
        })

        $tabs(".addCustomerBtn").click(function () {
            _$form.form('clear');
            $tabs('.addCustomerModal').modal({
                backdrop: "static"
            })
            //_unionInfoService.getMaxUnionInfoCode().done(function (data) {
            //    _$form.form('load', { unionCode: data.data });
            //    _$form.closest('div.modal-content').find(".btn-primary").removeClass('hide');
            //}).always(function () {
            //    abp.ui.clearBusy(_$Modal);
            //}).fail(function (data) {
            //    abp.ui.clearBusy(_$form);
            //});

            TabsChange("addCustomerModal", 1, false);

            $tabs(".addCustomerModal input[name=isSupplierAndCustomer]").change(function (e) {
                if ($(this).is(':checked')) {
                    TabsChange("addCustomerModal", 1, true);
                } else {
                    TabsChange("addCustomerModal", 1, false);
                }
            });
        })

        $tabs(tableId).datagrid
            ({
                fitColumns: true,
                width: '100%',
                fit: true,
                showFooter: true,
                //是否显示斑马线效果;同一行中显示数据;显示一个行号列;只允许选择一行
                striped: false, nowrap: true, rownumbers: true, singleSelect: true,
                queryParams: {
                    FilterUserState: 0
                },
                url: "/api/services/app/UnionInfo/GetPagedCustomerInfo",
                //分页相关属性
                pagination: true, pageSize: 20, pageList: [20, 50, 100, 150, 200], pageNumber: 1, sortName: 'CustomerCode', sortOrder: 'asc',
                onClickCell: function (index, field) {
                    DG.onClickCell(index, field, this);
                    editRow = index;
                    $tabs(tableId).datagrid('beginEdit', index);
                    DG.selectClickCell(field);
                },
                rowStyler: function (index, row) { },
                onLoadSuccess: function (data) {
                    $tabs(tableId).datagrid("keyCtr");
                    tableShow(tableId);
                },
                onBeginEdit: function (index, row) { },
                onAfterEdit: function (index, row, changes) { },
                columns: [[
                    {
                        field: 'Id', title: '操作', sortable: true, width: 150, align: 'center',
                        formatter: function (value, rowData, rowIndex) {
                            return renderGridButton([
                                addGridButton("编辑", EditCustomerInfo, true, ""),
                                addGridButton("详情", CustomerInfoInfoDetail, true, "")
                            ], 2);
                        }
                    },
                    {
                        field: 'customerCode', title: '客户编码', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'customerName', title: '客户名称', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'address', title: '地址', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'contact', title: '联系人', sortable: true, width: 150, align: 'center'
                    },
                    {
                        field: 'contactNumber', title: '联系电话', sortable: true, width: 150, align: 'center'
                    },
                ]],
            });
        function EditCustomerInfo(value, rowData, rowIndex) {
            BindingCustomerInfo(rowData.unionInfoId);

            $tabs(".addCustomerModal").modal("show");
            $tabs(".addCustomerModal input[name=isSupplierAndCustomer]").change(function (e) {
                if ($(this).is(':checked')) {
                    TabsChange("addCustomerModal", 1, true);
                } else {
                    TabsChange("addCustomerModal", 1, false);
                }
            });
            _$form.closest('div.modal-content').find(".btn-primary").removeClass('hide');
        }
        function CustomerInfoInfoDetail(value, rowData, rowIndex) {
            BindingCustomerInfo(rowData.unionInfoId);
            $tabs(".addCustomerModal").modal("show");
            TabsChange("addCustomerModal");
            _$form.closest('div.modal-content').find(".btn-primary").addClass('hide');
        }
        function BindingCustomerInfo(Id) {
            _unionInfoService.getUnionInfoForEdit(Id).done(function (data) {
                $tabs('form[name=form1]').form('load', data.data);
                if ($tabs(".addCustomerModal input[name=isSupplierAndCustomer]").is(':checked')) {
                    TabsChange("addCustomerModal", 1, true);
                } else {
                    TabsChange("addCustomerModal", 1, false);
                }
            }).always(function () {
                abp.ui.clearBusy(_$Modal);
            }).fail(function (data) {
                abp.ui.clearBusy(_$form);
            });
        }

        function CustomerSave() {
            if (!_$form.valid()) {
                return;
            }
            var FormInfo = _$form.serializeFormToObject();
            var UnionInfo = new Object();
            UnionInfo.CustomerUnion = new Object();
            UnionInfo.SupplierUnion = new Object();
            UnionInfo.Id = FormInfo.id;
            UnionInfo.UnionName = FormInfo.unionName;
            UnionInfo.UnionCode = FormInfo.unionCode;
            UnionInfo.ContactName = FormInfo.contactName;
            UnionInfo.ContactNumber = FormInfo.contactNumber;
            UnionInfo.Address = FormInfo.address;
            UnionInfo.HelpCode = FormInfo.helpCode;
            UnionInfo.Nature = typeof FormInfo.nature == "undefined" ? 0 : FormInfo.nature;
            UnionInfo.IsSupplierAndCustomer = typeof FormInfo.isSupplierAndCustomer == "undefined" ? 0 : FormInfo.isSupplierAndCustomer;

            UnionInfo.CustomerUnion.Id = FormInfo.customerId;
            UnionInfo.CustomerUnion.Level = FormInfo.customerLevel;
            UnionInfo.CustomerUnion.Bank = FormInfo.customerBank;
            UnionInfo.CustomerUnion.Account = FormInfo.customerAccount;
            UnionInfo.CustomerUnion.InvoiceHead = FormInfo.customerInvoiceHead;
            UnionInfo.CustomerUnion.PayWay = FormInfo.customerPayWay;
            UnionInfo.CustomerUnion.AccountDate = FormInfo.customerAccountDate;
            UnionInfo.CustomerUnion.UseStateType = FormInfo.customerUseStateType;
            UnionInfo.CustomerUnion.Remark = FormInfo.customerRemark;
            UnionInfo.CustomerUnion.isBilling = FormInfo.isBilling;
            UnionInfo.CustomerUnion.saleUserInfoId = FormInfo.saleUserInfoId;

            UnionInfo.SupplierUnion.Id = FormInfo.supplierId;
            UnionInfo.SupplierUnion.Bank = FormInfo.supplierBank;
            UnionInfo.SupplierUnion.Account = FormInfo.supplierAccount;
            UnionInfo.SupplierUnion.UseStateType = FormInfo.supplierUseStateType;
            UnionInfo.SupplierUnion.Remark = FormInfo.supplierRemark;
            UnionInfo.SupplierUnion.TaxBill = FormInfo.supplierTaxBill;
            UnionInfo.SupplierUnion.PayWay = FormInfo.supplierPayWay;
            if (FormInfo.isSupplierAndCustomer == "1") {
                UnionInfo.IsSupplier = 0;
                UnionInfo.IsCustomer = 0;
            } else {
                UnionInfo.IsSupplier = 1;
                UnionInfo.IsCustomer = 0;
                UnionInfo.SupplierUnion = new Object();
            }
            if (FormInfo.unionName == "") {
                abp.message.error("请填写名称", '提示');
                return
            }
            if (FormInfo.unionCode == "") {
                abp.message.error("请填写编码", '提示');
                return
            }
            if (FormInfo.contactName == "") {
                abp.message.error("请填写联系人", '提示');
                return
            }
            if (FormInfo.contactNumber == "") {
                abp.message.error("请填写联系电话", '提示');
                return
            }
            if (FormInfo.customerLevel == "") {
                abp.message.error("请填写客户级别", '提示');
                return
            }
            if (FormInfo.isBilling == "") {
                abp.message.error("请填写客户是否开票", '提示');
                return
            }
            if (FormInfo.saleUserInfoId == "") {
                abp.message.error("请填写默认销售员", '提示');
                return
            }
            if (FormInfo.customerPayWay == "") {
                abp.message.error("请填写客户结账方式", '提示');
                return
            }
            if (FormInfo.customerAccountDate == "") {
                abp.message.error("请填写客户账期", '提示');
                return
            }
            if (FormInfo.customerUseStateType == "") {
                abp.message.error("请选择客户状态", '提示');
                return
            }
            if (FormInfo.isSupplierAndCustomer == "1") {
                if (FormInfo.supplierTaxBill == "") {
                    abp.message.error("请填写供应商是否开票", '提示');
                    return
                }
                if (FormInfo.supplierPayWay == "") {
                    abp.message.error("请填写供应商结账方式", '提示');
                    return
                }
                if (FormInfo.supplierUseStateType == "") {
                    abp.message.error("请选择供应商状态", '提示');
                    return
                }
            }
            abp.ui.setBusy(_$form);
            if (UnionInfo.Id != "") {
                if (UnionInfo.CustomerUnion.Id == "") {
                    delete UnionInfo.CustomerUnion["Id"];
                }
                if (UnionInfo.SupplierUnion.Id == "") {
                    delete UnionInfo.SupplierUnion["Id"];
                }
                _unionInfoService.updateUnionInfo(UnionInfo).done(function (data) {
                    abp.message.success("操作成功", '提示');
                    abp.ui.clearBusy(_$form);
                    _$Modal.modal('hide');
                    $tabs(tableId).datagrid("reload");
                }).always(function () {
                    abp.ui.clearBusy(_$Modal);
                }).fail(function (data) {
                    abp.ui.clearBusy(_$form);
                });
            } else {
                delete UnionInfo["Id"];
                delete UnionInfo.CustomerUnion["Id"];
                delete UnionInfo.SupplierUnion["Id"];
                _unionInfoService.newUnionInfo(UnionInfo).done(function (data) {
                    abp.message.success("操作成功", '提示');
                    abp.ui.clearBusy(_$form);
                    _$Modal.modal('hide');
                    $tabs(tableId).datagrid("reload");
                }).always(function () {
                    abp.ui.clearBusy(_$Modal);
                }).fail(function (data) {
                    abp.ui.clearBusy(_$form);
                });
            }
        }
    })
})()