namespace StatusCheck.Models
{
    public class RequestResults
    {
        public DateTime RequestedAt = DateTime.Now; // время запроса
        public string Name = string.Empty;          // тип (запрос в веб, к БД и т.п.)
        public string Address = string.Empty;       // адрес
        public bool IsSuccessful;                   // статус проверки
        public long ResponseTime;                   // длительность ответа
    }
}
