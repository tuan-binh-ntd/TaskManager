namespace TaskManager.Infrastructure
{
    public class ConnectionStrings
    {
        /// <summary>
        /// Connection string for TaskManager DB
        /// </summary>
        public string DefaultConnection { get; set; } = string.Empty;
    }

    public class JwtSettings
    {
        /// <summary>
        /// Audience jwt
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Issuer jwt
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Khóa bảo mật jwt
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Thời gian hết hạn của token
        /// </summary>
        public string Subject { get; set; } = string.Empty;
    }
}
