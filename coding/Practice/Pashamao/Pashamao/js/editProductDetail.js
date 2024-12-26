
let product = jsonData.Item1;
let styles = jsonData.Item2;
let images = jsonData.Item3;

let currentImageIndex = 0;
let showNavImageCnt = 4;
let imageTotal = 0;
//刪除的圖片
let delImageList = [];

//刪除的細項
let delStyleList = [];

//舊的細項id跟圖片連結
let oldStyleImageUrl = [];

console.log(product, styles, images);

populateImage();
populateProduct();
populateStyle();

function populateImage() {
    images.forEach(image => {
        let imageUrl = image.ImageUrl;
        let lastIndex = imageUrl.lastIndexOf("\\");
        let imageName = imageUrl.substring(lastIndex + 1);

        let displayImageContainer = document.getElementById("displayImageContainer");
        let ProductImageBox = document.createElement("div");
        ProductImageBox.className = "displayImageBox";
        let ProductImage = document.createElement("img");
        ProductImage.id = imageName;
        ProductImage.setAttribute("data-id", image.ProductImageId);
        ProductImage.className = "img-fluid displayImage";
        ProductImage.src = imageUrl;
        ProductImageBox.appendChild(ProductImage);
        displayImageContainer.appendChild(ProductImageBox);
        imageTotal++
    })
    updateImageDisplay();
}

function populateProduct() {
    let productStatus = product.Status;
    document.getElementById("txbName").value = product.Name;
    document.getElementById("txbDescription").value = product.Description;
    document.getElementById("dropdownCategory").value = product.CategoryId
    document.getElementById("productIntroduce").value = product.Introduction;
    if (productStatus == true) {
        document.getElementById("btnProductStatusOn").className = "opacity-100 btn btn-dark";
        document.getElementById("btnProductStatusOff").className = "btn btn-outline-dark opacity-50";
    } else {
        document.getElementById("btnProductStatusOn").className = "btn btn-outline-dark opacity-50";
        document.getElementById("btnProductStatusOff").className = "opacity-100 btn btn-dark";
    }
}

function populateStyle() {
    styles.forEach(style => {
        let imageSrc = "";
        let imageName = "";

        if (style.ImageUrl == " ") {
            imageSrc = "/images/productImage/noImage.jpg";
            const timestamp = new Date().getTime();
            imageName = ` `;
        } else {
            let imageUrl = style.ImageUrl;
            let lastIndex = imageUrl.lastIndexOf("\\");
            imageName = imageUrl.substring(lastIndex + 1);
            imageSrc = style.ImageUrl;
        }

        addStyleRow(style, imageSrc, imageName);

        const tableBody = document.getElementById("styleTable").getElementsByTagName('tbody')[0];
        const imgElements = tableBody.querySelectorAll("img");

        oldStyleImage = {
            ProductStyleId: style.ProductStyleId,
            ImageUrl: imgElements[imgElements.length - 1].src
        }
        oldStyleImageUrl.push(oldStyleImage);
    })
}

