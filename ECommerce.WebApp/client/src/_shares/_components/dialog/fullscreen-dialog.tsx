import * as React from 'react';
import Dialog from '@mui/material/Dialog';
import Slide from '@mui/material/Slide';
import { TransitionProps } from '@mui/material/transitions';
import MuiIcon from '../mui-icon/mui-icon.component';
import { DialogContent } from '@mui/material';
import { ICON_NAME } from '../mui-icon/_enums/mui-icon.enum';

const Transition = React.forwardRef(function Transition(
    props: TransitionProps & {
        children: React.ReactElement;
    },
    ref: React.Ref<unknown>,
) {
    return <Slide direction="up" ref={ref} {...props} />;
});


type FullScreenDialogProps = {
    isOpen: boolean
    content: string;
    onClose: () => void,
}

export default function FullScreenDialog(props: FullScreenDialogProps) {
    const { isOpen, content, onClose } = props

    return (
        <Dialog
            fullScreen
            open={isOpen}
            onClose={onClose}
            TransitionComponent={Transition}
        >
            <DialogContent
                style={{ padding: "5px 14px 15px 14px", }}
            >
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
                    name={ICON_NAME.FEATHER.X}
                    onClick={onClose}
                />
                <div className="description" dangerouslySetInnerHTML={{ __html: content || '' }}></div>
            </DialogContent>
        </Dialog>
    );
}