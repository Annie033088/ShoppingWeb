let orderDetail = null;
let order = null;
let orderItem = null;
let orderState = null;

getOrderDetail();

function populateOrder() {
    document.getElementById("txbOrderNumber").innerHTML += order.OrderNumber;
    document.getElementById("txbOrderCreateTime").innerHTML += formatDateToYYYYMMDDHHMMSS(order.CreateTime);
    document.getElementById("txbOrderState").innerHTML += orderStateToText(order.CurrentState);
    document.getElementById("txbOrderRemark").innerHTML += order.Remark;
    document.getElementById("txbMemberId").innerHTML += order.MemberId;
    document.getElementById("txbRecipientName").innerHTML += order.RecipientName;
    document.getElementById("txbPhone").innerHTML += order.Phone;
    document.getElementById("txbAddress").innerHTML += order.Address;
    document.getElementById("txbShippingOption").innerHTML += order.ShippingOptionName
    document.getElementById("txbLogisticsNumber").value = order.LogisticsNumber;

    document.getElementById("txbOriginalAmount").innerHTML += order.OriginalAmount +" 元";
    document.getElementById("txbDiscountAmount").innerHTML += (order.OriginalAmount - order.DiscountedAmount) + " 元";
    document.getElementById("txbShippingFee").innerHTML += order.ShippingFee + " 元";
    document.getElementById("txbTotalAmount").innerHTML += order.TotalAmount + " 元";
}

function populateOrderStates(orderStates) {
    orderStates.forEach(orderState => {
        // 創建 div 元素
        const card = document.createElement('div');
        card.className = 'card col-3 mb-3';
        card.style.fontSize = '20px';

        // 創建 '狀態' 部分
        const orderStateDiv = document.createElement('div');
        orderStateDiv.className = 'd-flex w-100 mt-1';
        const orderStateSpan = document.createElement('span');
        orderStateSpan.className = 'badge bg-secondary d-flex justify-content-center text-center align-items-center w-100';
        orderStateSpan.style.height = '50px';
        orderStateSpan.id = 'txbOrderState';
        orderStateSpan.textContent = '狀態：' + orderStateToText(orderState.State);
        orderStateDiv.appendChild(orderStateSpan);
        card.appendChild(orderStateDiv);

        // 創建 '備註' 部分
        const remarkDiv = document.createElement('div');
        remarkDiv.className = 'd-flex w-100 mt-1 justify-content-center align-items-center badge bg-secondary';
        const remarkSpan = document.createElement('span');
        remarkSpan.id = 'txbOrderRemark';
        remarkSpan.textContent = '備註：';
        remarkDiv.appendChild(remarkSpan);

        //創建備註輸入框
        const textArea = document.createElement('textarea');
        textArea.className = 'form-control mt-1 w-100';
        textArea.style.resize = 'none';
        textArea.style.height = '150px';
        textArea.placeholder = '在此輸入內容...';
        textArea.value = orderState.Remark;
        remarkDiv.appendChild(textArea);


        // 創建 '確認修改' 按鈕
        const confirmButton = document.createElement('button');
        confirmButton.className = 'btn btn-outline-light ms-1';
        confirmButton.textContent = '修改';
        confirmButton.addEventListener("click", function () {
            editOrderStateRemark(orderState.OrderStateId, textArea.value)
        })
        remarkDiv.appendChild(confirmButton);

        card.appendChild(remarkDiv);

        // 創建 '創建時間' 部分
        const createTimeDiv = document.createElement('div');
        createTimeDiv.className = 'w-100 d-flex';
        const createTimeSpan = document.createElement('span');
        createTimeSpan.className = 'badge bg-light text-dark w-100';
        createTimeSpan.textContent = '創建時間：' + formatDateToYYYYMMDDHHMMSS(orderState.CreateTime);
        createTimeDiv.appendChild(createTimeSpan);
        card.appendChild(createTimeDiv);

        // 創建 '修改時間' 部分
        const modifyTimeDiv = document.createElement('div');
        modifyTimeDiv.className = 'w-100 d-flex';
        const modifyTimeSpan = document.createElement('span');
        modifyTimeSpan.className = 'badge bg-light text-dark w-100';
        modifyTimeSpan.textContent = '修改時間：' + formatDateToYYYYMMDDHHMMSS(orderState.UpdateTime);;
        modifyTimeDiv.appendChild(modifyTimeSpan);
        card.appendChild(modifyTimeDiv);

        // 將卡片填充到 #orderStateContainer 中
        const container = document.getElementById('orderStateContainer');
        container.appendChild(card);
    })}

