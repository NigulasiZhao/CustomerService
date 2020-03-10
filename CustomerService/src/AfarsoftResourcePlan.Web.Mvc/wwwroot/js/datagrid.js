var editRow = undefined;
var DG = {
    //表格工具栏功能
    insertRow: function (dataGridObj) {
        if (endEditing(dataGridObj)) {
            $(dataGridObj).datagrid('appendRow', {});
            editRow = $(dataGridObj).datagrid('getRows').length - 1;
            $(dataGridObj).datagrid('selectRow', editRow).datagrid('endEdit', editRow);
        }
    },
    insertInSeleRow: function (dataGridObj) {
        if (endEditing(dataGridObj)) {
            var row = $(dataGridObj).datagrid('getSelected');
            if (row == null) { return }
            var rowindex = $(dataGridObj).datagrid('getRowIndex', row);
            $(dataGridObj).datagrid('insertRow', {
                index: rowindex,	// 索引从0开始
                row: {}
            });
            editRow = rowindex;
            $(dataGridObj).datagrid('selectRow', editRow).datagrid('endEdit', editRow);
        }
    },
    DeleteRow: function (dataGridObj) {
        if (editRow == undefined) { return }
        var row = $(dataGridObj).datagrid('getSelected');

        if (row == null) { return }
        var rowindex = $(dataGridObj).datagrid('getRowIndex', row);
        $(dataGridObj).datagrid('deleteRow', rowindex);

        editRow == undefined;
        if (rowindex >= 1) {
            $(dataGridObj).datagrid('selectRow', rowindex - 1);
        } else {
            $(dataGridObj).datagrid('selectRow', rowindex - 0);
        }
    },
    watchSearchInput: function (_this) {
        if ($(_this).val() == '') {
            $(".datagrid-toolbar").next().find("div.datagrid-body").find("tr td[style='background: yellow;']").css("background", "none");
        }
    },
    //工具栏搜索事件
    searchPosition: function (dataGridId) {
        if ($("#iptSearch").val() == "") {
            alert('请输入查找条件');
        } else {
            var row = $(dataGridId).datagrid('getSelected');
            var rowIndex = $(dataGridId).datagrid('getRowIndex', $(dataGridId).datagrid('getSelected'));
            DG.SearchSameGoods(dataGridId, "#iptSearch", rowIndex + 1);
        }
    },
    //查询Datagrid中相同的商品并定位
    //targetGrid为Datagrid,targetText为查询条件框
    SearchSameGoods: function (targetGrid, targetText, index) {
        $(".datagrid-toolbar").next().find("div.datagrid-body").find("tr td[style='background: yellow;']").css("background", "none");
        //获取datagrid的所有列
        var datagridFieldArr = $(targetGrid).datagrid("options").columns[0].slice(0);
        if ($(targetGrid).datagrid("options").frozenColumns && $(targetGrid).datagrid("options").frozenColumns.length) {
            datagridFieldArr = ($(targetGrid).datagrid("options").frozenColumns[0]).concat(datagridFieldArr);
        }
        for (var i = 0; i < datagridFieldArr.length; i++) {
            if (datagridFieldArr[i].hidden) {
                datagridFieldArr.splice(i, 1);
                i--;
            } else {
                datagridFieldArr[i] = datagridFieldArr[i].field;
            }
        }
        $(targetGrid).datagrid('unselectAll');
        var SearchCondition = $(targetText).val().trim();
        //获取datagrid中每行的数据
        var RowsData = $(targetGrid).datagrid('getRows');
        var SelectFlag = 0;
        if (index >= RowsData.length) {
            index = 0;
        }
        var obj = {};
        obj.arr = [];
        obj.arr.length = RowsData.length;
        for (var i = 0; i < obj.arr.length; i++) {
            obj.arr[i] = false;
        }
        obj.isCheckAll = 'true';
        for (var i = index; i < RowsData.length; i++) {
            obj.arr[i] = true;
            for (var j = 0; j < datagridFieldArr.length; j++) {
                if (RowsData[i] && RowsData[i][datagridFieldArr[j]]) {
                    //判断是否为字符串类型               
                    if (Object.prototype.toString.call(RowsData[i][datagridFieldArr[j]]) != "[object String]") {
                        if (RowsData[i][datagridFieldArr[j]].toString().indexOf(SearchCondition) >= 0) {
                            $(targetGrid).datagrid('selectRow', i);
                            $(".datagrid-toolbar").next().find("div.datagrid-body").find("tr[datagrid-row-index = " + i + "]").find("td[field=" + datagridFieldArr[j] + "]").css("background", "yellow");
                            obj.arr[i] = true;
                            SelectFlag++;
                            return false;
                        }
                    } else {
                        if (RowsData[i][datagridFieldArr[j]].indexOf(SearchCondition) >= 0) {
                            $(targetGrid).datagrid('selectRow', i);
                            $(".datagrid-toolbar").next().find("div.datagrid-body").find("tr[datagrid-row-index = " + i + "]").find("td[field=" + datagridFieldArr[j] + "]").css("background", "yellow");
                            obj.arr[i] = true;
                            SelectFlag++;
                            return false;
                        }
                    }
                }
            }
        }
        obj.arr.forEach(function (item, indexs, self) {
            if (!item) {
                obj.isCheckAll = 'false';
            }
        })
        if (obj.isCheckAll = 'false' && obj.arr[obj.arr.length - 1] == true && SelectFlag == 0 && index > 0) {
            for (var i = 0; i < index; i++) {
                obj.arr[i] = true;
                for (var j = 0; j < datagridFieldArr.length; j++) {
                    if (RowsData[i] && RowsData[i][datagridFieldArr[j]]) {
                        if (Object.prototype.toString.call(RowsData[i][datagridFieldArr[j]]) != "[object String]") {
                            if (RowsData[i][datagridFieldArr[j]].toString().indexOf(SearchCondition) >= 0) {
                                $(targetGrid).datagrid('selectRow', i);
                                $(".datagrid-toolbar").next().find("div.datagrid-body").find("tr[datagrid-row-index = " + i + "]").find("td[field=" + datagridFieldArr[j] + "]").css("background", "yellow");
                                obj.arr[i] = true;
                                SelectFlag++;
                                return false;
                            }
                        } else {
                            if (RowsData[i][datagridFieldArr[j]].indexOf(SearchCondition) >= 0) {
                                $(targetGrid).datagrid('selectRow', i);
                                $(".datagrid-toolbar").next().find("div.datagrid-body").find("tr[datagrid-row-index = " + i + "]").find("td[field=" + datagridFieldArr[j] + "]").css("background", "yellow");
                                obj.arr[i] = true;
                                SelectFlag++;
                                return false;
                            }
                        }
                    }
                }
            }
        }
        if (SelectFlag == 0 && obj.isCheckAll != 'false') {
            alert('未查询到符合条件的商品');
        }
    },
    onClickCell: function (index, field, dataGridObj) {
        if (editRow != index) {
            if (endEditing(dataGridObj)) {
                $(dataGridObj).datagrid('selectRow', index).datagrid('beginEdit', index);
                var ed = $(dataGridObj).datagrid('getEditor', { index: index, field: field });
                if (ed) {
                    ($(ed.target).data('textbox') ? $(ed.target).textbox('textbox') : $(ed.target)).focus();
                }
                editRow = index;

            } else {
                setTimeout(function () {
                    $(dataGridObj).datagrid('selectRow', editRow);
                }, 0);
            }

        } else {

        }

        //$(dataGridObj).datagrid('unselectRow', index);
        //捕捉编辑框双击事件  弹出窗口
        setTimeout(function () {
            $('table tr td .readonly-textbox').dblclick(function () {
                if ($(this).find('.icon-add')) {
                    $(this).find('.icon-add').click();
                }
            });
            //捕捉编辑框enter键盘事件  弹出窗口
            $('table tr td .readonly-textbox').keydown(function (event) {
                var eCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                if (eCode == 13) {

                    //  $(this).find('.icon-add').click();
                }
            });
        }, 100);

        var EditingRows = $(dataGridObj).datagrid('getEditingRowIndexs');
        var arr = Object.keys(EditingRows);
        for (var i = 0; i < arr.length; i++) {
            $(dataGridObj).datagrid('endEdit', EditingRows[i]);
        }
    },
    //点击表格时聚焦表格内容
    selectClickCell: function (field) {
        var nextSelectIndex = 0;
        for (var i = 0; i < $(".datagrid-editable-input").length; i++) {
            if ($(".datagrid-editable-input").eq(i).parents('[field]').attr("field") == field) {
                nextSelectIndex = i;
            }
        }
        $('.datagrid-editable-input').eq(nextSelectIndex).next().children('input').eq(0).focus();
        $('.datagrid-editable-input').eq(nextSelectIndex).next().children('input').eq(0).select();
    },
    goodsFillWindow: function (recordGrid, targetGrid, row, allowedRepeat) {
        var SameFlag = 0;
        var SameInfo = "";
        var flag = 0;
        var Location = -1;
        var pos = 0;
        var recordGrid = $("#" + recordGrid);
        var targetGrid = $("#" + targetGrid);
        var goodsRowsData = recordGrid.datagrid('getSelections');
        if (row) {
            goodsRowsData = [];
            goodsRowsData.push(row);
        }
        var totalRow = targetGrid.datagrid("getData").total;
        if (targetGrid.length > 0) {
            var mainRow = targetGrid.datagrid('getSelected');
            var rowIndex = targetGrid.datagrid('getRowIndex', mainRow);
            var AllRows = targetGrid.datagrid('getRows');
            for (var i = 0; i < goodsRowsData.length; i++) {
                if (allowedRepeat) {
                    var purchaseIndex = rowIndex + i;
                    var taxRate = 16;
                    if (goodsRowsData[i].TaxRateId != null && goodsRowsData[i].TaxRateId.toString() != '' && goodsRowsData[i].TaxRateId != undefined) {
                        taxRate = Number(goodsRowsData[i].TaxRateId);
                    }
                } else {
                    if (flag == 0) {
                        var purchaseIndex = rowIndex + i;
                    } else {
                        flag = 0;
                    }
                    var taxRate = 17;
                    if (goodsRowsData[i].TaxRate != null && goodsRowsData[i].TaxRate != '' && goodsRowsData[i].TaxRate != undefined) {
                        taxRate = Number(goodsRowsData[i].TaxRate);
                    }
                }
                cauTaxRate = (100 + taxRate) / 100;
                goodsRowsData[i].IsLockInventory = 1;//设置默认锁定库存
                goodsRowsData[i].DiscountRate = 1;//设置默认扣率
                goodsRowsData[i].PreDiscountPrice = goodsRowsData[i].GoodsPrice;//设置扣前价
                goodsRowsData[i].SrcGoodsPrice = goodsRowsData[i].GoodsPrice;//设置源价

                if (allowedRepeat) {
                    if (purchaseIndex >= totalRow - 1) {
                        targetGrid.datagrid('insertRow', {
                            index: purchaseIndex,
                            row: goodsRowsData[i],
                        });
                        targetGrid.datagrid('updateRow', {
                            index: purchaseIndex,
                            row: {
                                TaxRate: taxRate,
                                GoodsPriceNoTax: CutTwo(goodsRowsData[i].GoodsPrice / cauTaxRate)
                            }
                        });
                    }
                    //更新数据
                    else {
                        targetGrid.datagrid('updateRow', {
                            index: purchaseIndex,
                            row: goodsRowsData[i],
                        });
                        targetGrid.datagrid('updateRow', {
                            index: purchaseIndex,
                            row: {
                                TaxRate: taxRate,
                                GoodsPriceNoTax: CutTwo(goodsRowsData[i].GoodsPrice / cauTaxRate)
                            }
                        });
                    }
                    if (i == goodsRowsData.length - 1) {
                        targetGrid.datagrid('selectRow', purchaseIndex + 1);
                        targetGrid.datagrid('beginEdit', purchaseIndex + 1);
                        if (editRow >= 0) {
                            editRow = purchaseIndex - goodsRowsData.length + 1;
                        }
                    }
                } else {
                    for (var j = 0; j < AllRows.length; j++) {
                        if (AllRows[j].GoodsId == goodsRowsData[i].GoodsId) {
                            if (Location == -1) {
                                Location = j;
                            }
                            SameFlag++;
                            SameInfo += SameFlag + ".第" + (j + 1) + "行商品编号为" + AllRows[j].GoodsCode + "的商品已存在;";
                            flag++;
                        }
                    }
                    if (flag == 0) {
                        if (purchaseIndex >= totalRow - 1) { //行数是否够用，不够的话先新增
                            targetGrid.datagrid('insertRow', {
                                index: purchaseIndex,
                                row: goodsRowsData[i],
                            });
                            targetGrid.datagrid('updateRow', {
                                index: purchaseIndex,
                                row: {
                                    TaxRate: taxRate,
                                    GoodsPriceNoTax: CutTwo(goodsRowsData[i].GoodsPrice / cauTaxRate)
                                }
                            });
                        }
                        //更新数据
                        else {
                            targetGrid.datagrid('updateRow', {
                                index: purchaseIndex,
                                row: goodsRowsData[i],
                            });
                            targetGrid.datagrid('updateRow', {
                                index: purchaseIndex,
                                row: {
                                    TaxRate: taxRate,
                                    GoodsPriceNoTax: CutTwo(goodsRowsData[i].GoodsPrice / cauTaxRate)
                                }
                            });
                        }
                    }
                    if (i == goodsRowsData.length - 1) {
                        if (SameFlag > 0) {
                            pos = purchaseIndex;
                        } else {
                            targetGrid.datagrid('selectRow', purchaseIndex + 1);
                            targetGrid.datagrid('beginEdit', purchaseIndex + 1);
                        }
                    }
                }
            }
            if (!allowedRepeat) {
                if (SameFlag > 0) {
                    targetGrid.datagrid('endEdit', rowIndex);
                    targetGrid.datagrid('selectRow', pos);
                    targetGrid.datagrid('beginEdit', pos);
                    //CE.messgeShow('', SameInfo, 5, closeMsgAndFocus, '', recordGrid);
                    abp.message.warn(SameInfo, '试一试标题');

                }
                //var mainRowT = targetGrid.datagrid('getSelected');
                var rowIndexT = targetGrid.datagrid('getRowIndex', mainRow);
                editRow = rowIndexT;
            }
        }
    },
    //填充方法
    goodsRowsWindow: function (targetGrid, row, allowedRepeat) {
        var SameFlag = 0;
        var SameInfo = "";
        var flag = 0;
        var Location = -1;
        var pos = 0;
        
        var targetGrid = $(targetGrid);
        var goodsRowsData = row;
        //if (row) {
        //    goodsRowsData.push(row);
        //}
        var totalRow = targetGrid.datagrid("getData").total;
        if (targetGrid.length > 0) {
            var mainRow = targetGrid.datagrid('getSelected');
            var rowIndex = targetGrid.datagrid('getRowIndex', mainRow);
            var AllRows = targetGrid.datagrid('getRows');
            for (var i = 0; i < goodsRowsData.length; i++) {
                if (allowedRepeat) {
                    var purchaseIndex = rowIndex + i;
                    var taxRate = 16;
                    if (goodsRowsData[i].saleTaxRate != null && goodsRowsData[i].saleTaxRate.toString() != '' && goodsRowsData[i].saleTaxRate != undefined) {
                        taxRate = Number(goodsRowsData[i].saleTaxRate);
                    }
                } else {
                    if (flag == 0) {
                        var purchaseIndex = rowIndex + i;
                    } else {
                        flag = 0;
                    }
                    var taxRate = 17;
                    if (goodsRowsData[i].saleTaxRate != null && goodsRowsData[i].saleTaxRate != '' && goodsRowsData[i].saleTaxRate != undefined) {
                        taxRate = Number(goodsRowsData[i].saleTaxRate);
                    }
                }
                cauTaxRate = (100 + taxRate) / 100;
                goodsRowsData[i].IsLockInventory = 1;//设置默认锁定库存
                goodsRowsData[i].DiscountRate = 1;//设置默认扣率
                goodsRowsData[i].PreDiscountPrice = goodsRowsData[i].goodsPrice;//设置扣前价
                goodsRowsData[i].SrcGoodsPrice = goodsRowsData[i].goodsPrice;//设置源价

                if (allowedRepeat) {
                    if (purchaseIndex >= totalRow - 1) {
                        targetGrid.datagrid('insertRow', {
                            index: purchaseIndex,
                            row: goodsRowsData[i],
                        });
                        targetGrid.datagrid('updateRow', {
                            index: purchaseIndex,
                            row: {
                                taxRate: taxRate,
                                //priceNoTaxRate: goodsRowsData[i].goodsPrice / cauTaxRate
                            }
                        });
                    }
                    //更新数据
                    else {
                        targetGrid.datagrid('updateRow', {
                            index: purchaseIndex,
                            row: goodsRowsData[i],
                        });
                        targetGrid.datagrid('updateRow', {
                            index: purchaseIndex,
                            row: {
                                taxRate: taxRate,
                                //priceNoTaxRate: goodsRowsData[i].goodsPrice / cauTaxRate
                            }
                        });
                    }
                    if (i == goodsRowsData.length - 1) {
                        targetGrid.datagrid('selectRow', purchaseIndex + 1);
                        targetGrid.datagrid('beginEdit', purchaseIndex + 1);
                        if (editRow >= 0) {
                            editRow = purchaseIndex - goodsRowsData.length + 1;
                        }
                    }
                } else {
                    for (var j = 0; j < AllRows.length; j++) {
                        //if (AllRows[j].id == goodsRowsData[i].id) {
                        if ((AllRows[j].goodsId && AllRows[j].goodsId == goodsRowsData[i].id) || AllRows[j].id == goodsRowsData[i].id) {
                            if (Location == -1) {
                                Location = j;
                            }
                            SameFlag++;
                            SameInfo += SameFlag + ".第" + (j + 1) + "行商品编号为" + AllRows[j].goodsCode + "的商品已存在;";
                            flag++;
                        }
                    }
                    if (flag == 0) {
                        if (purchaseIndex >= totalRow - 1) { //行数是否够用，不够的话先新增
                            targetGrid.datagrid('insertRow', {
                                index: purchaseIndex,
                                row: goodsRowsData[i],
                            });
                            targetGrid.datagrid('updateRow', {
                                index: purchaseIndex,
                                row: {
                                    taxRate: taxRate,
                                    //priceNoTaxRate: goodsRowsData[i].goodsPrice / cauTaxRate
                                }
                            });
                        }
                        //更新数据
                        else {
                            targetGrid.datagrid('updateRow', {
                                index: purchaseIndex,
                                row: goodsRowsData[i],
                            });
                            targetGrid.datagrid('updateRow', {
                                index: purchaseIndex,
                                row: {
                                    taxRate: taxRate,
                                    //priceNoTaxRate: goodsRowsData[i].goodsPrice / cauTaxRate
                                }
                            });
                        }
                    }
                    if (i == goodsRowsData.length - 1) {
                        if (SameFlag > 0) {
                            pos = purchaseIndex;
                        } else {
                            targetGrid.datagrid('selectRow', purchaseIndex + 1);
                            targetGrid.datagrid('beginEdit', purchaseIndex + 1);
                        }
                    }
                }
            }
            if (SameFlag > 0) {
                targetGrid.datagrid("updateRow", {
                    index: pos,
                    row: {
                        goodsName: ""
                    }
                });
                targetGrid.datagrid('endEdit', rowIndex);
                targetGrid.datagrid('selectRow', pos);
                targetGrid.datagrid('beginEdit', pos);
                abp.message.warn(SameInfo, '提示');
            }
            var rowIndexT = targetGrid.datagrid('getRowIndex', mainRow);
            editRow = rowIndexT;
        }
    },
    getGridColumns: function (dataGridId, RoleFieldPermissions) {
        var result = localStorage[dataGridId];
        if (result == null || result == undefined || result == "") {
            return null
        }
        if (RoleFieldPermissions != "" && RoleFieldPermissions != null) {
            return DG.FilterPermissionsColumn(dataGridId, JSON.parse(result), RoleFieldPermissions);
        } else {
            return JSON.parse(result);
        }
    },
    //Datagrid参数
    setQueryParamsRequest: function (table, formObj) {
        var queryParams = table.datagrid('options').queryParams;
        var sortName = table.datagrid('options').sortName;
        var sortOrder = table.datagrid('options').sortOrder;
        queryParams = $.extend({}, queryParams, formObj);
        table.datagrid('options').queryParams = queryParams;
        table.datagrid("reload");
    },
    //treegrid参数
    setTreeGridQueryParamsRequest: function (table, formObj) {
        var queryParams = $(table).treegrid('options').queryParams;
        var sortName = $(table).treegrid('options').sortName;
        var sortOrder = $(table).treegrid('options').sortOrder;
        queryParams = $.extend({}, queryParams, formObj);
        $(table).treegrid('options').queryParams = queryParams;
        $(table).treegrid("reload");
    },
    //保留两位小数
    CutTwo: function (num) {
        if (num != '' && num != 'undefined' && num != null) {
            return parseFloat(parseFloat(num).toFixed(3));
        }
        else {
            return 0.000;
        }
    },
    //保留四位小数
    CutFour: function (num) {
        if (num != '' && num != 'undefined' && num != null) {
            return parseFloat(parseFloat(num).toFixed(4));
        }
        else {
            return 0.0000;
        }
    },
    countColumns: function (obj) {
        var argArr = Array.prototype.slice.call(arguments, 1);
        var rows = $(obj).datagrid('getRows')//获取当前页的数据行  
        var totals = [];
        totals.length = argArr.length;
        for (var i = 0; i < totals.length; i++) {
            totals[i] = 0;
        }
        for (var i = 0; i < rows.length; i++) {
            for (var j = 0; j < argArr.length; j++) {
                var num = rows[i][argArr[j]];
                if (num != '' && num != 'undefined' && num != null) {
                    totals[j] += DG.CutFour(num);
                }
            }
        }
        var str = '';
        for (var i = 0; i < argArr.length; i++) {
            str += '"' + argArr[i] + '\": \"' + DG.CutFour(totals[i]) + '\",'
        }
        str = eval("(" + "{" + str + "}" + ")");
        //console.log(str);
        $(obj).datagrid('reloadFooter', [
            str
        ]);
        $('.datagrid-ftable .datagrid-row .datagrid-td-rownumber ').html('<div class="datagrid-cell-rownumber" style="font-weight: bold;">合计</div>');
    },
    bottomAmount: function (obj) {
        var argArr = Array.prototype.slice.call(arguments, 1);
        var rows = $(obj).datagrid('getRows')//获取当前页的数据行  
        var totals = [];
        totals.length = argArr.length;
        for (var i = 0; i < totals.length; i++) {
            totals[i] = 0;
        }
        for (var i = 0; i < rows.length; i++) {
            for (var j = 0; j < argArr.length; j++) {
                var num = rows[i][argArr[j]];
                if (num != '' && num != 'undefined' && num != null) {
                    totals[j] += DG.CutFour(num);
                }
            }
        }
        var str = '';
        for (var i = 0; i < argArr.length; i++) {
            str += '"' + argArr[i] + '\": \"' + DG.CutFour(totals[i]) + '\",'
        }
        str = eval("(" + "{" + str + "}" + ")");
        return str[argArr[0]];
    },
    calculationAmount: function (obj,index, row, changes) {
        if (changes.goodsQuantity == '' || row.id == undefined) {
            return;
        }
        if (row.goodsPrice) {
            $(obj).datagrid('updateRow', {
                index: index,
                row: {
                    priceNoTaxRate: DG.CutFour(row.goodsPrice / ((100 + parseInt(row.taxRate)) / 100))
                }
            })
            if (row.goodsQuantity) {
                $(obj).datagrid('updateRow', {
                    index: index,
                    row: {
                        goodsAmount: DG.CutFour(row.goodsPrice * parseInt(row.goodsQuantity)),
                        amountNoTaxRate: DG.CutFour(DG.CutFour(row.goodsPrice / ((100 + parseInt(row.taxRate)) / 100)) * parseInt(row.goodsQuantity))
                    }
                });
                DG.countColumns(obj, "goodsQuantity", "goodsAmount", "amountNoTaxRate");
            }
        }
    }
}

