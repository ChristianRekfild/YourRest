let StartDate
let EndDate;
export function ShowDataPicker(startDate, endDate) {

    $(document).ready(
        function () {
            moment.locale('ru');
            $('#my-datapicker').daterangepicker({
                "linkedCalendars": true,
                "autoUpdateInput": true,
                "showCustomRangeLabel": false,
                //"startDate": moment(Date.now()).format("DD.MM.YYYY"),
                //"endDate": moment(Date.now()).add(2, 'days').format("DD.MM.YYYY"),
                "startDate": moment(startDate, "DD.MM.yyyy"),
                "endDate": moment(endDate, "DD.MM.yyyy"),
                "opens": "center",
                locale: {
                    "applyLabel": "Выбрать",
                    "cancelLabel": "Отменить",
                    "format": "DD.MM.YYYY"
                }
            }, function (start, end, label) {
                StartDate = start.format('YYYY-MM-DD');
                EndDate = end.format('YYYY-MM-DD');
                $('#my-datapicker').val(StartDate);
            });
        }
    );
}
export let openForm = () => {
    let bntPosition = $('.auldt_btn').position();
    $('.form-popup').css('top', `${bntPosition.top + 55}px`);
    $('#myForm').show();
}
export let closeForm = () => $('#myForm').hide();

export function GetData() { return { StartDate, EndDate } };