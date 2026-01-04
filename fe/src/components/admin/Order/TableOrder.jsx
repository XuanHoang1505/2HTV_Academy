import { Button, Table, Tag } from "antd";
import { EyeOutlined } from "@ant-design/icons";

const TableOrder = (props) => {
  const {
    dataOrders,
    loading,
    currentPage,
    setCurrentPage,
    pageSize,
    setPageSize,
    total,
    filterStatus,
    setFilterStatus,
    handleViewDetail,
  } = props;

  const columns = [
    {
      title: "ID đơn hàng",
      dataIndex: "id",
      key: "id",
      width: 120,
      align: "center",
    },
    {
      title: "User ID",
      dataIndex: "userId",
      key: "userId",
      width: 300,
      ellipsis: true,
    },
    {
      title: "Tổng tiền",
      dataIndex: "amount",
      key: "amount",
      width: 150,
      align: "right",
      render: (amount) => (
        <span className="font-semibold text-green-600">
          {Number(amount).toLocaleString("vi-VN")} ₫
        </span>
      ),
      sorter: (a, b) => Number(a.amount) - Number(b.amount),
    },
    {
      title: "Trạng thái",
      dataIndex: "status",
      key: "status",
      width: 140,
      align: "center",
      render: (status) => {
        const statusConfig = {
          Pending: { color: "gold", text: "Chờ thanh toán" },
          Completed: { color: "green", text: "Đã hoàn thành" },
          Cancelled: { color: "red", text: "Đã hủy" },
        };
        const config = statusConfig[status] || {
          color: "default",
          text: status,
        };
        return <Tag color={config.color}>{config.text}</Tag>;
      },
    },
    {
      title: "Ngày tạo",
      dataIndex: "createdAt",
      key: "createdAt",
      width: 180,
      render: (date) => new Date(date).toLocaleString("vi-VN"),
      sorter: (a, b) => new Date(a.createdAt) - new Date(b.createdAt),
    },
    {
      title: "Hành động",
      key: "action",
      align: "center",
      width: 180,
      render: (_, record) => (
        <Button
          type="primary"
          size="small"
          icon={<EyeOutlined />}
          className="rounded-lg"
          onClick={() => handleViewDetail(record.id)}
        >
          Chi Tiết
        </Button>
      ),
    },
  ];

  const onChange = (pagination, filters) => {
    if (filters !== undefined && "status" in filters) {
      const newFilterStatus =
        filters.status && filters.status.length > 0 ? filters.status[0] : null;

      if (newFilterStatus !== filterStatus) {
        setFilterStatus(newFilterStatus);
        setCurrentPage(1);
      }
    }

    if (pagination && pagination.current) {
      if (pagination.current !== currentPage) {
        setCurrentPage(+pagination.current);
      }
    }

    if (pagination && pagination.pageSize) {
      if (pagination.pageSize !== pageSize) {
        setPageSize(+pagination.pageSize);
        setCurrentPage(1);
      }
    }
  };

  return (
    <div>
      <Table
        dataSource={dataOrders}
        columns={columns}
        rowKey="id"
        loading={loading}
        pagination={{
          current: currentPage,
          pageSize: pageSize,
          showSizeChanger: true,
          total: total,
          showTotal: (total, range) => {
            return (
              <div>
                {range[0]} - {range[1]} trên{" "}
                <span className="text-primary font-bold">{total}</span> đơn hàng
              </div>
            );
          },
        }}
        onChange={onChange}
        scroll={{ x: 1100 }}
      />
    </div>
  );
};

export default TableOrder;