var IPT = {
    checkIptHasData: function () {
        var activeId = $("#tab-content-tabitem > .active").attr("id");
        var ipts = $("#" + activeId + " .SearchForm").find(".publicIpts");
        var currentIdx = 0;
        for (var i = 0; i < ipts.length; i++) {
            if (ipts[i].className.indexOf('publicIptsHover') !== -1) {
                currentIdx = i;
                break;
            }
        }
        ipts.click(function () {
            currentIdx = $(this).index();
        })
        if (ipts.eq(currentIdx).find("span").length == "1" || ipts.eq(currentIdx).find("span").find("input").eq(1).val()) {
            return true;
        } else {
            return false;
        }
    },
    formatterDate: function (date) {
        var day = date.getDate() > 9 ? date.getDate() : "0" + date.getDate();
        var month = (date.getMonth() + 1) > 9 ? (date.getMonth() + 1) : "0"
            + (date.getMonth() + 1);
        //var hor = date.getHours();
        //var min = date.getMinutes();
        //var sec = date.getSeconds();
        return date.getFullYear() + '-' + month + '-' + day;

    },
}

function endEditing(dataGridObj) {
    if (editRow == undefined) { return true }
    if ($(dataGridObj).datagrid('validateRow', editRow)) {
        $(dataGridObj).datagrid('endEdit', editRow);
        $(dataGridObj).datagrid('selectRow', editRow);
        editRow = undefined;
        return true;
    } else {
        return false;
    }
}