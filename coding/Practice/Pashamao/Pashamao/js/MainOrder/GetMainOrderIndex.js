let lastGetOrderNum = null;
let lastGetPhone = null;
let lastGetDate = null;
let lastGetStatus = null;
let lastGetMemberId = null;

document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("btnSelectByOrderNumber").addEventListener('click', function () {
        const selectOrderNumber = document.getElementById("txbSelectOrderNumber").value;
        const regexOrderNumber = /^[0-9]{18}$/;

        if (!regexOrderNumber.test(selectOrderNumber)) {
            Swal.fire("請輸入正確的訂單編號!");
            return;
        }

        getOrderByOrderNum(selectOrderNumber, 1);
    });

    document.getElementById("btnSelectByPhone").addEventListener('click', function () {
        const selectPhone = document.getElementById("txbSelectPhone").value;
        const regexPhone = /^[0-9]{3}$/;

        if (!regexPhone.test(selectPhone)) {
            Swal.fire("請輸入電話末三碼");
            return;
        }

        getOrderByPhone(selectPhone, 1);
    });

    document.getElementById("btnSelectByMemberId").addEventListener('click', function(){
        const selectMemberId = document.getElementById("txbSelectMemberId").value;
        const regexMemberId = /^[0-9]{1,10}$/

        if (!regexMemberId.test(selectMemberId)) {
            Swal.fire("請輸入正確會員Id")
            return;
        }

        getOrderByMemberId(selectMemberId, 1)
    })

    document.getElementById("btnSelectByDate").addEventListener('click', function () {
        const selectDateStart = document.getElementById("txbDateStart").value;
        const selectDateEnd = document.getElementById("txbDateEnd").value;

        const selectDate = {
            StartDate: selectDateStart,
            EndDate: selectDateEnd
        };

        getOrderByDate(selectDate, 1);
    });

    document.getElementById("selectStatus").addEventListener("change", function (event) {
        const selectedValue = event.target.value;
        //如果把選項設定預設狀態
        if (selectedValue == 0) {
            lastGetStatus = null;
        } else {
            lastGetStatus = selectedValue;
        }

        document.getElementById("currentPage").innerHTML = 1;
        getOrder(1)
    });
    // 設定勾選/取消勾選所有訂單
    document.getElementById("ckbAllOrder").addEventListener("change", function (event) {
        const orderCkbs = document.querySelectorAll('tbody input[type = "checkbox"]');
        
        if (event.target.checked) {
            orderCkbs.forEach(function (orderCkb) {
                orderCkb.checked = true
            })
        } else {
            orderCkbs.forEach( function (orderCkb) {
                orderCkb.checked = false;
            })
        }
    })
    //編輯勾選的訂單狀態
    document.getElementById("btnEditOrderState").addEventListener("click", function () {
        //判斷選重的狀態是不是都一樣
        const orderCkbs = document.querySelectorAll('tbody input[type = "checkbox"]');
        let stateList = [];
        orderCkbs.forEach(function (orderCkb) {
            const parentTr = orderCkb.closest('tr');
        })
    })

    getAll();
});

function getAll() {
    lastGetOrderNum = null;
    lastGetPhone = null;
    lastGetMemberId = null;
    lastGetDate = null;
    lastGetStatus = null;
    document.getElementById("selectStatus").value = "0";
    document.getElementById("currentPage").innerHTML = 1;
    getOrder(1);
}

function getOrder(page) {

    if (lastGetDate == null) {
        lastGetDate = {
            StartDate: null,
            EndDate:null
        }
    }

    let selectOrderDto = {
        OrderNumber: lastGetOrderNum,
        Phone: lastGetPhone,
        MemberId: lastGetMemberId,
        StartDate: lastGetDate.StartDate,
        EndDate: lastGetDate.EndDate,
        Status: lastGetStatus,
        Page: page
    };

    axios.post("/MainOrder/GetOrder", { selectOrderDto })
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
            let orders = response.data.orders;
            if (orders == null) {
                document.getElementById("orderTable").getElementsByTagName('tbody')[0].innerHTML = "";
                document.getElementById("currentPage").innerHTML = 1;
                document.getElementById("lastPage").innerHTML = 1;
                return;
            }
            populateTable(orders);
            document.getElementById("lastPage").innerHTML = response.data.totalPage;
        })

        .catch(error => {
            console.error("fail", error);
        });
}

function firstPage() {
    //設定為第一頁
    document.getElementById("currentPage").innerHTML = 1;
    selectBySomething(1);
}

function previousPage() {
    let oldCurrentPage = parseInt(document.getElementById("currentPage").innerHTML);
    let newCurrentPage = oldCurrentPage;

    if (oldCurrentPage > 1) {
        newCurrentPage = oldCurrentPage - 1;
        document.getElementById("currentPage").innerHTML = newCurrentPage;
    }
    else {
        newCurrentPage = 1;
    }

    selectBySomething(newCurrentPage);
}

function nextPage() {
    let oldCurrentPage = parseInt(document.getElementById("currentPage").innerHTML);
    let maxPage = parseInt(document.getElementById("lastPage").innerHTML);
    let newCurrentPage = oldCurrentPage;

    if (oldCurrentPage < maxPage) {
        newCurrentPage = oldCurrentPage + 1;
        document.getElementById("currentPage").innerHTML = newCurrentPage;
    }
    else {
        newCurrentPage = maxPage;
    }

    selectBySomething(newCurrentPage);
}

