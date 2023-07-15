/* eslint-disable jsx-a11y/anchor-is-valid */
import React, { useState } from "react";
import { MuiIcon } from "src/_shares/_components";

const ReplyBoxLv1 = (props: { handleCloseReplyBox: any }) => {
  const { handleCloseReplyBox } = props;
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
  return (
    <>
      <div className="flex mt-2">
        <textarea
          name=""
          id=""
          placeholder="Bình luận lv1"
          className="flex w-full mb-2 rep-comment-val-lv1"
        ></textarea>
      </div>
      <div className="text-left">
        <div className="upload-image mb-2 mt-2">
          <label htmlFor="image-upload-reply" className="input-tile mb-2">
            <a className="upload">Chọn ảnh</a>
            <span className="ml-2">Thêm hình ảnh nếu có (Tối đa 3)</span>
          </label>
          <input
            id="image-upload-reply"
            type="file"
            multiple
            hidden
            onChange={handleImageUploadReplyBox}
          />
          <output
            className="images-uploaded flex mt-2"
            id="image-reply-preview"
          >
            {selectedImagesReplyBox.map((image) => (
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
    </>
  );
};

export default ReplyBoxLv1;
