import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

export default defineConfig({
    plugins: [plugin()],
    server: {
        port: 2502,
        proxy: {
            '/api': {
                target: 'http://localhost:5178',  
                changeOrigin: true,
                rewrite: path => path.replace(/^\/api/, '') 
            }
        }
    }
});
