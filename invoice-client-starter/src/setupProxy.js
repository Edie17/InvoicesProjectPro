/**
 * Development server proxy configuration.
 * Routes API requests to the backend server.
 */
const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function(app) {
  app.use(
    '/api',
    createProxyMiddleware({
      target: 'https://localhost:7071',
      changeOrigin: true,
      secure: false, // Important for self-signed certificates
      onProxyReq: (proxyReq, req, res) => {
        // Log all requests for debugging
        console.log('Proxy request:', req.method, req.path);
      },
      onProxyRes: (proxyRes, req, res) => {
        // Log responses
        console.log('Proxy response:', proxyRes.statusCode);
      },
      onError: (err, req, res) => {
        // Catch proxy errors
        console.error('Proxy error:', err);
      }
    })
  );
};