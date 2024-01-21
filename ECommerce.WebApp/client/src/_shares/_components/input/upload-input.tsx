import { useEffect, useRef, useState } from "react";
import MuiIcon from "../mui-icon/mui-icon.component";
import { ICON_NAME } from "../mui-icon/_enums/mui-icon.enum";
import CommonService from "src/_cores/_services/common.service";
import { ENV } from "src/_configs/enviroment.config";
import { Menu, MenuItem } from "@mui/material";

interface IUploadProps extends React.HTMLProps<HTMLInputElement> {
    onChangeFiles: (e: any) => void,
    filesLimit?: number,
    selectedFiles?: string[],
    uploadType: "products" | "brand" | "rates"
}

const UploadInput = (props: IUploadProps) => {
    const {
        onChangeFiles,
        filesLimit = 3, // default limit: 3
        uploadType,
        selectedFiles,
        ...rest
    } = props;

    const inputElement = useRef<any>(null);
    const [selectedFileNames, setSelectedFileNames] = useState<string[]>([]);
    const [delAnchorEl, setDelAnchorEl] = useState<null | HTMLElement>(null);
    const openDel = Boolean(delAnchorEl);

    useEffect(() => {
        if (selectedFiles && selectedFiles.length > 0) {
            setSelectedFileNames(selectedFiles);
        }
    }, [selectedFiles]);

    const handleOnChange = (e: { target: { files: any } }) => {
        const files = e.target.files;
        if (files.length > filesLimit) {
            return;
        }
        const formData = new FormData();
        formData.append('uploadType', uploadType);
        for (const file of files) {
            formData.append('files', file)
        }
        CommonService.uploadFiles(formData).then((res: any) => {
            if (res.data) { // files name sent from server
                const fileNames = selectedFileNames.concat(res.data);
                setSelectedFileNames(fileNames);
                onChangeFiles(fileNames);
            }
        }).catch((error: any) => {
            console.log(error);
        });
    };

    const handleRemoveFile = (_fileName: string) => {
        const params = {
            fileNames: [_fileName],
            uploadType: uploadType,
        }
        CommonService.removeFiles(params).then((res: any) => {
            if (res.isSucceed) {
                const updatedFileNames = selectedFileNames.filter((fileName) => fileName !== _fileName);
                setSelectedFileNames(updatedFileNames);
                onChangeFiles(updatedFileNames);
            }
        })
    };

    return (<>
        <input
            {...rest}
            type="file"
            ref={inputElement}
            onChange={handleOnChange}
        />
        {selectedFileNames.length > 0 && (
            <output className="images-uploaded flex mt-2">
                {selectedFileNames.map((fileName) => (
                    <div className="image__upload-item" key={fileName}>
                        <div className="image-uploaded relative w-[100px] h-[100px] mr-[4px]">
                            <img
                                src={`${ENV.IMAGE_URL}/${uploadType}/${fileName}`}
                                alt="Ảnh của bạn"
                                className="h-full w-full object-contain"
                            />
                            <span
                                className="absolute top-0 right-0 cursor-pointer remove__upload"
                                style={{
                                    backgroundColor: "#B22B27",
                                    borderRadius: "50%",
                                }}
                            >
                                <MuiIcon
                                    name={ICON_NAME.FEATHER.X}
                                    style={{
                                        stroke: "#FFFBF1",
                                        fontWeight: "900",
                                    }}
                                    className="feather feather-x"
                                    onClick={(event: React.MouseEvent<HTMLButtonElement>) => setDelAnchorEl(event.currentTarget)}
                                    aria-controls={openDel ? 'basic-menu' : undefined}
                                    aria-haspopup="true"
                                    aria-expanded={openDel ? 'true' : undefined}
                                />
                                <Menu
                                    anchorEl={delAnchorEl}
                                    open={openDel}
                                    onClose={() => setDelAnchorEl(null)}
                                    MenuListProps={{
                                        'aria-labelledby': 'basic-button',
                                    }}
                                >
                                    <MenuItem onClick={() => { handleRemoveFile(fileName); setDelAnchorEl(null); }}>Xác nhận xóa</MenuItem>
                                    <MenuItem onClick={() => setDelAnchorEl(null)}>Hủy</MenuItem>
                                </Menu>
                            </span>
                        </div>
                    </div>
                ))}
            </output>
        )}
    </>);
}

export default UploadInput;