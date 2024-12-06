/*function createRole() {
    axios.post("/UserRole/GetPermissionsView")
        .then(response => {
            Swal.fire({
                title: '角色權限設定',
                html: response.data, 
                showCancelButton: true,
                confirmButtonText: '確認',
                preConfirm: () => {
                    let selectedCkbs, roleName, roleDiscript = getFilledRoleInfo();
                    axios.post("/UserRole/AddRole", {
                        selectedCkbs: selectedCkbs, roleName: roleName, roleDiscript: roleDiscript
                    })
                        .then(response => {
                            window.location.href = "/MainUser/CreateUser";
                        })
                    return true; 
                }
            });
        })
}*/



function getFilledRoleInfo() {
    const roleName = document.getElementById("txbRoleName").value;
    const roleDiscript = document.getElementById("txbRoleDiscript").value;
    const mainCkbs = document.querySelectorAll('input[type="checkbox"][id^="ckbMain"]');
    const subCkbs = document.querySelectorAll('.subCkb');
    const selectedCkbs = [];

    mainCkbs.forEach(function (mainCkb) {
        mainCkb.addEventListener("change", function () {
            const parentId = mainCkb.id;
            const subCkbsForMain = document.querySelectorAll(`.subCkb[data-Parent="${parentId}"]`);
            subCkbsForMain.forEach(function (subCkb) {
                subCkb.checked = mainCkb.checked;
            });
        });
    });

    subCkbs.forEach(function (subCkb) {
        if (subCkb.checked) {
            selectedCkbs.push(subCkb.value);
        }
    });
    return selectedCkbs, roleName, roleDiscript
}