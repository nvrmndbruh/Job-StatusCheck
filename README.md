# STATUS CHECK by Knyazev Anton
Простое консольное приложение, которое предназначено для проверки доступности различных сайтов/баз данных MS SQL Server.
Утилита написана на C# версии .NET 9

***

## Функционал
- проверка доступности сайтов
- проверка доступности базы данных MS SQL Server
- сохранение подробных результатов проверки в формате JSONL (JSON Lines)
- возможность настройки отдельных проверок и самого приложения через файл `appconfig.json`
- возможность добавления собственных проверок через интерфейс `IStatusCheck`
- автоматическое определение новых проверок

## Настройка
В файле `appconfig.json` можно настроить базовую цель для каждой проверки, а также название и расположение файла вывода
```json
{
  "RequestsSettings": {
    "web": {
      "DefaultTarget": "baseurl.com"
    },
    "mssql": {
      "DefaultTarget": "connectionString"
    }
  },
  "OutputFilePath": "check_results.json"
}
```

## Запуск

### Стандартный сценарий
Для запуска приложение достаточно:
1. в консоли перейти в нужную директорию (`../StatusCheck/bin/debug/net9.0/`).
2. запустить приложение через `StatusCheck.exe`

В данном случае приложение просто запустит все доступные проверки с настройками из `appconfig.json`

### Запуск конкретной проверки
Для запуска одной определенной проверки нужно указать необходимый тег при запуске, например:
```bash
StatusCheck.exe web
```
В ссылка будет взята из `appconfig.json`. Доступны базовые проверки `web` и `mssql`
В качестве альтернативного подхода можно изменить ссылку по-умолчанию для проверки в файле конфигурации

### Указание собственной ссылки
Для этого необхомо после тега проверки указать адрес, например:
```bash
StatusCheck.exe web ya.ru
```

***

## Добавление собственной проверки
Для создания собственной проверки необходимо реализовать интерфейс `IStatusCheck`
```cs
public interface IStatusCheck
{
    public string Name { get; }
    public Task<RequestResults> CheckAsync(string target, CancellationToken cancellationToken = default);
}
```
Помимо этого, для того, чтобы проверка могла быть обнаружена, ей необходимо указать аттрибуты `RequestAttribute`
Пример:
```cs
[Request(
    name:"web",
    argument:"url")]
internal class WebRequest : IStatusCheck
{
    // ...
>
```

***

## Пример вывода в консоль
```bash
--[ STATUS CHECK by Knyazev Anton ]--

Available checks:
    mssql (MsSqlRequest), аргумент: connection string
    web (WebRequest), аргумент: url

Running "web"...

Web Request (soundcloud.com)
--> OK, MESSAGE: Website is accessible. Status: 200

All results saved to C:\Users\..\StatusCheck\bin\Debug\net9.0\check_results.json
```

## Пример сохранения результата
```json
{"requestedTime":"2025-10-27T23:12:01.8044149+05:00","name":"Web Request","target":"soundcloud.com","isSuccesful":true,"responseTime":472,"message":"Website is accessible. Status: 200"}
{"requestedTime":"2025-10-27T23:13:14.3334154+05:00","name":"Web Request","target":"asdfasdf.com","isSuccesful":true,"responseTime":571,"message":"Website is accessible. Status: 200"}
{"requestedTime":"2025-10-27T23:14:00.7732339+05:00","name":"Web Request","target":"https://www.instagram.com/","isSuccesful":true,"responseTime":860,"message":"Website is accessible. Status: 200"}
{"requestedTime":"2025-10-27T23:17:23.1571908+05:00","name":"Web Request","target":"http://grani.ru/","isSuccesful":false,"responseTime":21943,"message":"HTTP Error: An error occurred while sending the request."}
```
