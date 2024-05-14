
function showSuccessToast(title, text) {
    $('#toastTitleId').parent().addClass("bg-success");
    showToast(title, text);
}

function showErrorToast(title, text) {
    $('#toastTitleId').parent().addClass("bg-danger");
    showToast(title, text);
}

function showToast(title, text) {
    $('#toastTitleId').text(title);
    $('#toastTextId').text(text);

    var toast = new bootstrap.Toast($('#toastId'));
    toast.show();
}

function request(url, method, callback, data = null, userToken = null) {
    let request = {
        method,
        headers: {
            'accept': 'text/plain'
        }
    }

    if (userToken !== null) {
        request.headers['Authorization'] = "Bearer " + userToken;
    }

    if (data !== null) {
        request.headers['content-Type'] = "application/json";
        request.body = JSON.stringify(data)
    }

    let status;
    fetch(url, request)
        .then(response => {
            status = response.status;

            if (response.redirected) {
                window.location.href = response.url;
            }
            else {
                const contentType = response.headers.get('content-type');
                if (contentType && (contentType.includes('application/json') || contentType.includes('application/problem+json'))) {
                    return response.json();
                }
                else if (contentType && contentType.includes('text/html')) {
                    return response.text();
                }
                else {
                    return null;
                }
            }
        })
        .then(data => {
            callback({ body: data, status });
        })
        .catch(error => {
            console.error('Произошла ошибка:', error);
        });
}