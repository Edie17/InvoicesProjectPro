const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function(app) {
  app.use(
    createProxyMiddleware({
      target: 'https://localhost:7071',
      pathFilter: '/api',
      changeOrigin: true,
      secure: false,
    })
  );
};