function populateOrderItems(orderItems) {
    const tableBody = document.getElementById("orderItemTable").getElementsByTagName('tbody')[0];
    tableBody.innerHTML = '';

    orderItems.forEach((orderItem, index) => {
        const row = document.createElement('tr');
        const rowSort = document.createElement('th');
        rowSort.textContent = index + 1;
        rowSort.scope = "row";
        row.appendChild(rowSort);

        const cellName = document.createElement('td');
        cellName.textContent = orderItem.ProductName;

        row.appendChild(cellName);

        const cellStyle = document.createElement('td');
        cellStyle.textContent = orderItem.Style;
        row.appendChild(cellStyle);

        const cellQuantity = document.createElement('td');
        cellQuantity.textContent = orderItem.Quantity;
        row.appendChild(cellQuantity);

        const cellPrice = document.createElement('td');
        cellPrice.textContent = "$" + orderItem.Price + "元";
        row.appendChild(cellPrice);

        // 把這一行加到表格中
        tableBody.appendChild(row);
    });
}

function getOrderDetail() {
    axios.post("/MainOrder/GetOrderDetail", { orderId: orderId })
        .then(response => {
            let errorCode = response.data.errorCode;

            //沒有成功
            if (errorCode != errorCodeDefine.Success) {
                let message = errorCodeToMessage(errorCode);

                //顯示訊息
                Swal.fire(message)
                    .then(result => {
                        //確認後處理
                        if (result.isConfirmed) {
                            //被踢出去
                            if (errorCode == errorCodeDefine.KickOut || errorCode == errorCodeDefine.Baned
                                || errorCode == errorCodeDefine.PermissionModified) {
                                window.location.href = "/Login/Index";
                            }
                            //跳轉頁面(情況是未登入的使用者輸入登入後的URL)
                            if (errorCode == errorCodeDefine.UserNotLogged) {
                                window.location.href = "/Login/Index";
                            }
                        }
                    });
                return;
            }

            //成功的話
            orderDetail = response.data.orderDetail;

            if (orderDetail == null) {
                return;
            }

            order = orderDetail.order;
            orderItems = orderDetail.orderItemDtos;
            orderStates = orderDetail.orderStateDtos;

            populateOrder();
            populateOrderItems(orderItems);
            populateOrderStates(orderStates);
            console.log(orderDetail)
        });
}

function editOrderRemark() {
    let orderRemark = document.getElementById("txbOrderRemark").value;

    //驗證輸入符合訊息
    const remarkRegex = /^.{0,50}$/;
    if (!remarkRegex.test(orderRemark)) {
        Swal.fire('請輸入50字內備註');
        return false;
    }

    editOrderRemarkDto = {
        OrderId: order.OrderId,
        Remark: orderRemark
    }

    axios.post("/MainOrder/EditOrderRemark", { editOrderRemarkDto })
        .then(response => {
            let errorCode = response.data.errorCode;

            //沒有成功
            if (errorCode != errorCodeDefine.Success) {
                let message = errorCodeToMessage(errorCode);

                //顯示訊息
                Swal.fire(message)
                    .then(result => {
                        //確認後處理
                        if (result.isConfirmed) {
                            //被踢出去
                            if (errorCode == errorCodeDefine.KickOut || errorCode == errorCodeDefine.Baned
                                || errorCode == errorCodeDefine.PermissionModified) {
                                window.location.href = "/Login/Index";
                            }
                            //跳轉頁面(情況是未登入的使用者輸入登入後的URL)
                            if (errorCode == errorCodeDefine.UserNotLogged) {
                                window.location.href = "/Login/Index";
                            }
                        }
                    });
                return;
            }

            //成功的話
            setRedirectPage("MainOrder", `GetRedirectOrderDetailView?orderId=${order.OrderId}`);
        });
}

