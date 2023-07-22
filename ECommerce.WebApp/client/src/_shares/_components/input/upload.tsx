import { useRef, useState } from "react";
import MuiIcon from "../mui-icon/mui-icon.component";

interface IUploadProps extends React.HTMLProps<HTMLInputElement> {
    onChangeFiles: (e: any) => void,
}

const UploadInput = (props: IUploadProps) => {
    const { onChangeFiles, ...rest } = props;
    
    const inputElement = useRef<any>(null);
    const [selectedFiles, setSelectedFiles] = useState<{ id: number; src: string }[]>([]);
    
    const handleOnChange = (e: { target: { files: any } }) => {
        const files = e.target.files;
        const imageArray: any = [];
        const emitFiles: any = [];

        for (let i = 0; i < files.length; i++) {
            if (i < 3) {
                if (!files[i].type.match('image'))
                    continue;
                emitFiles.push(files[i]);
                const rd = new FileReader();
                rd.onload = (e) => {
                    if (e.target) {
                        const imageObject = {
                            ...e.target,
                            id: i,
                            name: files[i].name as string,
                            src: e.target.result as string,
                        };
                        imageArray.push(imageObject);
                        if (imageArray.length === Math.min(files.length, 3)) {
                            setSelectedFiles(imageArray)
                        }
                    }
                };
                rd.readAsDataURL(files[i]);
            }
        }
        onChangeFiles(emitFiles);
    };

    const handleRemoveFile = (_file: any) => {
        const updatedFiles = selectedFiles.filter((image) => image.id !== _file.id);
        setSelectedFiles(updatedFiles);
        if (inputElement.current) {
            const files = [...inputElement.current.files].filter((file) => file.name !== _file.name);
            const list = new DataTransfer();
            files.forEach((file) => {
                list.items.add(file);
            })
            inputElement.current.files = list.files;
            onChangeFiles(files);
        }
    };

    return (<>
        <input
            {...rest}
            type="file"
            ref={inputElement}
            onChange={handleOnChange}
        />
        <output className="images-uploaded flex mt-2">
            {selectedFiles.map((image) => (
                <div className="image__upload-item" key={image.id}>
                    <div className="image-uploaded relative w-[100px] h-[100px] mr-[4px]">
                        <img
                            src={image.src}
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
                                name="X"
                                style={{
                                    stroke: "#FFFBF1",
                                    fontWeight: "900",
                                }}
                                className="feather feather-x"
                                onClick={() => handleRemoveFile(image)}
                            />
                        </span>
                    </div>
                </div>
            ))}
        </output>
    </>);
}

export default UploadInput;