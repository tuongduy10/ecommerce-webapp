/* eslint-disable jsx-a11y/anchor-has-content */
import { useEffect, useState } from "react";
import { MuiIcon } from "src/_shares/_components";
import ProductDetailTabReplyBox from "../product-detail-tab-reply-box.component";
import ProductDetailTabReplyLv2 from "../product-detail-tab-reply-lv2.component";
import ImageCommentDialog from "src/_pages/product-detail/_dialogs/product-detail-img-comment.dialog";
import ProductDetailUpdateInfoComment from "../_update/product-detail-update-info-comment.component";
import ProductDetailUpdateReplyLv2 from "../_update/product-detail-update-reply-lv2.component";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";

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
                <div className="rating flex mb-2 items-center">
                  <span>Đánh giá</span>
                  <div className="stars ml-2">
                    {[...Array(5)].map((_, index) => (
                      <MuiIcon
                        key={index}
                        name="STAR"
                        className={`rate-value ${
                          index < checkStar ? "checked" : ""
                        }`}
                        onClick={() => handleRateClick(index)}
                        style={{
                          fontSize: "1.6rem",
                          marginBottom: "2px",
                          cursor: "pointer",
                        }}
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
                </div>
              </div>

              {/* COMMENT BLOCK LV1 */}

              <div className="tab__content-block block">
                <div className="user-comment mb-20">
                  {/*  USER LV1 */}

                  <div className="flex items-center justify-between flex-wrap">
                    <div className="flex items-center mb-2">
                      <span className="user__comment-name mr-2">
                        <strong>Brovu</strong>
                      </span>
                      <span
                        className="user__comment-role px-1 mr-2"
                        style={{ whiteSpace: "nowrap" }}
                      >
                        Quản trị viên
                      </span>
                    </div>
                    <span
                      className="user__comment-time ml-2 mb-2"
                      style={{ whiteSpace: "nowrap" }}
                    >
                      14:02, 08/06/2023
                    </span>
                  </div>

                  {/* STAR LV1 */}
                  {!isOpenUpdateCommentInfo && (
                    <div>
                      <div className="flex stars">
                        {[...Array(5)].map((_, index) => (
                          <MuiIcon
                            key={index}
                            name="STAR"
                            className="fa fa-star checked "
                            style={{
                              fontSize: "1.4rem",
                            }}
                          />
                        ))}
                      </div>
                      {/* COMMENT LV1 */}

                      <div className="comment my-2">Đẹp quá</div>
                      {/* IMAGES LV1 */}
                      <div className="image images-comment mb-2">
                        <ul className="image-list flex">
                          <li className="border">
                            <img
                              src="https://hihichi.com/images/products/product_c048ab77-33c9-49e6-89e1-051dfb8a9671.jpg"
                              alt="Ảnh của bạn"
                              onClick={handleClickOpenDialog}
                            />
                          </li>
                          <li className="border">
                            <img
                              src="https://hihichi.com/images/products/product_c048ab77-33c9-49e6-89e1-051dfb8a9671.jpg"
                              alt="Ảnh của bạn"
                              onClick={handleClickOpenDialog}
                            />
                          </li>
                        </ul>
                      </div>
                      {/* ANSWER/LIKE/DISLIKE/CANCEL LV1 */}
                      {!isOpenedReplyBox && (
                        <div className="flex items-center mb-2 reply-action action-box ">
                          <a
                            className="mr-2"
                            style={{ color: "#288ad9", cursor: "pointer" }}
                            onClick={handleOpenReplyBox}
                          >
                            Trả lời
                          </a>
                          <a
                            className="favor flex items-center"
                            style={{
                              cursor: "default",
                              marginRight: "5px",
                              height: "18px",
                              color: "#288ad9",
                            }}
                          >
                            {increaseLike === 0 && (
                              <MuiIcon
                                name={ICON_NAME.FEATHER.THUMBS_UP}
                                className="feather feather-thumbs-up like-svg"
                                style={{ cursor: "pointer" }}
                                onClick={handleIncreaseLike}
                              />
                            )}
                            {increaseLike > 0 && (
                              <MuiIcon
                                name="LIKE_FILL"
                                className="feather feather-thumbs-up like-svg"
                                style={{ cursor: "pointer" }}
                                onClick={handleDecreaseLike}
                              />
                            )}
                            <span
                              className="user__comment-time ml-2 like count"
                              style={{ color: "#707070" }}
                            >
                              {increaseLike}
                            </span>
                          </a>
                          <a
                            className="favor flex items-center"
                            style={{
                              cursor: "default",
                              marginRight: "5px",
                              height: "18px",
                              color: "#288ad9",
                            }}
                          >
                            {increaseDisLike === 0 && (
                              <MuiIcon
                                name="DISLIKE"
                                className="feather feather-thumbs-down dislike-svg"
                                style={{ cursor: "pointer" }}
                                onClick={handleIncreaseDisLike}
                              />
                            )}
                            {increaseDisLike > 0 && (
                              <MuiIcon
                                name="DISLIKE_FILL"
                                className="feather feather-thumbs-down dislike-svg"
                                style={{ cursor: "pointer" }}
                                onClick={handleDecreaseDisLike}
                              />
                            )}
                            <span
                              className="user__comment-time ml-2 dislike count"
                              style={{ color: "#707070" }}
                            >
                              {increaseDisLike}
                            </span>
                          </a>
                          <div style={{ position: "relative" }}>
                            <a
                              className="favor flex items-center"
                              onClick={handleOpenActionList}
                              style={{
                                marginRight: "5px",
                                height: "18px",
                              }}
                            >
                              <span
                                className="feather feather-more-horizontal"
                                style={{
                                  borderRadius: "50%",
                                  background: "#ddd",
                                  border: "1px solid #ddd",
                                  padding: "3px",
                                  position: "relative",
                                  width: "27px",
                                  height: "24px",
                                  cursor: "pointer",
                                }}
                              >
                                <MuiIcon
                                  name="MORE_HORIZ"
                                  style={{
                                    position: "absolute",
                                    top: "0",
                                    left: "0",
                                  }}
                                />
                              </span>
                            </a>
                            <div
                              className={`action-list ${
                                isOpenedActionList ? "" : "hidden"
                              }`}
                              style={{
                                position: "absolute",
                                width: "126px",
                                background: "#fff",
                                border: "1px solid #ddd",
                                borderRadius: "10px",
                                top: "24px",
                                left: "5px",
                                zIndex: "1",
                                cursor: "pointer",
                              }}
                            >
                              <ul className="text-center">
                                <li
                                  className="border-bottom"
                                  onClick={handleOpenUpdateCommentInfo}
                                >
                                  <a style={{ color: "var(--blue-dior)" }}>
                                    Chỉnh sửa
                                  </a>
                                </li>
                                <li className="border-bottom">
                                  <a style={{ color: "red" }}>Xóa</a>
                                </li>
                                <li>
                                  <a onClick={() => setOpenActionList(false)}>
                                    Hủy
                                  </a>
                                </li>
                              </ul>
                            </div>
                          </div>
                        </div>
                      )}
                    </div>
                  )}

                  {/* UPDATE INFO LV1 */}

                  {isOpenUpdateCommentInfo && (
                    <div className="update-comment my-2">
                      <ProductDetailUpdateInfoComment
                        handleCloseUpdateCommentInfo={
                          handleCloseUpdateCommentInfo
                        }
                      />
                    </div>
                  )}

                  {/*  REPLY BOX LV1 */}
                  <div
                    className={`reply-box ${isOpenedReplyBox ? "" : "hidden"}`}
                  >
                    <ProductDetailTabReplyBox
                      handleCloseReplyBox={handleCloseReplyBox}
                    />
                  </div>
                  <div className="listreply mt-2">
                    <ProductDetailTabReplyLv2
                      setOpenUpdateReplyLv2={setOpenUpdateReplyLv2}
                      isOpenUpdateReplyLv2={isOpenUpdateReplyLv2}
                    />
                    {isOpenUpdateReplyLv2 && (
                      <div className="update-comment mb-2">
                        <ProductDetailUpdateReplyLv2
                          handleCloseUpdateReplyLv2={handleCloseUpdateReplyLv2}
                        />
                      </div>
                    )}
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
