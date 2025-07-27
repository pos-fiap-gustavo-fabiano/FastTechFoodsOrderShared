namespace FastTechFoodsOrder.Shared.Results
{
    /// <summary>
    /// Códigos de erro padronizados para o sistema FastTechFoods
    /// </summary>
    public static class ErrorCodes
    {
        // Códigos gerais
        public const string ValidationError = "VALIDATION_ERROR";
        public const string NotFound = "NOT_FOUND";
        public const string Unauthorized = "UNAUTHORIZED";
        public const string Forbidden = "FORBIDDEN";
        public const string InternalError = "INTERNAL_ERROR";
        public const string ExternalServiceError = "EXTERNAL_SERVICE_ERROR";

        // Códigos específicos de pedidos
        public const string OrderNotFound = "ORDER_NOT_FOUND";
        public const string OrderInvalidStatus = "ORDER_INVALID_STATUS";
        public const string OrderStatusTransitionInvalid = "ORDER_STATUS_TRANSITION_INVALID";
        public const string OrderAlreadyCancelled = "ORDER_ALREADY_CANCELLED";
        public const string OrderAlreadyCompleted = "ORDER_ALREADY_COMPLETED";
        public const string OrderItemsRequired = "ORDER_ITEMS_REQUIRED";
        public const string OrderInvalidCustomer = "ORDER_INVALID_CUSTOMER";

        // Códigos específicos de produtos
        public const string ProductNotFound = "PRODUCT_NOT_FOUND";
        public const string ProductOutOfStock = "PRODUCT_OUT_OF_STOCK";
        public const string ProductInactive = "PRODUCT_INACTIVE";

        // Códigos específicos de clientes
        public const string CustomerNotFound = "CUSTOMER_NOT_FOUND";
        public const string CustomerInactive = "CUSTOMER_INACTIVE";

        // Códigos específicos de pagamento
        public const string PaymentFailed = "PAYMENT_FAILED";
        public const string PaymentMethodInvalid = "PAYMENT_METHOD_INVALID";
        public const string PaymentAlreadyProcessed = "PAYMENT_ALREADY_PROCESSED";
    }
}