function getImageAndEdit() {
    let imgHtml = "";
    let addImageList = [];
    let delOldImageList = []
    let imagesElement = document.querySelectorAll(".displayImage");
    let imagesHtmlStrings = [];
    let imageCount = 0;

    if (imagesElement.length > 0) {
        imagesElement.forEach((image, index) => {
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
                    delOldImageList.push(btn.dataset.id);
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
                    ProductImage.setAttribute("data-id", " ");
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
                    var machingImage = images.filter(img => img.ProductImageId == image.dataset.id);
                    if (machingImage.length >0) {
                        var delImage = {
                            ProductImageId: machingImage[0].ProductImageId,
                            ImageUrl: machingImage[0].ImageUrl
                        }
                        delImageList.push(delImage);
                    }
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
                <img id="addStyleImage" class="img-fluid styleImage" src="/images/productImage/noImage.jpg" alt="" data-name=" ">
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

                if (!file) {
                    return;
                }

                const timestamp = new Date().getTime();
                addStyleImage.dataset.name = `${timestamp}-${file.name}`;

                reader.onload = function (event) {
                    addStyleImage.src = event.target.result;
                    addImage = true;
                };
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

            let imageName = document.getElementById("addStyleImage").dataset.name;

            //驗證輸入符合訊息
            const nameRegex = /^.{1,25}$/;
            if (!nameRegex.test(styleName)) {
                Swal.showValidationMessage('請輸入項目名');
                return false;
            }

            const priceRegex = /^[0-9]{1,9}(\.[0-9]{1,2})?$/;
            if (!priceRegex.test(stylePrice)) {
                Swal.showValidationMessage('請輸入有效的價格（EX：123、123.45）');
                return false;
            }

            const QuantityRegex = /^\d{1,9}$/;
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
                <img id="addStyleImage" class="img-fluid styleImage" src="${imageSrc}" alt="" data-name="${oldImageName}">
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

                if (file) {
                    const timestamp = new Date().getTime();
                    addStyleImage.dataset.name = `${timestamp}-${file.name}`;
                }

                reader.onload = function (event) {
                    addStyleImage.src = event.target.result;
                    addImage = true;
                };
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
            let imageName = document.getElementById("addStyleImage").dataset.name;

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
            const nameRegex = /^.{1,25}$/;
            if (!nameRegex.test(styleName)) {
                Swal.showValidationMessage('請輸入項目名');
                return false;
            }

            const priceRegex = /^[0-9]{1,9}(\.[0-9]{1,2})?$/;
            if (!priceRegex.test(stylePrice)) {
                Swal.showValidationMessage('請輸入有效的價格（EX：123、123.45）');
                return false;
            }

            const QuantityRegex = /^\d{1,9}$/;
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

    if (styleData.ProductStyleId) {
        row.setAttribute('data-id', styleData.ProductStyleId);
    } else {
        row.setAttribute('data-id', ' ');
    }

    const timestamp = new Date().getTime();
    row.id = `rowStyle${timestamp}${styleData.ProductStyleId}`;
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
        newImageName = imageInRow.dataset.name;
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
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: '刪除',
        cancelButtonText: '取消'
    }).then((result) => {
        if (result.isConfirmed) {
            const row = document.getElementById(id);
            var matchStyle = styles.filter(style => style.ProductStyleId == row.dataset.id);
            style = {
                ProductStyleId: row.dataset.id,
                ImageUrl: matchStyle[0].ImageUrl
            }
            delStyleList.push(style);
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

function submitEditProduct() {
    const displayImageContainer = document.getElementById("displayImageContainer");
    const imageElements = displayImageContainer.querySelectorAll("img");
    let imageList = [];

    if (imageElements.length != 0) {
        for (var i = 0; i < imageElements.length; i++) {
            if (imageElements[i].dataset.id == " ") {
                let image = {
                    ImageName: imageElements[i].id,
                    ImageUrl: imageElements[i].src
                }
                imageList.push(image);
            }
        }
    }

    let productStatus = false;
    let productName = document.getElementById("txbName").value;
    let productDescription = document.getElementById("txbDescription").value;
    let productCategory = document.getElementById("dropdownCategory").value;
    let productIntroduce = document.getElementById("productIntroduce").value;
    const regexName = /^[^\s].{0,29}$/;
    const regexDescription = /^[\s\S]{0,40}$/;
    const regexIntroduction = /^[\s\S]{0,1500}$/;
    if (!regexName.test(productName)) {
        Swal.fire("請輸入30字以內商品名(第一個字不得為空)");
        return;
    }

    if (!regexDescription.test(productDescription)) {
        Swal.fire("請輸入40字以內的描述內容");
        return;
    }


    if (!regexIntroduction.test(productIntroduce)) {
        Swal.fire("請輸入1500字以內的介紹");
        return;
    }

    if (productDescription == "") productDescription = " ";
    if (productIntroduce == "") productIntroduce = " ";

    if (document.getElementById("btnProductStatusOn").className == "opacity-100 btn btn-dark") {
        productStatus = true;
    }

    let newProduct = {
        ProductId: product.ProductId,
        Name: productName,
        Description: productDescription,
        CategoryId: productCategory,
        Introduction: productIntroduce,
        Status: productStatus,
    }

    const tableBody = document.querySelector("tbody");
    const styleRows = tableBody.querySelectorAll("tr");
    let addStyleList = [];
    let editStyleList = [];

    if (styleRows.length < 1) {
        Swal.fire("請至少新增一個細項");
        return;
    }

    styleRows.forEach(row => {
        const nameInRow = row.querySelector(".styleName").textContent;
        const imageInRow = row.querySelector("img");
        const quantityInRow = row.querySelector(".styleQuantity").textContent;
        const priceInRow = row.querySelector(".stylePrice").textContent;
        const statusInRow = row.querySelector(".styleStatus").textContent;

        //styleId = 0 代表是新增的
        let styleId = 0;
        let status = false;
        let imageSrc = " ";
        let oldImageSrc = " ";

        if (statusInRow == "上架") {
            status = true;
        } else {
            status = false;
        }

        if (imageInRow.dataset.name == " " || null) {
            imageSrc = " ";
        } else {
            imageSrc = imageInRow.src;
        }

        //判斷是不是舊的細項
        styles.forEach(oldStyle => {
            //是
            if (`${oldStyle.ProductStyleId}` == row.dataset.id) {
                let matchingStylesImage = oldStyleImageUrl.filter(oldStyleImage => oldStyleImage.ProductStyleId == oldStyle.ProductStyleId);
                let oldImageUrl = matchingStylesImage[0].ImageUrl;

                //有沒有修改過!
                if (nameInRow == oldStyle.Style && imageInRow.src == oldImageUrl && quantityInRow == `${oldStyle.StockQuantity}`
                    && priceInRow == `${oldStyle.Price}` && status == oldStyle.Status) {
                    //沒有修改過
                    styleId = -1;
                } else {
                    //沒有修改過
                    styleId = oldStyle.ProductStyleId;
                    oldImageSrc = oldStyle.ImageUrl;
                }
            }
        })

        //代表這行細項資料沒有異動過
        if (styleId == -1)
        {
            return;
        }//代表是舊的style被修改
        else if (styleId != 0)
        {
            let style = {
                ProductStyleId: styleId,
                Style: nameInRow,
                Price: priceInRow,
                StockQuantity: quantityInRow,
                Status: status,
                NewImageUrl: imageSrc,
                OldImageUrl: oldImageSrc,
                ImageName: imageInRow.dataset.name
            }
            editStyleList.push(style);
        }//代表新增style
        else
        {

            let style = {
                ProductStyleId: styleId,
                Style: nameInRow,
                Price: priceInRow,
                StockQuantity: quantityInRow,
                Status: status,
                ImageUrl: imageSrc,
                ImageName: imageInRow.dataset.name
            }
            addStyleList.push(style);
        }
    })

    //產品沒有修改
    if (newProduct.Name == product.Name && newProduct.CategoryId == product.CategoryId && newProduct.Description == product.Description
        && newProduct.Introduction == product.Introduction && newProduct.Status == product.Status) {
        if (addStyleList.length == 0 && editStyleList.length == 0 && imageList.length == 0 && delImageList.length == 0 && delStyleList.length == 0) {
            Swal.fire("請修改商品內容");
            return;
        }
    }

    console.log(newProduct, addStyleList, editStyleList, delStyleList, imageList, delImageList);

    //styleList 包括修改的style以及新增的style(由後端去處理)
    /* axios.post("/MainProduct/SubmitEditProduct", {
         ProductDetail: newProduct,
         AddStyleList: addStyleList,
         EditStyleList: editStyleList,
         DelStyleList: delStyleList,
         ImageList: imageList,
         DelImageList:delImageList
     })
         .then(response => {
             if (response.data == true) {
                 Swal.fire("新增成功!")
                     .then((result) => {
                         if (result.isConfirmed) {
                             window.location.href = `/MainProduct/Index`;
                         }
                     })
             } else {
                 Swal.fire("新增失敗!")
                     .then((result) => {
                         if (result.isConfirmed) {
                             window.location.href = `/MainProduct/ProductDetail?ProductId=${product.ProductId}`;
                         }
                     })
             }
         })
         .catch(error => {
             console.error("fail", error);
         })*/
}

function submitEditImage() {
    const displayImageContainer = document.getElementById("displayImageContainer");
    const imageElements = displayImageContainer.querySelectorAll("img");
    let imageList = [];

    if (imageElements.length != 0) {
        for (var i = 0; i < imageElements.length; i++) {
            if (imageElements[i].dataset.id == " ") {
                let image = {
                    ImageName: imageElements[i].id,
                    ImageUrl: imageElements[i].src
                }
                imageList.push(image);
            }
        }
    }

    const formData = new FormData();

    formData.append('ProductId', product.ProductId);
    formData.append('ProductName', product.Name);
    formData.append('DelImageList', JSON.stringify(delImageList));

    imageList.forEach(img => {
        const imgBlob = dataURItoBlob(img.ImageUrl);
        formData.append('images[]', imgBlob, `${img.ImageName}`);
        console.log(imgBlob)
    });

   /* axios.post("/Product/EditProductImage", formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    })
        .then(response => {
            window.location.href = `/Product/GetProductDetail?ProductId=${product.ProductId}`
        })
        .catch(error => {
            console.error("fail", error);
        })*/
}


function dataURItoBlob(dataURI) {
    var byteString = atob(dataURI.split(',')[1]);
    var arrayBuffer = new ArrayBuffer(byteString.length);
    var uintArray = new Uint8Array(arrayBuffer);

    for (var i = 0; i < byteString.length; i++) {
        uintArray[i] = byteString.charCodeAt(i);
    }

    return new Blob([uintArray], { type: 'image/jpeg' });
}