let servicehost = 'http://localhost:5087/';

export const environment = {
  production: true,

  // API Base URLs
  apiUrl: servicehost,
  User: 'https://api.yourproduction.com/api/User/',

  Role: servicehost + 'Roles/',
  Branch: servicehost + 'Branch/',
  Company: servicehost + 'Company/',



  // Feature flags
  enableCaching: true,
  enableLogging: false,

  // Cache settings
  defaultCacheTTL: 300000, // 5 minutes

  // Retry settings
  maxRetries: 3,

  // Session settings
  sessionTimeout: 3600000, // 1 hour
};