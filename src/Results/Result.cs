namespace FastTechFoodsOrder.Shared.Results
{
    /// <summary>
    /// Representa o resultado de uma operação que pode ter sucesso ou falha
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Indica se a operação foi bem-sucedida
        /// </summary>
        public bool IsSuccess { get; protected set; }

        /// <summary>
        /// Indica se a operação falhou
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Mensagem de erro, se houver
        /// </summary>
        public string? ErrorMessage { get; protected set; }

        /// <summary>
        /// Código de erro, se houver
        /// </summary>
        public string? ErrorCode { get; protected set; }

        protected Result(bool isSuccess, string? errorMessage = null, string? errorCode = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Cria um resultado de sucesso
        /// </summary>
        /// <returns>Resultado de sucesso</returns>
        public static Result Success()
        {
            return new Result(true);
        }

        /// <summary>
        /// Cria um resultado de falha
        /// </summary>
        /// <param name="errorMessage">Mensagem de erro</param>
        /// <param name="errorCode">Código de erro opcional</param>
        /// <returns>Resultado de falha</returns>
        public static Result Failure(string errorMessage, string? errorCode = null)
        {
            return new Result(false, errorMessage, errorCode);
        }

        /// <summary>
        /// Converte implicitamente um booleano em Result
        /// </summary>
        /// <param name="success">Indica se é sucesso</param>
        public static implicit operator Result(bool success)
        {
            return success ? Success() : Failure("Operação falhou");
        }
    }
}
