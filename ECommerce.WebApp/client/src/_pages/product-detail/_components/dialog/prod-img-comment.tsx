import Dialog from "@mui/material/Dialog";

import DialogContent from "@mui/material/DialogContent";
import { MuiIcon } from "src/_shares/_components";

export default function ImageCommentDialog(props: any) {
  const { handleClickCloseDialog, openDialog } = props;
  return (
    <div>
      <Dialog
        open={openDialog}
        onClose={handleClickCloseDialog}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
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
              style={{ width: "100%", height: "100%" }}
              src="https://hihichi.com/images/products/product_cf7a781c-01b2-48d5-9781-337e4f091701.jpeg"
              alt="Hình ảnh của bạn"
            />
          </div>
        </DialogContent>
      </Dialog>
    </div>
  );
}
