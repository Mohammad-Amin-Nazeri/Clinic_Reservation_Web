/*
 *  Document   : be_comp_dialogs.js
 *  Author     : pixelcave
 *  Description: Custom JS code used in Dialogs Page
 */

// SweetAlert2, for more examples you can check out https://github.com/sweetalert2/sweetalert2
class pageDialogs {
    /*
     * SweetAlert2 demo functionality
     *
     */
    static sweetAlert2() {
        // Set default properties
        let toast = Swal.mixin({
            buttonsStyling: false,
            target: '#page-container',
            customClass: {
                confirmButton: 'btn btn-primary m-1',
                cancelButton: 'btn btn-danger m-1',
                input: 'form-control'
            }
        });

        // Init a simple dialog on button click
        let swalSimple = document.querySelector('.js-swal-simple');

        if (swalSimple) {
            swalSimple.addEventListener('click', e => {
                toast.fire({
                    title: 'سلام، این فقط یک پیام ساده است!',
                    confirmButtonText: 'باشه'
                });
            });
        }

        // Init an success dialog on button click
        let swalSuccess = document.querySelector('.js-swal-success');

        if (swalSuccess) {
            swalSuccess.addEventListener('click', e => {
                toast.fire(
                    {
                        icon: "success",
                        title: "موفقیت آمیز",
                        text: 'همه چیز کاملا به روز شد!',
                        confirmButtonText: 'باشه'
                    }
                );
            });
        }

        // Init an info dialog on button click
        let swalInfo = document.querySelector('.js-swal-info');

        if (swalInfo) {
            swalInfo.addEventListener('click', e => {
                toast.fire(
                    {
                        icon: "info",
                        title: "اطلاع رسانی",
                        text: 'فقط یک پیام اطلاع رسانی!',
                        confirmButtonText: 'باشه'
                    }
                );
            });
        }

        // Init an warning dialog on button click
        let swalWarning = document.querySelector('.js-swal-warning');

        if (swalWarning) {
            swalWarning.addEventListener('click', e => {
                toast.fire(
                    {
                        icon: "warning",
                        title: "هشدار",
                        text: 'چیزی به توجه شما نیاز دارد!',
                        confirmButtonText: 'باشه'
                    }
                );
            });
        }

        // Init an error dialog on button click
        let swalError = document.querySelector('.js-swal-error');

        if (swalError) {
            swalError.addEventListener('click', e => {
                toast.fire(
                    {
                        icon: "error",
                        title: "اوه نه...",
                        text: 'مشکلی پیش آمد!',
                        confirmButtonText: 'باشه'
                    }
                );
            });
        }

        // Init an question dialog on button click
        let swalQuestion = document.querySelector('.js-swal-question');

        if (swalQuestion) {
            swalQuestion.addEventListener('click', e => {
                toast.fire(
                    {
                        icon: "question",
                        title: "سوال",
                        text: 'آیا مطمئن هستید؟',
                        confirmButtonText: 'باشه'
                    }
                );
            });
        }

        // Init an example confirm dialog on button click
        let swalConfirm = document.querySelector('.js-swal-confirm');

        if (swalConfirm) {
            swalConfirm.addEventListener('click', e => {
                toast.fire({
                    title: 'آیا مطمئن هستید؟',
                    text: 'شما نمی توانید این فایل خیالی را بازیابی کنید!',
                    icon: 'warning',
                    showCancelButton: true,
                    customClass: {
                        confirmButton: 'btn btn-danger m-1',
                        cancelButton: 'btn btn-secondary m-1'
                    },
                    confirmButtonText: 'بله حذفش کن!',
                    cancelButtonText: 'لغو',
                    html: false,
                    preConfirm: e => {
                        return new Promise(resolve => {
                            setTimeout(() => {
                                resolve();
                            }, 50);
                        });
                    }
                }).then(result => {
                    if (result.value) {
                        toast.fire(
                            {
                                icon: "success",
                                title: "حذف شد!",
                                text: 'فایل خیالی شما حذف شده است.',
                                confirmButtonText: 'باشه'
                            }
                        );
                        // result.dismiss can be 'overlay', 'cancel', 'close', 'esc', 'timer'
                    } else if (result.dismiss === 'cancel') {
                        toast.fire(
                            {
                                icon: "error",
                                title: "لغو شد",
                                text: 'فایل خیالی شما امن است :)',
                                confirmButtonText: 'باشه'
                            }
                        );
                    }
                });
            });
        }

        // Init an example confirm alert on button click
        let swalCustom = document.querySelector('.js-swal-custom-position');

        if (swalCustom) {
            swalCustom.addEventListener('click', e => {
                toast.fire({
                    position: 'top-end',
                    title: 'عالی!',
                    text: 'موقعیت خوب!',
                    icon: 'success',
                    confirmButtonText: 'باشه'
                });
            });
        }
    }

    /*
     * Init functionality
     *
     */
    static init() {
        this.sweetAlert2();
    }
}

// Initialize when page loads
Dashmix.onLoad(() => pageDialogs.init());
