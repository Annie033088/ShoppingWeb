let lastGetOrderNum = null;
let lastGetPhone = null;
let lastGetDate = null;
let lastGetStatus = null;

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

    document.getElementById("btnSelectByDate").addEventListener('click', function () {
        const selectDateStart = document.getElementById("txbDateStart").value;
        const selectDateEnd = document.getElementById("txbDateEnd").value;

        const selectDate = {
            selectDateStart,
            selectDateEnd
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
    });
});

function getAll() {
    lastGetOrderNum = null;
    lastGetPhone = null;
    lastGetDate = null;
    lastGetStatus = null;
    document.getElementById("selectStatus").value = 0;
    document.getElementById("currentPage").innerHTML = 1;
    getOrder(1);
}

function getOrder(page) {
    let selectOrderDto = {
        OrderNumber: lastGetOrderNum,
        Phone: lastGetPhone,
        Date: lastGetDate,
        Status: lastGetStatus
    };

    console.log(selectOrderDto);

    /*axios.post("/MainOrder/GetOrder", { selectOrderDto })
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
                document.getElementById("productTable").getElementsByTagName('tbody')[0].innerHTML = "";
                document.getElementById("currentPage").innerHTML = 1;
                document.getElementById("lastPage").innerHTML = 1;
            }
            populateTable(orders);
            document.getElementById("lastPage").innerHTML = response.data.totalPage;
        })

        .catch(error => {
            console.error("fail", error);
        });*/
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
    if (lastCategoryId != null) {
        getProductByCategory(lastCategoryId, page);
    }
    else if (lastGetProductId != null) {
        getProductById(lastGetProductId, page);
    }
    else if (lastGetProductName != null) {
        getProductByName(lastGetProductName, page);
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
    lastGetDate = lastDate;
    getOrder(page);
}