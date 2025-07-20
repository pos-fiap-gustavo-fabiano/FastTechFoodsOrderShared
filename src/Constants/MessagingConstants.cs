namespace FastTechFoodsOrder.Shared.Constants
{
    /// <summary>
    /// Constantes para exchanges de mensageria
    /// </summary>
    public static class ExchangeNames
    {
        /// <summary>
        /// Exchange principal para eventos de pedidos
        /// </summary>
        public const string OrderEvents = "order.events.exchange";

        /// <summary>
        /// Exchange para eventos de status específicos
        /// </summary>
        public const string OrderStatus = "order.status.exchange";

        /// <summary>
        /// Exchange para dead letter queue
        /// </summary>
        public const string DeadLetter = "order.deadletter.exchange";
    }

    /// <summary>
    /// Constantes para routing keys
    /// </summary>
    public static class RoutingKeys
    {
        /// <summary>
        /// Routing key para pedidos criados
        /// </summary>
        public const string OrderCreated = "order.created";

        /// <summary>
        /// Routing key para pedidos pendentes
        /// </summary>
        public const string OrderPending = "order.pending";

        /// <summary>
        /// Routing key para pedidos aceitos
        /// </summary>
        public const string OrderAccepted = "order.accepted";

        /// <summary>
        /// Routing key para pedidos em preparação
        /// </summary>
        public const string OrderPreparing = "order.preparing";

        /// <summary>
        /// Routing key para pedidos prontos
        /// </summary>
        public const string OrderReady = "order.ready";

        /// <summary>
        /// Routing key para pedidos entregues
        /// </summary>
        public const string OrderDelivered = "order.delivered";

        /// <summary>
        /// Routing key para pedidos completados
        /// </summary>
        public const string OrderCompleted = "order.completed";

        /// <summary>
        /// Routing key para pedidos cancelados
        /// </summary>
        public const string OrderCancelled = "order.cancelled";

        /// <summary>
        /// Obtém a routing key baseada no status do pedido
        /// </summary>
        /// <param name="status">Status do pedido</param>
        /// <returns>Routing key correspondente</returns>
        public static string GetRoutingKeyByStatus(string status)
        {
            return status?.ToLower() switch
            {
                "pending" => OrderPending,
                "accepted" => OrderAccepted,
                "preparing" => OrderPreparing,
                "ready" => OrderReady,
                "delivered" => OrderDelivered,
                "completed" => OrderCompleted,
                "cancelled" => OrderCancelled,
                "created" => OrderCreated,
                _ => throw new ArgumentException($"Status '{status}' não possui routing key correspondente")
            };
        }
    }
}
