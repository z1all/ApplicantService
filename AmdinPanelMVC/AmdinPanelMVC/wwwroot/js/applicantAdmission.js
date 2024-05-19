
document.getElementById("addManagerButtonId")?.addEventListener('click', (e) => {
    e.preventDefault();

    const addManager = (data) => {
        if (data.status === 200) {
            window.location.reload();
        }
        else {
            showErrorToast(
                "Добавление менеджера",
                "Ошибка при отправке запроса на добавлении менеджера"
            );
        }
    }

    const managerId = document.getElementById('managerId').value;

    let data = {
        admissionId: getApplicant().applicantAdmission.id,
        managerId: managerId == "" ? null : managerId,
    }

    request('/Admission/AddManager', 'POST', addManager, data);
});

document.getElementById("rejectApplicantButtonId")?.addEventListener('click', (e) => {
    e.preventDefault();

    const rejectApplicant = (data) => {
        if (data.status === 200) {
            window.location.reload();
        }
        else {
            showErrorToast(
                "Отказ от абитуриента",
                "Ошибка при отправке запроса на отказ от абитуриента"
            );
        }
    }

    let data = {
        admissionId: getApplicant().applicantAdmission.id,
    }

    request('/Admission/RejectApplicant', 'POST', rejectApplicant, data);
});

document.getElementById("takeApplicantButtonId")?.addEventListener('click', (e) => {
    e.preventDefault();

    const takeApplicant = (data) => {
        if (data.status === 200) {
            window.location.reload();
        }
        else {
            showErrorToast(
                "Взятие абитуриента",
                "Ошибка при отправке запроса на взятие абитуриента"
            );
        }
    }

    let data = {
        admissionId: getApplicant().applicantAdmission.id,
    }

    request('/Admission/TakeApplicant', 'POST', takeApplicant, data);
});

document.getElementById("changeStatusButtonId")?.addEventListener('click', () => {
    openModal("changeStatusModalId");
});

document.getElementById("changeStatusModalId").addEventListener('submit', (e) => {
    e.preventDefault();

    const changeAdmission = (data) => {
        if (data.status === 200) {
            document.getElementById("statusContainerId").innerHTML = data.body;
        }
        else {
            showErrorToast(
                "Фильтрация поступлений",
                "Ошибка при отправке запроса на фильтрацию поступлений"
            );
        }
    }

    const data = Object.fromEntries(new FormData(e.target));
    data['admissionId'] = getApplicant().applicantAdmission.id;

    request('/Admission/ChangeStatus', 'POST', changeAdmission, data);
});

document.getElementById("applicantBasicDataFormId").addEventListener('submit', (e) => {
    e.preventDefault();

    $('#applicantFullNameId').next().text("");

    const changeBasicInfo = (data) => {
        if (data.status !== 200) {
            showErrorToast(
                "Обновление основных данных",
                "Ошибка при отправке запроса на обновление основных данных"
            );

            if (data.body.errors.FullName) {
                $('#applicantFullNameId').next().text(data.body.errors.FullName[0]);
            }
        }
    }

    const data = Object.fromEntries(new FormData(e.target));
    data['applicantId'] = getApplicant().applicantInfo.applicantProfile.id;

    request('/Admission/ChangeBasicInfo', 'POST', changeBasicInfo, data);
});

document.getElementById("applicantAdditionDataFormId").addEventListener('submit', (e) => {
    e.preventDefault();

    $('#applicantBirthdayId').next().text("");
    $('#applicantCitizenshipId').next().text("");
    $('#applicantPhoneNumberId').next().text("");

    const changeAdditionInfo = (data) => {
        if (data.status !== 200) {
            showErrorToast(
                "Обновление основных данных",
                "Ошибка при отправке запроса на обновление основных данных"
            );

            if (data.body.errors.Birthday) {
                $('#applicantBirthdayId').next().text(data.body.errors.Birthday[0]);
            }

            if (data.body.errors.Citizenship) {
                $('#applicantCitizenshipId').next().text(data.body.errors.Citizenship[0]);
            }

            if (data.body.errors.PhoneNumber) {
                $('#applicantPhoneNumberId').next().text(data.body.errors.PhoneNumber[0]);
            }
        }
    }

    const data = Object.fromEntries(new FormData(e.target));
    data['applicantId'] = getApplicant().applicantInfo.applicantProfile.id;

    request('/Admission/ChangeAdditionInfo', 'POST', changeAdditionInfo, data);
});

document.getElementById('saveOrderButtonId')?.addEventListener('click', (e) => {
    e.preventDefault();

    const changePriority = (data) => {
        if (data.status === 200) {
            updateProgramsOrder(programsOrderCopy);
        }
        else {
            let mess = "Ошибка при отправке запроса на обновление приоритета программ"

            if (data.body.errors.WrongProgramCount) {
                mess = "У абитуриента изменилось число программ, обновите страницу"
            }

            showErrorToast(
                "Удаление программы",
                mess,
            );
        }
    }

    const data = {
        applicantId: getApplicant().applicantInfo.applicantProfile.id,
        newPriorities: {
            newProgramPrioritiesOrder: programsOrderCopy
        }
    };

    request('/Admission/ChangePriorities', 'POST', changePriority, data);
})

let programsOrderCopy = null;
let savedApplicant = null;
const saveApplicant = (applicant) => {
    savedApplicant = applicant;

    programsOrderCopy = getProgramsOrder(savedApplicant);
    setListeners(programsOrderCopy);
};
const getApplicant = () => savedApplicant;

function getProgramsOrder(applicant) {
    let programsOrder = [];

    applicant.applicantAdmission.admissionPrograms.forEach(p => {
        programsOrder.push(p.educationProgram.id);
    });

    return programsOrder;
}

function updateProgramsOrder(programsOrder) {
    programsOrder.forEach((program, index) => {
        $(`#${program}`).find('.priority').text(index + 1);
    });
}

function setListeners(programs) {
    programs.forEach((program) => {
        $(`#${program}`).find('label[for="up"]').on('click', (e) => {
            let prev = $(`#${program}`).prev();
            let index = programs.indexOf(program);

            if (index > 0) {
                $(`#${program}`).insertBefore(prev);
                swapArrayElements(programs, index, index - 1);
            }
        });

        $(`#${program}`).find('label[for="down"]').on('click', (e) => {
            let next = $(`#${program}`).next();
            let index = programs.indexOf(program);

            if (index + 1 < programs.length) {
                $(`#${program}`).insertAfter(next);
                swapArrayElements(programs, index, index + 1);
            }
        });

        $(`#${program}`).find('.delete').on('click', (e) => {
            e.preventDefault();

            const deleteProgram = (data) => {
                if (data.status === 200) {
                    deleteProgramElement(program, programs);
                }
                else {
                    let mess = "Ошибка при отправке запроса на удаление программы"

                    showErrorToast(
                        "Удаление программы",
                        mess,
                    );
                }
            }

            const data = {
                applicantId: getApplicant().applicantInfo.applicantProfile.id,
                programId: program
            };

            request('/Admission/DeleteProgram', 'POST', deleteProgram, data);
        });
    });
}

function deleteProgramElement(program, programs) {
    $(`#${program}`).remove();
    programs.splice(programs.indexOf(program), 1);
    updateProgramsOrder(programs);
}

function getStrIndex(arr, str) {
    arr.indexOf(str);
}

function swapArrayElements(arr, index1, index2) {
    let temp = arr[index1];
    arr[index1] = arr[index2];
    arr[index2] = temp;
}