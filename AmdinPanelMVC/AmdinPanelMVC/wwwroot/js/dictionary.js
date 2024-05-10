
document.getElementById("updateButtonId").addEventListener("click", function () {
    fetch('/Admin/DictionaryUpdateStatus')
        .then(response => response.text())
        .then(html => {
            document.getElementById("dictionaryUpdateStatusContainer").innerHTML = html;
        });
});

document.getElementById("updateDictionaryButtonId").addEventListener("click", function (e) {
    e.preventDefault();
    
    const form = document.getElementById('fromDictionaryTypeId');

    let params = {
        method: 'POST',
        headers: {
            'accept': 'text/plain',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(Object.fromEntries(new FormData(form)))
    }

    fetch('/Admin/UpdateDictionary', params)
        .then(response => {
            if (response.redirected) {
                window.location.href = response.url;
            }
            else {
                return response.text();
            }
        })
        .then();
});