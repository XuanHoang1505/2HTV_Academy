import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { App } from "antd";
import CircularProgress from "@mui/material/CircularProgress";
import Button from "@mui/material/Button";
import { FiCheckCircle, FiDownload, FiArrowRight } from "react-icons/fi";
import { formatVND, formatDate } from "../../../utils/formatters";

const PurchaseSuccessPage = () => {
  const navigate = useNavigate();
  const { message } = App.useApp();
  const [orderDetails, setOrderDetails] = useState(null);
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <CircularProgress />
      </div>
    );
  }

  if (error || !orderDetails) {
    return (
      <section className="min-h-screen bg-gray-50">
        <div className="bg-gradient-to-r from-primary to-primary-dark py-8">
          <div className="container">
            <h1 className="text-3xl font-bold text-white">
              Mua khóa học thành công
            </h1>
          </div>
        </div>

        <div className="container py-12">
          <div className="max-w-2xl mx-auto">
            <div className="bg-white rounded-2xl shadow-lg p-8 md:p-12 text-center">
              <h2 className="text-2xl font-bold text-gray-900 mb-4">
                Không thể tải thông tin đơn hàng
              </h2>
              <p className="text-gray-600 mb-8">
                {error || "Vui lòng thử lại hoặc liên hệ hỗ trợ."}
              </p>
              <Button
                variant="contained"
                size="large"
                onClick={() => navigate("/khoa-hoc")}
                sx={{
                  backgroundColor: "#01579B",
                  fontWeight: 600,
                  textTransform: "none",
                  fontSize: "16px",
                  padding: "10px 24px",
                  "&:hover": {
                    backgroundColor: "#003D6B",
                  },
                }}
              >
                Quay lại khóa học
              </Button>
            </div>
          </div>
        </div>
      </section>
    );
  }

  return (
    <section className="min-h-screen fade-in-up bg-gray-50">
      <div className="bg-gradient-to-r from-primary to-primary-dark py-8">
        <div className="container">
          <h1 className="text-3xl font-bold text-white">
            Mua khóa học thành công
          </h1>
        </div>
      </div>

      <div className="container py-12">
        <div className="max-w-2xl mx-auto">
          <div className="bg-white rounded-2xl shadow-lg p-8 md:p-12 text-center">
            <div className="mb-6 flex justify-center">
              <div className="relative">
                <div className="w-24 h-24 bg-green-100 rounded-full flex items-center justify-center animate-pulse">
                  <FiCheckCircle className="w-16 h-16 text-green-500" />
                </div>
              </div>
            </div>

            <h2 className="text-3xl font-bold text-gray-900 mb-2">
              Chúc mừng!
            </h2>
            <p className="text-lg text-gray-600 mb-8">
              Bạn đã đăng ký khóa học thành công
            </p>

            <div className="bg-gray-50 rounded-xl p-8 mb-8 text-left">
              <h3 className="text-lg font-semibold text-gray-900 mb-6">
                Chi tiết đơn hàng
              </h3>

              <div className="space-y-4">
                <div className="flex justify-between items-center pb-4 border-b border-gray-200">
                  <span className="text-gray-600">Mã đơn hàng:</span>
                  <span className="font-semibold text-gray-900">
                    #{orderDetails?.purchaseId}
                  </span>
                </div>

                <div className="flex justify-between items-center pb-4 border-b border-gray-200">
                  <span className="text-gray-600">Mã giao dịch:</span>
                  <span className="font-semibold text-gray-900">
                    {orderDetails?.transactionId}
                  </span>
                </div>

                {courses.length > 0 && (
                  <div className="pb-4 border-b border-gray-200">
                    <span className="text-gray-600 block mb-3">
                      Khóa học đã mua:
                    </span>
                    <div className="space-y-2">
                      {courses.map((course) => (
                        <div
                          key={course.id}
                          className="bg-white p-3 rounded-lg border border-gray-100"
                        >
                          <p className="font-semibold text-gray-900">
                            {course.name}
                          </p>
                          <p className="text-sm text-primary mt-1">
                            {formatVND(course.price)}
                          </p>
                        </div>
                      ))}
                    </div>
                  </div>
                )}

                <div className="flex justify-between items-center pb-4 border-b border-gray-200">
                  <span className="text-gray-600">Tổng tiền:</span>
                  <span className="text-2xl font-bold text-primary">
                    {formatVND(orderDetails?.amount)}
                  </span>
                </div>

                <div className="flex justify-between items-center pb-4 border-b border-gray-200">
                  <span className="text-gray-600">Ngày mua:</span>
                  <span className="font-semibold text-gray-900">
                    {formatDate(orderDetails?.purchaseDate)}
                  </span>
                </div>

                <div className="flex justify-between items-center pb-4 border-b border-gray-200">
                  <span className="text-gray-600">Trạng thái thanh toán:</span>
                  <span className="font-semibold px-3 py-1 rounded-full text-sm bg-green-100 text-green-800">
                    {orderDetails?.paymentStatus}
                  </span>
                </div>

                <div className="flex justify-between items-center pb-4 border-b border-gray-200">
                  <span className="text-gray-600">Tên học viên:</span>
                  <span className="font-semibold text-gray-900">
                    {orderDetails?.studentName}
                  </span>
                </div>

                <div className="flex justify-between items-center">
                  <span className="text-gray-600">Email:</span>
                  <span className="font-semibold text-gray-900">
                    {orderDetails?.studentEmail}
                  </span>
                </div>
              </div>
            </div>

            <div className="bg-blue-50 border border-blue-200 rounded-xl p-4 mb-8 text-left">
              <p className="text-blue-900 text-sm">
                <strong>📧 Thông báo:</strong> Một email xác nhận đã được gửi
                đến <strong>{orderDetails?.studentEmail}</strong>. Vui lòng kiểm
                tra email để nhận hướng dẫn tiếp theo.
              </p>
            </div>
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <Button
                variant="outlined"
                size="large"
                startIcon={<FiDownload className="text-lg" />}
                onClick={() => {
                  message.info("Tính năng tải hóa đơn sẽ sớm cập nhật");
                }}
                sx={{
                  borderColor: "#01579B",
                  color: "#01579B",
                  fontWeight: 600,
                  textTransform: "none",
                  fontSize: "16px",
                  padding: "10px 24px",
                  "&:hover": {
                    borderColor: "#003D6B",
                    backgroundColor: "#f5f5f5",
                  },
                }}
              >
                Tải hóa đơn
              </Button>

              <Button
                variant="contained"
                size="large"
                endIcon={<FiArrowRight className="text-lg" />}
                onClick={() => navigate("/khoa-hoc-cua-toi")}
                sx={{
                  backgroundColor: "#01579B",
                  fontWeight: 600,
                  textTransform: "none",
                  fontSize: "16px",
                  padding: "10px 24px",
                  "&:hover": {
                    backgroundColor: "#003D6B",
                  },
                }}
              >
                Bắt đầu học tập
              </Button>
            </div>

            <div className="mt-8 pt-8 border-t border-gray-200">
              <p className="text-gray-600 text-sm mb-4">
                Nếu bạn gặp vấn đề hoặc có câu hỏi, vui lòng liên hệ với chúng
                tôi
              </p>
              <div className="flex flex-col sm:flex-row gap-4 justify-center text-sm">
                <button
                  onClick={() => navigate("/lien-he")}
                  className="text-primary hover:text-primary-dark font-semibold transition-colors"
                >
                  Liên hệ hỗ trợ
                </button>
                <span className="text-gray-300 hidden sm:block">|</span>
                <button
                  onClick={() => navigate("/khoa-hoc")}
                  className="text-primary hover:text-primary-dark font-semibold transition-colors"
                >
                  Xem thêm khóa học
                </button>
              </div>
            </div>
          </div>

          <div className="mt-8 text-center">
            <button
              onClick={() => navigate("/")}
              className="text-primary hover:text-primary-dark font-semibold transition-colors inline-flex items-center gap-2"
            >
              ← Quay lại trang chủ
            </button>
          </div>
        </div>
      </div>
    </section>
  );
};

export default PurchaseSuccessPage;
