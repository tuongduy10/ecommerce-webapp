/* eslint-disable jsx-a11y/anchor-is-valid */
import React, { useState } from "react";
import { MuiIcon } from "src/_shares/_components";

const ProductDetailTabReplyLv2 = (props: any) => {
  const { setOpenUpdateReplyLv2, isOpenUpdateReplyLv2 } = props;
  const [increaseLike, setIncreaseLike] = useState(0);
  const [increaseDisLike, setIncreaseDisLike] = useState(0);

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

  const [isOpenedActionList, setOpenActionList] = useState(false);

  const [isOpenedReplyBox, setOpenReplyBox] = useState(false);

  const [selectedImagesReplyBox, setSelectedImagesReplyBox] = useState<
    { id: number; src: string }[]
  >([]);

  const handleImageUploadReplyBox = (e: { target: { files: any } }) => {
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
              setSelectedImagesReplyBox(imageArray);
            }
          }
        };

        rd.readAsDataURL(files[i]);
      }
    }
  };

  const handleRemoveImageReplyBox = (id: number) => {
    const updatedImages = selectedImagesReplyBox.filter(
      (image) => image.id !== id
    );
    setSelectedImagesReplyBox(updatedImages);
  };

  const handleOpenReplyBox = () => {
    setOpenReplyBox(true);
  };

  const handleCloseReplyBox = () => {
    setOpenReplyBox(false);
  };

  const handleOpenActionList = () => {
    setOpenActionList(!isOpenedActionList);
  };
  return (
    <div
      className={`${!isOpenUpdateReplyLv2 ? "reply" : ""} user-comment mb-2`}
    >
      {/*  USER  */}
      <div className="flex items-center justify-between flex-wrap">
        <div className="flex items-center mb-2">
          <span className="user__comment-name flex items-center">
            <img
              src="https://hihichi.com/images/products/product_c048ab77-33c9-49e6-89e1-051dfb8a9671.jpg"
              alt="Hình ảnh của bạn"
              style={{ width: "30px", height: "30px", borderRadius: "50%" }}
              className="mr-2"
            />
            <strong>Brovu</strong>
          </span>
          <span
            className="user__comment-role ml-2 px-1"
            style={{ whiteSpace: "nowrap" }}
          >
            Quản trị viên
          </span>
        </div>
        <span className="user__comment-time ml-2">19:39, 09/06/2023</span>
      </div>

      {/* COMMENT LV2 */}
      {!isOpenUpdateReplyLv2 && (
        <div className="comment  my-2">
          <span style={{ color: "#288ad9" }}>@Brovu</span> rep
        </div>
      )}

      {/* UPDATE INFO LV2 */}
      {!isOpenedReplyBox && !isOpenUpdateReplyLv2 && (
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
                name="LIKE"
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

            {/* Open reply box, Like, Dislike level 2  */}
            <div
              className={`action-list ${isOpenedActionList ? "" : "hidden"}`}
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
                  onClick={() => {
                    setOpenUpdateReplyLv2(true);
                    setOpenActionList(false);
                  }}
                >
                  <a style={{ color: "var(--blue-dior)" }}>Chỉnh sửa</a>
                </li>
                <li className="border-bottom">
                  <a style={{ color: "red" }}>Xóa</a>
                </li>
                <li>
                  <a onClick={() => setOpenActionList(false)}>Hủy</a>
                </li>
              </ul>
            </div>
          </div>
        </div>
      )}
      <div className={`reply-box ${isOpenedReplyBox ? "" : "hidden"}`}>
        <div className="flex mt-2">
          <textarea
            name=""
            id=""
            placeholder="Bình luận"
            className="flex w-full mb-2 rep-comment-val-lv1"
          ></textarea>
        </div>
        <div className="text-left">
          <div className="upload-image mb-2 mt-2">
            <label htmlFor="image-upload-reply-lv2" className="input-tile mb-2">
              <a className="upload">Chọn ảnh</a>
              <span className="ml-2">Thêm hình ảnh nếu có (Tối đa 3)</span>
            </label>
            <input
              id="image-upload-reply-lv2"
              type="file"
              multiple
              hidden
              onChange={handleImageUploadReplyBox}
            />
            <output className="images-uploaded flex" id="image-reply-preview">
              {selectedImagesReplyBox.map((image) => (
                <div
                  className="image__upload-new-item image__upload-item mr-2 mt-2"
                  key={image.id}
                >
                  <div className="border mb-2 mx-auto">
                    <div
                      className="image-uploaded"
                      style={{
                        position: "relative",
                        width: "95px",
                        height: "95px",
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
                          style={{
                            stroke: "#FFFBF1",
                            fontWeight: "900",
                          }}
                          className="feather feather-x"
                          onClick={() => handleRemoveImageReplyBox(image.id)}
                        />
                      </span>
                    </div>
                  </div>
                </div>
              ))}
            </output>
          </div>
        </div>
        <div className="text-right flex justify-end ">
          <input
            onClick={handleCloseReplyBox}
            type="button"
            value="Hủy"
            style={{
              padding: " 6px 11px",
              cursor: "pointer",
              backgroundColor: "#unset",
              border: "1px solid #333",
              color: "#333",
              marginRight: "4px",
              marginTop: "px",
            }}
          />
          <button type="button">Gửi</button>
        </div>
      </div>
    </div>
  );
};

export default ProductDetailTabReplyLv2;