function editOrderStateRemark(orderStateId, remark) {
    //驗證輸入符合訊息
    const remarkRegex = /^.{0,50}$/;
    if (!remarkRegex.test(remark)) {
        Swal.fire('請輸入50字內備註');
        return false;
    }

    editOrderStateRemarkDto = {
        OrderStateId: orderStateId,
        Remark: remark
    };

    axios.post("/MainOrder/EditOrderStateRemark", { editOrderStateRemarkDto })
        .then(response => {
            let errorCode = response.data.errorCode;

            //沒有成功
            if (errorCode != errorCodeDefine.Success) {
                let message = errorCodeToMessage(errorCode);

                //顯示訊息
                Swal.fire(message)
                    .then(result => {
                        //確認後處理
                        if (result.isConfirmed) {
                            //被踢出去
                            if (errorCode == errorCodeDefine.KickOut || errorCode == errorCodeDefine.Baned
                                || errorCode == errorCodeDefine.PermissionModified) {
                                window.location.href = "/Login/Index";
                            }
                            //跳轉頁面(情況是未登入的使用者輸入登入後的URL)
                            if (errorCode == errorCodeDefine.UserNotLogged) {
                                window.location.href = "/Login/Index";
                            }
                        }
                    });
                return;
            }

            //成功的話
            setRedirectPage("MainOrder", `GetRedirectOrderDetailView?orderId=${order.OrderId}`);
        });
}

function formatDateToYYYYMMDDHHMMSS(dateString) {
    // 使用正則表達式提取時間戳部分
    var timestamp = dateString.match(/\/Date\((\d+)\)\//);

    if (timestamp && timestamp[1]) {
        var date = new Date(parseInt(timestamp[1], 10));  // 轉換為毫秒並創建 Date 物件

        // 取得年份、月份、日期、時、分、秒
        var year = date.getFullYear();
        var month = date.getMonth() + 1; // 月份從 0 開始，需加 1
        var day = date.getDate();
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();

        // 格式化為兩位數
        month = month < 10 ? '0' + month : month;
        day = day < 10 ? '0' + day : day;
        hours = hours < 10 ? '0' + hours : hours;
        minutes = minutes < 10 ? '0' + minutes : minutes;
        seconds = seconds < 10 ? '0' + seconds : seconds;

        // 返回格式化後的字串：YYYY-MM-DD HH:mm:ss
        return year + '-' + month + '-' + day + ' ' + hours + ':' + minutes + ':' + seconds;
    }
    return null;  // 如果無法匹配，返回 null
}

function orderStateToText(state) {
    switch (state) {
        case 1:
            return "待確認";
        case 2:
            return "待出貨";
        case 3:
            return "已出貨";
        case 4:
            return "包裹已抵達";
        case 5:
            return "完成";
        case 6:
            return "商品退回";
        case 7:
            return "取消";
        case 8:
            return "申請退貨";
        case 9:
            return "退貨";
        case 10:
            return "退款";
        default:
            return "";
    }
}

function editLogisticsNumber() {
    let logisticsNumber = document.getElementById("txbLogisticsNumber").value;

    //驗證輸入符合訊息
    const logisticsNumberRegex = /^[a-z0-9A-Z]{0,20}$/;
    if (!logisticsNumberRegex.test(logisticsNumber)) {
        Swal.fire('請輸入20字內物流編號');
        return false;
    }

    editLogisticsNumberDto = {
        OrderId: order.OrderId,
        LogisticsNumber: logisticsNumber
    };

    axios.post("/MainOrder/EditLogisticsNumber", { editLogisticsNumberDto })
        .then(response => {
            let errorCode = response.data.errorCode;

            //沒有成功
            if (errorCode != errorCodeDefine.Success) {
                let message = errorCodeToMessage(errorCode);

                //顯示訊息
                Swal.fire(message)
                    .then(result => {
                        //確認後處理
                        if (result.isConfirmed) {
                            //被踢出去
                            if (errorCode == errorCodeDefine.KickOut || errorCode == errorCodeDefine.Baned
                                || errorCode == errorCodeDefine.PermissionModified) {
                                window.location.href = "/Login/Index";
                            }
                            //跳轉頁面(情況是未登入的使用者輸入登入後的URL)
                            if (errorCode == errorCodeDefine.UserNotLogged) {
                                window.location.href = "/Login/Index";
                            }
                        }
                    });
                return;
            }

            //成功的話
            setRedirectPage("MainOrder", `GetRedirectOrderDetailView?orderId=${order.OrderId}`);
        });
}