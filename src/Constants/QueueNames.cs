namespace FastTechFoodsOrder.Shared.Constants
{
    /// <summary>
    /// Constantes para nomes das filas de mensageria
    /// </summary>
    public static class QueueNames
    {
        /// <summary>
        /// Fila para eventos de pedidos criados
        /// </summary>
        public const string OrderCreated = "order.created.queue";

        /// <summary>
        /// Fila para eventos de pedidos pendentes
        /// </summary>
        public const string OrderPending = "order.pending.queue";

        /// <summary>
        /// Fila para eventos de pedidos aceitos
        /// </summary>
        public const string OrderAccepted = "order.accepted.queue";

        /// <summary>
        /// Fila para eventos de pedidos em preparação
        /// </summary>
        public const string OrderPreparing = "order.preparing.queue";

        /// <summary>
        /// Fila para eventos de pedidos prontos
        /// </summary>
        public const string OrderReady = "order.ready.queue";

        /// <summary>
        /// Fila para eventos de pedidos entregues
        /// </summary>
        public const string OrderDelivered = "order.delivered.queue";

        /// <summary>
        /// Fila para eventos de pedidos completados
        /// </summary>
        public const string OrderCompleted = "order.completed.queue";

        /// <summary>
        /// Fila para eventos de pedidos cancelados
        /// </summary>
        public const string OrderCancelled = "order.cancelled.queue";
        public const string OrderUserCancelled = "order.user.cancelled.queue";
        public const string OrderDlQ = "order.dlq.queue";

        /// <summary>
        /// Obtém todas as filas de eventos de pedidos
        /// </summary>
        /// <returns>Array com todos os nomes das filas</returns>
        public static string[] GetAllQueues()
        {
            return new[]
            {
                OrderCreated,
                OrderPending,
                OrderAccepted,
                OrderPreparing,
                OrderReady,
                OrderDelivered,
                OrderCompleted,
                OrderCancelled,
                OrderDlQ
            };
        }

        /// <summary>
        /// Obtém o nome da fila baseado no status do pedido
        /// </summary>
        /// <param name="status">Status do pedido</param>
        /// <returns>Nome da fila correspondente</returns>
        public static string GetQueueByStatus(string status)
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
                _ => throw new ArgumentException($"Status '{status}' não possui fila correspondente")
            };
        }
    }
}
