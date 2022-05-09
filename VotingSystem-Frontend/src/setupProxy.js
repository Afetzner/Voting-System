import { createProxyMiddleware } from "http-proxy-middleware";

const context = [
  "/voting-system",
];

export default function (app) {
  const appProxy = createProxyMiddleware(context, {
    target: "https://localhost:7237",
    secure: false
  });
  app.use(appProxy);
}