function lastPage() {
    let maxPage = parseInt(document.getElementById("lastPage").innerHTML);
    document.getElementById("currentPage").innerHTML = maxPage;
    selectBySomething(maxPage);
}

function selectBySomething(page) {
    if (lastGetOrderNum != null) {
        getOrderByOrderNum(lastGetOrderNum, page);
    }
    else if (lastGetPhone != null) {
        getOrderByPhone(lastGetPhone, page);
    }
    else if (lastGetMemberId != null) {
        getOrderByMemberId(lastGetMemberId, page);
    }
    else if (lastGetDate != null) {
        getOrderByDate(lastGetDate, page);
    }
    else {
        getProduct(page);
    }
}

function getOrderByOrderNum(lastOrderNum, page) {
    //搜尋訂單編號的頁數只會有1頁
    document.getElementById("currentPage").innerHTML = 1;

    lastGetOrderNum = lastOrderNum;
    lastGetPhone = null;
    lastGetMemberId = null
    lastGetDate = null;
    getOrder(page);
}

function getOrderByPhone(lastPhone, page) {
    //第一次搜尋電話號碼
    if (lastGetPhone == null) document.getElementById("currentPage").innerHTML = 1;
    //搜尋不同號碼 重設頁數
    if (lastGetPhone != lastPhone) document.getElementById("currentPage").innerHTML = 1;

    lastGetOrderNum = null;
    lastGetPhone = lastPhone;
    lastGetMemberId = null;
    lastGetDate = null;
    getOrder(page);
}

function getOrderByMemberId(lastMemberId, page) {
    //第一次用memberId搜尋
    if (lastGetMemberId == null) document.getElementById("currentPage").innerHTML = 1;
    //搜尋不同會員 重設頁數
    if (lastGetMemberId != lastMemberId) document.getElementById("currentPage").innerHTML = 1;

    lastGetOrderNum = null;
    lastGetPhone = null;
    lastGetMemberId = lastMemberId;
    lastGetDate = null;
    getOrder(page);
}

function getOrderByDate(lastDate, page) {
    //第一次用日期搜尋
    if (lastGetDate == null) document.getElementById("currentPage").innerHTML = 1;
    //搜尋不同日期 重設頁數
    if (lastGetDate != lastDate) document.getElementById("currentPage").innerHTML = 1;

    lastGetOrderNum = null;
    lastGetPhone = null;
    lastGetMemberId = null;
    lastGetDate = lastDate;
    getOrder(page);
}

function populateTable(orders) {
    const tableBody = document.getElementById("orderTable").getElementsByTagName('tbody')[0];
    tableBody.innerHTML = '';

    orders.forEach((order, index) => {
        const row = document.createElement('tr');
        row.dataset.orderId = order.OrderId;
        const rowHead = document.createElement('th');
        const rowHeadSort = document.createElement('span');
        const rowHeadCkb = document.createElement('input');
        rowHead.scope = "row";
        rowHeadSort.textContent = index + 1;
        rowHeadCkb.type = "checkbox";
        rowHead.appendChild(rowHeadCkb);
        rowHead.appendChild(rowHeadSort);
        row.appendChild(rowHead);

        const cellOrderNumber = document.createElement('td');
        cellOrderNumber.style = " word-wrap: break-word;";
        cellOrderNumber.textContent = order.OrderNumber;
        row.appendChild(cellOrderNumber);

        const cellDate = document.createElement('td');
        var date = formatDateToYYYYMMDDHHMMSS(order.CreateTime)
        cellDate.textContent = date
        row.appendChild(cellDate);

        const cellMemberId = document.createElement('td');
        cellMemberId.textContent = order.MemberId
        row.appendChild(cellMemberId);

        const cellRecipient = document.createElement('td');
        cellRecipient.textContent = order.RecipientName + "(0" + order.Phone + ")";
        row.appendChild(cellRecipient);

        const cellState = document.createElement('td');
        cellState.className = "border border-secondary border-3 pt-3";
        cellState.textContent = orderStateToText(order.State);
        row.appendChild(cellState);

        const cellPrice = document.createElement('td');
        cellPrice.textContent = "$" +order.TotalPrice
        row.appendChild(cellPrice);

        const cellEdit = document.createElement('td');

        const cellEditBtn = document.createElement("button");
        cellEditBtn.className = "btnEditOrder";
        cellEditBtn.addEventListener("click", function () {
            setRedirectPage("MainOrder", `GetRedirectOrderDetailView?orderId=${order.OrderId}`);
        });
        cellEdit.appendChild(cellEditBtn);

        const cellDeleteBtn = document.createElement("button");
        cellDeleteBtn.className = "btnDeleteOrder";
        cellDeleteBtn.addEventListener("click", function () {
        });
        cellEdit.appendChild(cellDeleteBtn);

        row.appendChild(cellEdit);

        // 把這一行加到表格中
        tableBody.appendChild(row);
    });
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
            return "完成";
        case 5:
            return "買家未取商品";
        case 6:
            return "重新寄回";
        case 7:
            return "取消";
        case 8:
            return "退貨";
        default:
            return "";
    }
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
