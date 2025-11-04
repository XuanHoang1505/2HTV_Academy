import React from "react";
import Header from "./Header";
import { Outlet } from "react-router-dom";
import Footer from "./footer";

const MainLayout = () => {
  return (
    <>
      <div className="min-h-screen flex flex-col">
        <Header />
        <main className="flex-grow bg-gray-100">
          <Outlet />
        </main>
        <Footer />
      </div>
    </>
  );
};

export default MainLayout;
