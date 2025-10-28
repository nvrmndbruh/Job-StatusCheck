using Microsoft.Data.SqlClient;
using StatusCheck.Interfaces;
using StatusCheck.Models;
using System.Diagnostics;

namespace StatusCheck.Requests
{
    [Request(
        name:"mssql",
        argument:"connection string")]
    public class MsSqlRequest : IStatusCheck
    {
        public string Name => "MS SQL Request";

        public async Task<RequestResults> CheckAsync(string target, CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new RequestResults
            {
                Name = this.Name,
                Target = target,
            };

            try
            {
                await using var connection = new SqlConnection(target);
                await connection.OpenAsync(cancellationToken);

                stopwatch.Stop();

                result.IsSuccessful = true;
                result.ResponseTime = stopwatch.ElapsedMilliseconds;
                result.Message = $"MS SQL Server is accessible";
            }
            catch (SqlException ex)
            {
                stopwatch.Stop();
                result.IsSuccessful = false;
                result.ResponseTime = stopwatch.ElapsedMilliseconds;
                result.Message = $"MS SQL Server error: {ex.Message}";
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                result.IsSuccessful = false;
                result.ResponseTime = stopwatch.ElapsedMilliseconds;
                result.Message = $"Error: {ex.Message}";
            }

            return result;
        }
    }
}
