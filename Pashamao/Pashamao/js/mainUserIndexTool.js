let lastSortBtnColumn = "UserId";
let lastSortOrder = "Ascending";
let lastSelectTxbId = "";
let lastSelectColumn = "";
let lastSelectValue = "";

getAll();

function btnGetAllUser(){
    document.getElementById("currentPage").innerHTML = 1;
    getAll();
}

function getAll() {
    let btns = document.querySelectorAll(`.btnSort`);
    btns.forEach(btn => {
        btn.style.backgroundImage = "url(../images/arrowAscending.png)";
    });
    lastSelectTxbId = "";
    lastSelectColumn = "";
    lastSelectValue = "";
    getUser("UserId", 1, "Ascending");
}

function firstPage() {
    document.getElementById("currentPage").innerHTML = 1;

    if (lastSelectTxbId == "" || lastSelectColumn == "" || lastSelectValue == "") {
        getUser(lastSortBtnColumn, 1, lastSortOrder);
    }
    else {
        selectUser(lastSelectTxbId, lastSelectColumn, lastSelectValue, lastSortBtnColumn, 1, lastSortOrder);
    }
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

    if (lastSelectColumn === "" | lastSelectValue === "" | lastSelectTxbId === "") {
        getUser(lastSortBtnColumn, newCurrentPage, lastSortOrder);
    }
    else {
        selectUser(lastSelectTxbId, lastSelectColumn, lastSelectValue, lastSortBtnColumn, newCurrentPage, lastSortOrder);
    }
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

    if (lastSelectColumn === "" || lastSelectValue === "" || lastSelectTxbId === "") {
        getUser(lastSortBtnColumn, newCurrentPage, lastSortOrder);
    }
    else {
        selectUser(lastSelectTxbId, lastSelectColumn, lastSelectValue, lastSortBtnColumn, newCurrentPage, lastSortOrder);
    }
}

function lastPage() {
    let maxPage = parseInt(document.getElementById("lastPage").innerHTML);
    document.getElementById("currentPage").innerHTML = maxPage;

    if (lastSelectColumn === "" | lastSelectValue === "" | lastSelectTxbId === "") {
        getUser(lastSortBtnColumn, maxPage, lastSortOrder);
    }
    else {
        selectUser(lastSelectTxbId, lastSelectColumn, lastSelectValue, lastSortBtnColumn, maxPage, lastSortOrder);
    }
}

function getUser(column, page, sortOrder) {
    lastSortBtnColumn = column;
    lastSortOrder = sortOrder;

    let sortedUserDto = {
        SortColumn: column,
        Page: page,
        SortOrder: sortOrder
    };

    axios.post("/MainUser/GetSortedUser", { sortedUserDto })
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
                document.getElementById("currentPage").innerHTML = 1;
                document.getElementById("lastPage").innerHTML = 1;
                document.getElementById("userTable").getElementsByTagName('tbody')[0].innerHTML = "";
                return;
            }

            //成功的話
            if (response.data.users == null) {
                document.getElementById("currentPage").innerHTML = 1;
                document.getElementById("lastPage").innerHTML = 1;
                document.getElementById("userTable").getElementsByTagName('tbody')[0].innerHTML = "";
                return;
            }

            populateTable(response.data.users);
            document.getElementById("lastPage").innerHTML = response.data.totalPage;
        })
        .catch(error => {
            console.error("fail", error);
        });
}

function getSortedUser(btnId, column) {
    let btn = document.querySelector(`#${btnId}`);
    let currentBackground = getComputedStyle(btn).backgroundImage;
    let sortOrder;
    let currentPage = document.getElementById("currentPage").innerHTML;
    lastSortBtnColumn = column;

    if (currentBackground.includes('arrowAscending.png')) {
        sortOrder = 'Descending';
        btn.style.backgroundImage = "url('../images/arrowDescending.png')";
    }
    else {
        sortOrder = 'Ascending';
        btn.style.backgroundImage = "url(../images/arrowAscending.png)";
    }
    lastSortOrder = sortOrder;

    if (lastSelectColumn === "" | lastSelectValue === "" | lastSelectTxbId === "") {
        getUser(column, currentPage, sortOrder);
    }
    else {
        selectUser(lastSelectTxbId, lastSelectColumn, lastSelectValue, column, currentPage, sortOrder);
    }
}

