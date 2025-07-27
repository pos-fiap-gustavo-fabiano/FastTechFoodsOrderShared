namespace FastTechFoodsOrder.Shared.Results
{
    /// <summary>
    /// Métodos de extensão para facilitar o uso do Result Pattern
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Executa uma ação apenas se o resultado for sucesso
        /// </summary>
        /// <param name="result">Resultado</param>
        /// <param name="action">Ação a ser executada</param>
        /// <returns>O próprio resultado para encadeamento</returns>
        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.IsSuccess)
                action();
            
            return result;
        }

        /// <summary>
        /// Executa uma ação apenas se o resultado for sucesso
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="result">Resultado</param>
        /// <param name="action">Ação a ser executada com o valor</param>
        /// <returns>O próprio resultado para encadeamento</returns>
        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess && result.Value is not null)
                action(result.Value);
            
            return result;
        }

        /// <summary>
        /// Executa uma ação apenas se o resultado for falha
        /// </summary>
        /// <param name="result">Resultado</param>
        /// <param name="action">Ação a ser executada com a mensagem de erro</param>
        /// <returns>O próprio resultado para encadeamento</returns>
        public static Result OnFailure(this Result result, Action<string> action)
        {
            if (result.IsFailure)
                action(result.ErrorMessage ?? "Erro desconhecido");
            
            return result;
        }

        /// <summary>
        /// Executa uma ação apenas se o resultado for falha
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="result">Resultado</param>
        /// <param name="action">Ação a ser executada com a mensagem de erro</param>
        /// <returns>O próprio resultado para encadeamento</returns>
        public static Result<T> OnFailure<T>(this Result<T> result, Action<string> action)
        {
            if (result.IsFailure)
                action(result.ErrorMessage ?? "Erro desconhecido");
            
            return result;
        }

        /// <summary>
        /// Mapeia o valor do resultado para outro tipo
        /// </summary>
        /// <typeparam name="TSource">Tipo original</typeparam>
        /// <typeparam name="TTarget">Tipo de destino</typeparam>
        /// <param name="result">Resultado original</param>
        /// <param name="mapper">Função de mapeamento</param>
        /// <returns>Resultado com o valor mapeado</returns>
        public static Result<TTarget> Map<TSource, TTarget>(this Result<TSource> result, Func<TSource, TTarget> mapper)
        {
            if (result.IsFailure)
                return Result<TTarget>.Failure(result.ErrorMessage!, result.ErrorCode);

            try
            {
                var mappedValue = mapper(result.Value!);
                return Result<TTarget>.Success(mappedValue);
            }
            catch (Exception ex)
            {
                return Result<TTarget>.Failure($"Erro no mapeamento: {ex.Message}", ErrorCodes.InternalError);
            }
        }

        /// <summary>
        /// Obtém o valor do resultado ou um valor padrão em caso de falha
        /// </summary>
        /// <typeparam name="T">Tipo do valor</typeparam>
        /// <param name="result">Resultado</param>
        /// <param name="defaultValue">Valor padrão</param>
        /// <returns>Valor do resultado ou valor padrão</returns>
        public static T GetValueOrDefault<T>(this Result<T> result, T defaultValue = default!)
        {
            return result.IsSuccess ? result.Value! : defaultValue;
        }

        /// <summary>
        /// Executa uma função que retorna Result apenas se o resultado atual for sucesso
        /// </summary>
        /// <typeparam name="TSource">Tipo original</typeparam>
        /// <typeparam name="TTarget">Tipo de destino</typeparam>
        /// <param name="result">Resultado original</param>
        /// <param name="func">Função que retorna Result</param>
        /// <returns>Resultado encadeado</returns>
        public static Result<TTarget> Bind<TSource, TTarget>(this Result<TSource> result, Func<TSource, Result<TTarget>> func)
        {
            if (result.IsFailure)
                return Result<TTarget>.Failure(result.ErrorMessage!, result.ErrorCode);

            try
            {
                return func(result.Value!);
            }
            catch (Exception ex)
            {
                return Result<TTarget>.Failure($"Erro no bind: {ex.Message}", ErrorCodes.InternalError);
            }
        }

        /// <summary>
        /// Executa uma função que retorna Result apenas se o resultado atual for sucesso
        /// </summary>
        /// <param name="result">Resultado original</param>
        /// <param name="func">Função que retorna Result</param>
        /// <returns>Resultado encadeado</returns>
        public static Result Bind(this Result result, Func<Result> func)
        {
            if (result.IsFailure)
                return result;

            try
            {
                return func();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Erro no bind: {ex.Message}", ErrorCodes.InternalError);
            }
        }

        /// <summary>
        /// Combina múltiplos resultados em um único resultado
        /// </summary>
        /// <param name="results">Lista de resultados</param>
        /// <returns>Resultado combinado - sucesso apenas se todos forem sucesso</returns>
        public static Result Combine(params Result[] results)
        {
            var failures = results.Where(r => r.IsFailure).ToArray();
            
            if (!failures.Any())
                return Result.Success();

            var errorMessages = string.Join("; ", failures.Select(f => f.ErrorMessage));
            var firstErrorCode = failures.First().ErrorCode;
            
            return Result.Failure(errorMessages, firstErrorCode);
        }

        /// <summary>
        /// Combina múltiplos resultados com valores em uma lista
        /// </summary>
        /// <typeparam name="T">Tipo dos valores</typeparam>
        /// <param name="results">Lista de resultados com valores</param>
        /// <returns>Resultado com lista de valores - sucesso apenas se todos forem sucesso</returns>
        public static Result<IEnumerable<T>> Combine<T>(params Result<T>[] results)
        {
            var failures = results.Where(r => r.IsFailure).ToArray();
            
            if (!failures.Any())
            {
                var values = results.Select(r => r.Value!);
                return Result<IEnumerable<T>>.Success(values);
            }

            var errorMessages = string.Join("; ", failures.Select(f => f.ErrorMessage));
            var firstErrorCode = failures.First().ErrorCode;
            
            return Result<IEnumerable<T>>.Failure(errorMessages, firstErrorCode);
        }
    }
}
