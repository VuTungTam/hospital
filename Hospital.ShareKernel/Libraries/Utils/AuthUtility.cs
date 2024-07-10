using IdGen;
namespace Hospital.SharedKernel.Libraries.Utils
{
    public static class AuthUtility
    {
        private static object LockObj = new object();
        private static IdGenerator Generator = new IdGenerator(0);

        /// <summary>
        /// Tạo id theo twitter's snowflake
        /// </summary>
        public static long GenerateSnowflakeId()
        {
            lock (LockObj)
            {
                return Generator.CreateId();
            }
        }
    }
}
