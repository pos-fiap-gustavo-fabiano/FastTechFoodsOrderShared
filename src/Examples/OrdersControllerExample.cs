using Microsoft.AspNetCore.Mvc;
using FastTechFoodsOrder.Shared.Controllers;
using FastTechFoodsOrder.Shared.Results;

namespace FastTechFoodsOrder.Shared.Examples
{
    /// <summary>
    /// Exemplo de como usar o BaseController em um microserviço
    /// Este arquivo é apenas para demonstração e pode ser removido
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersControllerExample : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersControllerExample(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Obtém um pedido por ID
        /// </summary>
        /// <param name="id">ID do pedido</param>
        /// <returns>Dados do pedido</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _orderService.GetByIdAsync(id);
            return ToActionResult(result);
        }

        /// <summary>
        /// Cria um novo pedido
        /// </summary>
        /// <param name="request">Dados do pedido</param>
        /// <returns>Pedido criado</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var result = await _orderService.CreateAsync(request);
            return ToCreatedResult(result, "GetById", new { id = result.Value?.Id });
        }

        /// <summary>
        /// Atualiza o status de um pedido
        /// </summary>
        /// <param name="id">ID do pedido</param>
        /// <param name="request">Novo status</param>
        /// <returns>Resultado da operação</returns>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateStatusRequest request)
        {
            var result = await _orderService.UpdateStatusAsync(id, request.Status);
            return ToNoContentResult(result);
        }

        /// <summary>
        /// Lista pedidos com filtros
        /// </summary>
        /// <param name="customerId">ID do cliente (opcional)</param>
        /// <param name="status">Status do pedido (opcional)</param>
        /// <returns>Lista de pedidos</returns>
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? customerId, [FromQuery] string? status)
        {
            var result = await _orderService.ListAsync(customerId, status);
            return ToActionResult(result);
        }

        /// <summary>
        /// Cancela um pedido
        /// </summary>
        /// <param name="id">ID do pedido</param>
        /// <param name="request">Motivo do cancelamento</param>
        /// <returns>Resultado da operação</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(string id, [FromBody] CancelOrderRequest request)
        {
            var result = await _orderService.CancelAsync(id, request.Reason);
            return ToNoContentResult(result);
        }
    }

    // DTOs de exemplo - normalmente estariam em um projeto separado
    public class CreateOrderRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public List<OrderItemRequest> Items { get; set; } = new();
        public string DeliveryMethod { get; set; } = string.Empty;
    }

    public class OrderItemRequest
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    public class UpdateStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }

    public class CancelOrderRequest
    {
        public string Reason { get; set; } = string.Empty;
    }

    public class OrderDto
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Interface de exemplo - normalmente estaria em um projeto separado
    public interface IOrderService
    {
        Task<Result<OrderDto>> GetByIdAsync(string id);
        Task<Result<OrderDto>> CreateAsync(CreateOrderRequest request);
        Task<Result> UpdateStatusAsync(string id, string status);
        Task<Result<IEnumerable<OrderDto>>> ListAsync(string? customerId, string? status);
        Task<Result> CancelAsync(string id, string reason);
    }
}
