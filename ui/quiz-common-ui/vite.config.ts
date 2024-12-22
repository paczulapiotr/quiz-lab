import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import { resolve } from "path";
import dts from "vite-plugin-dts";
import { viteStaticCopy } from "vite-plugin-static-copy";

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    dts({ tsconfigPath: "./tsconfig.app.json" }),
    viteStaticCopy({
      targets: [{ src: "prettier.config.js", dest: "./ide" }],
    }),
  ],
  resolve: {
    alias: {
      "@": resolve(__dirname, "./lib"),
    },
  },
  build: {
    lib: {
      entry: {
        main: resolve(__dirname, "lib/main.ts"),
        components: resolve(__dirname, "lib/components/index.ts"),
        hooks: resolve(__dirname, "lib/hooks/index.ts"),
      },
      formats: ["es"],
      fileName: (_format, name) => `${name}.js`,
    },
    sourcemap: true,
    rollupOptions: {
      external: ["react", "react-dom", "react-router", "react/jsx-runtime"],
      output: {
        globals: {
          react: "React",
          "react-dom": "ReactDOM",
          "react-router": "ReactRouter",
          "react/jsx-runtime": "jsxRuntime",
        },
      },
    },
  },
});
