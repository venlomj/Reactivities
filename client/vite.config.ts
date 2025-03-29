import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import mkcert from "vite-plugin-mkcert";

export default defineConfig({
  build: {
    outDir: "../API/wwwroot",
    chunkSizeWarningLimit: 1500,
    emptyOutDir: true,
  },
  server: {
    port: 3000,
    https: true,
    host: "localhost",
    hmr: {
      protocol: "wss", // Use secure WebSocket
      host: "localhost", // Match the server host
    },
  },
  plugins: [react(), mkcert()],
});
