const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function(app) {
  app.use(
    '/api',
    createProxyMiddleware({
      target: 'https://localhost:7071',
      changeOrigin: true,
      secure: false, // Důležité pro self-signed certifikáty
      onProxyReq: (proxyReq, req, res) => {
        // Log všech požadavků pro debugging
        console.log('Proxy požadavek:', req.method, req.path);
      },
      onProxyRes: (proxyRes, req, res) => {
        // Log odpovědí
        console.log('Proxy odpověď:', proxyRes.statusCode);
      },
      onError: (err, req, res) => {
        // Zachycení chyb v proxy
        console.error('Proxy chyba:', err);
      }
    })
  );
};