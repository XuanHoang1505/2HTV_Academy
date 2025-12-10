import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import router from "./routes/index.jsx";
import { RouterProvider } from "react-router-dom";
import { App, ConfigProvider } from "antd";
import { AuthWrapper } from "./contexts/auth.context.jsx";
import viVN from "antd/locale/vi_VN";
import { createTheme, ThemeProvider } from "@mui/material/styles";

const muiTheme = createTheme({
  palette: {
    primary: {
      main: "#01579B",
    },
  },
});

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <ConfigProvider
      locale={viVN}
      theme={{ token: { colorPrimary: "#01579B" } }}
    >
      <ThemeProvider theme={muiTheme}>
        <AuthWrapper>
          <App>
            <RouterProvider router={router} />
          </App>
        </AuthWrapper>
      </ThemeProvider>
    </ConfigProvider>
  </StrictMode>
);
