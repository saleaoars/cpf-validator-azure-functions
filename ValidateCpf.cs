using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CpfValidatorFunctions
{
    public static class ValidateCpf
    {
        [FunctionName("ValidateCpf")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "validate-cpf")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Requisição recebida para validação de CPF.");

            string cpf = req.Query["cpf"];

            if (string.IsNullOrEmpty(cpf))
            {
                using var reader = new StreamReader(req.Body);
                var body = await reader.ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(body);
                cpf = cpf ?? data?.cpf;
            }

            if (string.IsNullOrEmpty(cpf))
            {
                return new BadRequestObjectResult(new
                {
                    message = "Informe o CPF na query string (?cpf=) ou no corpo da requisição em JSON { \"cpf\": \"12345678909\" }."
                });
            }

            var cleanedCpf = new string(cpf.Where(char.IsDigit).ToArray());

            bool isValid = CpfValidator.IsValid(cleanedCpf);

            return new OkObjectResult(new
            {
                cpf = cleanedCpf,
                isValid
            });
        }
    }

    public static class CpfValidator
    {
        public static bool IsValid(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;
            if (cpf.Length != 11) return false;

            string[] invalids = new[]
            {
                "00000000000","11111111111","22222222222","33333333333",
                "44444444444","55555555555","66666666666","77777777777",
                "88888888888","99999999999"
            };

            if (invalids.Contains(cpf)) return false;

            // 1º dígito verificador
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += (cpf[i] - '0') * (10 - i);

            int firstDigit = sum % 11;
            firstDigit = firstDigit < 2 ? 0 : 11 - firstDigit;

            if (firstDigit != (cpf[9] - '0')) return false;

            // 2º dígito verificador
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += (cpf[i] - '0') * (11 - i);

            int secondDigit = sum % 11;
            secondDigit = secondDigit < 2 ? 0 : 11 - secondDigit;

            if (secondDigit != (cpf[10] - '0')) return false;

            return true;
        }
    }
}
