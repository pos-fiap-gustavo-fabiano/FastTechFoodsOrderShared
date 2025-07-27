# FastTechFoodsOrder.Shared

[![.NET](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/)
[![Version](https://img.shields.io/badge/version-2.6.0-blue)](https://github.com/pos-fiap-gustavo-fabiano/FastTechFoodsOrderShared)

Uma biblioteca compartilhada .NET para o sistema FastTechFoods, fornecendo componentes comuns para gerenciamento de pedidos, incluindo enums, mensagens de integração e utilitários para manipulação de status de pedidos.

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

// Aceitar pedido
var orderAccepted = new OrderAcceptedMessage
{
    OrderId = "123",
    EventType = "OrderAccepted",
    EventDate = DateTime.UtcNow,
    CustomerId = "customer-123",
    Status = "accepted",
    UpdatedBy = "restaurant-staff",
    EstimatedPreparationTime = DateTime.UtcNow.AddMinutes(30)
};

// Cancelar pedido
var orderCancelled = new OrderCancelledMessage
{
    OrderId = "123",
    EventType = "OrderCancelled",
    EventDate = DateTime.UtcNow,
    CustomerId = "customer-123",
    Status = "cancelled",
    CancelReason = "Cliente solicitou cancelamento",
    CancelledBy = "customer",
    CancelledAt = DateTime.UtcNow
};
```

## 📁 Estrutura do Projeto

```
src/
├── Enums/
│   └── OrderStatus.cs              # Enum com os status dos pedidos
├── Integration/
│   └── Messages/
│       └── OrderEventMessage.cs   # Mensagens de eventos de pedidos
├── Utils/
│   └── OrderStatusUtils.cs        # Utilitários para manipulação de status
└── FastTechFoodsOrder.Shared.csproj
```

## 📚 API Reference

### OrderStatus Enum

| Status | Descrição | Próximos Status Válidos |
|--------|-----------|-------------------------|
| `Pending` | Pedido pendente | `Accepted`, `Cancelled` |
| `Accepted` | Pedido aceito | `Preparing`, `Cancelled` |
| `Preparing` | Em preparação | `Ready`, `Cancelled` |
| `Ready` | Pronto para entrega | `Delivered` |
| `Cancelled` | Cancelado | *(final)* |
| `Delivered` | Entregue | *(final)* |

### Mensagens de Eventos

#### Mensagens Base
- `OrderEventMessage` - Classe base para todos os eventos
- `OrderCreatedMessage` - Evento de criação de pedido
- `OrderItemMessage` - Representa um item do pedido

#### Mensagens de Status
- `OrderPendingMessage` - Pedido pendente
- `OrderAcceptedMessage` - Pedido aceito
- `OrderPreparingMessage` - Pedido em preparação
- `OrderReadyMessage` - Pedido pronto
- `OrderDeliveredMessage` - Pedido entregue
- `OrderCompletedMessage` - Pedido completado
- `OrderCancelledMessage` - Pedido cancelado

### OrderStatusUtils

#### Métodos Principais

```csharp
// Conversões
OrderStatus? ConvertStringToStatus(string? status)
string ConvertStatusToString(OrderStatus status)

// Validações
bool IsValidStatus(string? status)
bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)

// Utilitários
IEnumerable<string> GetAllValidStatuses()
string GetStatusDescription(OrderStatus status)
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
