import { useState } from "react";
import { IUserComment } from "src/_pages/product-detail/_interfaces/comment.interface";
import { MuiIcon } from "src/_shares/_components";
import { ICON_NAME } from "src/_shares/_components/mui-icon/_enums/mui-icon.enum";

const UserComment = (props: IUserComment) => {
    const { type } = props;
    const [selectedImages, setSelectedImages] = useState<{ id: number; src: string }[]>([]);
    const [enableActions, setEnableActions] = useState<boolean>(false);
    const [enableEdit, setEnableEdit] = useState<boolean>(false);
    const [enableReply, setEnableReply] = useState<boolean>(false);
    const [likeDislike, setLikeDislike] = useState<'' | 'like' | 'dislike'>('');

    const onEnableEdit = () => {
        setEnableReply(true);
        setEnableEdit(true);
        setEnableActions(false);
    }

    const onDisableEdit = () => {
        setEnableReply(false);
        setEnableEdit(false);
    }

    const toggleLikeDislike = (val = '' as '' | 'like' | 'dislike') => {
        if (val === likeDislike) {
            setLikeDislike('');
        } else {
            setLikeDislike(val);
        }
    }

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
        <div className={type === 'reply' ? 'reply mb-2' : ''}>
            {type === 'post' ? null : (
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
            )}
            {/* STAR LV1 */}
            {['post', 'comment'].includes(type) && (
                <div className="flex stars">
                    {(type === 'post' || enableEdit) && (
                        <span className="mr-2">Đánh giá</span>
                    )}
                    {[...Array(5)].map((_, index) => (
                        <MuiIcon
                            key={index}
                            fill="orange"
                            name={ICON_NAME.FEATHER.STAR}
                            style={{ fontSize: "1.4rem" }}
                        />
                    ))}
                </div>
            )}
            {!enableEdit && type !== 'post' && (
                <div>
                    {/* COMMENT LV1 */}
                    <div className="comment my-2">
                        {type === 'reply' && <span style={{ color: "#288ad9" }}>@Brovu </span>}
                        rep
                    </div>
                    {/* IMAGES LV1 */}
                    <div className="image images-comment mb-2">
                        <ul className="image-list flex">
                            <li className="border">
                                <img
                                    src="https://hihichi.com/images/products/product_c048ab77-33c9-49e6-89e1-051dfb8a9671.jpg"
                                    alt="Ảnh của bạn"
                                />
                            </li>
                            <li className="border">
                                <img
                                    src="https://hihichi.com/images/products/product_c048ab77-33c9-49e6-89e1-051dfb8a9671.jpg"
                                    alt="Ảnh của bạn"
                                />
                            </li>
                        </ul>
                    </div>
                </div>
            )}
            {!enableReply && type !== 'post' && (
                <div className="flex items-center mb-2 reply-action action-box">
                    <a
                        className="mr-2"
                        style={{ color: "#288ad9", cursor: "pointer" }}
                        onClick={() => setEnableReply(true)}
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
                        onClick={() => toggleLikeDislike('like')}
                    >
                        <MuiIcon
                            name={ICON_NAME.FEATHER.THUMBS_UP}
                            className="cursor-pointer"
                            fill={likeDislike === 'like' ? 'currentColor' : 'none'}
                        />
                        <span className="user__comment-time ml-2 like count" style={{ color: "#707070" }}>
                            10
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
                        onClick={() => toggleLikeDislike('dislike')}
                    >
                        <MuiIcon
                            name={ICON_NAME.FEATHER.THUMBS_DOWN}
                            className="cursor-pointer"
                            fill={likeDislike === 'dislike' ? 'currentColor' : 'none'}
                        />
                        <span
                            className="user__comment-time ml-2 dislike count"
                            style={{ color: "#707070" }}
                        >
                            5
                        </span>
                    </a>
                    <div style={{ position: "relative" }}>
                        <a
                            className="favor flex items-center"
                            style={{
                                marginRight: "5px",
                                height: "18px",
                            }}
                            onClick={() => setEnableActions(toggle => !toggle)}
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
                        {enableActions && (
                            <div
                                className={`action-list`}
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
                                    <li className="border-bottom">
                                        <a className="text-[var(--blue-dior)]"
                                            onClick={onEnableEdit}
                                        >Chỉnh sửa</a>
                                    </li>
                                    <li className="border-bottom">
                                        <a className="text-[red]">Xóa</a>
                                    </li>
                                    <li>
                                        <a onClick={() => setEnableActions(false)}>Hủy</a>
                                    </li>
                                </ul>
                            </div>
                        )}
                    </div>
                </div>
            )}
            {(enableReply || enableEdit || type === 'post') && (<>
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
                        <label htmlFor="image-upload-reply" className="input-tile mb-2">
                            <span className="upload">Chọn ảnh</span>
                            <span className="ml-2">Thêm hình ảnh nếu có (Tối đa 3)</span>
                        </label>
                        <input
                            id="image-upload-reply"
                            type="file"
                            multiple
                            hidden
                            onChange={handleImageUpload}
                        />
                        <output
                            className="images-uploaded flex mt-2"
                            id="image-reply-preview"
                        >
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
                </div>
                <div className="text-right flex justify-end ">
                    { type !== 'post' && (
                        <input
                            type="button"
                            value="Hủy"
                            className="cursor-pointer"
                            style={{
                                padding: "6px 11px",
                                backgroundColor: "#unset",
                                border: "1px solid #333",
                                color: "#333",
                                marginRight: "4px",
                            }}
                            onClick={onDisableEdit}
                        />
                    )}
                    <button type="button">
                        {type === 'post' && 'Gủi bình luận'}
                    </button>
                </div>
            </>)}
        </div>
    )
};

export default UserComment;