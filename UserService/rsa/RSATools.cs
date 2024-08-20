using System.Security.Cryptography;

namespace UserService.rsa
{
    public static class RSATools
    {
        public static RSA GetPublicKey() => GetRsa(File.ReadAllText("RSA/public_key.pem"));

        public static RSA GetPrivateKey() => GetRsa(File.ReadAllText("RSA/private_key.pem"));

        private static RSA GetRsa(string f)
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(f);
            return rsa;
        }
    }
}
