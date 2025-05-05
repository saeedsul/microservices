import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
    plugins: [react()],
    server: {
        port: 2502,
        proxy: {
            '/api': {
                target: 'http://localhost:5178', // Plain HTTP
                changeOrigin: true
            }
        }
    }
});
