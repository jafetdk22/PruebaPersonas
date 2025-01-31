
namespace WebPracitca.Controllers
{
    internal class ResponseGrid<T>
    {
        public List<PersonaResponse> Data { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public string Draw { get; set; }
    }
}