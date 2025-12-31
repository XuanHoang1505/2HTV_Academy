import { useEffect, useState } from "react";
import PersonIcon from "@mui/icons-material/Person";
import AutoStoriesIcon from "@mui/icons-material/AutoStories";
import MonetizationOnIcon from "@mui/icons-material/MonetizationOn";
import TrendingUpIcon from "@mui/icons-material/TrendingUp";
import {
  LineChart,
  Line,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";
import { Spin, Table } from "antd";
import { getDashboardService } from "../../../services/admin/dashboard.service";

const formatVND = (value) => {
  return new Intl.NumberFormat("vi-VN", {
    style: "currency",
    currency: "VND",
  }).format(value);
};

const DashboardPage = () => {
  const [loading, setLoading] = useState(true);
  const [dashboardData, setDashboardData] = useState({
    totalCourses: 0,
    totalStudents: 0,
    totalRevenue: 0,
    monthlyRevenue: [],
    monthlyStudents: [],
    courseStats: [],
    topCourses: [],
  });

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      const res = await getDashboardService();

      if (res.success && res.dashboard) {
        setDashboardData(res.dashboard);
      }
    } catch (error) {
      console.error("Load dashboard error:", error);
    } finally {
      setLoading(false);
    }
  };

  // Columns cho bảng thống kê khóa học
  const courseStatsColumns = [
    {
      title: "Tên khóa học",
      dataIndex: "courseTitle",
      key: "courseTitle",
      render: (text) => <span className="font-medium">{text}</span>,
    },
    {
      title: "Học viên",
      dataIndex: "students",
      key: "students",
      align: "center",
      render: (value) => (
        <span className="text-primary font-semibold">{value}</span>
      ),
    },
    {
      title: "Doanh thu",
      dataIndex: "revenue",
      key: "revenue",
      align: "right",
      render: (value) => (
        <span className="font-semibold">{formatVND(value)}</span>
      ),
    },
  ];

  // Columns cho top khóa học
  const topCoursesColumns = [
    {
      title: "Top",
      key: "index",
      width: 60,
      align: "center",
      render: (_, __, index) => (
        <span className="inline-flex items-center justify-center w-8 h-8 rounded-full bg-primary text-white font-bold">
          {index + 1}
        </span>
      ),
    },
    {
      title: "Khóa học phổ biến",
      dataIndex: "courseTitle",
      key: "courseTitle",
      render: (text) => <span className="font-medium">{text}</span>,
    },
    {
      title: "Học viên",
      dataIndex: "students",
      key: "students",
      align: "center",
      render: (value) => (
        <span className="text-primary font-semibold">{value}</span>
      ),
    },
    {
      title: "Doanh thu",
      dataIndex: "revenue",
      key: "revenue",
      align: "right",
      render: (value) => (
        <span className="font-semibold">{formatVND(value)}</span>
      ),
    },
  ];

  if (loading) {
    return (
      <div className="flex justify-center items-center h-96">
        <Spin size="large" />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        <div className="bg-white rounded-lg shadow-sm hover:shadow-md transition-shadow duration-300 p-6">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-gray-500 text-sm mb-1">Tổng học viên</p>
              <p className="text-2xl font-bold text-primary">
                {dashboardData.totalStudents}
              </p>
            </div>
            <PersonIcon
              sx={{ width: 50, height: 50 }}
              className="text-primary"
            />
          </div>
        </div>

        <div className="bg-white rounded-lg shadow-sm hover:shadow-md transition-shadow duration-300 p-6">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-gray-500 text-sm mb-1">Tổng khóa học</p>
              <p className="text-2xl font-bold text-primary">
                {dashboardData.totalCourses}
              </p>
            </div>
            <AutoStoriesIcon
              sx={{ width: 50, height: 50 }}
              className="text-primary"
            />
          </div>
        </div>

        <div className="bg-white rounded-lg shadow-sm hover:shadow-md transition-shadow duration-300 p-6 lg:col-span-2">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-gray-500 text-sm mb-1">Tổng doanh thu</p>
              <p className="text-2xl font-bold text-primary">
                {formatVND(dashboardData.totalRevenue)}
              </p>
            </div>
            <MonetizationOnIcon
              sx={{ width: 50, height: 50 }}
              className="text-primary"
            />
          </div>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
        <div className="bg-white rounded-lg shadow-sm p-6">
          <div className="flex items-center gap-2 mb-4">
            <h3 className="font-semibold text-gray-700 text-lg">
              Doanh thu theo tháng
            </h3>
          </div>

          {dashboardData.monthlyRevenue.length === 0 ? (
            <div className="flex justify-center items-center h-[300px] text-gray-400">
              Chưa có dữ liệu doanh thu
            </div>
          ) : (
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={dashboardData.monthlyRevenue}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="month" />
                <YAxis />
                <Tooltip formatter={(value) => formatVND(value)} />
                <Legend />
                <Line
                  type="monotone"
                  dataKey="revenue"
                  stroke="#01579B"
                  name="Doanh thu"
                  strokeWidth={2}
                  dot={{ fill: "#01579B", r: 4 }}
                />
              </LineChart>
            </ResponsiveContainer>
          )}
        </div>

        <div className="bg-white rounded-lg shadow-sm p-6">
          <div className="flex items-center gap-2 mb-4">
            <h3 className="font-semibold text-gray-700 text-lg">
              Đăng ký học viên theo tháng
            </h3>
          </div>

          {dashboardData.monthlyStudents.length === 0 ? (
            <div className="flex justify-center items-center h-[300px] text-gray-400">
              Chưa có dữ liệu học viên
            </div>
          ) : (
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={dashboardData.monthlyStudents}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="month" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Bar
                  dataKey="studentCount"
                  fill="#01579B"
                  name="Số học viên"
                  radius={[8, 8, 0, 0]}
                />
              </BarChart>
            </ResponsiveContainer>
          )}
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
        <div className="bg-white rounded-lg shadow-sm p-6">
          <h3 className="font-semibold text-gray-700 text-lg mb-4">
            Thống kê khóa học
          </h3>
          <Table
            columns={courseStatsColumns}
            dataSource={dashboardData.courseStats}
            rowKey="id"
            pagination={false}
            locale={{
              emptyText: "Chưa có dữ liệu khóa học",
            }}
          />
        </div>

        <div className="bg-white rounded-lg shadow-sm p-6">
          <h3 className="font-semibold text-gray-700 text-lg mb-4">
            Khóa học phổ biến nhất
          </h3>
          <Table
            columns={topCoursesColumns}
            dataSource={dashboardData.topCourses}
            rowKey="id"
            pagination={false}
            locale={{
              emptyText: "Chưa có dữ liệu",
            }}
          />
        </div>
      </div>
    </div>
  );
};

export default DashboardPage;
