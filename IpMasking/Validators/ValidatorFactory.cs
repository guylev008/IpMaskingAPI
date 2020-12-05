namespace IpMasking.Validators
{
    public static class ValidatorFactory<T>
        where T : new()
    {
        public static T GetValidator()
        {
            return new T();
        }
    }
}
