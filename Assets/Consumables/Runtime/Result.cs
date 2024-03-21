namespace Common.Consumables
{
    public struct Result<T>
    {
        public readonly T Object;
        public readonly bool IsExist;
        
        public string ErrorMessage { get; }

        private Result(T result, bool isExist)
        {
            Object = result;
            IsExist = isExist;
            ErrorMessage = string.Empty;
        }

        private Result(string errorMessage)
        {
            ErrorMessage = errorMessage;
            Object = default;
            IsExist = false;
        }

        public static Result<T> Success(T targetObject)
        {
            return new Result<T>(targetObject, true);
        }

        public static Result<T> Fail(string errorMessage)
        {
            return new Result<T>(errorMessage);
        }
    }
}