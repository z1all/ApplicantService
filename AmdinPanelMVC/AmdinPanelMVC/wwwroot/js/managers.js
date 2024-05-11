
execute();

function execute() {
    let deleteButtons = document.querySelectorAll('.delete-user');
    deleteButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            let store = this.parentNode.firstElementChild;
            let managerId = store.getAttribute("user-id");

            updateManagerList();
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
    $('#createManagerButtonFormId').attr("hidden", true);
    $('#changeManagerButtonFormId').removeAttr("hidden");
}

function viewCreateForm() {
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

/*

    document.getElementById("updateButtonId").addEventListener("click", function () {
        fetch('/Admin/DictionaryUpdateStatus')
            .then(response => response.text())
            .then(html => {
                document.getElementById("dictionaryUpdateStatusContainer").innerHTML = html;
            });
    });

    let buttons = document.querySelectorAll('.delete-user');
    buttons.forEach(function (button) {
        button.addEventListener('click', function () {
            let store = this.parentNode.firstElementChild;
            let managerId = store.getAttribute("user-id");
            console.log(managerId);

            fetch('/Admin/ManagerList')
                .then(response => response.text())
                .then(html => {
                    document.getElementById("managersContainerId").innerHTML = html;

                    const scripts = document.getElementById('managersContainerId').getElementsByTagName('script');

                    console.log(scripts)

                    for (let i = 0; i < scripts.length; i++) {
                        const script = scripts[i];
                        if (script.src) {
                            const newScript = document.createElement('script');
                            newScript.src = script.src;
                            document.head.appendChild(newScript);
                        } else {
                            eval(script.innerHTML);
                        }
                    }
                });
        });
    });

*/
