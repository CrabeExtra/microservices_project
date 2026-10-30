
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { fileURLToPath } from 'node:url';

const projectRoot = fileURLToPath(new URL('.', import.meta.url));

export default defineConfig({
  server: {
    port: 3001,
    host: true
  },
  plugins: [react()],
  resolve: {
    alias: {
      '@': projectRoot,
    },
  },
});
