import React, { useEffect, useState } from "react";
import { getAllOrders } from "../../../services/admin/order.service";
import TableOrder from "../../../components/admin/Order/TableOrder";

const OrderAdminPage = () => {
  const [dataOrders, setDataOrders] = useState([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  const [total, setTotal] = useState(0);
  const [filterStatus, setFilterStatus] = useState(null);

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
      />
    </>
  );
};

export default OrderAdminPage;
