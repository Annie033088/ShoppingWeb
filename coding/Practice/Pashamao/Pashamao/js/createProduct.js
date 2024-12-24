
let currentImageIndex = 0;
let showNavImageCnt = 4;
let imageTotal = 0;
function getImageAndEdit() {
    let imgHtml = "";
    let addImageList = [];
    let delOldImageList = []
    let images = document.querySelectorAll(".displayImage");
    let imagesHtmlStrings = [];
    let imageCount = 0;



    if (images.length >0) {
        images.forEach((image, index) => {
            let html = `
                    <div id="" class="imageContainer me-2 col-6" style=" position: relative; ">
                        <img id="${image.id}" src="${image.src}" class="img-fluid" />
                        <p class="imageName">${image.id}</p>
                        <button class = "btn btnDeleteImage" style=""  data-id="${image.id}">
                    </div>`
            imagesHtmlStrings.push(html);
            imageCount++;
        });
    }


    Swal.fire({
        title: '修改圖片',
        width: '80%',
        html: `
                <p class="text-danger">最多放10張圖片, 每張圖片最大1mb</p>
                <div id= "addImageBox">
                    <div id= "imagePreviews" style = "display: flex; align-items: flex-start; justify-content: space-between; position: relative; ">
                           ${imagesHtmlStrings}
                    </div>
                </div>
                `
        ,
        background: '#f0f0f0',
        showCancelButton: true,
        confirmButtonText: '確定',
        cancelButtonText: '取消',
        heightAuto: true,
        didOpen: () => {
            var addImageBox = document.getElementById('addImageBox');
            var imgUploadBox = document.createElement('div');
            var inputElement = document.createElement('input');
            var btnAddImage = document.createElement('button');
            imgUploadBox.id = "imgUploadBox";
            inputElement.type = "file";
            inputElement.id = "txbImage";
            inputElement.accept = "image/*";
            inputElement.style.display = "none";
            btnAddImage.id = "btnUploadImage";
            btnAddImage.className = "btn btnAdd"

            imgUploadBox.appendChild(btnAddImage);
            imgUploadBox.appendChild(inputElement);
            addImageBox.appendChild(imgUploadBox);


            const imagePreviews = document.getElementById('imagePreviews');
            const txbImage = document.getElementById('txbImage');
            const btnUploadImage = document.getElementById('btnUploadImage');
            btnUploadImage.addEventListener('click', () => {
                txbImage.click();
            });

            txbImage.addEventListener('change', (e) => {
                if (imageCount > 9) {
                    alert("超過十張");
                    return;
                }

                const file = e.target.files[0];

                if (file) {

                    if (file.name.length > 20) {
                        alert("圖片名過長");
                        return;
                    }

                    const maxSize = 1024 * 1024;//最大1mb

                    if (file.size > maxSize) {
                        alert("圖片檔案過大");
                        return;
                    } 
                }

                const reader = new FileReader();
                reader.onload = function (event) {
                    const imgNewBox = document.createElement('div');
                    const imgElement = document.createElement('img');
                    let imgNameElement = document.createElement('p');
                    const btnDeleteImage = document.createElement('button');
                    const timestamp = new Date().getTime();
                    imgNameElement.innerHTML = `${file.name}`;
                    imgElement.id = `${timestamp}-${file.name}`;
                    imgNewBox.className = "imageContainer me-2 col-6";
                    imgNewBox.style.position = "relative";
                    btnDeleteImage.className = "btn btnDeleteImage";

                    imgElement.src = event.target.result;
                    imgElement.className = "img-fluid newAddImages";

                    btnDeleteImage.addEventListener('click', (event) => {
                        const parentDiv = event.target.parentElement;

                        const index = addImageList.indexOf(imgElement.id);

                        if (index > -1) {
                            addImageList.splice(index, 1);
                        }

                        imageCount--;
                        parentDiv.remove();
                    });

                    imgNewBox.appendChild(imgElement);
                    imgNewBox.appendChild(imgNameElement);
                    imgNewBox.appendChild(btnDeleteImage);
                    imagePreviews.appendChild(imgNewBox);
                    addImageList.push(imgElement.id);
                    imageCount++;
                };
                reader.readAsDataURL(file);
            });

            const buttons = document.querySelectorAll('.btnDeleteImage')
            buttons.forEach(button => {
                button.addEventListener('click', (event) => {
                    const btn = event.target;
                    delOldImageList.push(btn.dataset.id)
                    const parentDiv = btn.parentElement;

                    imageCount--;
                    parentDiv.remove();
                });
            });

        },
        preConfirm: () => {
            return { delOldImageList: delOldImageList, addImageList: addImageList };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            if (result.value.addImageList.length > 0) {
                result.value.addImageList.forEach(imgId => {
                    const image = document.getElementById(imgId);
                    let displayImageContainer = document.getElementById("displayImageContainer");
                    let ProductImageBox = document.createElement("div");
                    ProductImageBox.className = "displayImageBox";
                    let ProductImage = document.createElement("img");
                    ProductImage.id = imgId;
                    ProductImage.className = "img-fluid displayImage";
                    ProductImage.src = image.src;
                    ProductImageBox.appendChild(ProductImage);
                    displayImageContainer.appendChild(ProductImageBox);
                    imageTotal++;
                });
                updateImageDisplay();
            }

            if (result.value.delOldImageList.length > 0) {
                result.value.delOldImageList.forEach(imgId => {
                    const image = document.getElementById(imgId);
                    const parentDiv = image.parentElement;
                    parentDiv.remove();
                    imageTotal--;
                })
            }
        }
    });
}

