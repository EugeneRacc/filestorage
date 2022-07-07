namespace Business.PasswordHash
{
    /// <summary>
    /// Class for hashing password with MD5 algorithm
    /// </summary>
    public static class MD5Hash
    {
        /// <summary>
        /// Gets the md5 hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Hashed string</returns>
        public static string GetMD5Hash(string input)
        {
            using (System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
                bs = x.ComputeHash(bs);
                System.Text.StringBuilder s = new System.Text.StringBuilder();
                foreach (byte b in bs)
                {
                    s.Append(b.ToString("x2").ToLower());
                }
                string password = s.ToString();
                return password;
            }
        }
    }
}