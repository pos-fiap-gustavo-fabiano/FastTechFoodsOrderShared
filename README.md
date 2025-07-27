# FastTechFoodsOrder.Shared

[![.NET](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/)
[![Version](https://img.shields.io/badge/version-2.6.0-blue)](https://github.com/pos-fiap-gustavo-fabiano/FastTechFoodsOrderShared)

Uma biblioteca compartilhada .NET para o sistema FastTechFoods, fornecendo componentes comuns para gerenciamento de pedidos, incluindo enums, mensagens de integração, constantes de mensageria, Result Pattern e utilitários para manipulação de status de pedidos.

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Características](#características)
- [Instalação](#instalação)
- [Uso](#uso)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [API Reference](#api-reference)
- [Contribuição](#contribuição)
- [Licença](#licença)

## 🚀 Sobre o Projeto

O FastTechFoodsOrder.Shared é uma biblioteca desenvolvida para centralizar componentes comuns utilizados no ecossistema de pedidos do FastTechFoods. Esta biblioteca fornece definições padronizadas para status de pedidos, mensagens de eventos, constantes de mensageria, Result Pattern e utilitários para facilitar a integração entre diferentes microserviços.

### Características

- ✅ **Enums padronizados** para status de pedidos
- ✅ **Mensagens de eventos** tipadas para integração entre microserviços
- ✅ **Utilitários** para conversão e validação de status
- ✅ **Constantes** para nomes de filas, exchanges e routing keys
- ✅ **Result Pattern** para padronização de retornos
- ✅ **BaseController** para respostas HTTP padronizadas
- ✅ **Validação de transições** de status
- ✅ **Compatibilidade** com .NET 9.0
- ✅ **Nullable reference types** habilitado

## 📦 Instalação

### Via NuGet Package Manager

```bash
Install-Package FastTechFoodsOrder.Shared
```

### Via .NET CLI

```bash
dotnet add package FastTechFoodsOrder.Shared
```

### Via PackageReference

```xml
<PackageReference Include="FastTechFoodsOrder.Shared" Version="2.6.0" />
```

## 🛠️ Uso

### Enums de Status

```csharp
using FastTechFoodsOrder.Shared.Enums;

// Usando o enum OrderStatus
var status = OrderStatus.Pending;
var nextStatus = OrderStatus.Accepted;
```

### Result Pattern

```csharp
using FastTechFoodsOrder.Shared.Results;

// Retorno simples de sucesso/falha
public Result ValidateOrder(Order order)
{
    if (order == null)
        return Result.Failure("Pedido não pode ser nulo", ErrorCodes.ValidationError);
    
    return Result.Success();
}

// Retorno com valor
public Result<Order> GetOrderById(string orderId)
{
    var order = repository.GetById(orderId);
    
    if (order == null)
        return Result<Order>.Failure("Pedido não encontrado", ErrorCodes.OrderNotFound);
    
    return Result<Order>.Success(order);
}

// Uso com métodos de extensão
var result = GetOrderById("123")
    .OnSuccess(order => logger.LogInfo($"Pedido {order.Id} encontrado"))
    .OnFailure(error => logger.LogError(error))
    .Map(order => new OrderDto { Id = order.Id, Status = order.Status });

// Obter valor ou padrão
var orderDto = result.GetValueOrDefault(new OrderDto());
```

### BaseController

```csharp
using FastTechFoodsOrder.Shared.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _orderService.GetByIdAsync(id);
        return ToActionResult(result); // Converte automaticamente Result<T> em IActionResult
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var result = await _orderService.CreateAsync(request);
        return ToCreatedResult(result, "GetById", new { id = result.Value?.Id });
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateStatusRequest request)
    {
        var result = await _orderService.UpdateStatusAsync(id, request.Status);
        return ToNoContentResult(result); // 204 No Content para updates
    }
}
```

### Constantes de Mensageria

```csharp
using FastTechFoodsOrder.Shared.Constants;

// Configurar consumidores usando constantes
SetupConsumer<OrderAcceptedMessage>(QueueNames.OrderAccepted);
SetupConsumer<OrderPreparingMessage>(QueueNames.OrderPreparing);
SetupConsumer<OrderReadyMessage>(QueueNames.OrderReady);
SetupConsumer<OrderCancelledMessage>(QueueNames.OrderCancelled);

// Usar exchanges e routing keys
var exchange = ExchangeNames.OrderEvents;
var routingKey = RoutingKeys.OrderAccepted;

// Obter fila por status
var queueName = QueueNames.GetQueueByStatus("accepted");

// Obter routing key por status
var routingKeyByStatus = RoutingKeys.GetRoutingKeyByStatus("preparing");
```

### Utilitários de Status

```csharp
using FastTechFoodsOrder.Shared.Utils;
using FastTechFoodsOrder.Shared.Enums;

// Conversão de string para enum
var status = OrderStatusUtils.ConvertStringToStatus("pending");

// Conversão de enum para string
var statusString = OrderStatusUtils.ConvertStatusToString(OrderStatus.Accepted);

// Validação de status
bool isValid = OrderStatusUtils.IsValidStatus("preparing");

// Verificação de transição válida
bool canTransition = OrderStatusUtils.IsValidStatusTransition(
    OrderStatus.Pending, 
    OrderStatus.Accepted
);

// Obter descrição amigável
string description = OrderStatusUtils.GetStatusDescription(OrderStatus.Preparing);
// Retorna: "Em Preparação"

// Obter todos os status válidos
var allStatuses = OrderStatusUtils.GetAllValidStatuses();
```

### Mensagens de Eventos

```csharp
using FastTechFoodsOrder.Shared.Integration.Messages;

// Criação de pedido
var orderCreated = new OrderCreatedMessage
{
    OrderId = "123",
    EventType = "OrderCreated",
    EventDate = DateTime.UtcNow,
    CustomerId = "customer-123",
    Status = "pending",
    Items = new List<OrderItemMessage>
    {
        new OrderItemMessage
        {
            ProductId = "prod-1",
            Name = "Hambúrguer",
            Quantity = 2,
            UnitPrice = 15.99m
        }
    },
    DeliveryMethod = "delivery",
    Total = 31.98m
};
```

## 📁 Estrutura do Projeto

```
src/
├── Constants/
│   ├── QueueNames.cs               # Constantes para nomes das filas
│   └── MessagingConstants.cs       # Constantes para exchanges e routing keys
├── Controllers/
│   └── BaseController.cs           # Controller base para respostas HTTP padronizadas
├── Enums/
│   └── OrderStatus.cs              # Enum com os status dos pedidos
├── Examples/
│   └── OrdersControllerExample.cs  # Exemplo de uso do BaseController
├── Integration/
│   └── Messages/
│       └── OrderEventMessage.cs   # Mensagens de eventos de pedidos
├── Results/
│   ├── Result.cs                   # Classe base do Result Pattern
│   ├── ResultT.cs                  # Result Pattern com valor genérico
│   ├── ErrorCodes.cs               # Códigos de erro padronizados
│   └── ResultExtensions.cs        # Métodos de extensão para Result
├── Utils/
│   └── OrderStatusUtils.cs        # Utilitários para manipulação de status
└── FastTechFoodsOrder.Shared.csproj
```

## 📚 API Reference

### Result Pattern

#### Classes Principais

```csharp
// Resultado simples
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public string? ErrorMessage { get; }
    public string? ErrorCode { get; }
    
    public static Result Success();
    public static Result Failure(string errorMessage, string? errorCode = null);
}

// Resultado com valor
public class Result<T> : Result
{
    public T? Value { get; }
    
    public static Result<T> Success(T value);
    public static new Result<T> Failure(string errorMessage, string? errorCode = null);
    public static Result<T> FromResult(Result result);
}
```

#### Códigos de Erro Padronizados

```csharp
public static class ErrorCodes
{
    // Gerais
    public const string ValidationError = "VALIDATION_ERROR";
    public const string NotFound = "NOT_FOUND";
    public const string InternalError = "INTERNAL_ERROR";
    
    // Pedidos
    public const string OrderNotFound = "ORDER_NOT_FOUND";
    public const string OrderInvalidStatus = "ORDER_INVALID_STATUS";
    public const string OrderStatusTransitionInvalid = "ORDER_STATUS_TRANSITION_INVALID";
    
    // Produtos
    public const string ProductNotFound = "PRODUCT_NOT_FOUND";
    public const string ProductOutOfStock = "PRODUCT_OUT_OF_STOCK";
    
    // Pagamentos
    public const string PaymentFailed = "PAYMENT_FAILED";
    public const string PaymentMethodInvalid = "PAYMENT_METHOD_INVALID";
}
```

#### Métodos de Extensão

```csharp
// Execução condicional
result.OnSuccess(() => DoSomething());
result.OnFailure(error => LogError(error));

// Mapeamento
var mappedResult = result.Map(value => new Dto(value));

// Bind (encadeamento de operações que retornam Result)
var finalResult = result.Bind(value => AnotherOperation(value));

// Combinação de múltiplos resultados
var combinedResult = ResultExtensions.Combine(result1, result2, result3);

// Obter valor ou padrão
var value = result.GetValueOrDefault(defaultValue);
```

### BaseController

O `BaseController` converte automaticamente objetos `Result` em respostas HTTP apropriadas, mapeando códigos de erro para status HTTP corretos.

#### Métodos Principais

```csharp
public abstract class BaseController : ControllerBase
{
    // Converte Result em ActionResult
    protected IActionResult ToActionResult(Result result);
    protected IActionResult ToActionResult<T>(Result<T> result);
    
    // Para operações de criação (201 Created)
    protected IActionResult ToCreatedResult<T>(Result<T> result, string? routeName = null, object? routeValues = null);
    
    // Para operações de atualização (204 No Content)
    protected IActionResult ToNoContentResult(Result result);
}
```

#### Mapeamento de Códigos de Erro

| Código de Erro | Status HTTP |
|----------------|-------------|
| `NOT_FOUND`, `ORDER_NOT_FOUND`, `PRODUCT_NOT_FOUND` | 404 Not Found |
| `VALIDATION_ERROR`, `ORDER_INVALID_STATUS` | 400 Bad Request |
| `UNAUTHORIZED` | 401 Unauthorized |
| `FORBIDDEN` | 403 Forbidden |
| `PRODUCT_OUT_OF_STOCK`, `ORDER_ALREADY_CANCELLED` | 409 Conflict |
| Outros | 500 Internal Server Error |

#### Formato de Resposta de Erro

```json
{
  "message": "Mensagem de erro",
  "code": "ERROR_CODE",
  "timestamp": "2025-07-27T10:30:00.000Z",
  "correlationId": "uuid-optional"
}
```

### OrderStatus Enum

| Status | Descrição | Próximos Status Válidos |
|--------|-----------|-------------------------|
| `Pending` | Pedido pendente | `Accepted`, `Cancelled` |
| `Accepted` | Pedido aceito | `Preparing`, `Cancelled` |
| `Preparing` | Em preparação | `Ready`, `Cancelled` |
| `Ready` | Pronto para entrega | `Delivered` |
| `Cancelled` | Cancelado | *(final)* |
| `Delivered` | Entregue | *(final)* |

### Constantes de Mensageria

#### QueueNames
```csharp
public static class QueueNames
{
    public const string OrderCreated = "order.created.queue";
    public const string OrderAccepted = "order.accepted.queue";
    public const string OrderPreparing = "order.preparing.queue";
    public const string OrderReady = "order.ready.queue";
    public const string OrderCancelled = "order.cancelled.queue";
    
    public static string[] GetAllQueues();
    public static string GetQueueByStatus(string status);
}
```

## 🎯 Exemplo Prático Completo

```csharp
using FastTechFoodsOrder.Shared.Constants;
using FastTechFoodsOrder.Shared.Results;
using FastTechFoodsOrder.Shared.Utils;

public class OrderService
{
    // Usar Result Pattern nos métodos
    public async Task<Result<Order>> CreateOrderAsync(CreateOrderRequest request)
    {
        // Validação
        var validationResult = ValidateCreateOrderRequest(request);
        if (validationResult.IsFailure)
            return Result<Order>.FromResult(validationResult);
        
        try
        {
            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                CustomerId = request.CustomerId,
                Status = OrderStatus.Pending
            };
            
            await repository.SaveAsync(order);
            
            // Publicar evento usando constantes
            await PublishOrderEvent(QueueNames.OrderCreated, order);
            
            return Result<Order>.Success(order);
        }
        catch (Exception ex)
        {
            return Result<Order>.Failure(
                "Erro ao criar pedido", 
                ErrorCodes.InternalError
            );
        }
    }
    
    public async Task<Result> UpdateOrderStatusAsync(string orderId, OrderStatus newStatus)
    {
        var orderResult = await GetOrderByIdAsync(orderId);
        if (orderResult.IsFailure)
            return Result.FromResult(orderResult);
        
        var order = orderResult.Value!;
        
        // Validar transição usando utilitários
        if (!OrderStatusUtils.IsValidStatusTransition(order.Status, newStatus))
        {
            return Result.Failure(
                $"Transição inválida de {order.Status} para {newStatus}",
                ErrorCodes.OrderStatusTransitionInvalid
            );
        }
        
        order.Status = newStatus;
        await repository.UpdateAsync(order);
        
        // Publicar evento na fila correta
        var queueName = QueueNames.GetQueueByStatus(newStatus.ToString().ToLower());
        await PublishOrderEvent(queueName, order);
        
        return Result.Success();
    }
}
```

## 🤝 Contribuição

1. Faça o fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto é licenciado sob a [MIT License](LICENSE).

## 🏗️ Desenvolvimento

### Pré-requisitos

- .NET 9.0 SDK
- Visual Studio 2022 ou VS Code

### Build

```bash
dotnet build
```

### Testes

```bash
dotnet test
```

### Publicação

```bash
dotnet pack --configuration Release
```

## 📞 Contato

Projeto FastTechFoods - [@pos-fiap-gustavo-fabiano](https://github.com/pos-fiap-gustavo-fabiano)

Link do Projeto: [https://github.com/pos-fiap-gustavo-fabiano/FastTechFoodsOrderShared](https://github.com/pos-fiap-gustavo-fabiano/FastTechFoodsOrderShared)

---

⚡ Feito com ❤️ para o ecossistema FastTechFoods