function imagePrevious() {
    if (currentImageIndex > 0) {
        currentImageIndex--;
        updateImageDisplay();
    }
}

function imageNext() {
    if (currentImageIndex < imageTotal - showNavImageCnt) {
        currentImageIndex++;
        updateImageDisplay();
    }
}

function updateImageDisplay() {

    let displayImages = document.querySelectorAll('.displayImage');
    const prevBtn = document.getElementById('btnPrevious');
    const nextBtn = document.getElementById('btnNext');

    displayImages.forEach((img, index) => {
        if (index < showNavImageCnt + currentImageIndex && index >= currentImageIndex) {
            img.style.display = 'block';
        } else {
            img.style.display = 'none';
        }
    });

    if (currentImageIndex <= 0) {
        prevBtn.style.opacity = "0";
        prevBtn.disabled = true;
    } else {
        prevBtn.style.opacity = "0.8";
        prevBtn.disabled = false;
    }

    if (currentImageIndex >= imageTotal - showNavImageCnt) {
        nextBtn.style.opacity = "0";
        nextBtn.disabled = true;
    } else {
        nextBtn.style.opacity = "0.8";
        nextBtn.disabled = false;
    }
}

function addStyle() {
    let addImage = false;
    let htmlStatus = `
                        <div id="addStyleStatusBox" class="input-group mt-3">
                            <span id="txtStatus" class="input-group-text">上/下架</span>
                            <button id="btnStatusOn" class="btn btn-outline-dark opacity-50">上架</button>
                            <button id="btnStatusOff" class="opacity-100 btn btn-dark" >下架</button>
                        </div>`

    Swal.fire({
        title: '新增細項',
        html: `
       <div class="addStyleBox">
            <div id="addStyleImageBox" class="position-relative" style="border:solid">
                <img id="addStyleImage" class="img-fluid styleImage" src="/images/productImage/noImage.jpg" alt="" data-id="noImage.jpg">
                <input type="file" id="txbAddImage" accept="image/*" style="display: none;">
                <p>點擊圖片修改</p>
            </div>
            <div id="addStyleNameBox" class="input-group mt-3">
                <span class="input-group-text">分類名</span>
                <input id="txbAddStyleName" type="text" class="form-control" value="">
            </div>

            <div id="addStylePriceBox" class="input-group mt-3">
                <span class="input-group-text">價格</span>
                <input id="txbAddStylePrice" type="text" class="form-control" value="">
            </div>

            <div id="addStyleQuantityBox" class="input-group mt-3">
                <span class="input-group-text">數量</span>
                <input id="txbAddStyleQuantity" type="text" class="form-control" value="">
            </div>
            ${htmlStatus}
        </div>
        `,
        showCancelButton: true,
        confirmButtonText: '確定',
        cancelButtonText: '取消',
        didOpen: () => {
            let addStyleImage = document.getElementById("addStyleImage");
            let txbAddImage = document.getElementById("txbAddImage");
            let btnStatusOn = document.getElementById("btnStatusOn");
            let btnStatusOff = document.getElementById("btnStatusOff");
            addStyleImage.addEventListener('click', () => { txbAddImage.click(); });

            txbAddImage.addEventListener('change', (e) => {
                const file = e.target.files[0];
                const reader = new FileReader();
                reader.onload = function (event) {
                    addStyleImage.src = event.target.result;
                    addImage = true;
                };
                const timestamp = new Date().getTime();
                addStyleImage.dataset.id = `${timestamp}-${file.name}`;
                reader.readAsDataURL(file);
            });

            btnStatusOn.addEventListener("click", function () {
                btnStatusOn.className = "opacity-100 btn btn-dark";
                btnStatusOff.className = "btn btn-outline-dark opacity-50";
            })
            btnStatusOff.addEventListener("click", function () {
                btnStatusOn.className = "btn btn-outline-dark opacity-50";
                btnStatusOff.className = "opacity-100 btn btn-dark";
            })

        },
        preConfirm: () => {
            let styleStatus = "";
            let styleName = document.getElementById("txbAddStyleName").value;
            let stylePrice = document.getElementById("txbAddStylePrice").value;
            let styleQuantity = document.getElementById("txbAddStyleQuantity").value;
            let btnStatusOn = document.getElementById("btnStatusOn").className;
            if (btnStatusOn == "opacity-100 btn btn-dark") { styleStatus = true; }
            else { styleStatus = false; }

            let addStyle = {
                Style: styleName,
                Price: stylePrice,
                StockQuantity: styleQuantity,
                Status: styleStatus,
            }

            let imageName = document.getElementById("addStyleImage").dataset.id;

            //驗證輸入符合訊息
            const nameRegex = /^.+$/;
            if (!nameRegex.test(styleName)) {
                Swal.showValidationMessage('請輸入項目名');
                return false;
            }

            const priceRegex = /^[0-9]+(\.[0-9]{1,2})?$/;
            if (!priceRegex.test(stylePrice)) {
                Swal.showValidationMessage('請輸入有效的價格（EX：123、123.45）');
                return false;
            }

            const QuantityRegex = /^\d+$/;
            if (!QuantityRegex.test(styleQuantity)) {
                Swal.showValidationMessage('請輸入有效的數量');
                return false;
            }

            return { addStyle: addStyle, imageName: imageName };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            const imgFile = document.getElementById("addStyleImage");
            addStyleRow(result.value.addStyle, imgFile.src, result.value.imageName);
        }
    });
}

