let StartDate
let EndDate;

Blazor.registerCustomEventType('updateselecteddate', {
    browserEventName: "change",
    createEventArgs: event => {
        return {
            StartDate,
            EndDate
        }
    }
});

export function ShowDataPicker(startDate, endDate) {

    $(document).ready(
        function () {
            moment.locale('ru');
            $('#my-datapicker').daterangepicker({
                "linkedCalendars": true,
                "autoUpdateInput": true,
                "showCustomRangeLabel": false,
                "startDate": moment(startDate, "DD.MM.YYYY"),
                "endDate": moment(endDate, "DD.MM.YYYY"),
                "opens": "center",
                locale: {
                    "applyLabel": "Выбрать",
                    "cancelLabel": "Отменить",
                    "format": "DD.MM.YYYY"
                }
            }, function (start, end, label) {
                StartDate = start.format();
                EndDate = end.format();
                let event = new Event("change", { bubbles: true });
                $('#my-datapicker')[0].dispatchEvent(event);
            });
        }
    );
}