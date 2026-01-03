import React, { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { App } from "antd";
import CircularProgress from "@mui/material/CircularProgress";
import Button from "@mui/material/Button";
import {
  FiCheckCircle,
  FiDownload,
  FiArrowRight,
  FiAlertCircle,
} from "react-icons/fi";
import { formatVND, formatDate } from "../../../utils/formatters";
import { getPurchaseByIdService } from "../../../services/student/purchase.service";
import axios from "axios";

const PurchaseSuccessPage = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [orderDetails, setOrderDetails] = useState(null);
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [paymentStatus, setPaymentStatus] = useState(null); // 'success' | 'failed' | 'pending'

  useEffect(() => {
    handleVnPayReturn();
  }, [searchParams]);

  const handleVnPayReturn = async () => {
    try {
      setLoading(true);

      const queryParams = {};
      searchParams.forEach((value, key) => {
        queryParams[key] = value;
      });

      const hasVnPayParams =
        searchParams.has("vnp_TxnRef") || searchParams.has("vnp_ResponseCode");

      if (hasVnPayParams) {
        const vnpayResponse = await axios.get(
          "http://localhost:5224/api/payment/vnpay-return",
          {
            params: queryParams,
          }
        );

        if (vnpayResponse.data.success) {
          setPaymentStatus("success");

          const purchaseId = vnpayResponse.data.purchaseId;
          await fetchPurchaseDetails(purchaseId);
        } else {
          setPaymentStatus("failed");
          setError(vnpayResponse.data.message || "Thanh toán không thành công");
          setLoading(false);
        }
      } else {
        // Trường hợp truy cập trực tiếp với purchaseId
        const purchaseId = searchParams.get("purchaseId");
        if (purchaseId) {
          await fetchPurchaseDetails(purchaseId);
        } else {
          setError("Không tìm thấy thông tin đơn hàng");
          setLoading(false);
        }
      }
    } catch (err) {
      console.error("Error handling VNPay return:", err);
      setPaymentStatus("failed");
      setError(
        err.response?.data?.message || "Có lỗi xảy ra khi xử lý thanh toán"
      );
      setLoading(false);
    }
  };

  const fetchPurchaseDetails = async (purchaseId) => {
    try {
      const response = await getPurchaseByIdService(purchaseId);

      if (response.success && response.data) {
        const purchaseData = response.data;

        if (purchaseData.status === "Completed") {
          setPaymentStatus("success");
        } else if (purchaseData.status === "Failed") {
          setPaymentStatus("failed");
          setError("Thanh toán không thành công");
        } else {
          setPaymentStatus("pending");
        }

        const formattedDetails = {
          purchaseId: purchaseData.id,
          amount: purchaseData.amount,
          purchaseDate: purchaseData.createdAt || new Date().toISOString(),
          userName: purchaseData.userName || "Người dùng",
          email: purchaseData.email || "",
          paymentStatus:
            purchaseData.status === "Completed"
              ? "Thành công"
              : purchaseData.status === "Failed"
              ? "Thất bại"
              : "Đang xử lý",
        };

        setOrderDetails(formattedDetails);

        if (purchaseData.items && purchaseData.items.length > 0) {
          const coursesList = purchaseData.items.map((item) => ({
            id: item.id,
            courseTitle: item.courseTitle || "Khóa học",
            price: item.price,
            discount: item.discount || 0,
          }));
          setCourses(coursesList);
        }
      } else {
        setError(response.message || "Không thể tải thông tin đơn hàng");
        setPaymentStatus("failed");
      }
    } catch (err) {
      console.error("Error fetching purchase details:", err);
      setError("Có lỗi xảy ra khi tải thông tin đơn hàng");
      setPaymentStatus("failed");
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="flex flex-col items-center justify-center min-h-screen gap-4">
        <CircularProgress />
        <p className="text-gray-600">Đang xử lý thanh toán...</p>
      </div>
    );
  }

  // Payment Failed View
  if (paymentStatus === "failed") {
    return (
      <section className="min-h-screen bg-gray-50">
        <div className="container py-12">
          <div className="max-w-2xl mx-auto">
            <div className="bg-white rounded-2xl shadow-lg p-8 md:p-12 text-center">
              <div className="mb-6 flex justify-center">
                <div className="w-24 h-24 bg-red-100 rounded-full flex items-center justify-center">
                  <FiAlertCircle className="w-16 h-16 text-red-500" />
                </div>
              </div>
              <h2 className="text-2xl font-bold text-gray-900 mb-4">
                Thanh toán thất bại
              </h2>
              <p className="text-gray-600 mb-8">
                {error || "Giao dịch không thành công. Vui lòng thử lại."}
              </p>
              <div className="flex flex-col sm:flex-row gap-4 justify-center">
                <Button
                  variant="outlined"
                  size="large"
                  onClick={() => navigate("/khoa-hoc")}
                  sx={{
                    borderColor: "#01579B",
                    color: "#01579B",
                    fontWeight: 600,
                    textTransform: "none",
                    fontSize: "16px",
                    padding: "10px 24px",
                  }}
                >
                  Quay lại khóa học
                </Button>
                <Button
                  variant="contained"
                  size="large"
                  onClick={() => navigate("/gio-hang")}
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
                  Thử lại thanh toán
                </Button>
              </div>
            </div>
          </div>
        </div>
      </section>
    );
  }

  // Error or No Order Details
  if (error || !orderDetails) {
    return (
      <section className="min-h-screen bg-gray-50">
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

  // Success View
  return (
    <section className="min-h-screen fade-in-up bg-gray-50">
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
              Bạn đã đăng ký khóa học thành công và enrollment đã được tạo tự
              động
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

                {courses.length > 0 && (
                  <div className="pb-4 border-b border-gray-200">
                    <span className="text-gray-600 block mb-3">
                      Khóa học đã mua:
                    </span>
                    <div className="space-y-2">
                      {courses.map((course) => (
                        <div
                          key={course.courseId}
                          className="bg-white p-3 rounded-lg border border-gray-100"
                        >
                          <p className="font-semibold text-gray-900">
                            {course.courseTitle}
                          </p>
                          <p className="text-sm text-primary mt-1">
                            {formatVND(
                              course.price -
                                (course.price * course.discount) / 100
                            )}
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
                    {orderDetails?.userName}
                  </span>
                </div>

                <div className="flex justify-between items-center">
                  <span className="text-gray-600">Email:</span>
                  <span className="font-semibold text-gray-900">
                    {orderDetails?.email}
                  </span>
                </div>
              </div>
            </div>

            <div className="flex flex-col sm:flex-row gap-4 justify-center">
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
              Quay lại trang chủ
            </button>
          </div>
        </div>
      </div>
    </section>
  );
};

export default PurchaseSuccessPage;