function editStyle(styleData, imageSrc, oldImageName, id) {
    let addImage = false;
    let htmlStatus = "";
    if (styleData.Status == true) {
        htmlStatus = `
                      <div id="addStyleStatusBox" class="input-group mt-3">
                          <span id="txtStatus" class="input-group-text">上/下架</span>
                          <button id="btnStatusOn" class="opacity-100 btn btn-dark">上架</button>
                          <button id="btnStatusOff" class="btn btn-outline-dark opacity-50" >下架</button>
                      </div>`
    }
    else {
        htmlStatus = `
                      <div id="addStyleStatusBox" class="input-group mt-3">
                          <span id="txtStatus" class="input-group-text">上/下架</span>
                          <button id="btnStatusOn" class="btn btn-outline-dark opacity-50">上架</button>
                          <button id="btnStatusOff" class="opacity-100 btn btn-dark" >下架</button>
                      </div>`
    }

    Swal.fire({
        title: '新增細項',
        html: `
       <div class="addStyleBox">
            <div id="addStyleImageBox" class="position-relative" style="border:solid">
                <img id="addStyleImage" class="img-fluid styleImage" src="${imageSrc}" alt="" data-id="${oldImageName}">
                <input type="file" id="txbAddImage" accept="image/*" style="display: none;">
                <p>點擊圖片修改</p>
            </div>
            <div id="addStyleNameBox" class="input-group mt-3">
                <span class="input-group-text">分類名</span>
                <input id="txbAddStyleName" type="text" class="form-control" value="${styleData.Style}">
            </div>

            <div id="addStylePriceBox" class="input-group mt-3">
                <span class="input-group-text">價格</span>
                <input id="txbAddStylePrice" type="text" class="form-control" value="${styleData.Price}">
            </div>

            <div id="addStyleQuantityBox" class="input-group mt-3">
                <span class="input-group-text">數量</span>
                <input id="txbAddStyleQuantity" type="text" class="form-control" value="${styleData.StockQuantity}">
            </div>
            ${htmlStatus}
        </div>
        `,
        showCancelButton: true,
        confirmButtonText: '確定',
        cancelButtonText: '取消',
        didOpen: () => {
            let addStyleImage = document.getElementById("addStyleImage");
            let txbAddImage = document.getElementById("txbAddImage");
            let btnStatusOn = document.getElementById("btnStatusOn");
            let btnStatusOff = document.getElementById("btnStatusOff");
            addStyleImage.addEventListener('click', () => { txbAddImage.click(); });

            txbAddImage.addEventListener('change', (e) => {
                const file = e.target.files[0];
                const reader = new FileReader();
                reader.onload = function (event) {
                    addStyleImage.src = event.target.result;
                    addImage = true;
                };
                const timestamp = new Date().getTime();
                addStyleImage.dataset.id = `${timestamp}-${file.name}`;
                reader.readAsDataURL(file);
            });

            btnStatusOn.addEventListener("click", function () {
                btnStatusOn.className = "opacity-100 btn btn-dark";
                btnStatusOff.className = "btn btn-outline-dark opacity-50";
            })
            btnStatusOff.addEventListener("click", function () {
                btnStatusOn.className = "btn btn-outline-dark opacity-50";
                btnStatusOff.className = "opacity-100 btn btn-dark";
            })

        },
        preConfirm: () => {
            let styleStatus = "";
            let styleName = document.getElementById("txbAddStyleName").value;
            let stylePrice = document.getElementById("txbAddStylePrice").value;
            let styleQuantity = document.getElementById("txbAddStyleQuantity").value;
            let btnStatusOn = document.getElementById("btnStatusOn").className;
            let imageName = document.getElementById("addStyleImage").dataset.id;

            if (btnStatusOn == "opacity-100 btn btn-dark") { styleStatus = true; }
            else { styleStatus = false; }

            if (styleName == styleData.Style && stylePrice == styleData.Price && styleQuantity == styleData.StockQuantity && styleStatus == styleData.Status && !addImage) {
                Swal.showValidationMessage('請修改資料');
                return false;
            }

            let editStyle = {
                Style: styleName,
                Price: stylePrice,
                StockQuantity: styleQuantity,
                Status: styleStatus,
            }


            //驗證輸入符合訊息
            const nameRegex = /^.+$/;
            if (!nameRegex.test(styleName)) {
                Swal.showValidationMessage('請輸入項目名');
                return false;
            }

            const priceRegex = /^[0-9]+(\.[0-9]{1,2})?$/;
            if (!priceRegex.test(stylePrice)) {
                Swal.showValidationMessage('請輸入有效的價格（EX：123、123.45）');
                return false;
            }

            const QuantityRegex = /^\d+$/;
            if (!QuantityRegex.test(styleQuantity)) {
                Swal.showValidationMessage('請輸入有效的數量');
                return false;
            }

            return { editStyle: editStyle, imageName: imageName };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            const imgFile = document.getElementById("addStyleImage");
            editStyleRow(result.value.editStyle, imgFile.src, result.value.imageName, id);
        }
    });

}

