
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

    const updateDictionary = (data) => {
        if (data.status !== 200) {
            showErrorToast(
                "Обновление справочников",
                "Ошибка при отправке запроса на обновление справочника(-ов)"
            );
        }
    }

    request('/Admin/UpdateDictionary', 'POST', updateDictionary, Object.fromEntries(new FormData(form)))

    //let params = {
    //    method: 'POST',
    //    headers: {
    //        'accept': 'text/plain',
    //        'Content-Type': 'application/json'
    //    },
    //    body: JSON.stringify(Object.fromEntries(new FormData(form)))
    //}

    //fetch('/Admin/UpdateDictionary', params)
    //    .then(response => {
    //        if (response.redirected) {
    //            window.location.href = response.url;
    //        }
    //        else {
    //            return response.text();
    //        }
    //    })
    //    .then();
});