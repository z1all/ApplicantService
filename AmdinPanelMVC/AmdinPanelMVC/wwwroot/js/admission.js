
document.getElementById("admissionFormId").addEventListener('submit', (e) => {
    e.preventDefault();

    updateAdmissionList();
});

updateAdmissionList()

function updateAdmissionList(page = 1) {
    const filterAdmission = (data) => {
        if (data.status !== 200) {
            showErrorToast(
                "Фильтрация поступлений",
                "Ошибка при отправке запроса на фильтрацию поступлений"
            );
        }

        $('#admissionsContainerId').empty();
        $('#admissionsContainerId').append(data.body);
    }

    const form = document.getElementById("admissionFormId")
    const data = Object.fromEntries(new FormData(form));

    data['Page'] = page;
    data['Size'] = $('#count_posts_on_page_id').val();

    data['FacultiesId'] = $('#facultiesId').val();

    setNull('FacultiesId', data);
    setNull('AdmissionStatus', data);
    setNull('ApplicantFullName', data);
    setNull('CodeOrNameProgram', data);

    request('/Admission/GetAdmission', 'POST', filterAdmission, data)
}

document.getElementById('count_posts_on_page_id').addEventListener('change', () => {
    updateAdmissionList();
});

function setNull(field, data) {
    if (data[field] === "" || data[field].length === 0) {
        data[field] = null;
    }
}

function setPageSize(size) {
    $('#count_posts_on_page_id').val(size);
}

function creatPageButtons(pagination) {
    $('#page_container_id').empty();
    let numeration = buildNumerationPage(pagination);
    for (let i = numeration.rightPage; i >= numeration.leftPage; i--) {
        let listItem = $('<li>', {
            'class': 'page-item reset' + (i === pagination.current ? ' active' : ''),
            html: $('<a>', {
                'class': 'page-link',
                text: i
            })
        });

        listItem.on('click', function (event) {
            event.preventDefault();

            updateAdmissionList(i);
        });

        $('#page_container_id').prepend(listItem);
    }
}

function buildNumerationPage(pagination, sides = 2) {
    let rightPage = Math.min(pagination.current + sides, pagination.count);
    let leftPage = Math.max(pagination.current - sides, 1);

    if (rightPage - leftPage < 2 * sides && pagination.count > 2 * sides) {
        if (pagination.current + sides > rightPage) {
            leftPage -= pagination.current + sides - rightPage;
        }
        else if (pagination.current - sides < leftPage) {
            rightPage += leftPage - pagination.current + sides;
        }
    }
    else if (pagination.count <= 2 * sides) {
        leftPage = 1;
        rightPage = pagination.count;
    }

    return { leftPage, rightPage };
}