function addStyleRow(styleData, imageSrc, imageName) {
    const tableBody = document.getElementById("styleTable").getElementsByTagName('tbody')[0];
    let rowCount = tableBody.querySelectorAll("tr").length

    const row = document.createElement('tr');
    row.setAttribute('data-id', rowCount + 1);
    row.id = `rowStyle${rowCount + 1}`;
    const cellStyleName = document.createElement('td');
    cellStyleName.textContent = styleData.Style;
    cellStyleName.className = "styleName";
    row.appendChild(cellStyleName);

    const cellStyleImage = document.createElement('td');
    let styleImage = document.createElement('img');
    styleImage.setAttribute('data-name', imageName);
    styleImage.src = imageSrc;
    styleImage.style = " height:100%";
    styleImage.className = "me-2 sryleImage";
    cellStyleImage.appendChild(styleImage);
    row.appendChild(cellStyleImage);

    const cellStockQuantity = document.createElement('td');
    cellStockQuantity.textContent = styleData.StockQuantity;
    cellStockQuantity.className = "styleQuantity";
    row.appendChild(cellStockQuantity);

    const cellStylePrice = document.createElement('td');
    cellStylePrice.textContent = styleData.Price;
    cellStylePrice.className = "stylePrice";
    row.appendChild(cellStylePrice);

    const cellStatus = document.createElement('td');
    cellStatus.textContent = styleData.Status == true ? "上架" : "下架";
    cellStatus.className = "styleStatus";
    row.appendChild(cellStatus);

    const cellEditData = document.createElement('td');

    const cellEditDataBtn = document.createElement("button");
    cellEditDataBtn.className = "btnEditStyleData btnEdit";
    cellEditDataBtn.addEventListener("click", function () {

        const nameInRow = row.querySelector(".styleName");
        const imageInRow = row.querySelector("img");
        const quantityInRow = row.querySelector(".styleQuantity");
        const priceInRow = row.querySelector(".stylePrice");
        const statusInRow = row.querySelector(".styleStatus");
        let status = true;

        if (statusInRow.textContent == "上架") {
            status = true;
        } else {
            status = false;
        }

        newStyleData = {
            Style: nameInRow.textContent,
            Price: priceInRow.textContent,
            StockQuantity: quantityInRow.textContent,
            Status: status,
        }
        newImageSrc = imageInRow.src;
        newImageName = imageInRow.dataset.id;
        editStyle(newStyleData, newImageSrc, newImageName, row.id);
    });
    const cellDelDataBtn = document.createElement("button");
    cellDelDataBtn.className = "btnDelStyleData btnDel";
    cellDelDataBtn.addEventListener("click", function () {
        delStyle(row.id);
    });
    cellEditData.appendChild(cellEditDataBtn);
    cellEditData.appendChild(cellDelDataBtn);
    row.appendChild(cellEditData);
    // 把這一行加到表格中
    tableBody.appendChild(row);
}

