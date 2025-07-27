using Microsoft.AspNetCore.Mvc;
using FastTechFoodsOrder.Shared.Results;

namespace FastTechFoodsOrder.Shared.Controllers
{
    /// <summary>
    /// Controller base que fornece métodos para converter Result em respostas HTTP padronizadas
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Converte um Result em uma resposta HTTP apropriada
        /// </summary>
        /// <param name="result">Resultado da operação</param>
        /// <returns>ActionResult apropriado</returns>
        protected IActionResult ToActionResult(Result result)
        {
            if (result.IsSuccess)
                return Ok();

            return GetErrorResponse(result.ErrorMessage!, result.ErrorCode);
        }

        /// <summary>
        /// Converte um Result&lt;T&gt; em uma resposta HTTP apropriada
        /// </summary>
        /// <typeparam name="T">Tipo do valor retornado</typeparam>
        /// <param name="result">Resultado da operação</param>
        /// <returns>ActionResult apropriado</returns>
        protected IActionResult ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);

            return GetErrorResponse(result.ErrorMessage!, result.ErrorCode);
        }

        /// <summary>
        /// Converte um Result&lt;T&gt; em uma resposta HTTP apropriada para operações de criação
        /// </summary>
        /// <typeparam name="T">Tipo do valor retornado</typeparam>
        /// <param name="result">Resultado da operação</param>
        /// <param name="routeName">Nome da rota para o recurso criado</param>
        /// <param name="routeValues">Valores da rota</param>
        /// <returns>ActionResult apropriado</returns>
        protected IActionResult ToCreatedResult<T>(Result<T> result, string? routeName = null, object? routeValues = null)
        {
            if (result.IsSuccess)
            {
                if (!string.IsNullOrEmpty(routeName) && routeValues != null)
                    return CreatedAtRoute(routeName, routeValues, result.Value);
                
                return Created(string.Empty, result.Value);
            }

            return GetErrorResponse(result.ErrorMessage!, result.ErrorCode);
        }

        /// <summary>
        /// Converte um Result&lt;T&gt; em uma resposta HTTP 204 No Content para operações de atualização
        /// </summary>
        /// <param name="result">Resultado da operação</param>
        /// <returns>ActionResult apropriado</returns>
        protected IActionResult ToNoContentResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();

            return GetErrorResponse(result.ErrorMessage!, result.ErrorCode);
        }

        /// <summary>
        /// Gera uma resposta de erro baseada no código de erro
        /// </summary>
        /// <param name="errorMessage">Mensagem de erro</param>
        /// <param name="errorCode">Código de erro</param>
        /// <returns>ActionResult de erro apropriado</returns>
        private IActionResult GetErrorResponse(string errorMessage, string? errorCode)
        {
            var errorResponse = new ErrorResponse
            {
                Message = errorMessage,
                Code = errorCode,
                Timestamp = DateTime.UtcNow
            };

            return errorCode switch
            {
                ErrorCodes.NotFound or 
                ErrorCodes.OrderNotFound or 
                ErrorCodes.ProductNotFound or 
                ErrorCodes.CustomerNotFound => NotFound(errorResponse),

                ErrorCodes.ValidationError or 
                ErrorCodes.OrderInvalidStatus or 
                ErrorCodes.OrderStatusTransitionInvalid or 
                ErrorCodes.OrderItemsRequired or 
                ErrorCodes.PaymentMethodInvalid => BadRequest(errorResponse),

                ErrorCodes.Unauthorized => Unauthorized(errorResponse),

                ErrorCodes.Forbidden => Forbid(),

                ErrorCodes.ProductOutOfStock or 
                ErrorCodes.OrderAlreadyCancelled or 
                ErrorCodes.OrderAlreadyCompleted or 
                ErrorCodes.PaymentAlreadyProcessed => Conflict(errorResponse),

                _ => StatusCode(500, errorResponse)
            };
        }
    }

    /// <summary>
    /// Classe para padronizar respostas de erro
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Mensagem de erro
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Código de erro
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Timestamp do erro
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// ID de correlação para rastreamento
        /// </summary>
        public string? CorrelationId { get; set; }
    }
}
