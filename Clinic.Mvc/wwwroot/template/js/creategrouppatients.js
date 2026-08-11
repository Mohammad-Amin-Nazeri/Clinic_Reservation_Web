let index = 1;

// Reorder Index
function reorderIndex() {
    const tbody = document.getElementById('formBody');
    const rows = tbody.querySelectorAll('tr');

    rows.forEach((row, newIndex) => {
        const newNumber = newIndex + 1;

        const numberSpan = row.querySelector('td:first-child span');
        if (numberSpan) {
            numberSpan.textContent = `${newNumber} )`;
        }

        const inputs = row.querySelectorAll('input , select');
        inputs.forEach(input => {
            const nameAttribute = input.getAttribute('name');
            if (nameAttribute) {
                const newName = nameAttribute.replace(/patients\[\d+\]/, `patients[${newIndex}]`);
                input.setAttribute('name', newName);
            }
        });
    });

    index = rows.length;
};

// Add Table Row To Form Table
document.getElementById('addRow').addEventListener('click',
    function () {
        const tbody = document.getElementById('formBody');
        const row = document.createElement('tr')

        row.innerHTML = `
        <td style="width:5%">
                                <span>${index + 1} )</span>
                            </td>
                            <td style="width:30%">
                                <div>
                                    <input class="form-control" name="patients[${index}].FullName" type="text" placeholder="نام و نام خانوادگی">
                                </div>
                            </td>

                            <td style="width:20%">
                                <div>
                                    <input class="form-control" name="patients[${index}].NationalId" type="text" placeholder="کد ملی">
                                </div>
                            </td>

                            <td style="width:15%">
                                <div>
                                    <input class="form-control" name="patients[${index}].Mobile" type="text" placeholder="تماس">
                                </div>
                            </td>

                            <td style="width:10%">
                                <div>
                                    <input class="form-control" name="patients[${index}].Age" type="number" placeholder="سن">
                                </div>
                            </td>

                            <td style="width:15%">
                                <select class="form-select" name="patients[${index}].Gender">
                                    <option value="Male">آقا</option>
                                    <option value="Female">خانم</option>
                                </select>
                            </td>

                            <td style="width:5%">
                                <div>
                                    <button class="btn btn-danger removeRow" type="button">
                                        <i class="far fa-trash-can"></i>
                                    </button>
                                </div>
                            </td>
        `;

        tbody.appendChild(row)
        index++;
    });

// Remove Row From Table
document.getElementById('patientsTable').addEventListener('click',
    function (e) {
        const removeBtn = e.target.closest('.removeRow');
        if (removeBtn) {
            const row = removeBtn.closest('tr');
            if (row) {
                row.remove();
                reorderIndex();
            }
        }
    });

// Validate Inputs
document.getElementById('patientsForm').addEventListener('submit', function (e) {
    const inputs = this.querySelectorAll('input');
    const messageList = document.getElementById('formMessage');
    const alertText = document.getElementById('alertText');

    let isValid = true;

    inputs.forEach(input => {
        if (!input.value.trim()) {
            isValid = false;
            input.style.border = '2px solid red'
        } else {
            input.style.border = '';
        }
    })

    if (!isValid) {
        e.preventDefault();
        messageList.textContent = 'فرم دارای یک مقدار خالی می باشد.';
        alertText.style.display = 'block'
    } else {
        alertText.style.display = 'none'
    }

});

function resetForm() {
    const inputs = document.querySelectorAll('input');
    const alertText = document.getElementById('alertText');

    inputs.forEach(input => {
        input.style.border = '';
    });

    alertText.style.display = 'none';
}