function editStyleRow(styleData, imageSrc, imageName, id) {
    const row = document.getElementById(id);
    const nameInRow = row.querySelector(".styleName");
    const imageInRow = row.querySelector("img");
    const quantityInRow = row.querySelector(".styleQuantity");
    const priceInRow = row.querySelector(".stylePrice");
    const statusInRow = row.querySelector(".styleStatus");
    nameInRow.textContent = styleData.Style;
    imageInRow.dataset.name = imageName;
    imageInRow.src = imageSrc;
    quantityInRow.textContent = styleData.StockQuantity;
    priceInRow.textContent = styleData.Price;
    statusInRow.textContent = styleData.Status == true ? "上架" : "下架";

    Swal.fire("修改成功!");
}

function delStyle(id) {
    Swal.fire({
        title: '確定要刪除這個項目嗎？',
        text: "這個操作無法恢復！",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: '刪除',
        cancelButtonText: '取消'
    }).then((result) => {
        if (result.isConfirmed) {
            const row = document.getElementById(id);
            row.remove();
        }
    })
}

function setProductStatusOn() {
    document.getElementById("btnProductStatusOn").className = "opacity-100 btn btn-dark";
    document.getElementById("btnProductStatusOff").className = "btn btn-outline-dark opacity-50";
}

function setProductStatusOff() {
    document.getElementById("btnProductStatusOn").className = "btn btn-outline-dark opacity-50";
    document.getElementById("btnProductStatusOff").className = "opacity-100 btn btn-dark";
}

function submitCreatProduct() {
    const displayImageContainer = document.getElementById("displayImageContainer");
    const imageElements = displayImageContainer.querySelectorAll("img");
    let imageList = [];

    for (var i = 0; i < imageElements.length; i++) {
        let image = {
            ImageName: imageElements[i].id,
            ImageUrl: imageElements[i].src
        }
        imageList.push(image);
    }

    let productStatus = false;

    if (document.getElementById("btnProductStatusOn").className == "opacity-100 btn btn-dark") {
        productStatus = true;
    }

    let product = {
        Name: document.getElementById("txbName").value,
        Description: document.getElementById("txbDescription").value,
        CategoryId: document.getElementById("dropdownCategory").value,
        Introduction: document.getElementById("productIntroduce").value,
        Status: productStatus,
    }

    const tableBody = document.querySelector("tbody");
    const styleRows = tableBody.querySelectorAll("tr");
    let styleList = [];

    styleRows.forEach(row=> {
        const nameInRow = row.querySelector(".styleName");
        const imageInRow = row.querySelector("img");
        const quantityInRow = row.querySelector(".styleQuantity");
        const priceInRow = row.querySelector(".stylePrice");
        const statusInRow = row.querySelector(".styleStatus");

        let style = {
            Style: nameInRow,
            Price: priceInRow,
            StockQuantity: quantityInRow,
            Status: statusInRow,
            ImageUrl: imageInRow.src,
            ImageName: imageInRow.dataset.name
        }
        styleList.push(style);
    })

    axios.post("/MainProduct/SubmitCreateProduct", {
        ProductDetail: product,
        StyleList: styleList,
        ImageList: imageList
    } )
        .then(response => {
            window.location.href = `/MainProduct/Index`
        })
        .catch(error => {
            console.error("fail", error);
        }) 
}
