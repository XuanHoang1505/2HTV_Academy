import React, { useEffect, useState } from "react";
import {
  getAllOrders,
  getOrderById,
} from "../../../services/admin/order.service";
import TableOrder from "../../../components/admin/Order/TableOrder";
import { App } from "antd";
import ModalDetailOrder from "../../../components/admin/Order/ModalDetailOrder";

const OrderAdminPage = () => {
  const [dataOrders, setDataOrders] = useState([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  const [total, setTotal] = useState(0);
  const [filterStatus, setFilterStatus] = useState(null);
  const [isModalDetailOrderOpen, setIsModalDetailOrderOpen] = useState(false);
  const [dataDetailOrder, setDataDetailOrder] = useState(null);
  const { notification } = App.useApp();

  useEffect(() => {
    fetchAllOrders();
  }, [currentPage, pageSize, filterStatus]);

  const fetchAllOrders = async () => {
    try {
      setLoading(true);

      const res = await getAllOrders(currentPage, pageSize, filterStatus);

      if (res.success) {
        setDataOrders(res.data);
        setTotal(res.pagination.total);

        if (res.pagination.currentPage) {
          setCurrentPage(res.pagination.currentPage);
        }
        if (res.pagination.limit) {
          setPageSize(res.pagination.limit);
        }
      } else {
        console.error(res.message);
      }
    } catch (error) {
      console.error("Error fetching orders:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleViewDetail = async (orderId) => {
    try {
      const res = await getOrderById(orderId);

      if (res.success) {
        setDataDetailOrder(res.data);
        setIsModalDetailOrderOpen(true);
      } else {
        notification.error({
          message: "Lỗi",
          description: res.message || "Không thể tải chi tiết đơn hàng",
        });
      }
    } catch (error) {
      console.error("Error fetching order detail:", error);
      notification.error({
        message: "Lỗi",
        description: "Có lỗi xảy ra khi tải chi tiết đơn hàng",
      });
    }
  };

  return (
    <>
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-gray-800 mb-3">
            Danh sách đơn hàng
          </h1>
        </div>
      </div>

      <TableOrder
        dataOrders={dataOrders}
        loading={loading}
        currentPage={currentPage}
        setCurrentPage={setCurrentPage}
        pageSize={pageSize}
        setPageSize={setPageSize}
        total={total}
        filterStatus={filterStatus}
        setFilterStatus={setFilterStatus}
        handleViewDetail={handleViewDetail}
      />

      <ModalDetailOrder
        isModalDetailOrderOpen={isModalDetailOrderOpen}
        setIsModalDetailOrderOpen={setIsModalDetailOrderOpen}
        dataDetailOrder={dataDetailOrder}
        setDataDetailOrder={setDataDetailOrder}
      />
    </>
  );
};

export default OrderAdminPage;
