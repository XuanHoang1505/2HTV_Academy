import { Space, Table, Tag, Button, Popconfirm, message, Input } from "antd";
import { EditOutlined, LockOutlined, UnlockOutlined } from "@ant-design/icons";
import {
  lockUserByIdService,
  unlockUserByIdService,
} from "../../../services/admin/user.service";

const TableUser = (props) => {
  const {
    dataUsers,
    loading,
    loadUser,
    setIsModalUpdateOpen,
    setDataUpdate,
    currentPage,
    setCurrentPage,
    pageSize,
    setPageSize,
    total,
    filterRole,
    setFilterRole,
  } = props;

  console.log("check page:", currentPage, pageSize);

  const columns = [
    {
      title: "STT",
      key: "index",
      width: 60,
      align: "center",
      render: (_, record, index) => {
        return index + 1 + (Number(currentPage) - 1) * Number(pageSize);
      },
    },
    {
      title: "Avatar",
      dataIndex: "imageUrl",
      key: "imageUrl",
      width: 80,
      align: "center",
      render: (avatar) => (
        <img
          src={avatar || "/default-avatar.png"}
          alt="Avatar"
          style={{
            width: 50,
            height: 50,
            borderRadius: "50%",
            objectFit: "cover",
          }}
        />
      ),
    },
    {
      title: "Họ và Tên",
      dataIndex: "fullName",
      key: "fullName",
      width: 180,
      sorter: (a, b) => a.fullName.length - b.fullName.length,
    },
    {
      title: "Email",
      dataIndex: "email",
      key: "email",
      width: 220,
    },
    {
      title: "Số điện thoại",
      dataIndex: "phoneNumber",
      key: "phoneNumber",
      width: 120,
      render: (phone) => phone || "---",
    },
    {
      title: "Vai trò",
      dataIndex: "role",
      key: "role",
      width: 100,
      align: "center",
      filters: [
        { text: "Admin", value: "Admin" },
        { text: "Student", value: "Student" },
      ],
      filteredValue: filterRole ? [filterRole] : null,
      render: (role) => {
        const colors = {
          Admin: "red",
          Student: "blue",
        };
        return <Tag color={colors[role] || "default"}>{role}</Tag>;
      },
    },
    {
      title: "Trạng thái",
      dataIndex: "isLocked",
      key: "isLocked",
      width: 150,
      align: "center",
      render: (isLocked) =>
        isLocked ? (
          <Tag color="#E34324">Đã khóa</Tag>
        ) : (
          <Tag color="#016425">Hoạt động</Tag>
        ),
      sorter: (a, b) => a.isLocked - b.isLocked,
    },
    {
      title: "Hành động",
      key: "action",
      width: 150,
      align: "center",
      fixed: "right",
      render: (_, record) => (
        <Space size="small">
          <Button
            type="link"
            icon={<EditOutlined />}
            onClick={() => handleEdit(record)}
          >
            Sửa
          </Button>
          {record.isLocked ? (
            <Popconfirm
              title="Xác nhận mở khóa?"
              description={`Bạn có chắc muốn mở khóa user "${record.fullName}"?`}
              onConfirm={() => handleUnLockUser(record.id)}
              okText="Mở khóa"
              cancelText="Hủy"
            >
              <Button type="link" icon={<UnlockOutlined />}>
                Mở khóa
              </Button>
            </Popconfirm>
          ) : (
            <Popconfirm
              title="Xác nhận khóa?"
              description={`Bạn có chắc muốn khóa user "${record.fullName}"?`}
              onConfirm={() => handleLockUser(record.id)}
              okText="Khóa"
              cancelText="Hủy"
              okButtonProps={{ danger: true }}
            >
              <Button type="link" danger icon={<LockOutlined />}>
                Khóa
              </Button>
            </Popconfirm>
          )}
        </Space>
      ),
    },
  ];

  const handleEdit = (record) => {
    setDataUpdate(record);
    setIsModalUpdateOpen(true);
  };

  const handleLockUser = async (id) => {
    try {
      await lockUserByIdService(id);
      message.success("Khóa user thành công");
      await loadUser();
    } catch (error) {
      message.error("Khóa user thất bại");
      console.error(error);
    }
  };

  const handleUnLockUser = async (id) => {
    try {
      await unlockUserByIdService(id);
      message.success("Mở khóa user thành công");
      await loadUser();
    } catch (error) {
      message.error("Mở khóa user thất bại");
      console.error(error);
    }
  };

  const onChange = (pagination, filters) => {
    if (filters !== undefined && "role" in filters) {
      const newFilterRole =
        filters.role && filters.role.length > 0 ? filters.role[0] : null;

      if (newFilterRole !== filterRole) {
        setFilterRole(newFilterRole);
        setCurrentPage(1);
      }
    }
    // change current page
    if (pagination && pagination.current) {
      if (pagination.current !== currentPage) {
        setCurrentPage(+pagination.current);
      }
    }
    // change page size
    if (pagination && pagination.pageSize) {
      if (pagination.pageSize !== pageSize) {
        setPageSize(+pagination.pageSize);
      }
    }
  };

  return (
    <div>
      <Table
        dataSource={dataUsers}
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
                <span className="text-primary font-bold">{total}</span> người
                dùng
              </div>
            );
          },
        }}
        onChange={onChange}
      />
    </div>
  );
};

export default TableUser;
