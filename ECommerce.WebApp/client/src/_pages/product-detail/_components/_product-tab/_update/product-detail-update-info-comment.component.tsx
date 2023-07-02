/* eslint-disable jsx-a11y/anchor-is-valid */
import { useState } from "react";
import { MuiIcon } from "src/_shares/_components";

const ProductDetailUpdateInfoComment = (props: any) => {
  const { handleCloseUpdateCommentInfo } = props;
  const [checkStar, setCheckStar] = useState(0);
  const [selectedImages, setSelectedImages] = useState<
    { id: number; src: string }[]
  >([]);

  const handleRateClick = (i: number) => {
    setCheckStar(i + 1);
  };

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
  return (
    <>
      <div className="rating flex mb-2 items-center">
        <span>Đánh giá</span>
        <div className="stars ml-2">
          {[...Array(5)].map((_, index) => (
            <MuiIcon
              key={index}
              name="STAR"
              className={`rate-value ${index < checkStar ? "checked" : ""}`}
              onClick={() => handleRateClick(index)}
              style={{ fontSize: "1.6rem", marginBottom: "2px" }}
            />
          ))}
        </div>
      </div>
      <div className="flex">
        <textarea
          className="comment-val flex w-full mb-2"
          name=""
          id=""
          placeholder="Bình luận"
        ></textarea>
      </div>
      <div className="upload-image mb-2  mt-2">
        <label htmlFor="image-new-upload" className="input-tile mb-2">
          <a className="upload">Chọn ảnh</a>
          <span className="ml-2">Thêm hình ảnh nếu có (Tối đa 3)</span>
        </label>
        <input
          id="image-new-upload"
          className="file-upload"
          type="file"
          multiple
          hidden
          onChange={handleImageUpload}
        />
        <output className="flex" id="image-preview">
          {selectedImages.map((image) => (
            <div
              className="image__upload-new-item"
              key={image.id}
              style={{ margin: "30px 0" }}
            >
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
                    style={{
                      stroke: "#FFFBF1",
                      fontWeight: "900",
                    }}
                    className="feather feather-x"
                    onClick={() => handleRemoveImage(image.id)}
                  />
                </span>
              </div>
            </div>
          ))}
        </output>
      </div>
      <div className="text-right flex justify-end ">
        <input
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
          onClick={handleCloseUpdateCommentInfo}
        />
        <button type="button">Cập nhật</button>
      </div>
    </>
  );
};

export default ProductDetailUpdateInfoComment;