function populateTable(users) {
    const tableBody = document.getElementById("userTable").getElementsByTagName('tbody')[0];
    tableBody.innerHTML = '';

    users.forEach((user, index) => {
        const row = document.createElement('tr');
        const rowSort = document.createElement('th');
        rowSort.textContent = index + 1;
        rowSort.scope = "row";
        row.appendChild(rowSort);

        const cellId = document.createElement('td');
        cellId.textContent = user.UserId;

        row.appendChild(cellId);

        const cellAcct = document.createElement('td');
        cellAcct.textContent = user.Account;
        row.appendChild(cellAcct);

        const cellName = document.createElement('td');
        cellName.textContent = user.Name;
        row.appendChild(cellName);

        const cellStatus = document.createElement('td');
        cellStatus.textContent = user.Status ? "啟用" : "禁用";
        row.appendChild(cellStatus);

        const cellRoleId = document.createElement('td');
        cellRoleId.textContent = user.RoleId;
        row.appendChild(cellRoleId);

        const cellEdit = document.createElement('td');

        const cellEditBtn = document.createElement("button");
        cellEditBtn.className = "btnEditUser";
        cellEditBtn.addEventListener("click", function () {
            editUserRoleAndStatus(user.UserId, user.RoleId, user.Status);
        });
        cellEdit.appendChild(cellEditBtn);

        const cellDeleteBtn = document.createElement("button");
        cellDeleteBtn.className = "btnDeleteUser";
        cellDeleteBtn.addEventListener("click", function () {
            delUser(user.UserId);
        });
        cellEdit.appendChild(cellDeleteBtn);

        if (user.RoleId != "1") {
            row.appendChild(cellEdit);
        }

        // 把這一行加到表格中
        tableBody.appendChild(row);
    });
}

