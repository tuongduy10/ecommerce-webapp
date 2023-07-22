import Dialog from "@mui/material/Dialog";

import DialogContent from "@mui/material/DialogContent";
import { MuiIcon } from "src/_shares/_components";

export default function GalleryDialog(props: any) {
  const { handleClickCloseDialog, openDialog, selectedImage } = props;
  return (
    <div>
      <Dialog open={openDialog} onClose={handleClickCloseDialog}>
        <DialogContent
          style={{ padding: "5px 14px 15px 14px", overflow: "hidden" }}
        >
          <div>
            <MuiIcon
              className="absolute right-4"
              style={{
                cursor: "pointer",
                float: "right",
                stroke: "#FFFBF1",
                fontWeight: "900",
                backgroundColor: "#B22B27",
                borderRadius: "50%",
              }}
              name="X"
              onClick={handleClickCloseDialog}
            />
            <img
              className="md:max-w-full md:h-auto"
              src={selectedImage}
              alt="Hình ảnh sản phẩm"
            />
          </div>
        </DialogContent>
      </Dialog>
    </div>
  );
}
