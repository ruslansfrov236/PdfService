const pdfView = document.querySelector("#pdfView");
const fileUpload = document.querySelector("#fileUpload");

fileUpload.addEventListener("change", (event) => {
    var file = event.target.files[0];
    if (file && file.type== "application/pdf"){
    const fileUrl = URL.createObjectURL(file);
    pdfView.innerHTML = `<embed src="${fileUrl}" width='100%' height='600px' />`
}else {
    pdfView.innerHTML=`<p>Xahiş olunur ki pdf file seçin!</p>`
}
});