namespace StatusCheck.Models
{
    public class RequestResults
    {
        public DateTime RequestedAt { get; set; } = DateTime.Now;   // время запроса
        public string Name { get; set; } = string.Empty;            // тип (запрос в веб, к БД и т.п.)
        public string Target { get; set; } = string.Empty;          // цель
        public bool IsSuccessful { get; set; }                      // статус проверки
        public long ResponseTime { get; set; }                      // длительность ответа
        public string Message { get; set; } = string.Empty;         // сообщение
    }
}
