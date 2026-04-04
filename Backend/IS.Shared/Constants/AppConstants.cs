namespace IS.Shared.Constants
{
    public static class AppConstants
    {
        public static class AuditActions
        {
            public const string Create = "CREATE";
            public const string Update = "UPDATE";
            public const string Delete = "DELETE";
        }

        public static class TableNames
        {
            public const string Product = "Product";
            public const string ProductSupplier = "ProductSupplier";
            public const string Supplier = "Supplier";
        }

        public static class FieldNames
        {
            public const string Name = "Name";
            public const string Description = "Description";
            public const string IsDeleted = "IsDeleted";
            public const string SupplierId = "SupplierId";
            public const string Price = "Price";
            public const string Stock = "Stock";
            public const string BatchNumber = "BatchNumber";
        }
    }
}