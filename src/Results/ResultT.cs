namespace FastTechFoodsOrder.Shared.Results
{
    /// <summary>
    /// Representa o resultado de uma operação que retorna um valor em caso de sucesso
    /// </summary>
    /// <typeparam name="T">Tipo do valor retornado</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// Valor retornado pela operação, se bem-sucedida
        /// </summary>
        public T? Value { get; private set; }

        private Result(bool isSuccess, T? value = default, string? errorMessage = null, string? errorCode = null)
            : base(isSuccess, errorMessage, errorCode)
        {
            Value = value;
        }

        /// <summary>
        /// Cria um resultado de sucesso com valor
        /// </summary>
        /// <param name="value">Valor a ser retornado</param>
        /// <returns>Resultado de sucesso com valor</returns>
        public static Result<T> Success(T value)
        {
            return new Result<T>(true, value);
        }

        /// <summary>
        /// Cria um resultado de falha
        /// </summary>
        /// <param name="errorMessage">Mensagem de erro</param>
        /// <param name="errorCode">Código de erro opcional</param>
        /// <returns>Resultado de falha</returns>
        public static new Result<T> Failure(string errorMessage, string? errorCode = null)
        {
            return new Result<T>(false, default, errorMessage, errorCode);
        }

        /// <summary>
        /// Converte implicitamente um valor T em Result&lt;T&gt;
        /// </summary>
        /// <param name="value">Valor a ser encapsulado</param>
        public static implicit operator Result<T>(T value)
        {
            return Success(value);
        }

        /// <summary>
        /// Converte um Result em Result&lt;T&gt;
        /// </summary>
        /// <param name="result">Result base</param>
        /// <returns>Result&lt;T&gt; convertido</returns>
        public static Result<T> FromResult(Result result)
        {
            return result.IsSuccess 
                ? Success(default(T)!) 
                : Failure(result.ErrorMessage!, result.ErrorCode);
        }
    }
}
