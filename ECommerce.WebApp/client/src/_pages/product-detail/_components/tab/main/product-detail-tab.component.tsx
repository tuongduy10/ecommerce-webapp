/* eslint-disable jsx-a11y/anchor-has-content */
import { useEffect, useState } from "react";
import { MuiIcon } from "src/_shares/_components";
import ReplyBoxLv1 from "../reply-box-lv1.component";
import ReplyBoxLv2 from "../reply-box-lv2.component";
import ImageCommentDialog from "src/_pages/product-detail/_components/dialog/product-detail-img-comment.dialog";
import UpdateReplyLv1 from "../update/update-reply-lv1.component";
import UpdateReplyLv2 from "../update/update-reply-lv2.component";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";
import UserComment from "../comment-actions/user-comment.component";

/* eslint-disable jsx-a11y/anchor-is-valid */
const ProductDetailTab = () => {
  const [activeTab, setActiveTab] = useState(0);
  const [checkStar, setCheckStar] = useState(0);
  const [selectedImages, setSelectedImages] = useState<
    { id: number; src: string }[]
  >([]);

  const [isOpenedActionList, setOpenActionList] = useState(false);

  const [isOpenedReplyBox, setOpenReplyBox] = useState(false);

  const [increaseLike, setIncreaseLike] = useState(0);
  const [increaseDisLike, setIncreaseDisLike] = useState(0);

  const [openDialog, setOpenDialog] = useState(false);

  const [isOpenUpdateCommentInfo, setOpenUpdateCommentInfo] = useState(false);
  const [isOpenUpdateReplyLv2, setOpenUpdateReplyLv2] = useState(false);

  const handleCloseUpdateReplyLv2 = () => {
    setOpenUpdateReplyLv2(false);
  };

  const handleOpenUpdateCommentInfo = () => {
    setOpenUpdateCommentInfo(true);
    setOpenActionList(false);
  };

  const handleCloseUpdateCommentInfo = () => {
    setOpenUpdateCommentInfo(false);
  };

  const handleClickOpenDialog = () => {
    setOpenDialog(true);
  };

  const handleClickCloseDialog = () => {
    setOpenDialog(false);
  };

  const handleIncreaseLike = () => {
    setIncreaseLike(increaseLike + 1);
  };

  const handleDecreaseLike = () => {
    setIncreaseLike(increaseLike - 1);
  };

  const handleIncreaseDisLike = () => {
    setIncreaseDisLike(increaseDisLike + 1);
  };

  const handleDecreaseDisLike = () => {
    setIncreaseDisLike(increaseDisLike - 1);
  };

  const handleOpenReplyBox = () => {
    setOpenReplyBox(true);
  };

  const handleCloseReplyBox = () => {
    setOpenReplyBox(false);
  };

  const handleRateClick = (i: number) => {
    setCheckStar(i + 1);
  };

  useEffect(() => {
    const tabs = document.querySelectorAll(".control-tab");
    tabs.forEach((tab, index) => {
      if (index === activeTab) {
        tab.classList.add("active");
      } else {
        tab.classList.remove("active");
      }
    });
  }, [activeTab]);

  const handleImageUpload = (e: { target: { files: any } }) => {
    const files = e.target.files;
    const imageArray: any = [];

    for (let i = 0; i < files.length; i++) {
      if (i < 3) {
        const rd = new FileReader();

        rd.onload = (e) => {
          if (e.target) {
            const imageObject = {
              id: i,
              src: e.target.result as string,
            };

            imageArray.push(imageObject);

            if (imageArray.length === Math.min(files.length, 3)) {
              setSelectedImages(imageArray);
            }
          }
        };

        rd.readAsDataURL(files[i]);
      }
    }
  };

  const handleRemoveImage = (id: number) => {
    const updatedImages = selectedImages.filter((image) => image.id !== id);
    setSelectedImages(updatedImages);
  };

  const handleOpenActionList = () => {
    setOpenActionList(!isOpenedActionList);
  };

  return (
    <div className="product__detail-tab">
      <ImageCommentDialog
        openDialog={openDialog}
        handleClickCloseDialog={handleClickCloseDialog}
      />
      <div className="product__tab-control">
        <ul className="flex justify-around">
          <li className="mx-2">
            <a
              className={`control-tab detail-title ${
                activeTab === 0 ? "active" : ""
              }`}
              onClick={() => setActiveTab(0)}
            >
              Thông tin sản phẩm
            </a>
          </li>
          <li className="mx-2">
            <a
              className={`control-tab detail-title ${
                activeTab === 1 ? "active" : ""
              }`}
              onClick={() => setActiveTab(1)}
            >
              Đánh giá sản phẩm
            </a>
          </li>
        </ul>
      </div>
      <div className="product__tab-content">
        {activeTab === 0 && (
          <div className="content-tab tab__product-info active">
            <div className="product-detail">
              <ul className="detail-list flex mb-4 flex-wrap justify-between">
                <li className="detail-item">
                  <span>
                    <b>Chất liệu</b>
                  </span>
                  <span>Thủy tinh cao cấp</span>
                </li>
              </ul>
              <div className="title">Mô tả ngắn</div>
              <div className="description"></div>
            </div>
          </div>
        )}
        {activeTab === 1 && (
          <div className="content-tab tab__product-comment">
            <div className="product-comment">
              {/* NOT BE USER */}

              {/* <div className="text-center mb-4">
                <button className="mx-auto">
                  <div
                    style={{ marginBottom: "10px" }}
                    className="flex  items-center "
                  >
                    <span
                      style={{
                        height: "24px",
                        marginRight: "10px",
                        position: "relative",
                      }}
                    >
                      <MuiIcon className="feather feather-edit-3" name="EDIT" />
                      <CustomIcon />
                    </span>
                    <p style={{ height: "16px" }}>Viết bình luận</p>
                  </div>
                </button>
              </div> */}

              {/* USER  */}
              <div className="tab__content-block">
                <div className="flex">
                  <span className="input-title mx-auto">
                    VIẾT NHẬN XÉT CỦA BẠN BÊN DƯỚI
                  </span>
                </div>
                {/* <div className="rating flex mb-2 items-center">
                  <span>Đánh giá</span>
                  <div className="stars flex ml-2">
                    {[...Array(5)].map((_, index) => (
                      <MuiIcon
                        key={index}
                        name={ICON_NAME.FEATHER.STAR}
                        className={`rate-value`}
                        onClick={() => handleRateClick(index)}
                        style={{
                          fontSize: "1.6rem",
                          marginBottom: "2px",
                          cursor: "pointer",
                        }}
                        fill={index < checkStar ? 'orange' : 'none'}
                      />
                    ))}
                  </div>
                </div>
                <div className="flex">
                  <textarea
                    className="comment-val flex w-full mb-2"
                    name=""
                    id=""
                    placeholder="Nhận xét của bạn về sản phẩm này"
                  ></textarea>
                </div>
                <div className="upload-image mb-2 mt-2">
                  <label htmlFor="image-upload" className="input-tile mb-2">
                    <a className="upload">Chọn ảnh</a>
                    <span className="ml-2">
                      Thêm hình ảnh sản phẩm nếu có (Tối đa 3)
                    </span>
                  </label>
                  <input
                    id="image-upload"
                    type="file"
                    multiple
                    hidden
                    onChange={handleImageUpload}
                  />
                  <output className="flex mt-2" id="image-preview">
                    {selectedImages.map((image) => (
                      <div className="image__upload-item" key={image.id}>
                        <div
                          className="image-uploaded"
                          style={{
                            position: "relative",
                            width: "100px",
                            height: "100px",
                            marginRight: "4px",
                          }}
                        >
                          <img
                            src={image.src}
                            alt="Ảnh của bạn"
                            style={{
                              height: "100%",
                              width: "100%",
                              objectFit: "contain",
                            }}
                          />
                          <span
                            className="remove__upload"
                            style={{
                              position: "absolute",
                              top: "0",
                              right: "0",
                              cursor: "pointer",
                              backgroundColor: "#B22B27",
                              borderRadius: "50%",
                            }}
                          >
                            <MuiIcon
                              name="X"
                              style={{ stroke: "#FFFBF1", fontWeight: "900" }}
                              className="feather feather-x"
                              onClick={() => handleRemoveImage(image.id)}
                            />
                          </span>
                        </div>
                      </div>
                    ))}
                  </output>
                </div>
                <div className="text-right">
                  <button className="post-comment" type="button">
                    Gửi bình luận
                  </button>
                </div> */}
                <UserComment type="post" />
              </div>

              {/* COMMENT */}
              <div className="tab__content-block block">
                <div className="user-comment mb-20">
                  <UserComment type='comment' />
                  <div className="listreply mt-2">
                    <UserComment type='reply' />
                    <UserComment type='reply' />
                    <UserComment type='reply' />
                  </div>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default ProductDetailTab;
