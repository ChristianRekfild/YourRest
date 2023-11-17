using Microsoft.AspNetCore.Components;

namespace YourRest.ClientWebApp.Views.Shared.Components
{
    public partial class NumericComponent
    {
        [Parameter]
        public string Title { get; set; }
        [Parameter]
        public int Value { get; set; }
        [Parameter]
        public int Min { get; set; } = int.MinValue;
        [Parameter]
        public int Max { get; set; } = int.MaxValue;
        [Parameter]
        public EventCallback<int> ValueChanged { get; set; }
        public async Task Decrease()
        {
            if (Value > Min)
                Value--;
            await ValueChanged.InvokeAsync(Value);
        }

        public async Task Increase()
        {
            if (Value >= Min && Value < Max)
                Value++;
           await ValueChanged.InvokeAsync(Value);
        }
        
    }

}
