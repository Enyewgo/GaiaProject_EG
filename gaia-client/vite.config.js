import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // כאן אנחנו אומרים ל-React: כל פעם שאתה רואה פנייה שמתחילה ב-api/
      // תעביר אותה לשרת ה-C# שרץ בפורט 7235
      "/api": {
        target: "https://localhost:7235",
        changeOrigin: true,
        secure: false, // חשוב מאוד! מאפשר להתעלם מבעיות אבטחה של localhost (SSL)
      },
    },
  },
});
