/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: "#01579B",
          light: "#0277BD",
          dark: "#003D6B",
          50: "#E1F5FE",
          100: "#B3E5FC",
          200: "#81D4FA",
          300: "#4FC3F7",
          400: "#29B6F6",
          500: "#01579B",
          600: "#004C8C",
          700: "#003D6B",
          800: "#002D4F",
          900: "#001A2E",
        },
        secondary: "#003D6B",
        accent: "#4CAF50",
      },
      backgroundColor: {
        primary: {
          DEFAULT: "#01579B",
          light: "#0277BD",
          dark: "#003D6B",
          50: "#E1F5FE",
          100: "#B3E5FC",
          200: "#81D4FA",
          300: "#4FC3F7",
          400: "#29B6F6",
          500: "#01579B",
          600: "#004C8C",
          700: "#003D6B",
          800: "#002D4F",
          900: "#001A2E",
        },
        secondary: "#051e34",
      },
      keyframes: {
        float: {
          "0%, 100%": { transform: "translateY(0px)" },
          "50%": { transform: "translateY(-20px)" },
        },
        "float-slow": {
          "0%, 100%": { transform: "translateY(0px)" },
          "50%": { transform: "translateY(-15px)" },
        },
        "float-slower": {
          "0%, 100%": { transform: "translateY(0px)" },
          "50%": { transform: "translateY(-10px)" },
        },
      },
      animation: {
        float: "float 3s ease-in-out infinite",
        "float-slow": "float-slow 4s ease-in-out infinite",
        "float-slower": "float-slower 5s ease-in-out infinite",
      },
    },
  },
  plugins: [],
};