function delUser(userId) {
    Swal.fire({
        title: '確定要刪除這個項目嗎？',
        text: "這個操作無法恢復！",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: '刪除',
        cancelButtonText: '取消'
    }).then((result) => {
        let userIdDto = {
            UserId: userId
        };

        if (result.isConfirmed) {
            axios.post("/MainUser/DeleteUser", { userIdDto })
                .then(response => {
                    let errorCode = response.data.errorCode;

                    //沒有成功
                    if (errorCode != errorCodeDefine.Success) {
                        let message = errorCodeToMessage(errorCode);

                        //顯示訊息並處理
                        Swal.fire(message)
                            .then(result => {
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
                    window.location.href = '/MainUser/Index';
                })
                .catch(error => { console.error(error); });
        }
    });
}

function selectUser(txbSelectElementId, selectColumn, value, sortColumn, page, sortOrder) {
    var searchInput = document.getElementById(txbSelectElementId);
    var selectText = searchInput.value;
    var regex = new RegExp(searchInput.pattern);

    //設定最後搜尋欄位
    lastSelectTxbId = txbSelectElementId;
    lastSelectColumn = selectColumn;

    //如果說是第一次搜尋(分成找上次搜尋的資料或者新的搜尋)
    if (value === "unknown") {
        lastSelectValue = selectText;
        value = selectText;
    }

    if (!regex.test(selectText)) {
        document.getElementById("searchWarning").innerText = "請輸入有效格式";
        return;
    }

    document.getElementById("searchWarning").innerText = " ";
    let selectUserModel = {
        SelectColumn: selectColumn,
        Value: value,
        SortColumn: sortColumn,
        Page: page,
        SortOrder: sortOrder
    };
    axios.post("/MainUser/SelectUser", { selectUserDto: selectUserModel })
        .then(response => {
            let errorCode = response.data.errorCode;

            //沒有成功
            if (errorCode != errorCodeDefine.Success) {
                let message = errorCodeToMessage(errorCode);

                Swal.fire(message)
                    .then(result => {
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

                document.getElementById("currentPage").innerHTML = 1;
                document.getElementById("lastPage").innerHTML = 1;
                document.getElementById("userTable").getElementsByTagName('tbody')[0].innerHTML = "";
                return;
            }


            //成功的話
            if (response.data.users == null) {
                document.getElementById("currentPage").innerHTML = 1;
                document.getElementById("lastPage").innerHTML = 1;
                document.getElementById("userTable").getElementsByTagName('tbody')[0].innerHTML = "";
                return;
            }

            populateTable(response.data.users);
            document.getElementById("currentPage").innerHTML = 1;
            document.getElementById("lastPage").innerHTML = response.data.totalPage;
        })
        .catch(error => {
            console.error("fail", error);
        });
}

function editUserRoleAndStatus(userId, userRoleId, userStatus) {
    let roleSelectOptions;
    let statusSelectOptions;

    if (userStatus) {
        statusSelectOptions = `<option value="true" selected>啟用</option>` + `<option value="false" >禁用</option>`;
    } else {
        statusSelectOptions = `<option value="true" >啟用</option>` + `<option value="false" selected>禁用</option>`;
    }

    axios.post("/MainUser/GetRoleIdAndName")
        .then(response => {
            let errorCode = response.data.errorCode;

            //沒有成功
            if (errorCode != errorCodeDefine.Success) {
                let message = errorCodeToMessage(errorCode);

                //顯示訊息並處理
                Swal.fire(message)
                    .then(result => {
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
            roleSelectOptions = populateRoleOptionData(response.data.roles, userRoleId);
            Swal.fire({
                title: '修改使用者',
                html: `
    <label for="roleSelect">選擇角色:</label>
    <select id="roleSelect" class="swal-selectRole">
        ${roleSelectOptions}
    </select>
    <br><br>
        <label for="statusSelect">選擇狀態:</label>
        <select id="statusSelect" class="swal-selectStatus">
            ${statusSelectOptions}
        </select>`
                ,
                icon: 'question',
                background: '#f0f0f0',
                showCancelButton: true,
                confirmButtonText: '確定',
                cancelButtonText: '取消',
                preConfirm: () => {
                    const roleSelectValue = document.getElementById('roleSelect').value;
                    const statusSelectValue = document.getElementById('statusSelect').value;

                    const editUserRoleAndStatus = {
                        UserId: userId,
                        RoleId: roleSelectValue,
                        Status: statusSelectValue
                    };

                    return { editUserRoleAndStatus };
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    axios.post("/MainUser/EditUserRoleAndStatus", {
                        editUserRoleAndStatus: result.value.editUserRoleAndStatus
                    })
                        .then(response => {
                            let errorCode = response.data.errorCode;

                            //沒有成功
                            if (errorCode != errorCodeDefine.Success) {
                                let message = errorCodeToMessage(errorCode);

                                //顯示訊息並處理
                                Swal.fire(message)
                                    .then(result => {
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
                            window.location.href = '/MainUser/Index';
                        })
                        .catch(error => {
                            console.error("fail", error);
                        });
                }
            });
        })
        .catch(error => {
            console.error("fail", error);
        });
}

function populateRoleOptionData(roles, roleId) {
    let selectOptions = '';
    roles.forEach(role => {
        if (role.RoleId != "1") {
            if (role.RoleId === roleId) {
                selectOptions += `<option value="${role.RoleId}" selected>角色名：${role.Name}</option>`;//－權限描述：${role.Description}
            } else {
                selectOptions += `<option value="${role.RoleId}">角色名：${role.Name}</option>`;
            }
        }
    });
    return selectOptions;
}
