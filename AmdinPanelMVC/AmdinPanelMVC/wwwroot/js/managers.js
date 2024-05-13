
execute();

function execute() {
    let deleteButtons = document.querySelectorAll('.delete-user');
    deleteButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            let store = this.parentNode.firstElementChild;
            let managerId = store.getAttribute("user-id");

            const deleteManager = (data) => {
                if (data.status === 200) {
                    showSuccessToast(
                        "Удаление менджера",
                        "Менеджер успешно удален"
                    );

                    updateManagerList();
                }
                else {
                    let message = "Ошибка при удалении менеджера"
                    if (data.body.errors.BadRoles) {
                        message = "Вы не можете удалить менеджера с ролью администратора"
                    }

                    showErrorToast(
                        "Удаление менджера",
                        message
                    );
                }
            }

            const manager = {
                managerId
            }

            request("/Admin/DeleteManager", "DELETE", deleteManager, manager);            
        });
    });

    let editButtons = document.querySelectorAll('.edit-user');
    editButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            let store = this.parentNode.firstElementChild;

            var form = document.getElementById("managerFormId");
            form.elements["Id"].value = store.getAttribute("user-id");
            form.elements["FullName"].value = store.getAttribute("user-name");
            form.elements["Email"].value = store.getAttribute("user-email");
            form.elements["FacultyId"].value = store.getAttribute("user-faculty-id") === null
                ? "" : store.getAttribute("user-faculty-id");
            form.elements["Password"].value = "1234";

            cleanErrors();
            viewChangeForm(store);
        });
    });

    let createButton = document.getElementById("createManagerButtonId");
    createButton.addEventListener('click', function () {
        var form = document.getElementById("managerFormId");
        form.elements["Id"].value = null;
        form.elements["FullName"].value = null;
        form.elements["Email"].value = null;
        form.elements["FacultyId"].value = "";
        form.elements["Password"].value = null;
        
        cleanErrors();
        viewCreateForm()
    });
}

function cleanErrors() {
    $('#fullNameId').next().text("");
    $('#emailId').next().text("");
    $('#passwordId').next().text("");
    $('#facultyId').next().text("");
}

function viewChangeForm(store) {
    $('#passwordContainerId').attr("hidden", true);
    $('#passwordId').attr("type", "text");
    $('#createManagerButtonFormId').attr("hidden", true);
    $('#changeManagerButtonFormId').removeAttr("hidden");
}

function viewCreateForm() {
    $('#passwordId').attr("type", "password");
    $('#passwordContainerId').removeAttr("hidden");
    $('#createManagerButtonFormId').removeAttr("hidden");
    $('#changeManagerButtonFormId').attr("hidden", true);
}

function updateManagerList() {
    fetch('/Admin/ManagerList')
        .then(response => response.text())
        .then(html => {
            document.getElementById("managersContainerId").innerHTML = html;

            execute();
        });
}
