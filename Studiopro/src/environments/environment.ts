let servicehost = 'http://localhost:5001/';

export const environment = {
  production: false,

  // API Base URLs
  apiUrl: servicehost,
  User: 'https://localhost:5001/api/User/',
  Role: servicehost + 'Roles/',
  Branch: servicehost + 'Branch/',
  Company: servicehost + 'Company/',
  
  Category: servicehost + 'Category/',
  Product: servicehost + 'Product/',
  Customer: servicehost + 'Customer/',
  Supplier: servicehost + 'Supplier/',
  PurchaseOrder: servicehost + 'PurchaseOrder/',
  SalesOrder: servicehost + 'SalesOrder/',
  Report: servicehost + 'Report/',
  
  // Feature flags
  enableCaching: true,
  enableLogging: true,
  
  // Cache settings
  defaultCacheTTL: 300000, // 5 minutes
  
  // Retry settings
  maxRetries: 2,
  
  // Session settings
  sessionTimeout: 3600000, // 1 hour
};