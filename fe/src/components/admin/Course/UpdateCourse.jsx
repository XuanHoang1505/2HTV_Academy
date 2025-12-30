import React, { useEffect, useState } from "react";
import {
  App,
  Button,
  Form,
  Input,
  Select,
  Upload,
  InputNumber,
  Divider,
  Space,
} from "antd";
import { PlusOutlined } from "@ant-design/icons";
import { updateCourseByIdService } from "../../../services/admin/course.service";
import { getAllCategoryService } from "../../../services/admin/category.service";

const UpdateCourse = ({ course, onCancel, onSuccess }) => {
  const [form] = Form.useForm();
  const { message, notification } = App.useApp();
  const [loading, setLoading] = useState(false);
  const [categories, setCategories] = useState([]);
  const [loadingCategories, setLoadingCategories] = useState(false);
  const [previewImage, setPreviewImage] = useState(null);

  useEffect(() => {
    loadCategories();
  }, []);

  useEffect(() => {
    if (course) {

      form.setFieldsValue({
        title: course.courseTitle,
        slug: course.slug,
        categoryId: course.categoryId,
        shortDescription: course.shortDescription,
        description: course.courseDescription,
        previewVideo: course.previewVideo,
        level: course.level,
        language: course.language,
        price: course.coursePrice,
        discount: course.discount,
        status: course.status,
      });
      setPreviewImage(course.courseThumbnail || null);
    }
  }, [course, form]);

  const loadCategories = async () => {
    try {
      setLoadingCategories(true);
      const res = await getAllCategoryService(1, 100, "");
      if (res.data) {
        setCategories(res.data);
      }
    } catch (error) {
      console.error("Load categories error:", error);
    } finally {
      setLoadingCategories(false);
    }
  };

  const handleUpdateCourse = async (values) => {
    try {
      setLoading(true);

      const formData = new FormData();

      formData.append("courseTitle", values.title.trim());
      formData.append("slug", values.slug.trim());
      formData.append("categoryId", values.categoryId);
      formData.append("courseDescription", values.description);
      formData.append("coursePrice", values.price);
      formData.append("level", values.level);
      formData.append("shortDescription", values.shortDescription);
      formData.append("discount", values.discount);
      formData.append("status", values.status);
      formData.append("language", values.language);

      if (values.previewVideo && values.previewVideo !== course.previewVideo) {
        formData.append("previewVideo", values.previewVideo);
      }

      // Xử lý upload thumbnail
      if (values.thumbnail?.[0]?.originFileObj) {
        formData.append(
          "courseThumbnailFile",
          values.thumbnail[0].originFileObj
        );
      }

      const res = await updateCourseByIdService(course.id, formData);

      if (res.success) {
        message.success("Cập nhật khóa học thành công");
        onSuccess?.();
      } else {
        notification.error({
          message: "Cập nhật thất bại",
          description: res.message || "Có lỗi xảy ra, vui lòng thử lại",
        });
      }
    } catch (error) {
      console.log("Update Course Error:", error);
      notification.error({
        message: "Cập nhật thất bại",
        description: error.message || "Không thể kết nối đến server",
        duration: 5,
      });
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = () => {
    setPreviewImage(course.courseThumbnail || null);
    form.resetFields();
    onCancel?.();
  };

  const handleFileChange = ({ fileList }) => {
    if (fileList.length > 0) {
      const file = fileList[0].originFileObj;
      const reader = new FileReader();
      reader.onload = (e) => {
        setPreviewImage(e.target.result);
      };
      reader.readAsDataURL(file);
    } else {
      setPreviewImage(course.thumbnail || null);
    }
  };

  const beforeUpload = (file) => {
    const isImage = file.type.startsWith("image/");
    if (!isImage) {
      message.error("Bạn chỉ có thể tải lên file ảnh!");
      return Upload.LIST_IGNORE;
    }
    const isLt2M = file.size / 1024 / 1024 < 2;
    if (!isLt2M) {
      message.error("Ảnh phải nhỏ hơn 2MB!");
      return Upload.LIST_IGNORE;
    }
    return false;
  };

  const categoryOptions = categories.map((cat) => ({
    value: cat.id,
    label: cat.name,
  }));

  return (
    <div className="bg-white rounded-lg p-6">
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold text-gray-800">Chỉnh sửa khóa học</h2>
      </div>

      <Divider />

      <div className="max-h-[600px] overflow-y-auto">
        <Form
          layout="vertical"
          form={form}
          autoComplete="off"
          onFinish={handleUpdateCourse}
          className="space-y-1"
        >
          <div className="mb-6 flex flex-col items-center">
            <div className="mb-4">
              {previewImage ? (
                <img
                  src={previewImage}
                  alt="Thumbnail"
                  className="w-40 h-40 rounded-lg object-cover border-2 border-gray-200"
                />
              ) : (
                <div className="w-40 h-40 rounded-lg bg-gray-200 flex items-center justify-center text-gray-400 text-2xl font-semibold">
                  Không có ảnh
                </div>
              )}
            </div>

            <Form.Item
              name="thumbnail"
              valuePropName="fileList"
              getValueFromEvent={(e) => {
                if (Array.isArray(e)) {
                  return e;
                }
                return e?.fileList;
              }}
              className="mb-0 text-center"
            >
              <Upload
                listType="text"
                maxCount={1}
                beforeUpload={beforeUpload}
                onChange={handleFileChange}
                disabled={loading}
              >
                <Button className="px-4 py-2 rounded-lg" disabled={loading}>
                  <PlusOutlined className="mr-2" />
                  Chọn ảnh thumbnail
                </Button>
              </Upload>
            </Form.Item>
          </div>

          <Form.Item
            name="title"
            label={
              <span className="font-medium text-gray-700">Tên khóa học </span>
            }
            rules={[
              { required: true, message: "Không được để trống tên khóa học" },
            ]}
            className="mb-4"
          >
            <Input
              placeholder="Nhập tên khóa học"
              disabled={loading}
              className="h-10 rounded-lg"
            />
          </Form.Item>

          <Form.Item
            name="slug"
            label={<span className="font-medium text-gray-700">Slug </span>}
            rules={[{ required: true, message: "Không được để trống slug" }]}
            className="mb-4"
          >
            <Input
              placeholder="Nhập slug"
              disabled={loading}
              className="h-10 rounded-lg"
            />
          </Form.Item>

          <Form.Item
            name="categoryId"
            label={<span className="font-medium text-gray-700">Danh mục </span>}
            rules={[{ required: true, message: "Vui lòng chọn danh mục" }]}
            className="mb-4"
          >
            <Select
              placeholder="Chọn danh mục"
              options={categoryOptions}
              loading={loadingCategories}
              disabled={loading || loadingCategories}
              className="h-10"
            />
          </Form.Item>

          <Form.Item
            name="shortDescription"
            label={
              <span className="font-medium text-gray-700">Mô tả ngắn</span>
            }
            className="mb-4"
          >
            <Input.TextArea
              placeholder="Nhập mô tả ngắn"
              disabled={loading}
              rows={2}
            />
          </Form.Item>

          <Form.Item
            name="description"
            label={
              <span className="font-medium text-gray-700">Mô tả chi tiết</span>
            }
            rules={[
              {
                required: true,
                message: "Không được để trống mô tả chi tiết",
              },
            ]}
            className="mb-4"
          >
            <Input.TextArea
              placeholder="Nhập mô tả chi tiết"
              disabled={loading}
              rows={4}
            />
          </Form.Item>

          <Form.Item
            name="previewVideo"
            label={
              <span className="font-medium text-gray-700">
                Link video xem trước
              </span>
            }
            className="mb-4"
          >
            <Input
              placeholder="Nhập link video (YouTube)"
              disabled={loading}
              className="h-10 rounded-lg"
            />
          </Form.Item>

          <div className="grid grid-cols-3 gap-4">
            <Form.Item
              name="level"
              label={<span className="font-medium text-gray-700">Cấp độ </span>}
              rules={[{ required: true, message: "Vui lòng chọn cấp độ" }]}
              className="mb-4"
            >
              <Select
                options={[
                  { value: "beginner", label: "Cơ bản" },
                  { value: "intermediate", label: "Trung cấp" },
                  { value: "advanced", label: "Trình độ cao" },
                ]}
                disabled={loading}
                className="h-10"
              />
            </Form.Item>

            <Form.Item
              name="language"
              label={
                <span className="font-medium text-gray-700">Ngôn ngữ</span>
              }
              className="mb-4"
            >
              <Select
                options={[
                  { value: "vi", label: "Tiếng Việt" },
                  { value: "en", label: "Tiếng Anh" },
                ]}
                disabled={loading}
                className="h-10"
              />
            </Form.Item>

            <Form.Item
              name="status"
              label={
                <span className="font-medium text-gray-700">Trạng thái</span>
              }
              className="mb-4"
            >
              <Select
                options={[
                  { value: "draft", label: "Bản nháp" },
                  { value: "published", label: "Đã xuất bản" },
                  { value: "archived", label: "Đã lưu trữ" },
                ]}
                disabled={loading}
                className="h-10"
              />
            </Form.Item>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <Form.Item
              name="price"
              label={
                <span className="font-medium text-gray-700">Giá gốc (VNĐ)</span>
              }
              rules={[{ required: true, message: "Không được để trống giá" }]}
              className="mb-4"
            >
              <InputNumber
                min={0}
                disabled={loading}
                className="w-full h-10 rounded-lg"
                placeholder="0"
              />
            </Form.Item>

            <Form.Item
              name="discount"
              label={
                <span className="font-medium text-gray-700">Giảm giá (%)</span>
              }
              className="mb-4"
            >
              <InputNumber
                min={0}
                max={100}
                disabled={loading}
                className="w-full h-10 rounded-lg"
                placeholder="0"
              />
            </Form.Item>
          </div>

          <Form.Item className="mb-0">
            <Space>
              <Button
                type="primary"
                htmlType="submit"
                loading={loading}
                size="large"
                className="rounded-lg"
              >
                Cập nhật
              </Button>
              <Button
                size="large"
                onClick={handleCancel}
                disabled={loading}
                className="rounded-lg"
              >
                Hủy bỏ
              </Button>
            </Space>
          </Form.Item>
        </Form>
      </div>
    </div>
  );
};

export default UpdateCourse;
