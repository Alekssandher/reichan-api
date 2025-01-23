using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidateSignature : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var body = context.ActionArguments["body"] as CreateSignedPostDto 
            ?? throw new InvalidOperationException("Requisition body invalid or missing.");
        
        Console.WriteLine(body.Date);
        string data = body.GetFormatedContent();
        string publicKey = body.ConvertBase64ToPem(body.PublicKey);
        string signatureBase64 = body.Signature;

        bool isValid = Validate(publicKey, data, signatureBase64);
        
        // if(!isValid) {
        //     context.Result = new BadRequestObjectResult(new { success = false, message = "Invalid signature" });
        // }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Nada a fazer após a execução da ação
    }

    public static bool Validate (string publicKey, string data, string signatureBase64){
        try
        {
            // Converter assinatura de base64 para bytes
            byte[] signatureBytes = Convert.FromBase64String(signatureBase64);

            // Converter o dado original para bytes
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            // Criar instância de RSA e importar a chave pública
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(publicKey);

                // Validar assinatura usando SHA256 como algoritmo de hash
                bool isValid = rsa.VerifyData(
                    dataBytes,
                    signatureBytes,
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1
                );

                return isValid;
            }
        }
        catch
        {
            return false; // Retorna falso em caso de qualquer falha
        }
    }
}
