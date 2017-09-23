using System;

namespace Business
{
    public static class Ensure
    {
        public static void IsNotNull(object argumentToTest, string argumentName)
        {
            if (argumentToTest == null)
            {
                throw new ArgumentNullException(argumentName, nameof(Ensure) + "." + nameof(IsNotNull));
            }
        }
    }
}
