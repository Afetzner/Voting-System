const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/voting-system",
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:7237',
        secure: false
    });

    app.use(appProxy);
};