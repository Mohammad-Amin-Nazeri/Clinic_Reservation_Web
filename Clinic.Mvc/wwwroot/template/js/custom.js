var Swal = require("sweetalert2");

// Pagination
function SubmitPageId(pageId){
    $("#PageId").val(pageId);
    $("#DataForm").submit();
}