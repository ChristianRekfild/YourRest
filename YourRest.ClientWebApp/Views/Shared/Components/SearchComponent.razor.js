let StartDate
let EndDate;
export function ShowDataPicker() {

    $(document).ready(
        function () {
            moment.locale('ru');
            $('#my-datapicker').daterangepicker({
                "linkedCalendars": true,
                "autoUpdateInput": true,
                "showCustomRangeLabel": false,
                "startDate": moment(Date.now()).format("DD.MM.YYYY"),
                "endDate": moment(Date.now()).add(2, 'days').format("DD.MM.YYYY"),
                "opens": "center",
                locale: {
                    "applyLabel": "Выбрать",
                    "cancelLabel": "Отменить",
                    "format": "DD.MM.YYYY"
                }
            }, function (start, end, label) {
                StartDate = start.format('YYYY-MM-DD');
                EndDate = end.format('YYYY-MM-DD');
            });

        }
    );
}
export function GetData() { return { StartDate, EndDate } };