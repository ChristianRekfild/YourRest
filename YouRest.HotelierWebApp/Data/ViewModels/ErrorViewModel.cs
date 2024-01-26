using System.Text;

namespace YouRest.HotelierWebApp.Data.ViewModels
{
    public class ErrorViewModel
    {
        private StringBuilder sb = new();
        public string Title { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; set; }

        public override string ToString()
        {
            foreach (var (key, value) in ValidationErrors)
                sb.AppendLine(string.Join("\r\n", value));
            return sb.ToString();
        }
    }
}
