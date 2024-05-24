document.getElementById("passportFormId")?.addEventListener('submit', (e) => {
    e.preventDefault();

    $('#seriesNumberId').next().text("");
    $('#birthPlaceId').next().text("");
    $('#issueYearId').next().text("");
    $('#issuedByWhomId').next().text("");

    const changePassport = (data) => {
        if (data.status !== 200) {
            let mess = "Ошибка при отправке запроса на обновление паспортных данных";

            if (data.body.errors.NoEditPermission) {
                mess = "Вы не можете редактировать данные студента, так как не являетесь его менеджером"
            }

            showErrorToast(
                "Обновление паспортных данных",
                mess
            );

            if (data.body.errors.SeriesNumber) {
                $('#seriesNumberId').next().text(data.body.errors.SeriesNumber[0]);
            }

            if (data.body.errors.BirthPlace) {
                $('#birthPlaceId').next().text(data.body.errors.BirthPlace[0]);
            }

            if (data.body.errors.IssueYear) {
                $('#issueYearId').next().text(data.body.errors.IssueYear[0]);
            }

            if (data.body.errors.IssuedByWhom) {
                $('#issuedByWhomId').next().text(data.body.errors.IssuedByWhom[0]);
            }
        }
    }

    const data = Object.fromEntries(new FormData(e.target));
    data['applicantId'] = getApplicantId();

    request('/Documents/ChangePassport', 'POST', changePassport, data);
});

document.getElementById("educationDocumentFormId")?.addEventListener('submit', (e) => {
    e.preventDefault();

    $('#nameId').next().text("");
    $('#documentTypeId').next().text("");

    const changeEducationDocument = (data) => {
        if (data.status !== 200) {
            let mess = "Ошибка при отправке запроса на обновление документа об образовании";

            if (data.body.errors.NoEditPermission) {
                mess = "Вы не можете редактировать данные студента, так как не являетесь его менеджером"
            }

            showErrorToast(
                "Обновление документа об образовании",
                mess
            );

            if (data.body.errors.Name) {
                $('#nameId').next().text(data.body.errors.Name[0]);
            }

            if (data.body.errors.EducationDocumentTypeId) {
                $('#documentTypeId').next().text(data.body.errors.EducationDocumentTypeId[0]);
            }

            if (data.body.errors.EducationDocumentAlreadyExist) {
                $('#documentTypeId').next().text(data.body.errors.EducationDocumentAlreadyExist[0]);
            }
        }
    }

    const data = Object.fromEntries(new FormData(e.target));
    data['applicantId'] = getApplicantId();
    data['documentId'] = getDocumentId();
    data['educationDocumentTypeId'] = $("#documentTypeId").val();

    request('/Documents/ChangeEducationDocument', 'POST', changeEducationDocument, data);
});

addListeners();
function addListeners() {
    const loadScanButtons = document.querySelectorAll('.loadScan');
    loadScanButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            const scanId = $(event.target).attr('scanId');

            const loadScan = (data) => {
                if (data.status !== 200) {
                    showErrorToast(
                        "Загрузка скана",
                        "Ошибка при отправке запроса на загрузку скана"
                    );
                }
                else {
                    var file = URL.createObjectURL(data.body);

                    const a = document.createElement('a');
                    a.href = file;
                    a.download = getFileName(data.responseHeaders);
                    document.body.appendChild(a);
                    a.click();

                    document.body.removeChild(a);

                    URL.revokeObjectURL(file);
                }
            }

            request(`/Documents/LoadScans?documentId=${getDocumentId()}&&applicantId=${getApplicantId()}&&scanId=${scanId}`, 'POST', loadScan);
        });
    });

    const deleteButtons = document.querySelectorAll('.deleteScan');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            const scanId = $(event.target).attr('scanId');

            const deleteScan = (data) => {
                if (data.status !== 200) {
                    let = "Ошибка при отправке запроса на удаление скана";

                    if (data.body.errors.NoEditPermission) {
                        mess = "Вы не можете редактировать данные студента, так как не являетесь его менеджером"
                    }

                    showErrorToast(
                        "Удаление скана",
                        mess
                    );
                }
                else {
                    updateScans();
                }
            }

            request(`/Documents/DeleteFile?documentId=${getDocumentId()}&&applicantId=${getApplicantId()}&&scanId=${scanId}`, 'POST', deleteScan);
        });
    });

    document.getElementById('addScanFormId')?.addEventListener('submit', (e) => {
        e.preventDefault();

        $('#formFileId').next().text("");

        const addFile = (data) => {
            if (data.status !== 200) {
                let = "Ошибка при отправке запроса на добавление скана";

                if (data.body.errors.NoEditPermission) {
                    mess = "Вы не можете редактировать данные студента, так как не являетесь его менеджером"
                }

                showErrorToast(
                    "Добавление скана",
                    mess
                );

                if (data.body.errors.File) {
                    $('#formFileId').next().text(data.body.errors.File[0]);
                }
            }
            else {
                updateScans();
            }
        }

        const formdata = new FormData();

        formdata.append("file", e.target.file.files[0]);
        formdata.append("applicantId", getApplicantId());
        formdata.append("documentId", getDocumentId());

        request(`/Documents/AddFile?documentId=${getDocumentId()}&&applicantId=${getApplicantId()}`, 'POST', addFile, formdata, true);
    });
}

function getFileName(reponseHeader) {
    const disposition = reponseHeader.get('Content-Disposition');
    let filename = 'downloaded_file';

    if (disposition && disposition.includes('attachment')) {
        const utf8FilenameRegex = /filename\*=(?:(['"])(.*?)\1|(.+?)(?:;|$))/;
        const asciiFilenameRegex = /filename=(?:(['"])(.*?)\1|([^;\n]*))/;

        let matches = utf8FilenameRegex.exec(disposition);
        if (matches) {
            filename = decodeURIComponent(matches[2] || matches[3]);
        } else {
            matches = asciiFilenameRegex.exec(disposition);
            if (matches) {
                filename = matches[2] || matches[3];
            }
        }

        filename = filename.replace("UTF-8''", '')
    }

    return filename;
}

function updateScans() {
    const getScans = (data) => {
        if (data.status !== 200) {
            showErrorToast(
                "Обновление сканов",
                "Ошибка при отправке запроса на обновление сканов"
            );
        }
        else {
            $('#scansContainerId').empty();
            $('#scansContainerId').append(data.body);
            addListeners();
        }
    }

    request(`/Documents/Scans?documentId=${getDocumentId()}&&applicantId=${getApplicantId()}`, 'GET', getScans);
}

let applicantId = null;
const setApplicantId = (id) => applicantId = id;
const getApplicantId = () => applicantId;

let documentId = null;
const setDocumentId = (id) => documentId = id;
const getDocumentId = () => documentId;