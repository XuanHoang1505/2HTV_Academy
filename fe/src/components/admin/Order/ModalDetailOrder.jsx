import { Modal, Descriptions, Tag, Divider } from "antd";
import React from "react";

const ModalDetailOrder = (props) => {
  const {
    isModalDetailOrderOpen,
    setIsModalDetailOrderOpen,
    dataDetailOrder,
    setDataDetailOrder,
  } = props;

  const handleCloseModal = () => {
    setIsModalDetailOrderOpen(false);
    setDataDetailOrder(null);
  };

  const getStatusConfig = (status) => {
    const statusConfig = {
      Pending: { color: "gold", text: "Chờ thanh toán" },
      Completed: { color: "green", text: "Đã hoàn thành" },
      Cancelled: { color: "red", text: "Đã hủy" },
    };
    return statusConfig[status] || { color: "default", text: status };
  };

  const formatCurrency = (amount) => {
    return Number(amount).toLocaleString("vi-VN") + " ₫";
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleString("vi-VN");
  };

  return (
    <Modal
      title={
        <div className="text-lg font-semibold text-gray-800">
          Chi tiết đơn hàng #{dataDetailOrder?.id}
        </div>
      }
      open={isModalDetailOrderOpen}
      onCancel={handleCloseModal}
      footer={null}
      width={800}
      className="top-8"
      styles={{
        body: { padding: "24px 0" },
      }}
    >
      {dataDetailOrder && (
        <div className="px-6">
          {/* Thông tin đơn hàng */}
          <Descriptions
            bordered
            column={2}
            size="middle"
            labelStyle={{ fontWeight: 600, width: "150px" }}
          >
            <Descriptions.Item label="Mã đơn hàng" span={2}>
              #{dataDetailOrder.id}
            </Descriptions.Item>

            <Descriptions.Item label="Tên học viên" span={2}>
              {dataDetailOrder.userName}
            </Descriptions.Item>

            <Descriptions.Item label="Email" span={2}>
              {dataDetailOrder.email}
            </Descriptions.Item>

            <Descriptions.Item label="User ID" span={2}>
              <span className="text-xs text-gray-600">
                {dataDetailOrder.userId}
              </span>
            </Descriptions.Item>

            <Descriptions.Item label="Tổng tiền" span={1}>
              <span className="font-semibold text-green-600 text-lg">
                {formatCurrency(dataDetailOrder.amount)}
              </span>
            </Descriptions.Item>

            <Descriptions.Item label="Trạng thái" span={1}>
              <Tag color={getStatusConfig(dataDetailOrder.status).color}>
                {getStatusConfig(dataDetailOrder.status).text}
              </Tag>
            </Descriptions.Item>

            <Descriptions.Item label="Ngày tạo" span={2}>
              {formatDate(dataDetailOrder.createdAt)}
            </Descriptions.Item>
          </Descriptions>

          {/* Danh sách khóa học */}
          <Divider orientation="left" className="mt-6 mb-4">
            <span className="font-semibold text-gray-800">
              Danh sách khóa học ({dataDetailOrder.items?.length || 0})
            </span>
          </Divider>

          <div className="space-y-3">
            {dataDetailOrder.items && dataDetailOrder.items.length > 0 ? (
              dataDetailOrder.items.map((item, index) => (
                <div
                  key={item.id}
                  className="bg-gray-50 p-4 rounded-lg border border-gray-200 hover:shadow-md transition-shadow"
                >
                  <div className="flex justify-between items-start">
                    <div className="flex-1">
                      <div className="flex items-center gap-2 mb-2">
                        <span className="bg-primary text-white text-xs px-2 py-1 rounded">
                          #{index + 1}
                        </span>
                        <span className="text-xs text-gray-500">
                          ID: {item.courseId}
                        </span>
                      </div>
                      <h4 className="font-semibold text-gray-900 mb-1">
                        {item.courseTitle}
                      </h4>
                    </div>
                    <div className="text-right">
                      <div className="font-bold text-primary text-lg">
                        {formatCurrency(item.price)}
                      </div>
                    </div>
                  </div>
                </div>
              ))
            ) : (
              <div className="text-center text-gray-500 py-4">
                Không có khóa học nào
              </div>
            )}
          </div>
        </div>
      )}
    </Modal>
  );
};

export default ModalDetailOrder;
