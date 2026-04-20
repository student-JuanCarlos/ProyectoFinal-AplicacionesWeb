using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SistemaLogistico.BussinesLogic.Utilidades
{
    public class UtilidadesService: IUtilidades
    {

        public string ConvertirSha256(string texto, string Email)
        {
            string contraseñHash = texto + Email;  //Al combinar el texto + Email, nos aseguremos de no repetir contraseñas hasheadas 

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(contraseñHash));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
