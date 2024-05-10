
let buttons = document.querySelectorAll('.delete-user');
buttons.forEach(function (button) {
    button.addEventListener('click', function () {
        let store = this.parentNode.firstElementChild;
        let managerId = store.getAttribute("user-id");
        console.log(managerId);
    });
});

